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
using System.Threading;
using APITests.EntityObjects.Models;
using APITests.Factories;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SeleniumTests.PageObjects.Components;
using SeleniumTests.PageObjects.CRUDPageObject;
using SeleniumTests.Setup;
using SeleniumTests.Utils;
using SeleniumTests.Enums;
using TechTalk.SpecFlow;
using Xunit;

namespace SeleniumTests.Steps.BotWritten.Filter
{
	[Binding]
	public sealed class FilterSteps  : BaseStepDefinition
	{
		private readonly GenericEntityPage _genericEntityPage;
		private readonly ContextConfiguration _contextConfiguration;
		private readonly bool _isFastText;
		private EntityFactory _entityFactory;
		private BaseEntity _createdEntityForTestFiltering;

		public FilterSteps(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
			_contextConfiguration = contextConfiguration;
			_genericEntityPage = new GenericEntityPage(_contextConfiguration);
			_isFastText = contextConfiguration.SeleniumSettings.FastText;
		}

		[When("I enter the (.*) date filter starting from (.*) days ago")]
		public void WhenIFilterCreatedDateStartingFromDaysAgo(string dateInputType, int addDays)
		{
			var today = DateTime.Now.Date;
			var date = today.AddDays(-addDays);
			new DatePickerComponent(_contextConfiguration, $"filter-{dateInputType.ToLower()} .flatpickr-input").SetDateRange(today, date);
		}

		[When("I enter the (.*) date filter starting in (.*) days")]
		public void WhenIFilterCreatedDateStartingInDays(string dateInputType, int addDays)
		{
			var startDate = DateTime.Now.Date.AddDays(addDays);
			var endDate = DateTime.Now.Date.AddDays(addDays + 1);
			new DatePickerComponent(_contextConfiguration, $"filter-{dateInputType.ToLower()} .flatpickr-input").SetDateRange(startDate, endDate);
		}

		[When("I apply the current filter")]
		public void WhenIApplyCurrentFilter()
		{
			_genericEntityPage.ApplyFilterButton.Click();
		}

		[StepDefinition("Each row has been (.*) within the last (.*) days")]
		public void EachRowHasBeenWithinTheLastDays(string filterInputType, int days)
		{
			// wait for the collection to exist on the page
			WaitUtils.elementState(_driverWait, By.XPath("//tr[contains(@class,'collection__item')]"), ElementState.EXISTS);
			// Replace with clientside and serverside test
			Thread.Sleep(500);

			// get the rows and attributes into the correct format
			var rows = _genericEntityPage.CollectionTable.FindElements(By.CssSelector("tbody > tr"));
			var dateAttributeRows = rows.Select(x => DateTime.Parse(x.GetAttribute($"data-{filterInputType}")));

			// set how far back we will be looking
			var historicDate = DateTime.Now.Date.AddDays(-days - 1);

			dateAttributeRows.Should().NotBeEmpty();

			// all  the dates present should be after the specified date
			foreach (var date in dateAttributeRows)
			{
				date.Should().BeAfter(historicDate);
			};
		}

		[StepDefinition("No row is within the applied current date range filters")]
		public void NoRowIsWithinTheDataRangeFilters()
		{
			var isEmpty = WaitUtils.elementState(_driverWait, By.XPath("//tr[contains(@class,'collection__item')]"), ElementState.NOT_EXIST);
			Assert.True(isEmpty);
		}

		[When("I enter the enum filter (.*) with the same value in the entity just created and click")]
		public void WhenEnterEnumFilterWithTheValueCreated(string enumColumnName)
		{
			if (_entityFactory == null)
			{
				throw new Exception("_entityFactory has not been instantiated");
			}

			string enumValue = _entityFactory.GetEnumValue(_createdEntityForTestFiltering, enumColumnName);
			TypingUtils.InputEntityAttributeByClass(_driver, $"filter-{enumColumnName}", enumValue, _isFastText);

			var builder = new Actions(_driver);
			builder.SendKeys(Keys.Enter).SendKeys(Keys.Escape).Perform();
			_genericEntityPage.ApplyFilterButton.Click();
		}

		[Then("The enum value created for (.*) is in each row of the the collection content")]
		public void TheStringToSearchIsInEachOfTheCollectionContent(string enumColumnName)
		{
			var enumValue = _entityFactory.GetEnumValue(_createdEntityForTestFiltering, enumColumnName);
			var isInEachRow = _genericEntityPage.TheEnumStringIsInEachOfTheRowContent(enumColumnName, enumValue, _genericEntityPage.CollectionTable);
			Assert.True(isInEachRow);
		}

		[Given("I have 1 valid (.*) entities with fixed string values (.*)")]
		public void IHaveValidEntitiesWithFixedStrValues(string entityName, string fixedValues)
		{
			_entityFactory = new EntityFactory(entityName, fixedValues);
			_createdEntityForTestFiltering = _entityFactory.ConstructAndSave(_testOutputHelper, 1)[0];
		}
	}
}