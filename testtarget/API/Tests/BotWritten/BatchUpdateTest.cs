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
using APITests.Factories;
using RestSharp;
using Xunit;
using Xunit.Abstractions;

namespace APITests.Tests.BotWritten
{
	public class BatchUpdateTest :  IClassFixture<StartupTestFixture>
	{
		private readonly StartupTestFixture _configure;
		private readonly ITestOutputHelper _output;
		private static CreateApiTests _createApiTests;

		public BatchUpdateTest(StartupTestFixture configure, ITestOutputHelper output)
		{
			_configure = configure;
			_output = output;
			_createApiTests = new CreateApiTests(new StartupTestFixture(), _output);
		}

		#region GraphQl Batch Update
		// % protected region % [Customize GraphqlBatchUpdateEntities here] off begin
		[SkippableTheory]
		[Trait("Category", "BotWritten")]
		[Trait("Category", "Integration")]
		[ClassData(typeof(EntityFactoryMultipleTheoryData))]
		public void GraphqlBatchUpdateEntities(EntityFactory entityFactory, int numEntities)
		{
			var entity = entityFactory.Construct();
			var entityProperties = entity.GetType().GetProperties();

			if (entityProperties.Any(x => x.PropertyType.IsEnum) || entity.HasFile)
			{
				throw new SkipException("Batch update is currently not supported on entities with enum or file");
			}

			var entityList = entityFactory.ConstructAndSave(_output, numEntities);

			//setup the rest client
			var client = new RestClient
			{
				BaseUrl = new Uri($"{_configure.BaseUrl}/api/graphql")
			};

			//setup the request
			var request = new RestRequest
			{
				Method = Method.POST,
				RequestFormat = DataFormat.Json
			};

			//get the authorization token and adds the token to the request
			var loginToken = new LoginToken(_configure.BaseUrl, _configure.SuperUsername, _configure.SuperPassword);
			var authorizationToken = $"{loginToken.TokenType} {loginToken.AccessToken}";
			request.AddHeader("Authorization", authorizationToken);

			request.AddHeader("Content-Type", "application/json");
			request.AddHeader("Accept", "*\\*");

			// form the query to update the entity
			var updateQuery = QueryBuilder.BatchUpdateEntityQueryBuilder(entityList);
			request.AddParameter("text/json", updateQuery, ParameterType.RequestBody);

			// mass update all entities in the list in a single request and check status code is ok
			RequestHelpers.ValidateResponse(client, Method.POST, request, HttpStatusCode.OK);
		}
		// % protected region % [Customize GraphqlBatchUpdateEntities here] end
		#endregion
	}
}