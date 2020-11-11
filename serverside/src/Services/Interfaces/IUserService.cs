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
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Primitives;
using Lm2348.Exceptions;
using Lm2348.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

// % protected region % [Add any extra iuserservice imports here] off begin
// % protected region % [Add any extra iuserservice imports here] end

namespace Lm2348.Services.Interfaces
{
	public interface IUserService
	{
		// % protected region % [Customise CheckCredentials signature here] off begin
		/// <summary>
		/// Check the username and password of a user.
		/// </summary>
		/// <param name="username">The username of the user</param>
		/// <param name="password">The password of the user</param>
		/// <param name="lockoutOnFailure">Flag indicating if the user account should be locked if the sign in fails</param>
		/// <param name="validateEmailConfirmation">Weather the login should check if the email has been confirmed</param>
		/// <returns>On success returns the user object, on failure throws an exception</returns>
		/// <exception cref="InvalidUserPasswordException">On the username or password being invalid</exception>
		Task<User> CheckCredentials(string username, string password, bool lockoutOnFailure = true, bool validateEmailConfirmation = true);
		// % protected region % [Customise CheckCredentials signature here] end

		/// <summary>
		/// Confirm an email for a user
		/// </summary>
		/// <param name="email">The email to confirm</param>
		/// <param name="token">The token to confirm the email with</param>
		/// <returns>An identity result identifying the success of the operation</returns>
		Task<IdentityResult> ConfirmEmail(string email, string token);

		/// <summary>
		/// Creates a ClaimsPrincipal using the specified authentication type
		/// </summary>
		/// <param name="user">The user to create the claims principal for</param>
		/// <param name="authenticationScheme">The authentication scheme used, cookie by default</param>
		/// <returns>The claims principal for the user</returns>
		Task<ClaimsPrincipal> CreateUserPrincipal(User user, string authenticationScheme = "Cookies");

		/// <summary>
		/// Deletes a user
		/// </summary>
		/// <param name="id">The id of the user to delete</param>
		/// <returns>Task containing boolean indicating success</returns>
		Task<bool> DeleteUser(Guid id);


		// % protected region % [customize exchange signature] off begin
		/// <summary>
		/// Creates a authentication ticket to identify a user
		/// </summary>
		/// <param name="request">The OpenId request for the user</param>
		/// <returns>The authentication ticket for the user</returns>
		/// <exception cref="InvalidUserPasswordException">Thrown when an invalid username or password is provided</exception>
		/// <exception cref="InvalidGrantTypeException">Thrown when an invalid OpenId grant type is provided</exception>
		Task<AuthenticationTicket> Exchange(OpenIdConnectRequest request);
		// % protected region % [customize exchange signature] end

		/// <summary>
		/// Gets a user result from a given claims principal
		/// </summary>
		/// <param name="principal">The claims principal to extract the user from</param>
		/// <returns>The user that is provided by the principal</returns>
		/// <exception cref="InvalidIdException">When the principal does not apply to a valid user</exception>
		Task<UserResult> GetUser(ClaimsPrincipal principal);

		/// <summary>
		/// Gets a user result from a given user
		/// </summary>
		/// <param name="user">The user to make the user result from</param>
		/// <returns>A user result</returns>
		Task<UserResult> GetUser(User user);

		/// <summary>
		/// Gets a user from a claims principal based off of the username
		/// </summary>
		/// <param name="principal">The claims principal to get the credentials from</param>
		/// <returns>A user, or null if one is not found</returns>
		Task<User> GetUserFromClaim(ClaimsPrincipal principal);

		/// <summary>
		/// Gets all users
		/// </summary>
		/// <returns>A task that resolves to a list of all users</returns>
		Task<List<UserResult>> GetUsers();

		/// <summary>
		/// Registers a new user given a registration model and a list of groups
		/// </summary>
		/// <param name="model">The registration model of the user to create</param>
		/// <param name="groups">The groups that the user shall be added to</param>
		/// <param name="sendRegisterEmail">
		/// Should an email be sent to validate the users email. If this is set to true then the user will not be
		/// immediately activated, otherwise no email will be sent and the user will be immediately activated.
		/// </param>
		/// <returns>An identity result indicating the success of the operation</returns>
		/// <exception cref="DuplicateUserException">On a user with this email already existing</exception>
		Task<RegisterResult> RegisterUser(RegisterModel model, IEnumerable<string> groups, bool sendRegisterEmail = false);

		/// <summary>
		/// Registers a new user given a user model, password and list of groups
		/// </summary>
		/// <param name="user">The user model of the user to create</param>
		/// <param name="password">The password of the user</param>
		/// <param name="groups">The groups that the user shall be added to</param>
		/// <param name="sendRegisterEmail">
		/// Should an email be sent to validate the users email. If this is set to true then the user will not be
		/// immediately activated, otherwise no email will be sent and the user will be immediately activated.
		/// </param>
		/// <returns>An identity result indicating the success of the operation</returns>
		Task<RegisterResult> RegisterUser(User user, string password, IEnumerable<string> groups, bool sendRegisterEmail = false);

		// % protected region % [customize sendpassportresetemail signature] off begin
		/// <summary>
		/// Sends a reset password email to a user
		/// </summary>
		/// <param name="user">The user to reset the password of</param>
		/// <returns>True if the email was successfully sent</returns>
		Task<bool> SendPasswordResetEmail(User user);
		// % protected region % [customize sendpassportresetemail signature] end

		/// <summary>
		/// updates a new user
		/// </summary>
		/// <param name="model">The registration model of the user to create</param>
		/// <returns>An identity result indicating the success of the operation</returns>
		/// <exception cref="DuplicateUserException">On a user with this email already existing</exception>
		Task<IdentityResult> UpdateUser(UserUpdateModel model);

		// % protected region % [Add any additional iuserservice methods here] off begin
		// % protected region % [Add any additional iuserservice methods here] end
	}
}