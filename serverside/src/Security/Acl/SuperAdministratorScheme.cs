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

namespace Lm2348.Security.Acl
{
	public class SuperAdministratorsScheme : IAcl
	{
		public string Group => "Super Administrators";
		public bool IsVisitorAcl => false;

		public bool GetCreate(User user, IEnumerable<IAbstractModel> models, SecurityContext context)
		{
			// % protected region % [Override create rule contents here here] off begin
			return true;
			// % protected region % [Override create rule contents here here] end
		}

		public Expression<Func<TModel, bool>> GetReadConditions<TModel>(User user, SecurityContext context)
			where TModel : IOwnerAbstractModel, new()
		{
			// % protected region % [Override read rule contents here here] off begin
			return model => true;
			// % protected region % [Override read rule contents here here] end
		}

		public Expression<Func<TModel, bool>> GetUpdateConditions<TModel>(User user, SecurityContext context)
			where TModel : IOwnerAbstractModel, new()
		{
			// % protected region % [Override conditional update rule contents here here] off begin
			return model => true;
			// % protected region % [Override conditional update rule contents here here] end
		}

		public Expression<Func<TModel, bool>> GetDeleteConditions<TModel>(User user, SecurityContext context)
			where TModel : IOwnerAbstractModel, new()
		{
			// % protected region % [Override conditional delete rule contents here here] off begin
			return model => true;
			// % protected region % [Override conditional delete rule contents here here] end
		}

		public bool GetUpdate(User user, IEnumerable<IAbstractModel> models, SecurityContext context)
		{
			// % protected region % [Override update rule contents here here] off begin
			return true;
			// % protected region % [Override update rule contents here here] end
		}

		public bool GetDelete(User user, IEnumerable<IAbstractModel> models, SecurityContext context)
		{
			// % protected region % [Override delete rule contents here here] off begin
			return true;
			// % protected region % [Override delete rule contents here here] end
		}

		public override bool Equals(object obj)
		{
			// % protected region % [Override Equals contents here here] off begin
			if (obj == null)
			{
				return false;
			}
			
			return GetType() == obj.GetType();
			// % protected region % [Override Equals contents here here] end
		}

		public override int GetHashCode()
		{
			// % protected region % [Override GetHashCode contents here here] off begin
			return GetType().GetHashCode();
			// % protected region % [Override GetHashCode contents here here] end
		}
	}
}