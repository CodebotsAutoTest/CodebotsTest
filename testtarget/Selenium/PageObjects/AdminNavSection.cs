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

using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using SeleniumTests.Setup;

namespace SeleniumTests.PageObjects
{
	///<summary>
	///The Admin Nav section represents the navigation bar that a admin would see
	///</summary>
	// % protected region % [Protected region incase the admin Nav Section should not extend the user Nav Section] off begin
	public class AdminNavSection : UserNavSection
	// % protected region % [Protected region incase the admin Nav Section should not extend the user Nav Section] end
	{
		// % protected region % [Add any web elements which are specific to logged in admins navigation] off begin
		// % protected region % [Add any web elements which are specific to logged in admins navigation] end

		public AdminNavSection(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
			InitializeSelectors();
		// % protected region % [Add any Admin nav specific construction ] off begin
		// % protected region % [Add any Admin nav specific construction ] end
		}

		public int TotalSubmenuLinks() => AdminNavSubLink.FindElements(By.TagName("li")).Count;

		public IList<IWebElement> GetAdminNavSubmenuLinks => AdminNavSubLink.FindElements(By.TagName("li")).ToList();

		public IWebElement GetAdminNavSubmenuLink(string linkText)
		{
			return GetAdminNavSubmenuLinks.FirstOrDefault(link => linkText.Equals(link.FindElement(By.TagName("a")).Text.ToLower()));
		}

		public IList<string> GetAdminNavSubmenuValues()
		{
			return GetAdminNavSubmenuLinks.Select(link => link.FindElement(By.TagName("a")).Text.ToLower()).ToList();
		}

		// Initialise all selectors
		private void InitializeSelectors()
		{
			selectorDict.Add("AdminNavMenu", (selector: "//nav[contains(@class,'nav--collapsed')]", type: SelectorType.XPath));

			// Admin Nav Links
			selectorDict.Add("AdminNavIconHome", (selector: "//a[contains(@class,'icon-home')]", type: SelectorType.XPath));
			selectorDict.Add("AdminNavHomeLink", (selector: "//a/span[contains(text(),'Home')]", type: SelectorType.XPath));
			selectorDict.Add("AdminNavLogoutLink", (selector: "//a/span[contains(text(),'Logout')]", type: SelectorType.XPath));

			// Admin Nav Sublinks
			selectorDict.Add("AdminNavSubLink", (selector: "//ul[contains(@class,'nav__sublinks--visible')]", type: SelectorType.XPath));

			// Admin Nav Icons
			selectorDict.Add("AdminNavToggle", (selector: "//a[contains(@class,'expand-icon')]", type: SelectorType.XPath));
			selectorDict.Add("AdminNavIconUsers", (selector: "//a[contains(@class,'icon-person-group')]", type: SelectorType.XPath));
			selectorDict.Add("AdminNavIconEntities", (selector: "//a[contains(@class,'icon-list')]", type: SelectorType.XPath));
			selectorDict.Add("AdminNavIconLogout", (selector: "//a[contains(@class,'icon-room')]", type: SelectorType.XPath));
		}

		// Admin Nav Menu section
		public IWebElement AdminNavMenu => FindElementExt("AdminNavMenu");

		// Admin Nav Links
		public IWebElement AdminNavHomeLink => FindElementExt("AdminNavHomeLink");
		public IWebElement AdminNavUsersLink => FindElementExt("AdminNavUsersLink");
		public IWebElement AdminNavLogoutLink => FindElementExt("AdminNavLogoutLink");
		public IWebElement AdminNavEntitiesLink => FindElementExt("AdminNavEntitiesLink");

		// Admin Nav Sublinks
		public IWebElement AdminNavSubLink => FindElementExt("AdminNavSubLink");
		public IWebElement AdminNavToggle => FindElementExt("AdminNavToggle");

		// Admin Nav link Icons
		public IWebElement AdminNavIconHome => FindElementExt("AdminNavIconHome");
		public IWebElement AdminNavIconUsers => FindElementExt("AdminNavIconUsers");
		public IWebElement AdminNavIconEntities => FindElementExt("AdminNavIconEntities");
		public IWebElement AdminNavIconLogout => FindElementExt("AdminNavIconLogout");

		// % protected region % [Add any methods which can be performed from the admin navigation section] off begin
		// % protected region % [Add any methods which can be performed from the admin navigation section] end
	}
}
