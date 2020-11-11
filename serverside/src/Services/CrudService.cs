/*
 * @bot-written
 * 
 * WARNING AND NOTICE
 * Any access, download, storage, and/or use of this source code is subject to the terms and conditions of the
 * Full Software Licence as accepted by you before being granted access to this source code and other materials,
 * the terms of which can be accessed on the Codebots website at https://codebots.com/full-software-licence. Any
 * commercial use in contravention of the terms of the Full Software Licence may be pursued by Codebots through
 * licence termination and further legal action, and be required to indemnify Codebots for any loss or damage,
 * including interest and costs. You are deemed to have accepted the terms of the Full Software Licence on any
 * access, download, storage, and/or use of this source code.
 * 
 * BOT WARNING
 * This file is bot-written.
 * Any changes out side of "protected regions" will be lost next time the bot makes any changes.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Lm2348.Enums;
using Lm2348.Helpers;
using Lm2348.Models;
using Lm2348.Utility;
using Lm2348.Graphql.Types;
using Lm2348.Models.RegistrationModels;
using Lm2348.Services.Interfaces;
using Lm2348.Services.Files;
using GraphQL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Z.EntityFramework.Plus;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Lm2348.Services
{
	public class CrudService : ICrudService
	{
		private readonly Lm2348DBContext _dbContext;
		private readonly UserManager<User> _userManager;
		private readonly ISecurityService _securityService;
		private readonly IIdentityService _identityService;
		private readonly ILogger<CrudService> _logger;
		private readonly IServiceProvider _serviceProvider;
		private readonly IUploadStorageProvider _storageProvider;
		private readonly IUserService _userService;
		private readonly IAuditService _auditService;

		public CrudService(
			Lm2348DBContext dbContext,
			UserManager<User> userManager,
			ISecurityService securityService,
			IIdentityService identityService,
			ILogger<CrudService> logger,
			IServiceProvider serviceProvider,
			IUploadStorageProvider storageProvider,
			IUserService userService,
			IAuditService auditService)
		{
			_dbContext = dbContext;
			_userManager = userManager;
			_securityService = securityService;
			_identityService = identityService;
			_logger = logger;
			_serviceProvider = serviceProvider;
			_storageProvider = storageProvider;
			_userService = userService;
			_auditService = auditService;
		}

		/// <inheritdoc />
		public IQueryable<T> GetById<T>(Guid id)
			where T : class, IOwnerAbstractModel, new()
		{
			return Get<T>().Where(model => model.Id == id);
		}

		/// <inheritdoc />
		public IQueryable<T> Get<T>(Pagination pagination = null, object auditFields = null)
			where T : class, IOwnerAbstractModel, new()
		{
			_identityService.RetrieveUserAsync().Wait();
			var dbSet = _dbContext.Set<T>() as IQueryable<T>;

			_auditService.CreateReadAudit(
				_identityService.User?.Id.ToString(),
				_identityService.User?.UserName,
				typeof(T).Name,
				auditFields);

			// % protected region % [Do extra things after get] off begin
			// % protected region % [Do extra things after get] end

			return dbSet
				.AddReadSecurityFiltering(_identityService, _userManager, _dbContext, _serviceProvider)
				.AddPagination(pagination);
		}

		/// <inheritdoc />
		public async Task<T> Create<T>(
			T model,
			UpdateOptions options = null,
			CancellationToken cancellation = default)
			where T : class, IOwnerAbstractModel, new()
		{
			var result = await Create(new List<T> {model}, options, cancellation);
			return result.First();
		}

		/// <inheritdoc />
		public async Task<ICollection<T>> Create<T>(
			ICollection<T> models,
			UpdateOptions options = null,
			CancellationToken cancellation = default)
			where T : class, IOwnerAbstractModel, new()
		{
			var entityName = typeof(T).Name;
			await _identityService.RetrieveUserAsync();
			var dbSet = _dbContext.Set<T>();

			using (var transaction = _dbContext.Database.BeginTransaction())
			{
				try
				{
					await MergeReferences(models, options, cancellation);

					foreach (var model in models)
					{
						// Update is used here so references are properly handled
						dbSet.Update(model);
					}

					// Ensure that we create all of the base entities instead of updating
					var addedEntries = _dbContext
						.ChangeTracker
						.Entries()
						.Where(entry => models.Contains(entry.Entity));
					foreach (var entry in addedEntries)
					{
						entry.State = EntityState.Added;
					}

					var fileModels = _dbContext
						.ChangeTracker
						.Entries()
						.Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
						.Select(e => e.Entity)
						.ToList();
					if (options?.Files != null)
					{
						await SaveFiles(fileModels, options.Files, cancellation);
					}
					else
					{
						ClearFileAttributes(models);
					}

					await AssignModelMetaData(_identityService.User, cancellation);
					ValidateModels(_dbContext.ChangeTracker
						.Entries()
						.Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
						.Select(e => e.Entity));

					// % protected region % [Do extra things after create] off begin
					// % protected region % [Do extra things after create] end

					var errors = await _securityService.CheckDbSecurityChanges(_identityService, _dbContext);
					if (errors.Any())
					{
						throw new AggregateException(
							errors.Select(error => new InvalidOperationException(error)));
					}

					foreach (var entry in _dbContext.ChangeTracker.Entries<IAbstractModel>().ToList())
					{
						await entry.Entity.BeforeSave(entry.State, _dbContext, _serviceProvider);
					}

					var changes = _dbContext
						.ChangeTracker
						.Entries<IAbstractModel>()
						.Select(e => new ChangeState { State = e.State, Entry = e })
						.ToList();
					await _dbContext.SaveChangesAsync(cancellation);

					foreach (var change in changes)
					{
						await change.Entry.Entity.AfterSave(change.State, _dbContext, _serviceProvider, changes);
					}

					transaction.Commit();

					return models;
				}
				catch (Exception e)
				{
					_logger.LogError("Error completing create action - {Error}", e.ToString());
					throw;
				}
			}
		}

		/// <inheritdoc />
		public async Task<T> Update<T>(
			T model,
			UpdateOptions options = null,
			CancellationToken cancellation = default)
			where T : class, IOwnerAbstractModel, new()
		{
			var result = await Update(new List<T> {model}, options, cancellation);
			return result.First();
		}

		/// <inheritdoc />
		public async Task<ICollection<T>> Update<T>(
			ICollection<T> models,
			UpdateOptions options = null,
			CancellationToken cancellation = default)
			where T : class, IOwnerAbstractModel, new()
		{
			await _identityService.RetrieveUserAsync();
			var dbSet = _dbContext.Set<T>();

			using (var transaction = _dbContext.Database.BeginTransaction())
			{
				try
				{
					await MergeReferences(models, options, cancellation);

					foreach (var model in models)
					{
						dbSet.Update(model);
					}

					var fileModels = _dbContext
						.ChangeTracker
						.Entries()
						.Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
						.Select(e => e.Entity)
						.ToList();
					if (options?.Files != null)
					{
						await SaveFiles(fileModels, options.Files, cancellation);
					}
					else
					{
						ClearFileAttributes(models);
					}

					await AssignModelMetaData(_identityService.User, cancellation);
					ValidateModels(_dbContext.ChangeTracker
						.Entries()
						.Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
						.Select(e => e.Entity));

					// % protected region % [Do extra things after update] off begin
					// % protected region % [Do extra things after update] end

					var errors = await _securityService.CheckDbSecurityChanges(_identityService, _dbContext);
					if (errors.Any())
					{
						throw new AggregateException(
							errors.Select(error => new InvalidOperationException(error)));
					}

					foreach (var entry in _dbContext.ChangeTracker.Entries<IAbstractModel>().ToList())
					{
						await entry.Entity.BeforeSave(entry.State, _dbContext, _serviceProvider);
					}


					var changes = _dbContext
						.ChangeTracker
						.Entries<IAbstractModel>()
						.Select(e => new ChangeState { State = e.State, Entry = e })
						.ToList();
					await _dbContext.SaveChangesAsync(cancellation);

					foreach (var change in changes)
					{
						await change.Entry.Entity.AfterSave(change.State, _dbContext, _serviceProvider, changes);
					}

					transaction.Commit();

					return models;
				}
				catch (Exception e)
				{
					_logger.LogError("Error completing update action - {Error}", e.ToString());
					throw;
				}
			}
		}

		/// <inheritdoc />
		public async Task<BooleanObject> ConditionalUpdate<T>(
			IQueryable<T> models,
			MemberInitExpression updateMemberInitExpression,
			CancellationToken cancellation = default)
			where T : class, IOwnerAbstractModel, new()
		{
			var param = Expression.Parameter(typeof(T), "model");
			var replacer = new ParameterReplacer(param);
			var updateFactory = Expression.Lambda<Func<T, T>>(replacer.Visit(updateMemberInitExpression), param);
			await models.AddUpdateSecurityFiltering(_identityService, _userManager, _dbContext, _serviceProvider).UpdateAsync(updateFactory, cancellation);

			// % protected region % [Do extra things after delete] off begin
			// % protected region % [Do extra things after delete] end

			var errors = await _securityService.CheckDbSecurityChanges(_identityService, _dbContext);
			if (errors.Any())
			{
				throw new AggregateException(errors.Select(error => new InvalidOperationException(error)));
			}
			await _dbContext.SaveChangesAsync(cancellation);
			return new BooleanObject { Value = true };
		}

		/// <inheritdoc />
		public async Task<Guid> Delete<T>(Guid id, CancellationToken cancellation = default)
			where T : class, IAbstractModel
		{
			var result = await Delete<T>(new List<Guid> {id}, cancellation);
			return result.First();
		}

		/// <inheritdoc />
		public async Task<ICollection<Guid>> Delete<T>(List<Guid> ids, CancellationToken cancellation = default)
			where T : class, IAbstractModel
		{
			await _identityService.RetrieveUserAsync();
			var dbSet = _dbContext.Set<T>();

			await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellation);
			var models = await dbSet.Where(o => ids.Contains(o.Id)).ToListAsync(cancellation);

			dbSet.RemoveRange(models);

			// % protected region % [Do extra things after delete] off begin
			// % protected region % [Do extra things after delete] end

			var errors = await _securityService.CheckDbSecurityChanges(_identityService, _dbContext);
			if (errors.Any())
			{
				throw new AggregateException(errors.Select(error => new InvalidOperationException(error)));
			}

			await AssignModelMetaData(_identityService.User, cancellation);

			foreach (var entry in _dbContext.ChangeTracker.Entries<IAbstractModel>().ToList())
			{
				await entry.Entity.BeforeSave(entry.State, _dbContext, _serviceProvider);
			}

			var changes = _dbContext
				.ChangeTracker
				.Entries<IAbstractModel>()
				.Select(e => new ChangeState { State = e.State, Entry = e })
				.ToList();
			try {
				await _dbContext.SaveChangesAsync(cancellation);
			}
			catch (Exception e)
			{
				var exceptionData = e.InnerException.Data;
				if (exceptionData.Contains("Detail"))
				{
					string errorMessage = exceptionData["Detail"].ToString();
					throw new AggregateException(new InvalidOperationException(errorMessage));
				}
				throw new AggregateException(new InvalidOperationException(e.Message));
			}

			foreach (var change in changes)
			{
				await change.Entry.Entity.AfterSave(change.State, _dbContext, _serviceProvider, changes);
			}

			transaction.Commit();

			return ids;
		}

		/// <inheritdoc />
		public async Task<BooleanObject> ConditionalDelete<T>(IQueryable<T> models, CancellationToken cancellation = default)
			where T : class, IOwnerAbstractModel, new()
		{
			try
			{
				await models.AddDeleteSecurityFiltering(_identityService, _userManager, _dbContext, _serviceProvider).DeleteAsync(cancellation);
			}
			catch (Exception e) {
				var exceptionData = e.Data;
				if (exceptionData.Contains("Detail"))
				{
					string errorMessage = exceptionData["Detail"].ToString();
					throw new AggregateException(new InvalidOperationException(errorMessage));
				}
				throw new AggregateException(new InvalidOperationException(e.Message));
			}

			// % protected region % [Do extra things after delete] off begin
			// % protected region % [Do extra things after delete] end

			var errors = await _securityService.CheckDbSecurityChanges(_identityService, _dbContext);
			if (errors.Any())
			{
				throw new AggregateException(errors.Select(error => new InvalidOperationException(error)));
			}
			await _dbContext.SaveChangesAsync(cancellation);
			return new BooleanObject { Value = true };
		}

		/// <inheritdoc />
		public async Task<ICollection<User>> CreateUser<TModel, TRegisterModel>(
			ICollection<TRegisterModel> models,
			UpdateOptions options = null,
			CancellationToken cancellation = default)
			where TModel : User, IOwnerAbstractModel, new()
			where TRegisterModel : IRegistrationModel<TModel>
		{
			// % protected region % [Customise the registration before any action] off begin
			// % protected region % [Customise the registration before any action] end
			await _identityService.RetrieveUserAsync();

			var dbModels = models.Select(m => m.ToModel()).ToList();
			var dbSet = _dbContext.Set<TModel>();

			var roles = models.SelectMany(m => m.Groups).ToList();
			var dbRoles = await _dbContext.Roles.Where(r => roles.Contains(r.Name)).ToListAsync(cancellation);
			// % protected region % [Customise the registration after initial lookups] off begin
			// % protected region % [Customise the registration after initial lookups] end


			using (var transaction = _dbContext.Database.BeginTransaction())
			{
				try
				{
					await MergeReferences(dbModels, options, cancellation);

					var modelPairs = models.Select(model => (model, model.ToModel())).ToList();

					// Create each user
					var createdUsers = new List<User>();
					foreach (var (registrationModel, model) in modelPairs)
					{
						// % protected region % [Customise for each registration model] off begin
						// % protected region % [Customise for each registration model] end

						if (model.Id == Guid.Empty)
						{
							model.Id = Guid.NewGuid();
						}

						foreach (var group in registrationModel.Groups)
						{
							var role = dbRoles.First(r => r.Name == group);
							_dbContext.UserRoles.Add(new IdentityUserRole<Guid>{UserId = model.Id, RoleId = role.Id});
						}

						// Validate the password matches the applications password strength
						var validationTasks = _userManager
							.PasswordValidators
							.Select(v => v.ValidateAsync(_userManager, model, registrationModel.Password));
						var validationResults = await Task.WhenAll(validationTasks);
						var failed = validationResults.Where(r => r.Succeeded == false).ToList();
						if (failed.Any())
						{
							throw new AggregateException(failed
								.SelectMany(f => f.Errors.Select(s => s.Description))
								.Select(s => new InvalidOperationException(s)));
						}

						// % protected region % [Customise user model here] off begin
						// % protected region % [Customise user model here] end

						model.UserName = model.Email;
						model.PasswordHash = _userManager.PasswordHasher.HashPassword(model, registrationModel.Password);
						model.ConcurrencyStamp = await _userManager.GenerateConcurrencyStampAsync(model);
						model.NormalizedEmail = _userManager.NormalizeEmail(model.Email);
						model.NormalizedUserName = _userManager.NormalizeName(model.UserName);
						model.EmailConfirmed = true;
						model.SecurityStamp = Guid.NewGuid().ToString();
						dbSet.Update(model);

						createdUsers.Add(model);
					}

					// Ensure that we create all of the base entities instead of updating
					var addedEntries = _dbContext.ChangeTracker.Entries()
						.Where(entry => createdUsers.Contains(entry.Entity));
					foreach (var entry in addedEntries)
					{
						entry.State = EntityState.Added;
					}

					var fileModels = _dbContext
						.ChangeTracker
						.Entries()
						.Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
						.Select(e => e.Entity)
						.ToList();
					if (options?.Files != null)
					{
						await SaveFiles(fileModels, options.Files, cancellation);
					}
					else
					{
						ClearFileAttributes(models);
					}

					await AssignModelMetaData(_identityService.User, cancellation);
					ValidateModels(_dbContext.ChangeTracker
						.Entries()
						.Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
						.Select(e => e.Entity));

					foreach (var user in createdUsers)
					{
						// % protected region % [Add in any custom creates for each created user] off begin
						// % protected region % [Add in any custom creates for each created user] end
						user.Owner = user.Id;
					}

					var errors = await _securityService.CheckDbSecurityChanges(_identityService, _dbContext);
					if (errors.Any())
					{
						throw new AggregateException(
							errors.Select(error => new InvalidOperationException(error)));
					}

					foreach (var entry in _dbContext.ChangeTracker.Entries<IAbstractModel>().ToList())
					{
						await entry.Entity.BeforeSave(entry.State, _dbContext, _serviceProvider);
					}

					var changes = _dbContext
						.ChangeTracker
						.Entries<IAbstractModel>()
						.Select(e => new ChangeState { State = e.State, Entry = e })
						.ToList();
					await _dbContext.SaveChangesAsync(cancellation);

					foreach (var change in changes)
					{
						await change.Entry.Entity.AfterSave(change.State, _dbContext, _serviceProvider, changes);
					}

					transaction.Commit();

					return createdUsers;
				}
				catch (Exception e)
				{
					_logger.LogError("Error completing create user action - {Error}", e.ToString());
					throw;
				}
			}
		}

		/// <inheritdoc />
		public async Task<UploadFile> GetFile(Guid id, CancellationToken cancellation = default)
		{
			// % protected region % [Override GetFile here] off begin
			await _identityService.RetrieveUserAsync();

			var file = await _dbContext
				.Files
				.FirstOrDefaultAsync(f => f.Id == id, cancellation);

			if (file == null)
			{
				throw new FileNotFoundException("File not found");
			}

			return file;
			// % protected region % [Override GetFile here] end
		}

		private bool CanViewEntity<T>(T entity) where T : IOwnerAbstractModel, new()
		{
			var filter = SecurityService
				.CreateReadSecurityFilter<T>(_identityService, _userManager, _dbContext, _serviceProvider);
			var objList = new List<T> {entity};
			return objList.Where(filter.Compile()).Any();
		}

		private void ThrowFileError(UploadFile file, string entityType)
		{
			_logger.LogInformation(
				"User {User} with invalid security attempted to access file with ID {ID}",
				_identityService.User?.UserName,
				file.Id,
				entityType,
				file);
			throw new UnauthorizedAccessException("Insufficient access to view this file");
		}

		private async Task SaveFiles<T>(
			IEnumerable<T> models,
			IFormFileCollection files,
			CancellationToken cancellation = default)
		{
			foreach (var model in models)
			{
				var modelType = model.GetType();
				var fileAttrs = ReflectionCache.GetFileAttributes(modelType);

				if (fileAttrs.Count <= 0)
				{
					continue;
				}

				foreach (var attr in fileAttrs)
				{
					if (!(attr.GetValue(model) is Guid fileId))
					{
						continue;
					}

					var file = files.FirstOrDefault(f => f.Name == fileId.ToString());

					if (file == null)
					{
						continue;
					}

					await using var fileStream = file.OpenReadStream();
					var serverFileId = Guid.NewGuid().ToString();
					await _storageProvider.PutAsync(new StoragePutOptions
					{
						Container = modelType.Name,
						FileName = serverFileId,
						Content = fileStream,
						ContentType = file.ContentType,
						CreateContainerIfNotExists = true,
					}, cancellation);

					var dbFile = new UploadFile
					{
						Id = Guid.NewGuid(),
						FileName = file.FileName,
						FileId = serverFileId,
						Length = file.Length,
						ContentType = file.ContentType,
						Container = modelType.Name,
					};

					var existingFile = (await _dbContext.Entry(model).GetDatabaseValuesAsync(cancellation))?[attr.Name];

					attr.SetValue(model, dbFile.Id);
					_dbContext.Files.Add(dbFile);

					if (existingFile is Guid oldFileId)
					{
						var fileEntry = await _dbContext
							.Files
							.FirstOrDefaultAsync(f => f.Id == oldFileId, cancellation);
						if (fileEntry != null)
						{
							_dbContext.Files.Remove(fileEntry);
						}
					}
				}
			}
		}

		private static void ClearFileAttributes<T>(IEnumerable<T> models)
		{
			foreach (var model in models)
			{
				var modelType = model.GetType();
				var fileAttrs = ReflectionCache.GetFileAttributes(modelType);

				foreach (var attr in fileAttrs)
				{
					attr.SetValue(model, null);
				}
			}
		}

		private async Task AssignModelMetaData(User user, CancellationToken cancellation = default)
		{
			foreach (var entry in _dbContext.ChangeTracker.Entries<IOwnerAbstractModel>())
			{
				switch (entry.State)
				{
					case EntityState.Added:
						entry.Entity.Created = DateTime.UtcNow;
						entry.Entity.Modified = DateTime.UtcNow;
						if (entry.Entity.Owner == Guid.Empty)
						{
							entry.Entity.Owner = user?.Id ?? Guid.Empty;
						}

						// If we haven't been given an id to create against then we need to make a new one
						if (entry.Entity.Id == Guid.Empty)
						{
							entry.Entity.Id = Guid.NewGuid();
						}

						break;
					case EntityState.Modified:
						// Unset fields we don't want to be changed on update
						entry.Property("Owner").IsModified = false;
						entry.Property("Created").IsModified = false;
						entry.Entity.Modified = DateTime.UtcNow;
						break;
				}
			}

			// Users have a concurrency stamp so they need to be pulled from the db and have
			// the concurrency stamp applied to each of the objects we save back to the database
			var userEntries = _dbContext.ChangeTracker
				.Entries<User>()
				.ToList();

			// When updating users the core object should not ever change
			foreach (var entry in userEntries)
			{
				switch (entry.State)
				{
					case EntityState.Added:
						// A user should always own themselves
						entry.Entity.Owner = entry.Entity.Id;
						break;
					case EntityState.Modified:
						// % protected region % [Adjust modification of user models here] off begin
						var databaseProperties = await entry.GetDatabaseValuesAsync(cancellation);
						var proposedProperties = entry.CurrentValues;

						foreach (var userProperty in GetNonModifiableUserProperties())
						{
							proposedProperties[userProperty] = databaseProperties[userProperty];
						}

						entry.OriginalValues.SetValues(databaseProperties);
						entry.Property("Discriminator").IsModified = false;
						// % protected region % [Adjust modification of user models here] end
						break;
				}
			}
		}

		// % protected region % [Configure the modelled groups here] off begin
		/// <summary>
		/// Return the list of fields that should not be modified on the User.cs Entity.
		/// </summary>
		/// <returns></returns>
		private List<string> GetNonModifiableUserProperties()
		{
			return typeof(User)
				.GetProperties()
				.Select(p => p.Name)
				.Where(n => n != "Acls")
				.ToList();
		}
		// % protected region % [Configure the modelled groups here] end

		private static void ValidateModels<T>(IEnumerable<T> models)
		{
			var validationExceptions = models.SelectMany(model =>
			{
				var errors = new List<ValidationResult>();
				model.ValidateObjectFields(errors);
				if (errors.Count > 0)
				{
					return new List<ValidationException>
					{
						new ValidationException(string.Join(
							"; ",
							errors.Select(e => e.ErrorMessage).ToArray()))
					};
				}
				return new List<ValidationException>();
			}).ToList();

			if (validationExceptions.Count > 0)
			{
				throw new AggregateException(validationExceptions);
			}
		}

		private async Task MergeReferences<T>(ICollection<T> models, UpdateOptions options, CancellationToken cancellation = default)
			where T : IOwnerAbstractModel, new()
		{
			if (options == null) return;

			var referencesToMerge = typeof(T)
				.GetProperties()
				.SelectMany(prop => prop
					.GetCustomAttributes(typeof(EntityForeignKey), false)
					.Select(attr => attr as EntityForeignKey))
				.Where(attr => options.MergeReferences != null &&
								options.MergeReferences.Contains(attr?.Name, StringComparer.OrdinalIgnoreCase))
				.ToList();

			if (options.MergeReferences != null)
			{
				foreach (var reference in options.MergeReferences)
				{
					try
					{
						var foreignAttribute = referencesToMerge.First(attr => string
							.Equals(attr?.Name, reference, StringComparison.OrdinalIgnoreCase));
						await models.First().CleanReference(foreignAttribute.Name, models, _dbContext, cancellation);
					}
					catch
					{
						// ignored
					}
				}
			}
		}

		// % protected region % [Add extra functions in crudService] off begin
		// % protected region % [Add extra functions in crudService] end

		private static string ObjectAsString(object obj, IEnumerable<string> properties, string openDelimiter, string closeDelimiter, string separator)
		{
			return string.Join(
				separator,
				properties
					.Select(obj.GetPropertyValue)
					.Select((property) =>
					{
						var propertyString = "";
						switch (property)
						{
							case DateTime dt:
								propertyString = dt.ToIsoString();
								break;
							case null:
								break;
							default:
								propertyString = property.ToString();
								break;
						}
						return openDelimiter + (propertyString) + closeDelimiter;
					})
				);
		}

		private static IEnumerable<string> GetExportProperties<T>()
		{
			var properties = ObjectHelper.GetNonReferenceProperties(typeof(T)).ToArray();
			var propertyNames = properties.Select(p => char.ToLowerInvariant(p.Name[0]) + p.Name.Substring(1)).ToList();
			if (propertyNames.Contains("owner"))
			{
				propertyNames.Remove("owner");
			}

			return propertyNames;
		}
	}

	public class PaginationOptions
	{
		/// <summary>
		/// What page to fetch for pagination
		/// </summary>
		public int? PageNo { get; set; }

		/// <summary>
		/// The size of each page for pagination
		/// </summary>
		public int? PageSize { get; set; }
	}

	public class Pagination
	{
		public Pagination() { }

		public Pagination(PaginationOptions options)
		{
			PageNo = options?.PageNo;
			PageSize = options?.PageSize;
		}

		public int? PageSize { get; set; }
		public int? PageNo { get; set; }

		public int? SkipAmount
		{
			get
			{
				if (!isValid())
				{
					return null;
				}
				return (PageNo - 1) * PageSize;
			}
		}

		public bool isValid()
		{
			return PageSize != null && PageSize > 0 && PageNo != null && PageNo > 0;
		}
	}

	public class ExportParameters
	{
		[Required]
		[DataType(DataType.Date)]
		public DateTime FromDate { get; set; }

		[Required]
		[DataType(DataType.Date)]
		public DateTime ToDate { get; set; }
	}

	public class UpdateOptions
	{
		public IEnumerable<string> MergeReferences { get; set; }
		public IFormFileCollection Files { get; set; }
	}
}