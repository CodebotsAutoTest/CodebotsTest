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

using System.Linq;
using APITests.Utils;
using APITests.EntityObjects.Models;
using APITests.Factories;
using TechTalk.SpecFlow;
using SeleniumTests.Setup;
using SeleniumTests.PageObjects.BotWritten.UserPageObjects;
using SeleniumTests.PageObjects.ResetPasswordPageObject;
using SeleniumTests.Utils;
using Xunit;

namespace SeleniumTests.Steps.BotWritten.PasswordReset
{
	[Binding]
	public sealed class ResetPasswordSteps  : BaseStepDefinition
	{
		private readonly ContextConfiguration _contextConfiguration;
		private readonly RequestResetPasswordPage _requestResetPasswordPage;
		private readonly ResetPasswordPage _resetPasswordPage;
		private readonly LoginPage _loginPage;
		private readonly string _newPassword;
		private Email _passwordResetEmail;
		private UserBaseEntity _userEntity;

		public ResetPasswordSteps(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
			_contextConfiguration = contextConfiguration;
			_requestResetPasswordPage = new RequestResetPasswordPage(_contextConfiguration);
			_resetPasswordPage = new ResetPasswordPage(_contextConfiguration);
			_loginPage = new LoginPage(_contextConfiguration);
			_newPassword = "asdfghjkl123A@";
		}

		[Given(@"I navigate to the request reset password page")]
		public void GivenINavigateToTheRequestResetPasswordPage()
		{
			_loginPage.Navigate();
			_loginPage.PasswordResetLink.Click();
		}

		[Given(@"I complete the form requesting to reset my (.*) account password")]
		public void GivenICompleteTheFormRequestingToResetMyPassword(string userType)
		{
			_userEntity = new UserEntityFactory(userType).ConstructAndSave(_testOutputHelper);
			WaitUtils.waitForPage(_driverWait);
			_requestResetPasswordPage.SetEmailAndSubmit(_userEntity.EmailAddress);
		}

		[Then(@"I expect to recieve an email with a link to reset my password")]
		public void ThenIExpectToRecieveAnEmailWithALinkToResetMyPassword()
		{
			_passwordResetEmail = FileReadingUtilities.ReadPasswordResetEmail(_userEntity.EmailAddress);
			Assert.NotNull(_passwordResetEmail.Link);
		}

		[When(@"I follow the link and complete the reset password form")]
		public void WhenIFollowTheLinkAndCompleteTheResetPasswordForm()
		{
			_driver.Navigate().GoToUrl(_passwordResetEmail.Link);
			WaitUtils.waitForPage(_driverWait);
			Assert.NotNull(_passwordResetEmail.Link);
			_resetPasswordPage.EnterNewPasswordAndSubmit(_newPassword);
		}

		[Then(@"I expect that my old password will not log me in")]
		public void ThenIExpectThatMyOldPasswordWillNotLogMeIn()
		{
			WaitUtils.waitForPage(_driverWait);
			_loginPage.Login(_userEntity.EmailAddress, _userEntity.Password);
			WaitUtils.waitForPage(_driverWait);
			Assert.Equal(_driver.Url, _baseUrl + "/login");
		}

		[Then(@"I expect my new password will log me in")]
		public void ThenIExpectMyNewPasswordWillLogMeIn()
		{
			WaitUtils.waitForPage(_driverWait);
			_loginPage.Login(_userEntity.EmailAddress, _newPassword);
			_driverWait.Until(d => d.Manage().Cookies.AllCookies.Any(x => x.Name == "XSRF-TOKEN"));

			var cookies = _driver.Manage().Cookies;
			Assert.NotNull(cookies.AllCookies.FirstOrDefault(x => x.Name == "XSRF-TOKEN"));
		}
	}
}
