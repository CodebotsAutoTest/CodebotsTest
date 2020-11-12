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
using OpenQA.Selenium;
using SeleniumTests.PageObjects.CRUDPageObject;
using SeleniumTests.PageObjects.BotWritten.UserPageObjects;
using SeleniumTests.Setup;
using SeleniumTests.Utils;
using SeleniumTests.Enums;
using TechTalk.SpecFlow;
using Xunit;

namespace SeleniumTests.Steps.BotWritten.Utility
{
	[Binding]
	public sealed class VerifySteps  : BaseStepDefinition
	{
		private readonly ContextConfiguration _contextConfiguration;
		private readonly GenericEntityPage _genericEntityPage;

		public VerifySteps(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
			_contextConfiguration = contextConfiguration;
			_genericEntityPage = new GenericEntityPage(_contextConfiguration);
		}

		[StepDefinition("I assert that I am redirected from (.*) to login page")]
		public void AssertRedirectedFromToLoginPage (String entityName)
		{
			LoginPage page = new LoginPage (_contextConfiguration);
			var expectedPage = page.Url + "?redirect=/admin/" + entityName.ToLower();
			_driverWait.Until(x => expectedPage == x.Url);
		}

		[ObsoleteAttribute]
		[StepDefinition("I expect the element with (.*) of (.*) to contain the text '(.*)'")]
		public void ExpectElementByToContainText(SelectorPathType selector, string path, string expectedText)
		{
			By elementBy = WebElementUtils.GetElementAsBy(selector, path);
			var elementText = _driver.FindElement(elementBy).Text;
			Assert.Equal(expectedText, elementText);
		}

		[ObsoleteAttribute]
		[StepDefinition("I expect the element with (.*) of (.*) to contain the date (.*)")]
		public void ExpectAnElementToBePresentBy(SelectorPathType selector, string path, string expectedDate)
		{
			By elementBy = WebElementUtils.GetElementAsBy(selector, path);
			var elementText = _driver.FindElement(elementBy).Text;
			Assert.Equal(expectedDate, elementText);
		}

		[ObsoleteAttribute]
		[StepDefinition("I expect the element with (.*) of (.*) to be visible")]
		public void ExpectAnElementToBePresentBy(SelectorPathType selector, string path)
		{
			By elementBy = WebElementUtils.GetElementAsBy(selector, path);
			Assert.True(_driver.FindElement(elementBy).Displayed);
		}

		[StepDefinition("The string (.*) is in each row of the the collection content")]
		public void TheStringToSearchIsInEachOfTheCollectionContent(string stringToSearch)
		{
			bool isInEachRow = _genericEntityPage.TheCommonStringIsInEachOfTheRowContent(stringToSearch, _genericEntityPage.CollectionTable);
			Assert.True(isInEachRow);
		}
	}
}