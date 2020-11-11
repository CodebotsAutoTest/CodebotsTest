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
using System.Linq;
using Npgsql;
using Audit.EntityFramework;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Lm2348.Models {
	// % protected region % [Change the class signature here] off begin
	public class Lm2348DBContext : AuditIdentityDbContext<User, Group, Guid>, IDataProtectionKeyContext
	// % protected region % [Change the class signature here] end
	{
		private readonly ILogger<Lm2348DBContext> _logger;

		public string SessionUserId { get; }
		public string SessionUser { get; }
		public string SessionId { get; }

		// % protected region % [Add any custom class variables] off begin
		// % protected region % [Add any custom class variables] end

		public DbSet<UploadFile> Files { get; set; }
		public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

		static Lm2348DBContext()
		{
			// % protected region % [Add extra methods to the static constructor here] off begin
			// % protected region % [Add extra methods to the static constructor here] end
		}

		public Lm2348DBContext(
			// % protected region % [Add any custom constructor paramaters] off begin
			// % protected region % [Add any custom constructor paramaters] end
			DbContextOptions<Lm2348DBContext> options,
			IHttpContextAccessor httpContextAccessor,
			ILogger<Lm2348DBContext> logger) : base(options)
		{
			_logger = logger;

			SessionUser = httpContextAccessor?.HttpContext?.User?.Identity?.Name;
			SessionUserId = httpContextAccessor?.HttpContext?.User?.FindFirst("UserId")?.Value;
			SessionId = httpContextAccessor?.HttpContext?.TraceIdentifier;

			// % protected region % [Add any constructor config here] off begin
			// % protected region % [Add any constructor config here] end
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Configure models from the entity diagram
			modelBuilder.HasPostgresExtension("uuid-ossp");

			// Configure the user and group models
			modelBuilder.ApplyConfiguration(new UserConfiguration());
			modelBuilder.ApplyConfiguration(new GroupConfiguration());

			// Configure the file upload models
			modelBuilder.ApplyConfiguration(new UploadFileConfiguration());

			// % protected region % [Add any further model config here] off begin
			// % protected region % [Add any further model config here] end
		}

		/// <summary>
		/// Gets a DbSet of a certain type from the context
		/// </summary>
		/// <param name="name">The name of the DbSet to retrieve</param>
		/// <typeparam name="T">The type to cast the DbSet to</typeparam>
		/// <returns>A DbSet of the given type</returns>
		[Obsolete("Please obtain the db set from the db context with generic type param instead.")]
		public DbSet<T> GetDbSet<T>(string name = null) where T : class, IAbstractModel
		{
			// % protected region % [Add any extra logic on GetDbSet here] off begin
			// % protected region % [Add any extra logic on GetDbSet here] end

			return GetType().GetProperty(name ?? typeof(T).Name).GetValue(this, null) as DbSet<T>;
		}

		/// <summary>
		/// Gets a DbSet as an IQueryable over the owner abstract model
		/// </summary>
		/// <param name="name">The name of the DbSet to retrieve</param>
		/// <returns>The DbSet as an IQueryable over the OwnerAbstractModel or null if it doesn't exist</returns>
		public IQueryable GetOwnerDbSet(string name)
		{
			return GetType().GetProperty(name).GetValue(this, null) as IQueryable;
		}

		// % protected region % [Add any extra db config here] off begin
		// % protected region % [Add any extra db config here] end
	}
}
