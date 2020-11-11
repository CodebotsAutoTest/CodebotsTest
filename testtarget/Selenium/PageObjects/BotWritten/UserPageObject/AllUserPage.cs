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

using OpenQA.Selenium;
using SeleniumTests.Setup;
using System.Collections.Generic;

// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace SeleniumTests.PageObjects.BotWritten.UserPageObject
{
	public class AllUserPage : BasePage
	{
		public List<string> ExpectedColumnHeadings = new List<string>{"Type", "Email", "Activated"};
		public override string Url => baseUrl + "/admin/user";
		public IWebElement ListHeader => FindElementExt("ListHeader");
		public IWebElement CreateNewButton => FindElementExt("CreateNewButton");
		public IWebElement SelectUserTypeModal => FindElementExt("SelectUserTypeModal");
		public IWebElement SelectUserTypeDropdown => FindElementExt("SelectUserTypeDropdown");
		public IEnumerable<IWebElement> ListHeaders => ListHeader.FindElements(By.CssSelector("th.sortable"));

		public AllUserPage(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
			InitializeSelectors();
		}

		private void InitializeSelectors()
		{
			selectorDict.Add("CreateNewButton", (selector: "button.icon-create", type: SelectorType.CSS));
			selectorDict.Add("ListHeader", (selector: "tr.list__header", type: SelectorType.CSS));
			selectorDict.Add("SearchBox", (selector: "search__collection", type: SelectorType.CSS));
			selectorDict.Add("SelectUserTypeModal", (selector: "div.ReactModal__Overlay", type: SelectorType.CSS));
			selectorDict.Add("SelectUserTypeDropdown", (selector: "div.ReactModal__Overlay div.input-group__dropdown", type: SelectorType.CSS));
		}
	}
}