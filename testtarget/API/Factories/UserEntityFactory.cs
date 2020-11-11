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
using APITests.EntityObjects.Models;
using Xunit.Abstractions;

namespace APITests.Factories
{
	public class UserEntityFactory : IXunitSerializable
	{
		private string _type;
		private readonly string _fixedStrValues;

		public UserEntityFactory(string type, string fixedStrValues = null)
		{
			_type = type;
			_fixedStrValues = fixedStrValues;
		}

		public UserEntityFactory()
		{

		}

		public UserBaseEntity Construct(bool isValid = true)
		{
			switch (_type)
			{
				default:
					throw new Exception($"Cannot find entity type {_type}");
			}
		}

		public List<UserBaseEntity> Construct(int numEntities)
		{
			var entityList = new List<UserBaseEntity>(numEntities);
			for (var i = 0; i < numEntities; i++)
			{
				entityList.Add(Construct());
			}
			return entityList;
		}

		public List<UserBaseEntity> ConstructAndSave(ITestOutputHelper output, int numEntities)
		{
			var entityList = new List<UserBaseEntity>();
			var options = _fixedStrValues == null ? BaseEntity.ConfigureOptions.CREATE_ATTRIBUTES_AND_REFERENCES : BaseEntity.ConfigureOptions.CREATE_REFERENCES_ONLY;

			for (var i = 0; i < numEntities; i++)
			{
				var entity = Construct();
				entity.Configure(options);
				entity.Save();
				output.WriteLine($"Database Saved Entity:\n{entity.EntityName}:\n{entity.ToJson()}\n");
				entityList.Add(entity);
			}
			return entityList;
		}

		public UserBaseEntity ConstructAndSave(ITestOutputHelper output) => ConstructAndSave(output, 1)[0];

		public void Deserialize(IXunitSerializationInfo info) => _type = info.GetValue<string>("type");

		public void Serialize(IXunitSerializationInfo info) => info.AddValue("type", _type, typeof(string));

		public override string ToString() => $"Type = {_type}";

		public string GetFixedString() => _fixedStrValues;

		public string GetEnumValue(UserBaseEntity entity, string enumColumnName)
		{
			switch (_type)
			{
				default:
					return null;
			}
		}
	}
}