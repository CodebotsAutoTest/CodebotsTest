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
using Lm2348.Models;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Lm2348.Security
{
	/// <summary>
	/// Defines the methods and data that must be provided to an ACL rule for runtime security
	/// </summary>
	public interface IAcl
	{
		string Group { get; }
		bool IsVisitorAcl { get; }

		Expression<Func<TModel, bool>> GetReadConditions<TModel>(User user, SecurityContext context)
			where TModel : IOwnerAbstractModel, new();
		Expression<Func<TModel, bool>> GetUpdateConditions<TModel>(User user, SecurityContext context)
			where TModel : IOwnerAbstractModel, new();
		Expression<Func<TModel, bool>> GetDeleteConditions<TModel>(User user, SecurityContext context)
			where TModel : IOwnerAbstractModel, new();
		bool GetCreate(User user, IEnumerable<IAbstractModel> models, SecurityContext context);
		bool GetUpdate(User user, IEnumerable<IAbstractModel> models, SecurityContext context);
		bool GetDelete(User user, IEnumerable<IAbstractModel> models, SecurityContext context);

		// % protected region % [Add any extra acl interface members here] off begin
		// % protected region % [Add any extra acl interface members here] end
	}
}