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
using System.IO;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace ServersideTests.Helpers
{
	public class ServerBuilderOptions
	{
		/// <summary>
		/// The name of the in memory database to use
		/// </summary>
		public string DatabaseName { get; set; } = Path.GetRandomFileName();

		/// <summary>
		/// Should the data seed helper be called to initialise data
		/// This will create the user super@example.com for testing
		/// </summary>
		public bool InitialiseData { get; set; } = true;

		/// <summary>
		/// The claims principal used to represent the testing user
		/// </summary>
		public ClaimsPrincipal UserPrincipal { get; set; } = ServerBuilder.CreateUserPrincipal(
			Guid.NewGuid(),
			"super@example.com",
			"super@example.com",
			new [] {"Visitors", "Super Administrators"});

		/// <summary>
		/// Configuration function for the database for the tests
		/// </summary>
		public Func<ServerBuilderOptions, Action<DbContextOptionsBuilder>> DatabaseOptions { get; set; } = builderOptions => options =>
		{
			options.UseInMemoryDatabase(builderOptions.DatabaseName);
			options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
			options.UseOpenIddict<Guid>();
		};

		/// <summary>
		/// Configure any additional services for the tests
		/// </summary>
		public Action<IServiceCollection, ServerBuilderOptions> ConfigureServices { get; set; } = null;

		// % protected region % [Add any additional server builder options here] off begin
		// % protected region % [Add any additional server builder options here] end
	}
}