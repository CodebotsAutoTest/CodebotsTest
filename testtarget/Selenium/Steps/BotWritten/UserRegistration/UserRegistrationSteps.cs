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
using System.Linq;
using APITests.EntityObjects.Models;
using SeleniumTests.Enums;
using SeleniumTests.Setup;
using TechTalk.SpecFlow;
using SeleniumTests.PageObjects.BotWritten.UserPageObjects;
using SeleniumTests.Utils;
using APITests.Factories;
using APITests.Utils;
using Xunit;

namespace SeleniumTests.Steps.BotWritten.UserRegistration
{
	[Binding]
	public class UserRegistrationFeatureSteps  : BaseStepDefinition
	{
		private readonly ContextConfiguration _contextConfiguration;
		private readonly LoginPage _loginPage;
		private readonly RegisterUserSelectionPage _registerUserSelectionPage;
		private RegisterUserBasePage RegisterUserPage;
		private UserBaseEntity UserEntity;
		private Email RegistrationEmail;

		public UserRegistrationFeatureSteps(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
			_contextConfiguration = contextConfiguration;
			_loginPage = new LoginPage(contextConfiguration);
			_registerUserSelectionPage = new RegisterUserSelectionPage(contextConfiguration);
		}

		[Given(@"I navigate to the registration page")]
		public void GivenINavigateToTheRegistrationPage()
		{
			_loginPage.Navigate();
			WaitUtils.waitForPage(_driverWait);
			_loginPage.RegisterButton.Click();
		}

		[Given(@"I choose (.*) as my user type")]
		public void GivenIChooseUserAsMyUserType(UserType userType)
		{
			RegisterUserPage = _registerUserSelectionPage.Select(userType);
			WaitUtils.waitForPage(_driverWait);
		}

		[Given(@"I complete the (.*) user registration form")]
		public void GivenICompleteTheRegistrationForm(string userType)
		{
			UserEntity = new UserEntityFactory(userType).Construct();
			UserEntity.Configure(BaseEntity.ConfigureOptions.CREATE_ATTRIBUTES_ONLY);
			RegisterUserPage.Register(UserEntity);
		}

		[Then(@"I will see a registration success message")]
		public void ThenIRegistrationSuccessMessage()
		{
			var registrationSuccessPage = new RegistrationSuccessPage(_contextConfiguration);
			_driverWait.Until(x => registrationSuccessPage.registrationEmail.Displayed);
			Assert.True(registrationSuccessPage.successHeader.Displayed);
			Assert.Equal(UserEntity.EmailAddress, registrationSuccessPage.registrationEmail.Text);
		}

		[StepArgumentTransformation]
		public static UserType TransformStringToUserTypeEnum(string userType)
		{
			switch (userType)
			{
				default:
					throw new Exception($"{userType} enum is not handled");
			}
		}
	}
}