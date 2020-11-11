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
using System.Linq.Expressions;
using System.Threading.Tasks;
using Lm2348.Graphql.Helpers;
using Lm2348.Helpers;
using Lm2348.Models;
using Lm2348.Services;
using Lm2348.Utility;
using GraphQL;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace Lm2348.Graphql.Fields
{
	public class UpdateMutation
	{
		/// <summary>
		/// Creates a mutation that will update entities in the database
		/// </summary>
		/// <param name="name">The name of the model to update</param>
		/// <typeparam name="TModel">The type of the model to update</typeparam>
		/// <returns>A function that takes a graphql context and returns a list of the updated models</returns>
		public static Func<ResolveFieldContext<object>, Task<object>> CreateUpdateMutation<TModel>(string name)
			where TModel : class, IOwnerAbstractModel, new()
		{
			// % protected region % [Override CreateUpdateMutation here] off begin
			return async context =>
			{
				var graphQlContext = (Lm2348GraphQlContext) context.UserContext;
				var crudService = graphQlContext.CrudService;
				var models = context.GetArgument<List<TModel>>(name.ToCamelCase() + "s");
				var mergeReferences = context.GetArgument<List<string>>("mergeReferences");

				try
				{
					if (models == null)
					{
						throw new AggregateException(new Exception("No entities provided to save, aborting!"));
					}

					return await crudService.Update(models, new UpdateOptions
					{
						MergeReferences = mergeReferences,
						Files = graphQlContext.Files,
					});
				}
				catch (AggregateException exception)
				{
					context.Errors.AddRange(exception.InnerExceptions.Select(error => new ExecutionError(error.Message)));
					return new List<TModel>();
				}
			};
			// % protected region % [Override CreateUpdateMutation here] end
		}

		/// <summary>
		/// Creates a mutation that will update things from the database by a where condition
		/// </summary>
		/// <param name="name">The name of the model to update</param>
		/// <typeparam name="TModel">The type of the model to update</typeparam>
		/// <returns>A function that takes a graphql context and returns whether the delte is successful</returns>
		public static Func<ResolveFieldContext<object>, Task<object>> CreateConditionalUpdateMutation<TModel>(string name)
			where TModel : class, IOwnerAbstractModel, new()
		{
			// % protected region % [Override CreateConditionalUpdateMutation here] off begin
			return async context =>
			{
				var graphQlContext = (Lm2348GraphQlContext)context.UserContext;
				var crudService = graphQlContext.CrudService;
				var user = graphQlContext.User;
				var dbSet = graphQlContext.DbContext.Set<TModel>().AsQueryable();

				var models = QueryHelpers.CreateConditionalWhere(context, dbSet);
				models = QueryHelpers.CreateIdsCondition(context, models);
				models = QueryHelpers.CreateIdCondition(context, models);
				var fieldsToUpdate = context.GetArgument<List<string>>("fieldsToUpdate");
				var valuesToUpdate = context.GetArgument<TModel>("valuesToUpdate");

				var createObject = Expression.New(typeof(TModel));

				var fields = new List<MemberBinding>();
				foreach (string field in fieldsToUpdate)
				{
					var modelType = valuesToUpdate.GetType();
					var prop = modelType.GetProperty(field.ConvertToPascalCase());

					object value;
					try
					{
						value = prop.GetValue(valuesToUpdate);
					}
					catch (NullReferenceException)
					{
						throw new ArgumentException($"Property {field} does not exist in the entity");
					}

					var target = Expression.Constant(value, prop.PropertyType);

					fields.Add(Expression.Bind(prop, target));
				}
				var initializePropertiesOnObject = Expression.MemberInit(
					createObject,
					fields);

				try
				{
					return await crudService.ConditionalUpdate(models, initializePropertiesOnObject);
				}
				catch (AggregateException exception)
				{
					context.Errors.AddRange(
						exception.InnerExceptions.Select(error => new ExecutionError(error.Message)));
					return false;
				}
			};
			// % protected region % [Override CreateConditionalUpdateMutation here] end
		}
	}
}