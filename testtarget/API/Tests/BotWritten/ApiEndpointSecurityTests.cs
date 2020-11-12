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
using System.Net;
using APITests.Setup;
using APITests.TheoryData.BotWritten;
using APITests.Utils;
using RestSharp;
using Xunit;
using Xunit.Abstractions;

namespace APITests.Tests.BotWritten
{
	[Trait("Category", "BotWritten")]
	[Trait("Category", "Integration")]
	public class ApiSecurityTests : IClassFixture<StartupTestFixture>
	{
		private readonly StartupTestFixture _configure;
		private readonly ITestOutputHelper _output;

		public ApiSecurityTests(StartupTestFixture configure, ITestOutputHelper output)
		{
			_configure = configure;
			_output = output;
		}

		/// <summary>
		/// Tests that unauthorized users can access the graphql endpoints.
		/// Should receive a  a HTPP Status Code OK.
		/// </summary>
		/// <param name="entityName"></param>
		[Theory]
		[ClassData(typeof(EntityNamePluralizedTheoryData))]
		public void TestGraphqlEndPointsUnauthorized(string entityName)
		{
			var api = new WebApi(_configure, _output);

			var query = new RestSharp.JsonObject();
			query.Add("query", "{ " + entityName + "{id}}");
			var response = api.Post($"/api/graphql", query);

			// we should get a valid response back
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		}

		[Theory]
		[ClassData(typeof(VisitorUnauthorisedEntityNameTheoryData))]
		public void TestApiEndPointsUnauthorized(string entityName)
		{
			var api = new WebApi(_configure, _output);
			var response = api.Get($"/api/entity/{entityName}");

			// valid response code
			Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
		}
	}
}