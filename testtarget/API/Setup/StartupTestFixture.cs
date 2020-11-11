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
using System.Linq;
using Lm2348.Models;
using APITests.Utils;
using Microsoft.EntityFrameworkCore;
using APITests.Settings;
using Microsoft.Extensions.Configuration;
// % protected region % [Custom imports] off begin
// % protected region % [Custom imports] end

namespace APITests.Setup
{
	public class StartupTestFixture
	{
		public string BaseUrl { get; }

		public string TestUsername { get; }

		public string TestPassword { get;}

		public string SuperUsername { get; }

		public string SuperPassword { get; }

		public DbContextOptions<Lm2348DBContext> DbContextOptions {get;}

		public Guid SuperOwnerId { get; private set; }

		public IConfigurationRoot AppSettings { get; }

		public SiteSettings SiteSettings { get; }

		public IConfigurationRoot UserSettings { get; }

		// % protected region % [Add extra class fields here] off begin
		// % protected region % [Add extra class fields here] end

		public StartupTestFixture()
		{
			// % protected region % [Adjust the appsettings configuration here] off begin
			AppSettings = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddXmlFile("appsettings.Test.xml", optional: true, reloadOnChange: false)
				.AddEnvironmentVariables()
				.AddEnvironmentVariables("Lm2348_")
				.AddEnvironmentVariables($"Lm2348_Test_")
				.Build();
			// % protected region % [Adjust the appsettings configuration here] end

			// % protected region % [Adjust the site configuration here] off begin
			//load in site configuration
			var siteConfiguration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddIniFile("SiteConfig.ini", optional: true, reloadOnChange: false)
				.Build();

			SiteSettings = new SiteSettings();
			siteConfiguration.GetSection("site").Bind(SiteSettings);
			// % protected region % [Adjust the site configuration here] end

			// % protected region % [Adjust the user configuration here] off begin
			//load in the user configurations
			UserSettings = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddIniFile("UserConfig.ini", optional: true, reloadOnChange: false)
				.Build();

			var superUserSettings = new UserSettings();
			var testUserSettings = new UserSettings();
			UserSettings.GetSection("super").Bind(superUserSettings);
			UserSettings.GetSection("test").Bind(testUserSettings);
			// % protected region % [Adjust the user configuration here] end

			// % protected region % [Adjust the site url and user config here] off begin
			var baseUrlFromEnvironment = Environment.GetEnvironmentVariable("BASE_URL");
			BaseUrl = baseUrlFromEnvironment ?? SiteSettings.BaseUrl;

			TestUsername = testUserSettings.Username;
			TestPassword = testUserSettings.Password;
			SuperUsername = superUserSettings.Username;
			SuperPassword = superUserSettings.Password;
			// % protected region % [Adjust the site url and user config here] end

			// % protected region % [Adjust the dbcontext settings here] off begin
			var dbConnectionString = AppSettings["ConnectionStrings:DbConnectionString"];
			DbContextOptions = new DbContextOptionsBuilder<Lm2348DBContext>()
				.UseNpgsql(dbConnectionString)
				.Options;
			// % protected region % [Adjust the dbcontext settings here] end

			PingServer.TestConnection(BaseUrl);
			// % protected region % [Adjust the dbcontext] off begin
			using (var context = new Lm2348DBContext(DbContextOptions, null, null))
			{
				SuperOwnerId = context.Users.First(x => x.UserName == SuperUsername).Id;
			}
			// % protected region % [Adjust the dbcontext] end
		}

		// % protected region % [Add extra methods here] off begin
		// % protected region % [Add extra methods here] end
	}
}
