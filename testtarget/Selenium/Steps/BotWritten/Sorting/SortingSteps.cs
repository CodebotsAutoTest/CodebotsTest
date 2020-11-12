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

using APITests.Utils;
using SeleniumTests.PageObjects.CRUDPageObject;
using SeleniumTests.Setup;
using SeleniumTests.Utils;
using TechTalk.SpecFlow;
using Xunit;

namespace SeleniumTests.Steps.BotWritten.Sorting
{
	[Binding]
	public sealed class SortingSteps : BaseStepDefinition
	{
		private readonly ContextConfiguration _contextConfiguration;

		 public SortingSteps(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
			_contextConfiguration = contextConfiguration;
			// % protected region % [Add any additional setup options here] off begin
			// % protected region % [Add any additional setup options here] end
		}

		[When("I sort (.*) by (.*)")]
		public void SortBy(string entityName, string columnName)
		{
			var page = new GenericEntityPage(entityName, _contextConfiguration);
			var columnHeader = page.ColumnHeader(columnName);
			var initialHeaderState = columnHeader.GetAttribute("class");
			columnHeader.Click();
			_driverWait.Until(_ => columnHeader.GetAttribute("class") != initialHeaderState);
		}

		[Then("I assert that (.*) in (.*) of type (.*) is properly sorted in (.*)")]
		public void AssertThatTypeProperlySortedIn(string columnName, string pageName, string attributeType, string sortOrder)
		{
			var page = new GenericEntityPage(pageName, _contextConfiguration);
			// Get original list
			var originalList = page.GetColumnContents(columnName);
			// Filter out non-alphanumeric contents except Date, Time, DateTime
			var sortingList = SortingUtils.FilterOutNonAlphanumeric(attributeType, originalList);
			switch (sortingList.Count )
			{
				case 0:
					Assert.Empty(sortingList); // When table content is all non-alphanumeric/non-date/times
					break;
				default:
					var sortedList = SortingUtils.AssertSorted(_contextConfiguration.CultureInfo, attributeType, sortingList, sortOrder);
					Assert.Equal(sortedList,sortingList, new EqualityListComparer<string>());
					break;
			}
		}
	}
}