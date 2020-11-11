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
using System.Security.Claims;
using AspNet.Security.OpenIdConnect.Primitives;
using Lm2348;
using Lm2348.Helpers;
using Lm2348.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServersideTests.Helpers
{
	public static class ServerBuilder
	{
		/// <summary>
		/// Creates a web host with an in memory database for testing
		/// </summary>
		/// <param name="builderOptions">Options for the host</param>
		/// <returns>The new web host</returns>
		public static IWebHost CreateServer(ServerBuilderOptions builderOptions = null)
		{
			builderOptions ??= new ServerBuilderOptions();

			var httpContext = new DefaultHttpContext { User = builderOptions.UserPrincipal };

			// % protected region % [Add any additional actions before build here] off begin
			// % protected region % [Add any additional actions before build here] end

			var host = WebHost.CreateDefaultBuilder()
				.ConfigureAppConfiguration((builderContext, config) =>
				{
					// % protected region % [Configure app configuration here] off begin
					var env = builderContext.HostingEnvironment;
					config.Sources.Clear();
					config.AddXmlFile("appsettings.xml", optional: false, reloadOnChange: false);
					config.AddXmlFile($"appsettings.Test.xml", optional: true, reloadOnChange: false);
					config.AddEnvironmentVariables();
					// % protected region % [Configure app configuration here] end
				})
				.UseStartup<Startup>()
				.UseEnvironment("Development")
				.ConfigureServices(sc =>
				{
					// % protected region % [Configure services here] off begin
					sc.AddDbContext<Lm2348DBContext>(builderOptions.DatabaseOptions(builderOptions));
					sc.AddScoped<IHttpContextAccessor>(_ => new HttpContextAccessor
					{
						HttpContext = httpContext
					});
					builderOptions.ConfigureServices?.Invoke(sc, builderOptions);
					// % protected region % [Configure services here] end
				})
				.Build();

			if (builderOptions.InitialiseData)
			{
				var dataSeed = host.Services.GetRequiredService<DataSeedHelper>();
				dataSeed.Initialize();
			}

			// % protected region % [Add any additional actions after build here] off begin
			// % protected region % [Add any additional actions after build here] end

			return host;
		}

		/// <summary>
		/// Creates a claims principal for a user
		/// </summary>
		/// <param name="userId">The id of the user</param>
		/// <param name="userName">The username of the user</param>
		/// <param name="email">The email of the user</param>
		/// <param name="roles">The groups that the user is in</param>
		/// <returns>A claims principal to represent this information</returns>
		public static ClaimsPrincipal CreateUserPrincipal(Guid userId, string userName, string email, IEnumerable<string> roles)
		{
			var identity = new ClaimsIdentity(
				CookieAuthenticationDefaults.AuthenticationScheme,
				OpenIdConnectConstants.Claims.Name,
				OpenIdConnectConstants.Claims.Role);
			identity.AddClaim(new Claim("UserId", userId.ToString()));
			identity.AddClaim(new Claim(OpenIdConnectConstants.Claims.Subject, userName));
			identity.AddClaim(new Claim(OpenIdConnectConstants.Claims.Name, email));
			identity.AddClaims(roles.Select(r => new Claim(ClaimTypes.Role, r)));

			// % protected region % [Add any additional claims here] off begin
			// % protected region % [Add any additional claims here] end

			return new ClaimsPrincipal(identity);
		}
	}
}