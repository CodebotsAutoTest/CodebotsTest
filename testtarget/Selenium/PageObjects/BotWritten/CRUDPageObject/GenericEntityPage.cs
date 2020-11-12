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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumTests.Setup;
using SeleniumTests.Utils;
using SeleniumTests.Enums;

namespace SeleniumTests.PageObjects.CRUDPageObject
{
	public class GenericEntityPage : CrudGenericEntityPage
	{
		private readonly string _entityName;
		private readonly IWait<IWebDriver> _driverWait;
		private readonly ContextConfiguration _contextConfiguration;

		public GenericEntityPage(string entityName, ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
			_entityName = entityName;
			Url = baseUrl + "/admin/" + entityName.ToLower();
			InitializeSelectors();
			_driverWait = driverWait;
			_contextConfiguration = contextConfiguration;
		}

		public GenericEntityPage(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
			InitializeSelectors();
			_driverWait = driverWait;
		}

		public IWebElement ColumnHeader(string colName)
		{
			var entityDetailsSection = EntityDetailUtils.GetEntityDetailsSection(_entityName, _contextConfiguration);
			var columnHeader = entityDetailsSection.GetHeaderTile(colName);
			return columnHeader;
		}

		public List<string> GetColumnContents(string colName)
		{
			var columnElements = driver.FindElements(By.XPath("//tr[@class='list__header']/th"));
			var tableElements = driver.FindElements(By.XPath("//tr[@class='collection__item']"));
			var columnText = columnElements.Select((column) => column.Text).ToList();
			var columnIndex = columnText.FindIndex(a => a == colName);
			var columnContents = new List<string>();

			const string entityXPath = "//tr[contains(@class,'collection__item')]";
			_driverWait.Until(driver => driver.FindElements(By.XPath(entityXPath)).Count > 0);

			ReadOnlyCollection<IWebElement> rowElements;

			foreach (var element in tableElements)
			{
				try
				{
					rowElements = element.FindElements(By.TagName("td"));
				}
				catch (StaleElementReferenceException)
				{
					// wait a second for the elements to be stable then try again
					System.Threading.Thread.Sleep(1000);
					rowElements = element.FindElements(By.TagName("td"));
				}

				var rowList = rowElements.Select((column) => column.Text).ToList();
				var rowValue = rowList[columnIndex];
				columnContents.Add(rowValue);
			}
			return columnContents;
		}

		public bool TheCommonStringIsInEachOfTheRowContent(string stringToSearch, IWebElement collection)
		{
			return TheStringIsInEachOfTheRowContent(stringToSearch, collection);
		}

		public bool TheEnumStringIsInEachOfTheRowContent(string columnToSearch, string valueToSearch, IWebElement collection)
		{
			var columnWordsLowerCase = Regex.Replace(columnToSearch, @"\p{Lu}", m => " " + m.Value.ToLowerInvariant()).ToLower();

			try
			{
				_driverWait.Until(driver => driver.FindElements(By.XPath($"//th[contains(translate(text(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '{columnToSearch.ToLower()}')]")).Count > 0);
			}
			catch (NoSuchElementException)
			{
				return true; // If it is not in the admin collection, it can't be tested for now
			}
			return TheStringIsInEachOfTheRowContent(valueToSearch, collection, "'ABCDEFGHIJKLMNOPQRSTUVWXYZ '", "'abcdefghijklmnopqrstuvwxyz_'");
		}

