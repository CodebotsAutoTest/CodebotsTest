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
using System.Threading;
using System.Threading.Tasks;
using Lm2348.Security;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Lm2348.Models
{
	public interface IOwnerAbstractModel : IAbstractModel
	{
		Guid Owner { get; set; }
		IEnumerable<IAcl> Acls { get; }

		/// <summary>
		/// Deletes all entities of a specified type that are related to the provided entity
		/// </summary>
		/// <param name="reference">The reference to delete entities from</param>
		/// <param name="models">A list of models of the type of the current object which will have their related data deleted</param>
		/// <param name="dbContext">The database context that contains the data</param>
		/// <param name="cancellation">Cancellation token for the operation</param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		Task<int> CleanReference<T>(
			string reference,
			IEnumerable<T> models,
			Lm2348DBContext dbContext,
			CancellationToken cancellation = default)
			where T : IOwnerAbstractModel;

		// % protected region % [Add any owner abstract model configuration here] off begin
		// % protected region % [Add any owner abstract model configuration here] end
	}
}