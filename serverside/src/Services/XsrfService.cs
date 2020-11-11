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
using System.Security.Claims;
using Lm2348.Services.Interfaces;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;

namespace Lm2348.Services
{
	public class XsrfService : IXsrfService
	{
		private const string TokenName = "XSRF-TOKEN";

		private readonly IAntiforgery _antiforgery;

		public XsrfService(IAntiforgery antiforgery)
		{
			_antiforgery = antiforgery;
		}

		// % protected region % [customize exchange signature] off begin
		public void AddXsrfToken(HttpContext context)
		{
			if (string.IsNullOrEmpty(context.User.Identity.Name))
			{
				return;
			}

			var tokens = _antiforgery.GetAndStoreTokens(context);

			var date = new DateTime(DateTime.Now.Ticks, DateTimeKind.Unspecified);
			date = date.AddDays(7);

			context.Response.Cookies.Append(
				TokenName,
				tokens.RequestToken,
				new CookieOptions
				{
					HttpOnly = false,
					Expires = new DateTimeOffset(date, TimeSpan.FromHours(0))
				});
		}
		// % protected region % [customize exchange signature] end


		public void AddXsrfToken(HttpContext context, ClaimsPrincipal userClaim)
		{
			var existingClaim = context.User;
			context.User = userClaim;

			AddXsrfToken(context);

			context.User = existingClaim;
		}
	}
}