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
using Lm2348.Graphql.Types;
using Lm2348.Helpers;
using Lm2348.Models;
using Lm2348.Services;
using GraphQL.EntityFramework;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace Lm2348.Graphql.Helpers
{
	public class QueryHelpers
	{
		/// <summary>
		/// Creates a resolve function that returns a list a queryable of models.
		/// This respects the security settings and properly applies the auditing context
		/// </summary>
		/// <typeparam name="TModel">The type of the model to create the function for</typeparam>
		/// <returns>A function that takes a graphql context and returns a queryable of models</returns>
		public static Func<ResolveFieldContext<object>, IQueryable<TModel>> CreateResolveFunction<TModel>()
			where TModel : class, IOwnerAbstractModel, new()
		{
			// % protected region % [Override CreateResolveFunction here] off begin
			return context =>
			{
				var graphQlContext = (Lm2348GraphQlContext) context.UserContext;
				var crudService = graphQlContext.CrudService;
				var auditFields = AuditReadData.FromGraphqlContext(context);
				return crudService.Get<TModel>(auditFields: auditFields).AsNoTracking();
			};
			// % protected region % [Override CreateResolveFunction here] end
		}

		/// <summary>
		/// Creates a conditional where statement that handles both AND and OR conditions
		/// </summary>
		/// <param name="context">The graphql context to of the query</param>
		/// <param name="models">A queryable to apply the condition over</param>
		/// <param name="argName">The name of the graphql arg to fetch from the context</param>
		/// <typeparam name="T">The type of the model to apply the conditional over</typeparam>
		/// <returns>A new queryable that has the conditions applied</returns>
		/// <see cref="QueryableExtensions.AddConditionalWhereFilter{T}"/>
		public static IQueryable<T> CreateConditionalWhere<T>(
			ResolveFieldContext<object> context,
			IQueryable<T> models,
			string argName = "conditions")
			where T : IOwnerAbstractModel
		{
			// % protected region % [Override CreateConditionalWhere here] off begin
			if (context.HasArgument(argName))
			{
				var wheres = context.GetArgument<List<List<WhereExpression>>>(argName);
				return models.AddConditionalWhereFilter(wheres);
			}

			return models;
			// % protected region % [Override CreateConditionalWhere here] end
		}

		/// <summary>
		/// Creates a where statement where all the conditions joined by an AND
		/// </summary>
		/// <param name="context">The graphql context to of the query</param>
		/// <param name="models">A queryable to apply the condition over</param>
		/// <param name="argName">The name of the graphql arg to fetch from the context</param>
		/// <typeparam name="T">The type of the model to apply the conditional over</typeparam>
		/// <returns>A new queryable that has the conditions applied</returns>
		public static IQueryable<T> CreateWhereCondition<T>(ResolveFieldContext<object> context, IQueryable<T> models, string argName = "where")
			where T : IOwnerAbstractModel
		{
			// % protected region % [Override CreateWhereCondition here] off begin
			if (context.HasArgument(argName))
			{
				var wheres = context.GetArgument<List<WhereExpression>>(argName);
				return models.AddWhereFilter(wheres);
			}

			return models;
			// % protected region % [Override CreateWhereCondition here] end
		}

		/// <summary>
		/// Creates a condition where the model matches the given id
		/// </summary>
		/// <param name="context">The graphql context to of the query</param>
		/// <param name="models">A queryable to apply the condition over</param>
		/// <param name="argName">The name of the graphql arg to fetch from the context</param>
		/// <typeparam name="T">The type of the model to apply the conditional over</typeparam>
		/// <returns>A new queryable that has the conditions applied</returns>
		public static IQueryable<T> CreateIdCondition<T>(ResolveFieldContext<object> context, IQueryable<T> models, string argName = "id")
			where T : IOwnerAbstractModel
		{
			// % protected region % [Override CreateIdCondition here] off begin
			if (context.HasArgument(argName))
			{
				var id = context.GetArgument<Guid>(argName);
				models = models.Where(model => model.Id == id);
			}

			return models;
			// % protected region % [Override CreateIdCondition here] end
		}

		/// <summary>
		/// Creates a condition where the model matches a set of ids
		/// </summary>
		/// <param name="context">The graphql context to of the query</param>
		/// <param name="models">A queryable to apply the condition over</param>
		/// <param name="argName">The name of the graphql arg to fetch from the context</param>
		/// <typeparam name="T">The type of the model to apply the conditional over</typeparam>
		/// <returns>A new queryable that has the conditions applied</returns>
		public static IQueryable<T> CreateIdsCondition<T>(ResolveFieldContext<object> context, IQueryable<T> models, string argName = "ids")
			where T : IOwnerAbstractModel
		{
			// % protected region % [Override CreateIdsCondition here] off begin
			if (context.HasArgument(argName))
			{
				var ids = context.GetArgument<List<Guid>>(argName);
				models = models.Where(model => ids.Contains(model.Id));
			}

			return models;
			// % protected region % [Override CreateIdsCondition here] end
		}

		/// <summary>
		/// Applies a skip condition to a query
		/// </summary>
		/// <param name="context">The graphql context to of the query</param>
		/// <param name="models">A queryable to apply the condition over</param>
		/// <param name="argName">The name of the graphql arg to fetch from the context</param>
		/// <typeparam name="T">The type of the model to apply the conditional over</typeparam>
		/// <returns>A new queryable that has the conditions applied</returns>
		public static IQueryable<T> CreateSkip<T>(ResolveFieldContext<object> context, IQueryable<T> models, string argName = "skip")
			where T : IOwnerAbstractModel
		{
			// % protected region % [Override CreateSkip here] off begin
			if (context.HasArgument(argName))
			{
				var skip = context.GetArgument<int>(argName);
				models = models.Skip(skip);
			}

			return models;
			// % protected region % [Override CreateSkip here] end
		}

		/// <summary>
		/// Applies a take condition to a query
		/// </summary>
		/// <param name="context">The graphql context to of the query</param>
		/// <param name="models">A queryable to apply the condition over</param>
		/// <param name="argName">The name of the graphql arg to fetch from the context</param>
		/// <typeparam name="T">The type of the model to apply the conditional over</typeparam>
		/// <returns>A new queryable that has the conditions applied</returns>
		public static IQueryable<T> CreateTake<T>(ResolveFieldContext<object> context, IQueryable<T> models, string argName = "take")
			where T : IOwnerAbstractModel
		{
			// % protected region % [Override CreateTake here] off begin
			if (context.HasArgument(argName))
			{
				var take = context.GetArgument<int>(argName);
				models = models.Take(take);
			}

			return models;
			// % protected region % [Override CreateTake here] end
		}

		/// <summary>
		/// Orders a set of models by a list of order by conditions. The order by conditions are applied first to last.
		/// </summary>
		/// <param name="context">The graphql context to of the query</param>
		/// <param name="models">A queryable to apply the condition over</param>
		/// <param name="argName">The name of the graphql arg to fetch from the context</param>
		/// <typeparam name="T">The type of the model to apply the conditional over</typeparam>
		/// <returns>A new queryable that has the conditions applied</returns>
		public static IQueryable<T> CreateOrderBy<T>(ResolveFieldContext<object> context, IQueryable<T> models, string argName = "orderBy")
			where T : IOwnerAbstractModel
		{
			// % protected region % [Override CreateOrderBy here] off begin
			if (context.HasArgument(argName))
			{
				var orderBys = context.GetArgument<List<OrderBy>>(argName);
				return models.AddOrderBys(orderBys);
			}

			return models;
			// % protected region % [Override CreateOrderBy here] end
		}
	}
}