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
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Lm2348.Models;
using Lm2348.Models.RegistrationModels;
using Lm2348.Services.Interfaces;
using ServersideTests.Helpers;
using ServersideTests.Helpers.EntityFactory;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace ServersideTests.Tests.Integration.BotWritten.GroupSecurityTests
{
	public abstract class BaseCreateTest : IDisposable
	{
		private IWebHost _host = null;
		private IServiceScope _scope = null;

		public BaseCreateTest()
		{
			// % protected region % [Add constructor logic here] off begin
			// % protected region % [Add constructor logic here] end
		}

		public void Dispose()
		{
			// % protected region % [Configure dispose here] off begin
			_host?.Dispose();
			_scope?.Dispose();
			// % protected region % [Configure dispose here] end
		}

		public async Task CreateSecurityTest<T>(T model, string message, string groupName)
			where T : class, IOwnerAbstractModel, new()
		{
			// % protected region % [Overwrite create security test here] off begin
			// Arrange.
			IServiceProvider serviceProvider;
			if (groupName != null)
			{
				_host = ServerBuilder.CreateServer(new ServerBuilderOptions
				{
					UserPrincipal = ServerBuilder.CreateUserPrincipal(
					Guid.NewGuid(),
					$"test_{groupName.ToLower()}@example.com",
					$"test_{groupName.ToLower()}@example.com",
					new[] { groupName }),
				});
				_scope = _host.Services.CreateScope();
				serviceProvider = _scope.ServiceProvider;

				var user = new User
				{
					Email = $"test_{groupName.ToLower()}@example.com",
					Discriminator = "User"
				};
				await serviceProvider.GetRequiredService<IUserService>().RegisterUser(
					user,
					Guid.NewGuid().ToString(),
					new[] { groupName });
			}
			else
			{
				_host = ServerBuilder.CreateServer(new ServerBuilderOptions
				{
					UserPrincipal = null,
				});
				_scope = _host.Services.CreateScope();
				serviceProvider = _scope.ServiceProvider;
			}

			await using var database = serviceProvider.GetRequiredService<Lm2348DBContext>();

			var crudService = serviceProvider.GetRequiredService<ICrudService>();
			var factory = new EntityFactory<T>()
				.UseAttributes()
				.UseReferences()
				.UseOwner(Guid.NewGuid())
				.TrackEntities();
			var entity = factory.Generate().First();

			entity.Id = Guid.Empty;
			var dependencies = factory
				.GetAllEntities()
				.Where(x => x.Id != entity.Id)
				.ToList();
			database.AddRange(dependencies);
			await database.SaveChangesAsync();

			Func<Task> act = async () =>  await crudService.Create(entity);

			if (message == null)
			{
				act.Should().NotThrow($"{groupName} should be allowed to create {model.GetType().Name}");
			}
			else
			{
				act
					.Should()
					.Throw<AggregateException>($"{groupName} should not be allowed to create {model.GetType().Name}")
					.WithInnerException<InvalidOperationException>()
					.And
					.Message
					.Contains(message);
			}
			// % protected region % [Overwrite create security test here] end
		}

		public async Task CreateUserTest<T, TDto>(T model, TDto dto, string message, string groupName)
			where T : User, IOwnerAbstractModel, new()
			where TDto : ModelDto<T>, IRegistrationModel<T>, new()
		{
			// % protected region % [Overwrite create user security test here] off begin
			// Arrange.
			IServiceProvider serviceProvider;
			if (groupName != null)
			{
				_host = ServerBuilder.CreateServer(new ServerBuilderOptions
				{
					UserPrincipal = ServerBuilder.CreateUserPrincipal(
					Guid.NewGuid(),
					$"test_{groupName.ToLower()}@example.com",
					$"test_{groupName.ToLower()}@example.com",
					new[] { groupName }),
				});
				_scope = _host.Services.CreateScope();
				serviceProvider = _scope.ServiceProvider;

				var user = new User
				{
					Email = $"test_{groupName.ToLower()}@example.com",
					Discriminator = "User"
				};
				await serviceProvider.GetRequiredService<IUserService>().RegisterUser(
					user,
					Guid.NewGuid().ToString(),
					new[] { groupName });
			}
			else
			{
				_host = ServerBuilder.CreateServer(new ServerBuilderOptions
				{
					UserPrincipal = null,
				});
				_scope = _host.Services.CreateScope();
				serviceProvider = _scope.ServiceProvider;
			}
			using var database = serviceProvider.GetRequiredService<Lm2348DBContext>();

			var crudService = serviceProvider.GetRequiredService<ICrudService>();
			var factory = new EntityFactory<T>()
				.UseAttributes()
				.UseReferences()
				.UseOwner(Guid.NewGuid())
				.TrackEntities();
			var entity = factory.Generate().First();

			entity.Id = Guid.Empty;
			var dependencies = factory
				.GetAllEntities()
				.Where(x => x.Id != entity.Id)
				.ToList();
			database.AddRange(dependencies);
			await database.SaveChangesAsync();

			var userDto = new TDto();
			userDto.LoadModelData(entity);
			userDto.Email = entity.Email;
			userDto.Password = Guid.NewGuid().ToString();

			// Act.
			Func<Task> act = async () => await crudService.CreateUser<T, TDto>(new List<TDto> { userDto });

			// Assert.
			if (message == null)
			{
				act.Should().NotThrow($"{groupName} should be allowed to create {model.GetType().Name}");
			}
			else
			{
				act
					.Should()
					.Throw<AggregateException>($"{groupName} should not be allowed to create {model.GetType().Name}")
					.WithInnerException<InvalidOperationException>()
					.And
					.Message
					.Contains(message);
			}
			// % protected region % [Overwrite create user security test here] end
		}
	}
}