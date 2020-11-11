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
using System.Threading.Tasks;
using Lm2348.Models;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Lm2348.Services.Interfaces
{
	public interface IIdentityService
	{
		/// <summary>
		/// To set and get the Fetched flag
		/// </summary>
		bool Fetched { get; set; }

		/// <summary>
		/// The groups that the user is in
		/// </summary>
		IList<string> Groups { get; set; }
		
		/// <summary>
		/// The user is performing actions in the scope
		/// </summary>
		User User { get; set; }

		/// <summary>
		/// Retrieves the user from the database if they have not already been fetched. The user to retrieve is taken
		/// from the http context of the scope. If this function has already been called in the scope then it will be a
		/// no op on future calls/
		/// </summary>
		/// <returns>A task that resolves when the user and groups are fetched</returns>
		Task RetrieveUserAsync();

		// % protected region % [Add any further interface members here] off begin
		// % protected region % [Add any further interface members here] end
	}
}