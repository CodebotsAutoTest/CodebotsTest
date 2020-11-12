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
using FluentAssertions;
using Lm2348.Controllers.Entities;
using Lm2348.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ServersideTests.Helpers;
using ServersideTests.Helpers.EntityFactory;
using Xunit;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

// % protected region % [Add any additional imports here] off begin
// % protected region % [Add any additional imports here] end

namespace ServersideTests.Tests.Integration.BotWritten
{
	[Trait("Category", "BotWritten")]
	[Trait("Category", "Unit")]
	public class CrudTests : IDisposable
	{
		private readonly IWebHost _host;
		private readonly Lm2348DBContext _database;
		private readonly IServiceScope _scope;
		private readonly IServiceProvider _serviceProvider;
		// % protected region % [Add any additional members here] off begin
		// % protected region % [Add any additional members here] end

		public CrudTests()
		{
			// % protected region % [Configure constructor here] off begin
			_host = ServerBuilder.CreateServer();
			_scope = _host.Services.CreateScope();
			_serviceProvider = _scope.ServiceProvider;
			_database = _serviceProvider.GetRequiredService<Lm2348DBContext>();
			// % protected region % [Configure constructor here] end
		}
		
		public void Dispose()
		{
			// % protected region % [Configure dispose here] off begin
			_host?.Dispose();
			_database?.Dispose();
			_scope?.Dispose();
			// % protected region % [Configure dispose here] end
		}


		// % protected region % [Customise a Entity crud tests here] off begin
		[Fact]
		public async void AEntityControllerGetTest()
		{
			// Arrange
			using var controller = _serviceProvider.GetRequiredService<AEntityController>();
			var entities = new EntityFactory<AEntity>(10)
				.UseAttributes()
				.UseReferences()
				.UseOwner(Guid.NewGuid())
				.Generate()
				.ToList();
			_database.AddRange(entities);
			await _database.SaveChangesAsync();

			// Act
			var data = await controller.Get(null, default);

			// Assert
			data.Data.Select(d => d.Id).Should().Contain(entities.Select(d => d.Id));
		}
		// % protected region % [Customise a Entity crud tests here] end

		// % protected region % [Customise b Entity crud tests here] off begin
		[Fact]
		public async void BEntityControllerGetTest()
		{
			// Arrange
			using var controller = _serviceProvider.GetRequiredService<BEntityController>();
			var entities = new EntityFactory<BEntity>(10)
				.UseAttributes()
				.UseReferences()
				.UseOwner(Guid.NewGuid())
				.Generate()
				.ToList();
			_database.AddRange(entities);
			await _database.SaveChangesAsync();

			// Act
			var data = await controller.Get(null, default);

			// Assert
			data.Data.Select(d => d.Id).Should().Contain(entities.Select(d => d.Id));
		}
		// % protected region % [Customise b Entity crud tests here] end

	// % protected region % [Add any additional tests here] off begin
	// % protected region % [Add any additional tests here] end
	}
}