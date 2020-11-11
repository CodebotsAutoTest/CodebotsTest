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
using System.Threading.Tasks;
using Lm2348.Exceptions;
using Lm2348.Services;
using Lm2348.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Lm2348.Utility;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

// % protected region % [Customise Authorization Library imports here] off begin
using AspNet.Security.OpenIdConnect.Primitives;
using OpenIddict.Mvc.Internal;
// % protected region % [Customise Authorization Library imports here] end

// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Lm2348.Controllers
{
	/// <summary>
	/// Controller for authenticating existing users
	/// </summary>
	[Route("/api/authorization")]
	[ApiController]
	public class AuthorizationController : Controller
	{
		// % protected region % [Customise AuthorizationController fields here] off begin
		private readonly IUserService _userService;
		private readonly IXsrfService _xsrfService;
		private readonly ILogger<AuthorizationController> _logger;
		// % protected region % [Customise AuthorizationController fields here] end

		// % protected region % [Add any additional fields here] off begin
		// % protected region % [Add any additional fields here] end

		public AuthorizationController(
			// % protected region % [Add any constructor arguments here] off begin
			// % protected region % [Add any constructor arguments here] end
			IUserService userService, 
			IXsrfService xsrfService,
			ILogger<AuthorizationController> logger)
		{
			// % protected region % [Add any constructor initial logic here] off begin
			// % protected region % [Add any constructor initial logic here] end
		
			// % protected region % [Customise AuthorizationController middle logic here] off begin
			_userService = userService;
			_xsrfService = xsrfService;
			_logger = logger;
			// % protected region % [Customise AuthorizationController middle logic here] end

			// % protected region % [Add any constructor end logic here] off begin
			// % protected region % [Add any constructor end logic here] end
		}

		// % protected region % [Customise Exchange method implementation here] off begin
		/// <summary>
		/// Grants a token to authenticate a user for a session. Tokens should be used by clients that don't support
		/// cookies such as mobile apps and api consumers.
		/// </summary>
		/// <param name="request">
		/// An x-www-form-urlencoded body with keys for grant_type, username and password.
		/// The only current supported grant type is "password"
		/// </param>
		/// <returns>A sign in result for with an access_token field for authentication</returns>
		[HttpPost("connect/token")]
		[Produces("application/json")]
		public async Task<ActionResult<OpenIdConnectResponse>> Exchange([ModelBinder(typeof(OpenIddictMvcBinder))] OpenIdConnectRequest request)
		{
			try
			{
				var ticket = await _userService.Exchange(request);
				return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
			}
			catch (InvalidUserPasswordException e)
			{
				return Unauthorized(new OpenIdConnectResponse
				{
					Error = OpenIdConnectConstants.Errors.InvalidGrant,
					ErrorDescription = e.Message
				});
			}
			catch (InvalidGrantTypeException e)
			{
				return Unauthorized(new OpenIdConnectResponse
				{
					Error = OpenIdConnectConstants.Errors.UnsupportedGrantType,
					ErrorDescription = e.Message
				});
			}
			catch
			{
				return Unauthorized();
			}
		}
		// % protected region % [Customise Exchange method implementation here] end

		// % protected region % [Configure authorization login logic here] off begin
		/// <summary>
		/// Logs into the site providing an auth cookie and a xsrf token
		/// </summary>
		/// <param name="details">The details required to login</param>
		/// <returns>
		/// 200 OK on success, or 401 on failure. If the request is successful it returns a XSRF token, an antiforgery
		/// token and a login token as cookies.
		/// </returns>
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody]LoginDetails details)
		{
			try
			{
				var user = await _userService.CheckCredentials(details.Username, details.Password);
				var principal = await _userService.CreateUserPrincipal(user);
				await HttpContext.SignInAsync(
					CookieAuthenticationDefaults.AuthenticationScheme,
					principal,
					new AuthenticationProperties
					{
						IsPersistent = details.RememberMe
					});

				_xsrfService.AddXsrfToken(HttpContext, principal);

				return Ok(await _userService.GetUser(user));
			}
			catch (InvalidUserPasswordException e)
			{
				var error = new IdentityError { Code = string.Empty, Description = e.Message };
				var errors = new List<IdentityError> { error };
				
				AddErrors(errors);
				return Unauthorized(ModelState.GetNormalisedErrors());
			}
			catch
			{
				return Unauthorized();
			}
		}
		// % protected region % [Configure authorization login logic here] end

		/// <summary>
		/// Removes the authentication cookie that can be used to authenticate against the site
		/// </summary>
		/// <param name="redirect">A redirect to send to send after the finish of the request</param>
		/// <returns>
		/// Either a 200 OK or a 302 Found if a redirect url is present
		/// </returns>
		[HttpGet("logout")]
		[HttpPost("logout")]
		public async Task<IActionResult> Logout([FromQuery] string redirect)
		{
			// % protected region % [Configure logout login logic here] off begin
			// Sign out of the session
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			// Delete any cookies
			foreach (var cookiesKey in HttpContext.Request.Cookies.Keys)
			{
				HttpContext.Response.Cookies.Delete(cookiesKey);
			}

			// If given a redirect url go to that page, otherwise return Ok
			if (string.IsNullOrWhiteSpace(redirect))
			{
				return Ok();
			}

			return Redirect(redirect);
			// % protected region % [Configure logout login logic here] end
		}

		private void AddErrors(IEnumerable<IdentityError> errors)
		{
			// % protected region % [Override AddErrors here] off begin
			foreach (var error in errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
			// % protected region % [Override AddErrors here] end
		}

		// % protected region % [Add any authorization controller methods here] off begin
		// % protected region % [Add any authorization controller methods here] end
	}

	/// <summary>
	/// The details needed to login using cookie auth
	/// </summary>
	public class LoginDetails
	{
		// % protected region % [Customise login details] off begin
		/// <summary>
		/// The username of the user that is logging in
		/// </summary>
		public string Username { get; set; }

		/// <summary>
		/// The password of the user that is logging in
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// Should the user be sent a persistent token instead of a session token
		/// </summary>
		public bool RememberMe { get; set; }
		// % protected region % [Customise login details] end
	}

	// % protected region % [Add any additional methods here] off begin
	// % protected region % [Add any additional methods here] end
}
