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
using System.Collections.Generic;
using Lm2348.Models;

namespace ServersideTests.Helpers
{
	/// <summary>
	/// A comparer for to abstract models that compares only if the ids are the same
	/// </summary>
	public class ModelIdComparer : IEqualityComparer<IAbstractModel>
	{
		public bool Equals(IAbstractModel x, IAbstractModel y)
		{
			return (x, y) switch
			{
				(null, null) => true,
				(_, null) => false,
				(null, _) => false,
				var (first, second) => first.Id == second.Id,
			};
		}

		public int GetHashCode(IAbstractModel obj)
		{
			return obj.GetHashCode();
		}
	}
}