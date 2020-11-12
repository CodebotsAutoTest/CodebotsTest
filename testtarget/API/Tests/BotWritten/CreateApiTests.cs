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
using System.Net;
using APITests.EntityObjects.Models;
using APITests.Factories;
using APITests.Setup;
using APITests.Utils;
using APITests.TheoryData.BotWritten;
using EntityObject.Enums;
using RestSharp;
using Xunit;
using Xunit.Abstractions;

namespace APITests.Tests.BotWritten
{

	public class CreateApiTests : IClassFixture<StartupTestFixture>
	{
		private readonly StartupTestFixture _configure;
		private readonly ITestOutputHelper _output;

		public CreateApiTests(StartupTestFixture configure, ITestOutputHelper output)
		{
			_configure = configure;
			_output = output;
		}

		// % protected region % [Customize Create Entity tests here] off begin
		[Theory]
		[Trait("Category", "BotWritten")]
		[Trait("Category", "Integration")]
		[ClassData(typeof(EntityFactorySingleTheoryData))]
		[ClassData(typeof(EntityFactoryMultipleTheoryData))]
		public List<BaseEntity> CreateEntities(EntityFactory entityFactory, int numEntities)
		{
			// get the list of entities we will be creating
			var entityList = CreateReferencedEntities(entityFactory, numEntities);

			var api = new WebApi(_configure, _output);

			/*
			 * get the first entity out of the list, we will be iterating over this one and creating the references needed
			 * for it. This means that all the created entities share the same references
			 */
			var query = QueryBuilder.CreateEntityQueryBuilder(entityList);
			api.ConfigureAuthenticationHeaders();

			var response = api.Post($"/api/graphql", query);
			

			//valid ids returned and a valid response
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);

			//return the object at the end, this can be reused.
			return entityList;
		}
		// % protected region % [Customize Create Entity tests here] end


		// % protected region % [Customize Create Referenced Entity tests here] off begin
		private List<BaseEntity> CreateReferencedEntities(EntityFactory entityFactory, int numEntities)
		{
			// get the list of entities we will be creating
			var entityList = entityFactory.Construct(numEntities);

			//iterate overall of our references and recursively create them.
			foreach (var entity in entityList)
			{
				foreach (var reference in entity.References)
				{
					var referenceIdName = reference.OppositeName + "Id";

					// check if the reference has already been filled out
					if (!entity.ReferenceIdDictionary.ContainsKey(referenceIdName))
					{
						//ignore self references
						if (!reference.Optional)
						{
							var createdEntity =
								CreateEntities(new EntityFactory(reference.EntityName, entityFactory.GetFixedString()),
									1);

							var referenceDictionary = new Dictionary<string, ICollection<Guid>>()
							{
								{
									referenceIdName, new List<Guid> {createdEntity[0].Id}
								}
							};
							//add the created entity to our dictionary for the child entity for all entities in the list
							if (reference.Type == ReferenceType.ONE && reference.OppositeType == ReferenceType.ONE)
							{
								// if one-to-one each reference must be unique
								entityList.FirstOrDefault(x => x == entity).SetReferences(referenceDictionary);
								entityList.FirstOrDefault(x => x == entity).ReferenceIdDictionary[referenceIdName] =
									createdEntity[0].Id;
								entityList.FirstOrDefault(x => x == entity).ParentEntities.Add(createdEntity[0]);
							}
							else
							{
								// if one-to-many or many-to-many entities can share a reference.
								entityList.ForEach(x => x.SetReferences(referenceDictionary));
								entityList.ForEach(x => x.ReferenceIdDictionary[referenceIdName] = createdEntity[0].Id);
								entityList.ForEach(x => x.ParentEntities.Add(createdEntity[0]));
							}
						}
					}
				}
			}
			return entityList;
		}
		// % protected region % [Customize Create Referenced Entity tests here] end
	}
}