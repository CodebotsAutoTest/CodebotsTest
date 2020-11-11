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
using System.Threading;
using SeleniumTests.PageObjects.TopbarAdminPageObject;
using SeleniumTests.Setup;
using TechTalk.SpecFlow;
using SeleniumTests.Enums;
using Xunit;

namespace SeleniumTests.Steps.BotWritten.TopBar
{
	[Binding]
	public class TopBarAdminStepDefinitions  : BaseStepDefinition
	{
		private readonly TopBarMenuAdmin _elementsAdminTopBar;
		private readonly ContextConfiguration _contextConfiguration;

		public TopBarAdminStepDefinitions(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
			_contextConfiguration = contextConfiguration;
			_elementsAdminTopBar = new TopBarMenuAdmin(contextConfiguration);
			// % protected region % [Add any additional setup options here] off begin
			// % protected region % [Add any additional setup options here] end
		}

		[StepDefinition(@"I click on the Topbar Link")]
		public void WhenIClickOnTheTopbarLink()
		{
			(new TopBarMenuAdmin(_contextConfiguration)).TopBarLink.Click();
		}

		[StepDefinition(@"I assert that the admin bar is on the (.*)")]
		public void ThenIAssertThatTheAdminBarIsOnTheAdmin(TopbarMenuType topBarMenuText)
		{
			Assert.Equal(topBarMenuText.ToString().ToLower(), _elementsAdminTopBar.TopBarLink.Text.ToLower());

			switch (topBarMenuText)
			{
				case TopbarMenuType.ADMIN:
					Assert.Equal($"{_baseUrl}/", _driver.Url);
					break;
				case TopbarMenuType.FRONTEND:
					Assert.Equal($"{_baseUrl}/admin", _driver.Url);
					break;
				default:
					throw new Exception($"Could not find {topBarMenuText} url");
			}
		}

		[StepArgumentTransformation]
		public static TopbarMenuType TransformStringToTopbarMenuTypeEnum(string topbarMenuType)
		{
			// case insensitive
			switch (topbarMenuType.ToLower())
			{
				case "admin":
					return TopbarMenuType.ADMIN;
				case "frontend":
					return TopbarMenuType.FRONTEND;
				default:
					throw new Exception($"{topbarMenuType}enum is not handled");
			}
		}
	}
}