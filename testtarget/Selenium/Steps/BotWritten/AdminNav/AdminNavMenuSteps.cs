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
using SeleniumTests.Enums;
using SeleniumTests.PageObjects;
using SeleniumTests.Setup;
using SeleniumTests.Utils;
using TechTalk.SpecFlow;
using Xunit;

namespace SeleniumTests.Steps.BotWritten.AdminNav
{
	[Binding]
	public class AdminMenuNavigationSteps : BaseStepDefinition
	{
		private readonly AdminNavSection _adminNavSection;

		public AdminMenuNavigationSteps(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
			_adminNavSection = new AdminNavSection(contextConfiguration);
			// % protected region % [Add any additional setup options here] off begin
			// % protected region % [Add any additional setup options here] end
		}

		[Then(@"The Admin Nav Menu is displayed")]
		public void ThenIAssertThatTheAdminNavMenuIsDisplayed()
		{
			Assert.True(_adminNavSection.AdminNavMenu.Displayed);
		}

		[When(@"I toggle the Admin Nav section")]
		public void WhenIToggleTheAdminNavSection()
		{
			_adminNavSection.AdminNavToggle.Click();
		}

		[StepDefinition(@"I click on (.*) Nav link on the Admin Nav section")]
		public void WhenIClickOnLinkOfTheAdminNavSection(AdminSubMenuType subMenuType)
		{
			var adminNavSection = new AdminNavSection(_contextConfiguration);
			switch (subMenuType)
			{
				case AdminSubMenuType.USERS:
					adminNavSection.AdminNavIconUsers.ClickWithWait(_driverWait);
					break;
				case AdminSubMenuType.ENTITIES:
					adminNavSection.AdminNavIconEntities.ClickWithWait(_driverWait);
					break;
				default:
					throw new ArgumentException("Invalid Submenu Type: " + subMenuType);
			}
		}

		[Then(@"I see the Admin Submenus like")]
		public void ThenIAssertThatISeeUsersTheSubmenusLike(Table table)
		{
			// extract data from table
			var adminNavDataFromTable = table.GetTableData().Select(t => t.ToLower()).ToList();
			Assert.Equal(adminNavDataFromTable, _adminNavSection.GetAdminNavSubmenuValues());
		}

		[Then(@"I assert that (.*) Nav links are displayed")]
		public void ThenIAssertThatTheSubmenuDisplaysUsersNavLinks(int submenuNavLinks)
		{
			// submenu is displayed
			Assert.True(_adminNavSection.AdminNavSubLink.Displayed);
			Assert.Equal(submenuNavLinks, _adminNavSection.TotalSubmenuLinks());
		}

		[When(@"I click the (.*) admin submenu")]
		public void WhenIClickTheExpeditionAdminSubmenu( string linkName)
		{
			_adminNavSection.GetAdminNavSubmenuLink(linkName.ToLower()).Click();
		}

		[When(@"I am logged out of the site via admin nav link")]
		public void WhenIAmLoggedOutOfTheSiteViaAdminNavLink()
		{
			_adminNavSection.AdminNavIconLogout.Click();
		}

		[When(@"I click the home link of the admin nav section")]
		public void WhenIClickTheHomeLinkOfTheAdminNavSection()
		{
			_adminNavSection.AdminNavIconHome.Click();
		}
	}
}

