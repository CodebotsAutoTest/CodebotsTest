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
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Lm2348.Models;
using Lm2348.Services.Interfaces;
using ServersideTests.Helpers;
using ServersideTests.Helpers.EntityFactory;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace ServersideTests.Tests.Integration.BotWritten.GroupSecurityTests
{
	public class BaseReadTest : IDisposable
	{
		private IWebHost _host = null;
		private IServiceScope _scope = null;

		public BaseReadTest()
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

		public async Task ReadTest<T>(T entity, bool canRead, string groupName)
			where T : class, IOwnerAbstractModel, new()
		{
			// % protected region % [Overwrite read security test here] off begin
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
			var entities = new EntityFactory<T>(10)
				.UseAttributes()
				.UseReferences()
				.UseOwner(Guid.NewGuid())
				.Generate()
				.ToList();
			database.AddRange(entities);
			await database.SaveChangesAsync();

			// Act.
			var data = await crudService.Get<T>().ToListAsync();

			// Assert.
			if (canRead)
			{
				data.Select(d => d.Id).Should().Contain(entities.Select(d => d.Id));
			}
			else
			{
				data.Should().BeEmpty("Should be empty because security denies all access");
			}
			// % protected region % [Overwrite read security test here] end
		}
	}
}