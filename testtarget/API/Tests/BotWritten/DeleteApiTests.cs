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
using System.Net;
using APITests.Factories;
using APITests.EntityObjects.Models;
using APITests.Setup;
using APITests.Utils;
using APITests.TheoryData.BotWritten;
using RestSharp;
using Xunit;
using Xunit.Abstractions;

// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace APITests.Tests.BotWritten
{
	[Trait("Category", "BotWritten")]
	[Trait("Category", "Integration")]
	public class DeleteApiTests : IClassFixture<StartupTestFixture>
	{
		private readonly StartupTestFixture _configure;
		private readonly CreateApiTests _createApiTests;
		private readonly ITestOutputHelper _output;

		public DeleteApiTests(StartupTestFixture configure, ITestOutputHelper output)
		{
			_configure = configure;
			_output = output;
			_createApiTests = new CreateApiTests(new StartupTestFixture(), _output);
		}

		#region GraphQl delete
		[Theory]
		[ClassData(typeof(EntityFactorySingleTheoryData))]
		[ClassData(typeof(EntityFactoryMultipleTheoryData))]
		public void GraphqlDeleteEntities(EntityFactory entityFactory, int numEntities)
		{
			var entityList = entityFactory.ConstructAndSave(_output, numEntities);
			GraphQlDelete(entityList);
		}


		// % protected region % [Customize Graphql Delete tests here] off begin
		/// <summary>
		/// This function is called recursively to delete
		/// entities, it is abstracted away from the creation tests
		/// so it can run independently
		/// </summary>
		/// <param name="entityList">
		/// Takes a list of entities as input, these entities are delete and their
		/// parent entities are also deleted.
		/// </param>
		internal void GraphQlDelete(List<BaseEntity> entityList)
		{
			var api = new WebApi(_configure, _output);

			// form the query to delete the entity
			// mass delete all entities in the list in a single request and check status code is ok
			var deleteQuery = QueryBuilder.DeleteEntityQueryBuilder(entityList);
			api.ConfigureAuthenticationHeaders();
			api.Post($"/api/graphql", deleteQuery);

			/*
			 * Run recursively for parent entities,
			 * The first item is passed through because each entity in
			 * the list share the same parent references
			 */
			foreach (var parentEntity in entityList[0].ParentEntities)
			{
				GraphQlDelete(new List<BaseEntity>() { parentEntity });
			}
		}
		// % protected region % [Customize Graphql Delete tests here] end
		#endregion

		#region Rest Endpoint Delete
		// % protected region % [Customize Api Delete Entity tests here] off begin
		[Theory]
		[ClassData(typeof(EntityFactoryTheoryData))]
		public void ApiDeleteEntities(EntityFactory entityFactory)
		{
			// create some test entities over the api to run the delete tests against
			var entityList = entityFactory.ConstructAndSave(_output, 1);

			foreach (var entityObject in entityList)
			{
				// instantiate a list of entity names and guids to be deleted
				var entityKeysGuids = new List<KeyValuePair<string, Guid>>();
				entityKeysGuids.Add(new KeyValuePair<string, Guid>(entityObject.EntityName, entityObject.Id));

				// populate the list using information returned from create entities.
				GetParentKeysGuids(entityObject).ForEach(x => entityKeysGuids.Add(x));

				foreach (var entityKeyGuid in entityKeysGuids)
				{
					// instantiate a new rest client, and configure the Uri

					var endPoint = $"/api/entity/{entityKeyGuid.Key}/{entityKeyGuid.Value}";
					var api = new WebApi(_configure, _output);
					api.ConfigureAuthenticationHeaders();

					Assert.Equal(HttpStatusCode.OK, api.Get(endPoint).StatusCode);
					Assert.Equal(HttpStatusCode.OK, api.Delete(endPoint).StatusCode);
					Assert.Equal(HttpStatusCode.NoContent, api.Get(endPoint).StatusCode);
					Assert.Equal(HttpStatusCode.OK, api.Delete(endPoint).StatusCode);
				}
			}
		}
		// % protected region % [Customize Api Delete Entity tests here] end
		#endregion

		internal List<KeyValuePair<string, Guid>> GetParentKeysGuids(BaseEntity entityObject)
		{
			var entityKeysGuids = new List<KeyValuePair<string, Guid>>();

			foreach (var parentEntity in entityObject.ParentEntities)
			{
				if (parentEntity.EntityName != entityObject.EntityName)
				{
					entityKeysGuids.Add(new KeyValuePair<string, Guid>(parentEntity.EntityName, parentEntity.Id));
					GetParentKeysGuids(parentEntity).ForEach(x => entityKeysGuids.Add(x));
				}
			}
			return entityKeysGuids;
		}

		// % protected region % [Add any further methods here] off begin
		// % protected region % [Add any further methods here] end
	}
}