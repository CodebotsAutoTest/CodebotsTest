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
using System.Linq.Expressions;
using Lm2348.Helpers;
using Lm2348.Models;
using Lm2348.Services;

namespace Lm2348.Utility
{
	public static class UsersFilter
	{
		/// <summary>
		/// A security filter that can be used over a list of all user entities
		/// </summary>
		/// <param name="user"> The user attempting to perform the operation on all users </param>
		/// <param name="groups"> The groups the user belongs to </param>
		/// <param name="operation"> The database operation the user is trying to perform  </param>
		/// <param name="serviceProvider">Service provider to pass to the ACLs</param>
		/// <returns> An expression that can be used for the where condition of a linq query </returns>
		public static Expression<Func<User, bool>> AllUsersFilter(
			User user, 
			IList<string> groups, 
			DATABASE_OPERATION operation,
			IServiceProvider serviceProvider)
		{
			return ExpressionHelper.OrExpressions(
				new List<Expression<Func<User, bool>>>
				{
				}
			);
		}

		/// <summary>
		/// A security filter for a single user entity
		/// </summary>
		/// <param name="user"> The user attempting to perform the operation </param>
		/// <param name="groups"> The groups the user belongs to </param>
		/// <param name="discriminator"> The user model discriminator </param>
		/// <param name="operation"> The database operation the user is trying to perform  </param>
		/// <param name="serviceProvider">Service provider to pass to the ACLs</param>
		/// <typeparam name="TModel"> The user model trying to be accessed </typeparam>
		/// <returns> An expression that can be used for the where condition of a linq query </returns>
		private static Expression<Func<User, bool>> UserFilter<TModel>(
			User user, 
			IList<string> groups,
			string discriminator,
			DATABASE_OPERATION operation,
			IServiceProvider serviceProvider)
			where TModel : class, IOwnerAbstractModel, new()
		{
			return ExpressionHelper.AndExpressions(
				new List<Expression<Func<User, bool>>>
				{
					SecurityService.GetAggregatedUserModelAcls<TModel>(user, groups, operation, serviceProvider),
					u => u.Discriminator == discriminator
				}
			);
		}
	}
}