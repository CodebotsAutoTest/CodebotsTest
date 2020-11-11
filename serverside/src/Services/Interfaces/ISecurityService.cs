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

namespace Lm2348.Services.Interfaces
{
	public interface ISecurityService
	{
		/// <summary>
		/// Checks weather all the changes in the change tracker of the scoped db context are abiding the security rules
		/// </summary>
		/// <param name="identityService">The identity service to fetch the user from</param>
		/// <param name="dbContext">The DbContext to check the changes against</param>
		/// <returns>A List of security exceptions as strings for each violation of the security rules</returns>
		Task<List<string>> CheckDbSecurityChanges(IIdentityService identityService, Lm2348DBContext dbContext);
	}
}