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
using Lm2348.Graphql.Helpers;
using Lm2348.Models;
using GraphQL.Types;
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace Lm2348.Graphql.Fields
{
	public class ConditionalQuery
	{
		/// <summary>
		/// Creates a resolve function for a query that can both AND and OR conditions together
		/// </summary>
		/// <typeparam name="TModel">The type of model to create the query for</typeparam>
		/// <returns>A function that takes a graphql context and returns a list of models</returns>
		public static Func<ResolveFieldContext<object>, IQueryable<TModel>> CreateConditionalQuery<TModel>()
			where TModel : class, IOwnerAbstractModel, new()
		{
			// % protected region % [Override CreateConditionalQuery here] off begin
			return context =>
			{
				// Fetch the models that we need
				var models = QueryHelpers.CreateResolveFunction<TModel>()(context);

				// Apply the conditions to the query
				models = QueryHelpers.CreateConditionalWhere(context, models);

				return models;
			};
			// % protected region % [Override CreateConditionalQuery here] end
		}
	}
}