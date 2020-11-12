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
using System.Net;
using APITests.Setup;
using APITests.TheoryData.BotWritten;
using APITests.Utils;
using Newtonsoft.Json.Linq;
using RestSharp;
using Xunit;
using Xunit.Abstractions;

namespace APITests.Tests.BotWritten
{
	[Trait("Category", "BotWritten")]
	[Trait("Category", "Integration")]
	public class ApiTests : IClassFixture<StartupTestFixture>
	{
		private readonly StartupTestFixture _configure;
		private readonly ITestOutputHelper _output;

		public ApiTests(StartupTestFixture configure, ITestOutputHelper output)
		{
			_configure = configure;
			_output = output;
		}

		// % protected region % [Customize GraphQl endpoint tests here] off begin
		[Theory]
		[ClassData(typeof(EntityNamePluralizedTheoryData))]
		public void TestGraphqlEndPoints(string entityName)
		{

			var api = new WebApi(_configure, _output);

			var query = new RestSharp.JsonObject();
			query.Add("query", "{ " + entityName + "{id}}");

			api.ConfigureAuthenticationHeaders();
			var response = api.Post($"/api/graphql", query);

			//check the ids are valid
			var validIds = JObject.Parse(response.Content)["data"][entityName]
				.Select(o => o["id"].Value<string>())
				.All(o => !string.IsNullOrWhiteSpace(o));

			//valid ids returned and a valid response
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		}
		// % protected region % [Customize GraphQl endpoint tests here] end

		// % protected region % [Customize Api endpoint tests here] off begin
		[Theory]
		[ClassData(typeof(EntityNameTheoryData))]
		public void TestApiEndPoints(string entityName)
		{
			var api = new WebApi(_configure, _output);
			api.ConfigureAuthenticationHeaders();
			var response = api.Get($"/api/entity/{entityName}");

			// a valid response code
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		}
		// % protected region % [Customize Api endpoint tests here] end
	}
}