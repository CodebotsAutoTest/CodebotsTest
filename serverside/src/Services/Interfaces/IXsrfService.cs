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
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Lm2348.Services.Interfaces
{
	public interface IXsrfService
	{
		/// <summary>
		/// Adds an XSRF token to the HttpContext using the user claim attached to the HttpContext
		/// </summary>
		/// <param name="context">The HttpContext to add the token to</param>
		void AddXsrfToken(HttpContext context);

		/// <summary>
		/// Adds a XSRF token to the HttpContext using the provided user claim
		/// </summary>
		/// <param name="context">The HttpContext to add the token to</param>
		/// <param name="userClaim">The ClaimsPrincipal to generate the XSRF token with</param>
		void AddXsrfToken(HttpContext context, ClaimsPrincipal userClaim);
	}
}