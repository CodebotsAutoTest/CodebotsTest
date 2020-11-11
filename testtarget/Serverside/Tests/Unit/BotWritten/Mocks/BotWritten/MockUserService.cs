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
using Lm2348.Models;
using Lm2348.Services;
using Lm2348.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace ServersideTests.Mocks
{
	public class MockUserService: Mock<UserService>
	{
		public MockUserService(
			IOptions<IdentityOptions> identityOptions,
			SignInManager<User> signInManager,
			UserManager<User> userManager,
			IHttpContextAccessor httpContextAccessor,
			RoleManager<Group> roleManager,
			IEmailService emailService,
			IConfiguration configuration) :
			base(
				identityOptions,
				signInManager,
				userManager,
				httpContextAccessor,
				roleManager,
				emailService,
				configuration)
		{

		}

		public static MockUserService GetMockUserService(
			IOptions<IdentityOptions> identityOptions = null,
			SignInManager<User> signInManager = null,
			UserManager<User> userManager = null,
			IHttpContextAccessor httpContextAccessor = null,
			RoleManager<Group> roleManager = null,
			IEmailService emailService = null,
			IConfiguration configuration = null)
		{
			return new MockUserService(
				identityOptions ?? new Mock<IOptions<IdentityOptions>>().Object,
				signInManager ?? MockSignInManager.GetMockSignInManager(userManager: userManager ?? MockUserManager.GetMockUserManager().Object).Object,
				userManager ?? MockUserManager.GetMockUserManager().Object,
				httpContextAccessor ?? new Mock<IHttpContextAccessor>().Object,
				roleManager ?? MockRoleManager.GetMockRoleManager().Object,
				emailService ?? new Mock<IEmailService>().Object,
				configuration ?? new Mock<IConfiguration>().Object);
		}
	}
}