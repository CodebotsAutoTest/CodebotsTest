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
using System.Globalization;
using System.IO;
using System.Linq;
using APITests.Setup;
using APITests.TheoryData.BotWritten;
using APITests.Utils;
using APITests.EntityObjects.Models;
using APITests.Factories;
using CsvHelper;
using FluentAssertions;
using RestSharp;
using Xunit;
using Xunit.Abstractions;

// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace APITests.Tests.BotWritten
{
	[Trait("Category", "BotWritten")]
	[Trait("Category", "Integration")]
	public class ExportTests : IClassFixture<StartupTestFixture>
	{
		private readonly StartupTestFixture _configure;
		private readonly ITestOutputHelper _output;

		public ExportTests(StartupTestFixture configure, ITestOutputHelper output)
		{
			_configure = configure;
			_output = output;
		}

		// % protected region % [Customize Export Entity tests here] off begin
		[Theory]
		[ClassData(typeof(EntityFactorySingleTheoryData))]
		[ClassData(typeof(EntityFactoryMultipleTheoryData))]
		public void ExportEntity(EntityFactory entityFactory, int numEntities)
		{
			var entityList = entityFactory.ConstructAndSave(_output, numEntities);
			var entityName = entityList[0].EntityName;

			var api = new WebApi(_configure, _output);


			var query = QueryBuilder.CreateExportQuery(entityList);
			var queryList = new JsonArray { new JsonArray { query } };
			api.ConfigureAuthenticationHeaders();
			var response = api.Post($"/api/entity/{entityName}/export", queryList);

			var responseDictionary = CsvToDictionary(response.Content)
				.ToDictionary(pair => pair.Key.ToLowerInvariant(), pair => pair.Value);

			foreach (var entity in entityList)
			{
				var entityDict = entity.ToDictionary()
					.ToDictionary(pair => pair.Key.ToLowerInvariant(), pair => pair.Value);

				if (entity is UserBaseEntity)
				{
					// export will not contain password
					entityDict.Remove("password");
				}


				foreach (var attributeKey in entityDict.Keys.Select(x => x.ToLowerInvariant()))
				{
					responseDictionary.Should().ContainKey(attributeKey);
					responseDictionary[attributeKey]
						.Should()
						.Contain(entityDict[attributeKey])
						.And
						.HaveCount(numEntities);
				}
			}
		}
		// % protected region % [Customize Export Entity tests here] end


		// % protected region % [Customize CsvToDictionary logic here] off begin
		private static Dictionary<string, List<string>> CsvToDictionary(string csv)
		{
			var entityDictionary = new Dictionary<string, List<string>>();

			using var stringReader = new StringReader(csv);
			using var reader = new CsvReader(stringReader, CultureInfo.InvariantCulture);

			foreach (var record in reader.GetRecords<dynamic>())
			{
				if (record is IDictionary<string, object> recordDictionary)
				{
					foreach (var key in recordDictionary.Keys)
					{
						if (!entityDictionary.ContainsKey(key))
						{
							entityDictionary[key] = new List<string>();
						}
						entityDictionary[key].Add((string)recordDictionary[key]);
					}
				}
			}
			return entityDictionary;
		}
		// % protected region % [Customize CsvToDictionary logic here] end

	}
}