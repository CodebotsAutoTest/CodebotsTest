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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using APITests.EntityObjects.Models;
using Lm2348.Helpers;
using APITests.Factories;
using EntityObject.Enums;
using RestSharp;

namespace APITests.Utils
{
	public static class QueryBuilder
	{
		private static string ConstructFieldQueryPart(BaseEntity baseEntity)
		{
			var attributes = baseEntity.Attributes.Select(x => x.Name.LowerCaseFirst()).ToList();
			var references = baseEntity.References.Select(x => x.OppositeName.LowerCaseFirst()+ "Id").ToList();
			var propertyNames = attributes.Concat(references).ToList();
			var manyReferences = baseEntity.References.Where(x => x.Type == ReferenceType.MANY);

			if (propertyNames.Contains("entityName"))
			{
				propertyNames.Remove("entityName");
			}

			// Remove many to many entity references
			// TODO: THIS IS A BANDAID- MUST BE FIXED DOES NOT CORRECTLY HANDLE ENTITY ASSOCIATIONS.
			foreach (var manyReference in manyReferences)
			{
				var manyReferenceEntityName = manyReference.OppositeName.LowerCaseFirst() + "Id";
				if (propertyNames.Contains(manyReferenceEntityName))
				{
					propertyNames.Remove(manyReferenceEntityName);
				}
			}

			propertyNames.Add("id");

			//for each of the key value pairs, get the key name and join by commas
			return string.Join(',', propertyNames);
		}

		private static RestSharp.JsonObject ConstructQuery(BaseEntity entity ,ArrayList myAL)
		{
			var modelType = entity.GetType();

			var entityVar = new RestSharp.JsonObject
			{
				{ $"{modelType.Name.LowerCaseFirst()}", myAL }
			};

			var fieldQueryPart = ConstructFieldQueryPart(entity);
			var creationType = entity is UserBaseEntity ? "CreateInput" : "Input";
			var queryPart = $"mutation create{modelType.Name} (${modelType.Name.LowerCaseFirst()}: [{modelType.Name}{creationType}]) {{ create{modelType.Name}({modelType.Name.LowerCaseFirst()}s: ${modelType.Name.LowerCaseFirst()}){{ {fieldQueryPart} }} }}";

			var query = new RestSharp.JsonObject
			{
				{ "operationName", $"create{modelType.Name}" },
				{ "variables", entityVar },
				{ "query", queryPart }
			};

			return query;
		}

		/// <summary>
		/// Query builder for multiple entities
		/// </summary>
		/// <param name="baseEntities"></param>
		/// <returns>The graphql query as a json object</returns>
		public static RestSharp.JsonObject CreateEntityQueryBuilder(List<BaseEntity> baseEntities)
		{
			var entityList = new ArrayList();
			foreach (var entity in baseEntities)
			{
				var objectParams = entity.ToJson();

				//this will update all of our foreign reference ids for our parent objects
				foreach (var reference in entity.ReferenceIdDictionary)
				{
					if (entity.References.First(x => x.OppositeName + "Id" == reference.Key).Type == ReferenceType.MANY)
					{
						var referenceEntity = entity.References
							.First(x => x.OppositeName + "Id" == reference.Key)
							.OppositeName
							.LowerCaseFirst();

						var manyToManyList = new List<RestSharp.JsonObject>
						{
							new RestSharp.JsonObject { [reference.Key.LowerCaseFirst()] = reference.Value }
						};
						objectParams[referenceEntity + "s"] = manyToManyList;
					}
					else
					{
						objectParams[reference.Key.LowerCaseFirst()] = reference.Value;
					}
				}
				entityList.Add(objectParams);
			}
			return ConstructQuery(baseEntities[0], entityList);
		}

