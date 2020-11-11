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

using Lm2348.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace ServersideTests.Mocks
{
	public class MockSignInManager : Mock<SignInManager<User>>
	{

		public MockSignInManager(
			UserManager<User> userManager,
			IHttpContextAccessor httpContext,
			IUserClaimsPrincipalFactory<User> claimsPrincipalFactory,
			IOptions<IdentityOptions> options,
			ILogger<SignInManager<User>> logger,
			IAuthenticationSchemeProvider authenticationScheme,
			IUserConfirmation<User> userConfirmation
		) :
			base(
			userManager,
			httpContext,
			claimsPrincipalFactory,
			options,
			logger,
			authenticationScheme,
			userConfirmation)
		{
		}

		public static MockSignInManager GetMockSignInManager(
			UserManager<User> userManager = null,
			IHttpContextAccessor httpContext = null,
			IUserClaimsPrincipalFactory<User> claimsPrincipalFactory = null,
			IOptions<IdentityOptions> options = null,
			ILogger<SignInManager<User>> logger = null,
			IAuthenticationSchemeProvider authenticationScheme = null,
			IUserConfirmation<User> userConfirmation = null
			)
		{
			return new  MockSignInManager(
				userManager ?? MockUserManager.GetMockUserManager().Object,
				httpContext ?? new Mock<IHttpContextAccessor>().Object,
				claimsPrincipalFactory ?? new Mock<IUserClaimsPrincipalFactory<User>>().Object,
				options ?? new Mock<IOptions<IdentityOptions>>().Object,
				logger ?? new Mock<ILogger<SignInManager<User>>>().Object,
				authenticationScheme ?? new Mock<IAuthenticationSchemeProvider>().Object,
				userConfirmation ?? new Mock<IUserConfirmation<User>>().Object);
		}
	}
}