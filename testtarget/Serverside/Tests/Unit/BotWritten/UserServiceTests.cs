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
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Lm2348.Exceptions;
using Lm2348.Models;
using Lm2348.Services;
using ExpectedObjects;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using MockQueryable.Moq;
using Moq;
using ServersideTests.Mocks;
using TestDataLib;
using Xunit;

// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace ServersideTests.Tests.Unit.BotWritten
{
	[Trait("Category", "BotWritten")]
	[Trait("Category", "Unit")]
	public class UserServiceTests
	{
		private readonly Fixture _fixture;
		private readonly User _testUser;
		private readonly MockUserManager _mockUserManager;
		private readonly Mock<RoleManager<Group>> _mockRoleManager;
		private readonly IList<string> _groupListNames;

		public UserServiceTests()
		{
			_fixture = new Fixture();

			// create a user by setting creating
			_testUser = new User
			{
				UserName = _fixture.Create("Username"),
				Email = DataUtils.RandEmail()
			};

			_mockUserManager = MockUserManager.GetMockUserManager();
			_mockRoleManager = MockRoleManager.GetMockRoleManager();


			var testGroupListName = _fixture.Create("Group");
			_groupListNames = new List<string> { testGroupListName };
		}

		// % protected region % [Customize UserServiceGetUserTest here] off begin
		/// <summary>
		/// Attempt to get a user
		/// </summary>
		[Fact]
		public async void UserServiceGetUserTest()
		{
			//arrange
			var testGroups = _groupListNames.Select(x => new Group { Name = x });
			var mockTestGroups = testGroups.AsQueryable().BuildMock();

			_mockUserManager.Setup(x => x.GetRolesAsync(_testUser)).Returns(Task.FromResult(_groupListNames));
			_mockRoleManager.Setup(x => x.Roles).Returns(mockTestGroups.Object);

			var userService =
				MockUserService.GetMockUserService(
					signInManager: MockSignInManager.GetMockSignInManager(userManager: _mockUserManager.Object).Object,
					userManager: _mockUserManager.Object,
					roleManager: _mockRoleManager.Object
				).Object;

			// act
			var result = await userService.GetUser(_testUser);

			// assert
			result.Groups
				.Should()
				.HaveCount(testGroups.Count());

			result.Groups.Select(x => x.Name).Should().Equal(_groupListNames);

			Assert.Equal(_testUser.UserName, result.Email);
		}
		// % protected region % [Customize UserServiceGetUserTest here] end

		// % protected region % [Customize UserServiceRegisterDuplicateUserTest here] off begin
		/// <summary>
		/// Attempt to create a user which is already in the system,
		/// should return a duplicate user exception
		/// </summary>
		[Fact]
		public void UserServiceRegisterDuplicateUserTest()
		{
			// arrange
			// create a registration model with same email as the test user
			var registrationModel = new RegisterModel
			{
				Email = _testUser.Email,
				Password = _fixture.Create<string>(),
				Groups = _groupListNames
			};

			_mockUserManager
				.Setup(x => x.FindByEmailAsync(_testUser.Email))
				.Returns(Task.FromResult(_testUser));

			var userService =
				MockUserService.GetMockUserService(
					signInManager: MockSignInManager.GetMockSignInManager(userManager: _mockUserManager.Object).Object,
					userManager: _mockUserManager.Object,
					roleManager: _mockRoleManager.Object
				).Object;

			// act
			Func<Task> act = async () =>
				await userService.RegisterUser(registrationModel, _groupListNames);

			// assert
			act.Should().Throw<DuplicateUserException>();
		}
		// % protected region % [Customize UserServiceRegisterDuplicateUserTest here] end

		// % protected region % [Customize UserServiceRegisterUserTest here] off begin
		/// <summary>
		/// Attempt to create a user successfully,
		/// should return a duplicate user exception
		/// </summary>
		[Fact]
		public async void UserServiceRegisterUserTest()
		{
			// arrange
			var mockedResult = new MockIdentityResult().MockResultSuccession(true);

			var testPassword = _fixture.Create<string>();

			// create a registration model with same email as the test user
			var registrationModel = new RegisterResult
			{
				Result = mockedResult,
				User = _testUser
			};

			_mockUserManager
				.Setup(x => x.CreateAsync(_testUser, testPassword))
				.Returns(Task.FromResult((IdentityResult)mockedResult));

			var usersList = new List<User> { _testUser };

			var mockUsers = usersList.AsQueryable().BuildMock();
			_mockUserManager.Setup(x => x.Users).Returns(mockUsers.Object);

			var userService =
				MockUserService.GetMockUserService(
					signInManager: MockSignInManager.GetMockSignInManager(userManager: _mockUserManager.Object).Object,
					userManager: _mockUserManager.Object,
					roleManager: _mockRoleManager.Object
				).Object;

			// act
			var result = await userService.RegisterUser(_testUser, testPassword, _groupListNames);

			// assert
			registrationModel.ToExpectedObject().ShouldMatch(result);
		}
		// % protected region % [Customize UserServiceRegisterUserTest here] end	
	
		// % protected region % [Add any additional tests here] off begin
		// % protected region % [Add any additional tests here] end
	}
}