		/// <summary>
		/// Builds a graphql query to create invalid entities
		/// </summary>
		/// <param name="baseEntities"></param>
		/// <returns>The graphql query as a json object</returns>
		public static RestSharp.JsonObject InvalidEntityQueryBuilder(BaseEntity entity, List<RestSharp.JsonObject> invalidEntityJsons)
		{
			var myAL = new ArrayList();

			foreach (var invalidEntityJson in invalidEntityJsons)
			{
				//this will update all of our foreign reference ids for our parent objects
				foreach (var reference in entity.ReferenceIdDictionary)
				{

					// Set foreign-key for non null keys
					if (invalidEntityJson.Keys.Contains(reference.Key.LowerCaseFirst()))
					{
						// Handle many -> many relations
						var manyReference = entity.References
							.FirstOrDefault(r => r.OppositeName + 's' == reference.Key);

						if (manyReference != null)
						{
							invalidEntityJson[reference.Key.LowerCaseFirst()] = new List<RestSharp.JsonObject>
							{
								new RestSharp.JsonObject
								{
									[manyReference.OppositeName.LowerCaseFirst() + "Id"] = reference.Value
								}
							};
						}
						// Handle one -> many and one -> one
						else
						{
							invalidEntityJson[reference.Key.LowerCaseFirst()] = reference.Value;
						}
					}
				}
				myAL.Add(invalidEntityJson);
			}

			return ConstructQuery(entity, myAL);
		}

		/// <summary>
		/// Query builder for deleting an entity
		/// </summary>
		/// <param name="entityKeyGuid"></param>
		/// <returns>The graphql query as a json object</returns>
		public static RestSharp.JsonObject DeleteEntityQueryBuilder(List<BaseEntity> entityList)
		{
			var entityName = entityList[0].EntityName;
			var guids = new List<Guid>();
			entityList.ForEach(x => guids.Add(x.Id));

			var entityVar = new RestSharp.JsonObject();
			entityVar.Add($"{entityName.LowerCaseFirst()}Ids", guids.ToArray());

			var queryPart = $@"mutation delete (${entityList[0].EntityName.LowerCaseFirst()}Ids: [ID])
				{{ delete{entityName}({entityName.LowerCaseFirst()}Ids:
				${entityName.LowerCaseFirst()}Ids){{ id __typename }} }}";

			var query = new RestSharp.JsonObject
			{
				{ "operationName", "delete" },
				{ "variables", entityVar },
				{ "query", queryPart }
			};

			return query;
		}

		/// <summary>
		///	Query builder for batch updating of an entity
		///	Takes a list of BaseEntities to update.
		/// Generates new attribute values for all entities to be updated to,
		/// and returns a GraphQL query to perform the update with.
		/// </summary>
		/// <param name="baseEntities">The list of BaseEntities to be updated</param>
		/// <returns>The graphql query as a json object</returns>
		public static RestSharp.JsonObject BatchUpdateEntityQueryBuilder(List<BaseEntity> baseEntities)
		{
			string entityName = baseEntities[0].EntityName;
			var attributeNames = baseEntities[0].Attributes.Select(x => x.Name).ToList();
			var newAttributes = new EntityFactory(entityName).Construct().ToJson();
			var valuesToUpdate = new RestSharp.JsonObject();
			attributeNames.ForEach(x => valuesToUpdate.Add(x.LowerCaseFirst(), $"{newAttributes[x.LowerCaseFirst()]}"));

			// Build the entity variables part
			var entityVar = new RestSharp.JsonObject
			{
				{ $"idsToUpdate",  baseEntities.Select(x => x.Id).ToArray()},
				{ "valuesToUpdate", valuesToUpdate},
				{ "fieldsToUpdate", attributeNames.ToArray()},
			};

			// Build the graphQL Query part
			var queryPart = $@"mutation update{entityName}sConditional (
				$valuesToUpdate: {entityName}Input,
				$fieldsToUpdate: [String],
				$idsToUpdate:[ID])
				{{update{entityName}sConditional(
					ids: $idsToUpdate,
					valuesToUpdate: $valuesToUpdate,
					fieldsToUpdate: $fieldsToUpdate){{
						value
					}}
				}}";

			// Constructing query string
			RestSharp.JsonObject query = new RestSharp.JsonObject
			{
				{ "operationName", $"update{entityName}sConditional" },
				{ "variables", entityVar },
				{ "query", queryPart }
			};
			return query;
		}

		public static RestSharp.JsonObject CreateExportQuery(List<BaseEntity> entityList)
		{
			var entityIds = new List<string>() { };
			entityList.ForEach(x => entityIds.Add(x.Id.ToString()));

			return new RestSharp.JsonObject
			{
				["path"] = "id",
				["comparison"] = "in",
				["value"] = entityIds
			};
		}
	}
}