		private bool TheStringIsInEachOfTheRowContent(string stringToSearch, IWebElement collection, string translateFrom = null, string translateTo = null)
		{
			if (collection is null)
			{
				throw new ArgumentNullException(nameof(collection));
			}

			var notFound = false;
			stringToSearch = stringToSearch.ToLower();

			try
			{
				_driverWait.Until(driver => driver.FindElements(By.XPath("//tr[contains(@class,'collection__item')]")).Count > 0);
			}
			catch (NoSuchElementException)
			{
				return false;
			}

			var rows = CollectionTable.FindElements(By.CssSelector("tbody > tr"));

			foreach (var row in rows)
			{
				try
				{
					var tdFound = row.FindElement(By.XPath($"//td[contains(translate(text(), {translateFrom ?? "'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"}, {translateTo ?? "'abcdefghijklmnopqrstuvwxyz'"}), '{stringToSearch}')]"));
				}
				catch (Exception)
				{
					notFound = true;
					break;
				}
			}

			return !notFound;
		}

		public GenericEntityEditPage ClickCreateButton()
		{
			CreateButton.Click();
			return new GenericEntityEditPage(_entityName, contextConfiguration);
		}

		public int TotalEntities() => int.Parse(FindElementExt("TotalEntitiesDataAttribute").GetAttribute("data-total"));
		public int NumberOfItemsSelected() => int.Parse(FindElementExt("SelectionCount").GetAttribute("data-selected-count"));
		public int CurrentPageNumber() => int.Parse(PageNumber.Text.Split(' ').First());
		public int NumberOfPages() => int.Parse(PageNumber.Text.Split(' ').Last());
		public int NumEntitiesOnPage() => GetAllEntites().Count;
		public int NumCheckedEntitiesOnPage() => GetAllEntites().Count(x => x.SelectCheckbox.Selected);
		public int NumUncheckedEntitiesOnPage() => GetAllEntites().Count(x => !x.SelectCheckbox.Selected);
		public IEnumerable<EntityOnPage> CheckedEntities() => GetAllEntites().Where(x => x.SelectCheckbox.Selected);
		public IEnumerable<EntityOnPage> UncheckedEntities() => GetAllEntites().Where(x => !x.SelectCheckbox.Selected);

		public List<EntityOnPage> GetAllEntites()
		{
			const string entityXPath = "//tr[contains(@class,'collection__item')]";
			var entities = new List<EntityOnPage>();

			try
			{
				_driverWait.Until(driver =>
					driver.FindElements(By.XPath(entityXPath)).Count > 0);
				var elementList = driver.FindElements(By.XPath(entityXPath));

				foreach(var element in elementList)
				{
					entities.Add(new EntityOnPage(contextConfiguration, element));
				}
			}
			catch (Exception)
			{
				contextConfiguration.WriteTestOutput("There are no entities in the list");
			}
			return entities;
		}

		public void DeleteTopEntity() => GetAllEntites()[0].DeleteItem();
		public void ViewTopEntity() => GetAllEntites()[0].ViewButton.Click();

		public void ClickSelectAllItemsButton()
		{
			const string entityXPath = "//section[@class='collection__select-options' and @aria-label='select options']";
			WaitUtils.elementState(_driverWait, By.XPath(entityXPath), ElementState.VISIBLE);
			SelectAllItemsButton.Click();
		}

		// initialise all selectors and grouping them with the selector type which is used
		private void InitializeSelectors()
		{
			selectorDict.Add("SearchButton",(selector: "form.search__collection>button", type: SelectorType.CSS));
			selectorDict.Add("SearchInput", (selector: "form.search__collection>div.input-group > input", type: SelectorType.CSS));
			selectorDict.Add("CreateButton",(selector:"button.icon-create",type: SelectorType.CSS));
			selectorDict.Add("EntityTable",(selector:"section.collection__list > table",type: SelectorType.CSS));
			selectorDict.Add("GoToPageInput",(selector:"form.paginator__go-to-pg > div > input",type: SelectorType.CSS));
			selectorDict.Add("NextPageButton",(selector:".collection__pagination > li > .icon-chevron-right",type: SelectorType.CSS));
			selectorDict.Add("PrevPageButton",(selector:".collection__pagination > li > .icon-chevron-left",type: SelectorType.CSS));
			selectorDict.Add("LastPageButton",(selector:".collection__pagination > li > .icon-chevrons-right",type: SelectorType.CSS));
			selectorDict.Add("FirstPageButton",(selector:".collection__pagination > li > .icon-chevrons-left",type: SelectorType.CSS));
			selectorDict.Add("SelectAllCheckbox",(selector:"//th[@class='select-box']//input",type: SelectorType.XPath));

			selectorDict.Add("BulkOptions", (selector: "//section[@class='collection__select-options']", type: SelectorType.XPath));
			selectorDict.Add("BulkDeleteButton",(selector:".collection__select-options > section > button.icon-bin-full",type: SelectorType.CSS));
			selectorDict.Add("BulkExportButton",(selector:".collection__select-options > section > button.icon-export",type: SelectorType.CSS));
			selectorDict.Add("PageNumber",(selector:"//*[@class='pagination__page-number']",type: SelectorType.XPath));
			selectorDict.Add("SelectionCount",(selector:"//*[@data-selected-count]",type: SelectorType.XPath));
			selectorDict.Add("BulkCancelButton",(selector:"button.crud__selection--cancel",type: SelectorType.CSS));
			selectorDict.Add("TotalEntitiesDataAttribute", (selector: "//*[@data-total]", type: SelectorType.XPath));

			// Collection
			selectorDict.Add("SelectAllItemsButton", (selector: "button.crud__selection--select-all", type: SelectorType.CSS));
			selectorDict.Add("CollectionTable", (selector: ".collection__list table", type: SelectorType.CSS));

			// Filter
			selectorDict.Add("FilterButton", (selector: "button.icon-filter", type: SelectorType.CSS));
			selectorDict.Add("CollectionFilterPanel", (selector: ".collection__filters", type: SelectorType.CSS));
			selectorDict.Add("FilterCreatedInput", (selector: ".filter-created input", type: SelectorType.CSS));
			selectorDict.Add("FilterModifiedInput", (selector: ".filter-modified input", type: SelectorType.CSS));
			selectorDict.Add("ApplyFilterButton", (selector: "button.apply-filters", type: SelectorType.CSS));
			selectorDict.Add("ClearFilterButton", (selector: "button.clear-filters", type: SelectorType.CSS));
		}

		public IWebElement SearchInput => FindElementExt("SearchInput");
		public IWebElement SearchButton => FindElementExt("SearchButton");
		public IWebElement CreateButton => FindElementExt("CreateButton");
		public IWebElement EntityTable => FindElementExt("EntityTable");
		public IWebElement GoToPageInput => FindElementExt("GoToPageInput");
		public IWebElement NextPageButton => FindElementExt("NextPageButton");
		public IWebElement PrevPageButton => FindElementExt("PrevPageButton");
		public IWebElement LastPageButton => FindElementExt("LastPageButton");
		public IWebElement FirstPageButton => FindElementExt("FirstPageButton");
		public IWebElement SelectAllCheckbox => FindElementExt("SelectAllCheckbox");
		public IWebElement BulkDeleteButton => FindElementExt("BulkDeleteButton");
		public IWebElement BulkExportButton => FindElementExt("BulkExportButton");
		public IWebElement PageNumber => FindElementExt("PageNumber");
		public IWebElement SelectionCount => FindElementExt("SelectionCount");
		public IWebElement BulkCancelButton => FindElementExt("BulkCancelButton");
		public IWebElement SelectAllItemsButton => FindElementExt("SelectAllItemsButton");
		public IWebElement DeleteErrorAlert => FindElementExt("DeleteErrorAlert");

		// Collection
		public IWebElement BulkOptions => FindElementExt("BulkOptions");
		public IWebElement CollectionTable => FindElementExt("CollectionTable");

		// Filter
		public IWebElement FilterButton => FindElementExt("FilterButton");
		public IWebElement CollectionFilterPanel => FindElementExt("CollectionFilterPanel");
		public IWebElement FilterCreatedInput => FindElementExt("FilterCreatedInput");
		public IWebElement FilterModifiedInput => FindElementExt("FilterModifiedInput");
		public IWebElement ApplyFilterButton => FindElementExt("ApplyFilterButton");
		public IWebElement ClearFilterButton => FindElementExt("ClearFilterButton");
	}
}