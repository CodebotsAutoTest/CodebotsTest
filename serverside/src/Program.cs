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
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Lm2348
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

			// % protected region % [Customise swagger cli generation here] off begin
			if (args.Length > 0 && args[0] == "swagger")
			{
				Console.WriteLine(GenerateSwagger(args));
				return;
			}
			// % protected region % [Customise swagger cli generation here] end

			try
			{
				// % protected region % [Customise web host creation here] off begin
				Log.Information("Starting web host");
				CreateWebHostBuilder(args).Build().Run();
				// % protected region % [Customise web host creation here] end
			}
			catch (Exception ex)
			{
				// % protected region % [Customise program catch logging here] off begin
				Log.Fatal(ex, "Host terminated unexpectedly");
				throw;
				// % protected region % [Customise program catch logging here] end
			}
			finally
			{
				// % protected region % [Customise program cleanup here] off begin
				Log.CloseAndFlush();
				// % protected region % [Customise program cleanup here] end
			}
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration((builderContext, config) =>
				{
					// % protected region % [Configure environment settings here] off begin
					var env = builderContext.HostingEnvironment;

					config.SetBasePath(env.ContentRootPath);
					config.AddXmlFile("appsettings.xml", optional: false, reloadOnChange: true);
					config.AddXmlFile($"appsettings.{env.EnvironmentName}.xml", optional: true, reloadOnChange: true);
					config.AddEnvironmentVariables();
					config.AddEnvironmentVariables("Lm2348_");
					config.AddEnvironmentVariables($"Lm2348_{env.EnvironmentName}_");
					config.AddCommandLine(args);
					// % protected region % [Configure environment settings here] end
				})
				.UseSerilog()
				// % protected region % [Add any further web host configuration here] off begin
				// % protected region % [Add any further web host configuration here] end
				// % protected region % [Change Startup to custom Startup class here] off begin
				.UseStartup<Startup>();
				// % protected region % [Change Startup to custom Startup class here] end

		private static string GenerateSwagger(string[] args)
		{
			// % protected region % [Customise GenerateSwagger here] off begin
			var host = CreateWebHostBuilder(args.Skip(1).ToArray()).Build();
			var sw = (ISwaggerProvider)host.Services.GetService(typeof(ISwaggerProvider));
			var doc = sw.GetSwagger("json", null, "/");
			return JsonConvert.SerializeObject(
				doc,
				Formatting.Indented,
				new JsonSerializerSettings
				{
					NullValueHandling = NullValueHandling.Ignore,
					ContractResolver = new DefaultContractResolver()
				}
			);
			// % protected region % [Customise GenerateSwagger here] end
		}

		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	}
}
