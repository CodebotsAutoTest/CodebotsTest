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
using SeleniumTests.Setup;
using SeleniumTests.Utils;
using SeleniumTests.PageObjects.BotWritten.UserPageObject;
using TechTalk.SpecFlow;
using Xunit;

namespace SeleniumTests.Steps.BotWritten
{
	[Binding]
	public sealed class UserListSteps  : BaseStepDefinition
	{
		private readonly AllUserPage _allUserPage;
		private readonly ContextConfiguration _contextConfiguration;

		public UserListSteps(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
			_contextConfiguration = contextConfiguration;
			_allUserPage = new AllUserPage(contextConfiguration);
			// % protected region % [Add any additional setup options here] off begin
			// % protected region % [Add any additional setup options here] end
		}

		[StepDefinition(@"I navigate to the all user page")]
		public void NavigateToTheAllUserPage()
		{
			_allUserPage.Navigate();
			WaitUtils.waitForPage(_driverWait);
		}

		[Then(@"I verify the contents of the All User page")]
		public void VerifyTheContentOfTheAllUserPage()
		{
			_driverWait.Until(_ =>_allUserPage.CreateNewButton.Displayed);
			_driverWait.Until(_ =>_allUserPage.ListHeader.Displayed);
			var columnHeadings = _allUserPage.ListHeaders.ToList();
			Assert.True(columnHeadings.TrueForAll(c => _allUserPage.ExpectedColumnHeadings.Contains(c.Text)));
			_allUserPage.CreateNewButton.Click();
			_driverWait.Until(_ =>_allUserPage.SelectUserTypeModal.Displayed);
			_driverWait.Until(_ =>_allUserPage.SelectUserTypeDropdown.Displayed);
		}
	}
}