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

using SeleniumTests.PageObjects.BotWritten.UserPageObjects;
using SeleniumTests.Setup;
using TechTalk.SpecFlow;
using Xunit;

// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace SeleniumTests.Steps.BotWritten.Login
{
	[Binding]
	public class LoginSteps  : BaseStepDefinition
	{
		private readonly ContextConfiguration _contextConfiguration;
		private readonly LoginPage _loginPage;

		public LoginSteps(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
			_contextConfiguration = contextConfiguration;
			_loginPage = new LoginPage(_contextConfiguration);
			// % protected region % [Add any additional setup options here] off begin
			// % protected region % [Add any additional setup options here] end
		}

		// % protected region % [Customize LoginAsUser logic here] off begin
		[Given("I login to the site as a user")]
		public void LoginAsUser()
		{
			var userName = _contextConfiguration.SuperUserConfiguration.Username;
			var password = _contextConfiguration.SuperUserConfiguration.Password;
			GivenIAttemptToLogin(userName, password, "success");
		}
		// % protected region % [Customize LoginAsUser logic here] end


		[Given(@"I login to the site with username (.*) and password (.*) then I expect login (.*)")]
		public void GivenIAttemptToLogin(string user, string pass, string success)
		{
			_loginPage.Navigate();
			_loginPage.Login(user, pass);
			try
			{
				// % protected region % [The default page to route to after login, change to suit needs] off begin
				_driverWait.Until(wd => wd.Url == _baseUrl + "/");
				// % protected region % [The default page to route to after login, change to suit needs] end
				Assert.Equal("success", success);
			}
			catch (OpenQA.Selenium.UnhandledAlertException)
			{
				Assert.Equal("failure", success);
			}
			catch (OpenQA.Selenium.WebDriverTimeoutException)
			{
				// % protected region % [Customize GivenIAttemptToLogin login url for asserssion here] off begin
				Assert.Equal(_driver.Url, _baseUrl + "/login");
				// % protected region % [Customize GivenIAttemptToLogin login url for asserssion here] end
			}
		}
	}
}