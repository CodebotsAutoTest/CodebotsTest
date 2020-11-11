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
using Lm2348.Services;

namespace Lm2348.Helpers
{
	public static class ExpressionHelper
	{
		/// <summary>
		/// Aggregates a list of expressions using an OR conjunction
		/// </summary>
		/// <param name="expressions"> The expressions to aggregate </param>
		/// <typeparam name="TModel"> Model returned by expressions </typeparam>
		/// <returns> An expression that can be used for the where condition of a linq query </returns>
		public static Expression<Func<TModel, bool>> OrExpressions<TModel>(
			IEnumerable<Expression<Func<TModel, bool>>> expressions)
		{
			Expression<Func<TModel, bool>> baseRule = _ => false;
			var filter = Expression.OrElse(baseRule.Body, baseRule.Body);

			filter = expressions.Aggregate(filter, (current, expression) =>
				Expression.OrElse(current, expression.Body));

			var param = Expression.Parameter(typeof(TModel), "model");
			var replacer = new ParameterReplacer(param);

			return Expression.Lambda<Func<TModel, bool>>(replacer.Visit(filter), param);
		}

		/// <summary>
		/// Aggregates a list of expressions using an AND conjunction
		/// </summary>
		/// <param name="expressions"> The expressions to aggregate </param>
		/// <typeparam name="TModel"> Model returned by expressions </typeparam>
		/// <returns> An expression that can be used for the where condition of a linq query </returns>
		public static Expression<Func<TModel, bool>> AndExpressions<TModel>(
			IEnumerable<Expression<Func<TModel, bool>>> expressions)
		{
			Expression<Func<TModel, bool>> baseRule = _ => true;
			var filter = Expression.AndAlso(baseRule.Body, baseRule.Body);

			filter = expressions.Aggregate(filter, (current, expression) =>
				Expression.AndAlso(current, expression.Body));

			var param = Expression.Parameter(typeof(TModel), "model");
			var replacer = new ParameterReplacer(param);

			return Expression.Lambda<Func<TModel, bool>>(replacer.Visit(filter), param);
		}
	}
}