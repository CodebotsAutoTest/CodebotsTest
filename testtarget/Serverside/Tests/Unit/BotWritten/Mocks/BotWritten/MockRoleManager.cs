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
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;

namespace ServersideTests.Mocks
{
	public class MockRoleManager : Mock<RoleManager<Group>>
	{

		public MockRoleManager(
			IRoleStore<Group> store,
			IEnumerable<IRoleValidator<Group>> roleValidators,
			ILookupNormalizer keyNormalizer,
			IdentityErrorDescriber errors,
			ILogger<RoleManager<Group>> logger

		) :
		base(
			store,
			roleValidators,
			keyNormalizer,
			errors,
			logger
		)
		{
		}

		public static MockRoleManager GetMockRoleManager(
			IRoleStore<Group> store = null,
			IEnumerable<IRoleValidator<Group>> roleValidators = null,
			ILookupNormalizer keyNormalizer = null,
			IdentityErrorDescriber errors = null,
			ILogger<RoleManager<Group>> logger = null)
		{
			return new  MockRoleManager(
 				store ?? new Mock<IRoleStore<Group>>().Object,
				roleValidators,
				keyNormalizer,
				errors,
				logger);
		}
	}
}