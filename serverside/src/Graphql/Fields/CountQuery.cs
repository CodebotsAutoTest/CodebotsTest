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
using System.Threading.Tasks;
using Lm2348.Graphql.Helpers;
using Lm2348.Graphql.Types;
using Lm2348.Models;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace Lm2348.Graphql.Fields
{
	public class CountQuery
	{
		/// <summary>
		/// Creates a query that counts the number of models that comply to a set of conditions
		/// </summary>
		/// <typeparam name="TModel">The type of model to count</typeparam>
		/// <returns>A function that takes a graphql context and returns a count of models that satisfy the condition</returns>
		public static Func<ResolveFieldContext<object>, Task<object>> CreateCountQuery<TModel>()
			where TModel : class, IOwnerAbstractModel, new()
		{
			// % protected region % [Override CreateCountQuery here] off begin
			return async context =>
			{
				// Fetch the models that we need
				var models = QueryHelpers.CreateResolveFunction<TModel>()(context);

				// Apply conditions to the query
				models = QueryHelpers.CreateWhereCondition(context, models);
				models = QueryHelpers.CreateIdsCondition(context, models);
				models = QueryHelpers.CreateIdCondition(context, models);

				return new NumberObject {Number = await models.CountAsync()};
			};
			// % protected region % [Override CreateCountQuery here] end
		}

		/// <summary>
		/// Creates a query that counts the number of models that comply to a set of conditions.
		/// This query can perform both AND and OR conditions.
		/// </summary>
		/// <typeparam name="TModel">The type of model to count</typeparam>
		/// <returns>A function that takes a graphql context and returns a list of models</returns>
		public static Func<ResolveFieldContext<object>, Task<object>> CreateConditionalCountQuery<TModel>()
			where TModel : class, IOwnerAbstractModel, new()
		{
			// % protected region % [Override CreateConditionalCountQuery here] off begin
			return async context =>
			{
				// Fetch the models that we need
				var models = QueryHelpers.CreateResolveFunction<TModel>()(context);

				// Apply conditions to the query
				models = QueryHelpers.CreateConditionalWhere(context, models);
				models = QueryHelpers.CreateIdsCondition(context, models);
				models = QueryHelpers.CreateIdCondition(context, models);

				return new NumberObject {Number = await models.CountAsync()};
			};
			// % protected region % [Override CreateConditionalCountQuery here] end
		}
	}
}