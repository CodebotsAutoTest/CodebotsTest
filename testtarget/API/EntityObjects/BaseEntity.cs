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
using APITests.Setup;
using APITests.Utils;
using EntityObject.Enums;
using Microsoft.EntityFrameworkCore;
using Lm2348DBContext = Lm2348.Models.Lm2348DBContext;
using RestSharp;
// % protected region % [Custom imports] off begin
// % protected region % [Custom imports] end

namespace APITests.EntityObjects.Models
{
	public class Reference
	{
		public string Name { get; set; }
		public string OppositeName { get; set; }
		public string EntityName { get; set; }
		public bool Optional { get; set; }
		public ReferenceType Type { get; set; }
		public ReferenceType OppositeType { get; set; }
	}

	public class Attribute
	{
		public string Name { get; set; }
		public bool IsRequired { get; set;}
	}

	public abstract class BaseEntity
	{
		public abstract void Configure(ConfigureOptions option);
		public abstract Guid Save();
		public abstract Dictionary<string, string> ToDictionary();
		public abstract List<Guid> GetManyToManyReferences (string reference);
		public abstract void SetReferences (Dictionary<string, ICollection<Guid>> entityReferences);
		public abstract string GetInvalidAttribute(string attribute, string validator);
		public abstract RestSharp.JsonObject ToJson();
		public abstract (int min, int max) GetLengthValidatorMinMax(string attribute);
		public abstract ICollection<(List<string> expectedErrors, RestSharp.JsonObject jsonObject)> GetInvalidMutatedJsons();
		public ICollection<Reference> References = new List<Reference>();
		public ICollection<Attribute> Attributes = new List<Attribute>();
		public ICollection<BaseEntity> ParentEntities = new List<BaseEntity>();
		public Guid Id = Guid.NewGuid();
		public DateTime Created = DateTime.Now;
		public DateTime Modified = DateTime.Now;
		public Dictionary<string, Guid?> ReferenceIdDictionary { get; set;} = new Dictionary<string, Guid?>();
		public string EntityName { get; set; }
		public virtual bool HasFile { get; set; } = false;
		private readonly StartupTestFixture _configure = new StartupTestFixture();

		internal Guid SaveThroughGraphQl(BaseEntity model)
		{
			var api = new WebApi(_configure);
			var query = QueryBuilder.CreateEntityQueryBuilder(new List<BaseEntity>{model});
			api.ConfigureAuthenticationHeaders();
			
			if (model is IFileContainingEntity fileContainingEntity)
			{
				var headers = new Dictionary<string, string>{{"Content-Type", "multipart/form-data"}};
				var files = fileContainingEntity.GetFiles().Where(file => file != null);;
				var param = new Dictionary<string, object>
				{
					{"operationName", query["operationName"]},
					{"variables", query["variables"]},
					{"query", query["query"]}
				};
				
				api.Post($"/api/graphql", param, headers, DataFormat.None, files);
				return Id;
			}
			api.Post($"/api/graphql", query);
			return Id;
		}

		internal Guid SaveToDB<T>(T model) where T : class, Lm2348.Models.IOwnerAbstractModel
		{
			var configure = new StartupTestFixture();
			// % protected region % [Adjust the db context if required] off begin
			var context = new Lm2348DBContext(configure.DbContextOptions, null, null);
			// % protected region % [Adjust the db context if required] end
			model.Owner = configure.SuperOwnerId;
			var dbSet = context.GetDbSet<T>(typeof(T).Name);
			dbSet.Update(model);
			var addedEntry = context
				.ChangeTracker
				.Entries()
				.First(entry => model.Equals(entry.Entity));

			addedEntry.State = EntityState.Added;
			context.SaveChanges();
			context.Dispose();
			return model.Id;
		}

		public enum ConfigureOptions
		{
			CREATE_ATTRIBUTES_AND_REFERENCES,
			CREATE_ATTRIBUTES_ONLY,
			CREATE_REFERENCES_ONLY,
			CREATE_INVALID_ATTRIBUTES,
			CREATE_INVALID_ATTRIBUTES_VALID_REFERENCES
		}
	}
}