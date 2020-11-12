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
using SeleniumTests.PageObjects.CRUDPageObject;
using SeleniumTests.Setup;
using SeleniumTests.Utils;
using SeleniumTests.Enums;
using TechTalk.SpecFlow;
using Xunit;

// % protected region % [Add any additional imports here] off begin
// % protected region % [Add any additional imports here] end

namespace SeleniumTests.Steps.BotWritten
{
	[Binding]
	public sealed class EntityListSteps  : BaseStepDefinition
	{
		private readonly GenericEntityPage _genericEntityPage;
		private readonly ContextConfiguration _contextConfiguration;

		// % protected region % [Add any additional fields here] off begin
		// % protected region % [Add any additional fields here] end
		 public EntityListSteps(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
			_contextConfiguration = contextConfiguration;
			_genericEntityPage = new GenericEntityPage(_contextConfiguration);
			// % protected region % [Add any additional setup options here] off begin
			// % protected region % [Add any additional setup options here] end
		}

		[Then(@"I select all items in the collection")]
		public void IselectAllItemsInTheCollection()
		{
			var totalNumOfItems = _genericEntityPage.TotalEntities();
			_genericEntityPage.ClickSelectAllItemsButton();
			Assert.Equal(totalNumOfItems, _genericEntityPage.NumberOfItemsSelected());
		}

		[Given("I navigate to the (.*) backend page")]
		public void GivenINavigateToTheBackendPage(string pageName)
		{
			new GenericEntityPage(pageName, _contextConfiguration).Navigate();
		}

		[Then("I assert that I am on the (.*) backend page")]
		public void AssertOnBackendPage(string pageName)
		{
			var page = new GenericEntityPage(pageName.RemoveWordsSpacing(), _contextConfiguration);
			page.AssertOnPage();
		}

		[Then("(.*) entities on current page should be selected")]
		public void numEntitiesOnCurrentPageShouldBeSelected(int numSelected)
		{
			int NumCheckedEntitiesOnPage = _genericEntityPage.NumCheckedEntitiesOnPage();
			Assert.Equal(numSelected, NumCheckedEntitiesOnPage);
		}

		[Then(@"I click the bulk bar cancel button")]
		public void ThenIClickTheCancelButtonOnTheBar()
		{
			_genericEntityPage.BulkCancelButton.Click();
		}

		[Then("The bulk options bar shows up with correct information")]
		public void TheBulkOptionsBarShowsUpWIthCorrectInformation()
		{
			WaitUtils.elementState(_driverWait, _genericEntityPage.GetWebElementBy("BulkDeleteButton"), ElementState.EXISTS);
			WaitUtils.elementState(_driverWait, _genericEntityPage.GetWebElementBy("BulkExportButton"), ElementState.EXISTS);
			Assert.Equal(_genericEntityPage.NumEntitiesOnPage(), _genericEntityPage.NumberOfItemsSelected());
		}

		[Then(@"I assert that the Created and Modified datepickers on the (.*) page are readonly")]
		public void ThenIAssertThatTheCreatedAndModifiedDatepickersOnThePageAreReadonly(string entityName)
		{
			var entityOnEditPage = new GenericEntityEditPage(entityName, _contextConfiguration);
			Assert.True(WebElementUtils.IsReadonly(entityOnEditPage.CreateAtDatepickerField));
			Assert.True(WebElementUtils.IsReadonly(entityOnEditPage.ModifiedAtDatepickerField));
		}

		[Then("The filter panel shows up with correct information")]
		public void TheFilterPanelShowsUpWithCorrectInformation()
		{
			Assert.True(_genericEntityPage.ElementExists("CollectionFilterPanel"));
			Assert.True(_genericEntityPage.ElementExists("FilterCreatedInput"));
			Assert.True(_genericEntityPage.ElementExists("FilterModifiedInput"));
			Assert.True(_genericEntityPage.ElementExists("ApplyFilterButton"));
			Assert.True(_genericEntityPage.ElementExists("ClearFilterButton"));
		}


		[When("I click on the next page button and validate page content")]
		public void WhenIClickTheNextButton()
		{
			int pageNumberBeforeClickingNextPage = _genericEntityPage.CurrentPageNumber();
			_genericEntityPage.NextPageButton.Click();
			int pageNumberAfterClickingNextPage =  _genericEntityPage.CurrentPageNumber();
			// Check if you were already on the last page before clicking next
			if (pageNumberBeforeClickingNextPage == _genericEntityPage.NumberOfPages())
			{
				// and if you were, assert that the page number has not increased after
				// pressing the next page button.
				Assert.True(pageNumberAfterClickingNextPage == _genericEntityPage.NumberOfPages());
			}
			else
			{
				// Otherwise, assert that the page number has increased by 1.
				Assert.True(pageNumberBeforeClickingNextPage + 1 == pageNumberAfterClickingNextPage);
			}
		}

		[When("I click on the previous page button and validate page content")]
		public void WhenIClickThePreviousButton()
		{
			int pageNumberBeforeClickingNextPage =  _genericEntityPage.CurrentPageNumber();
			_genericEntityPage.PrevPageButton.Click();
			int pageNumberAfterClickingNextPage =  _genericEntityPage.CurrentPageNumber();
			// Check if you were already on the first page before clicking previous page button.
			if (pageNumberBeforeClickingNextPage == 1) {
				// and if you were, assert that the page number has not decreased
				// after clicking the prev page button.
				Assert.Equal(1,pageNumberAfterClickingNextPage);
			} else {
				// Otherwise, assert that the page number has decreased by 1.
				Assert.Equal(pageNumberAfterClickingNextPage+1, pageNumberBeforeClickingNextPage);
			}
		}

		[When("I click on the last page button and validate page content")]
		public void WhenIClickTheLastButton()
		{
			_genericEntityPage.LastPageButton.Click();
			Assert.Equal(_genericEntityPage.CurrentPageNumber(), _genericEntityPage.NumberOfPages());
		}

		[When("I click on the first page button and validate page content")]
		public void WhenIClickTheFirstButton()
		{
			_genericEntityPage.FirstPageButton.Click();
			Assert.Equal(1,_genericEntityPage.CurrentPageNumber());
		}

		[When("I select all entities on (.*)")]
		public void WhenIClickTheSelectAllCheckbox(string selectType)
		{
			if (selectType.Contains("all pages"))
			{
				_genericEntityPage.ClickSelectAllItemsButton();
			}
			else
			{
				_genericEntityPage.SelectAllCheckbox.Click();
			}
		}


		[When("I click the filter Button on the (.*) page")]
		public void WhenIClickOnFilterButton(string entityName)
		{
			var page = new GenericEntityPage(entityName, _contextConfiguration);
			page.FilterButton.Click();
		}

		[When("I enter the string (.*) to search and click filter button")]
		public void WhenIEnterStringToSearch(string stringToSearch)
		{
			_genericEntityPage.SearchInput.SendKeys(stringToSearch);
			_genericEntityPage.SearchButton.Click();
		}

		[When("I unselect (.*) entities")]
		public void WhenIUnselectEntities(int numEntities)
		{
			while(_genericEntityPage.NumUncheckedEntitiesOnPage() < numEntities)
			{
				_genericEntityPage.CheckedEntities().ToList()[0].SelectCheckbox.Click();
			}
		}

		[When("I click the select all check box")]
		public void WhenIClickTheSelectAllCheckBox()
		{
			_genericEntityPage.SelectAllCheckbox.Click();
		}

		// % protected region % [Add any additional methods here] off begin
		// % protected region % [Add any additional methods here] end
	}
}
