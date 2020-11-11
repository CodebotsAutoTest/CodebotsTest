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
using OpenQA.Selenium;
using SeleniumTests.Setup;
using SeleniumTests.Utils;

namespace SeleniumTests.PageObjects
{
	///<summary>
	///The base section page, has functionalit requried by all other sections
	///</summary>
	public class BaseSection
	{
		protected IWebDriver driver;
		protected bool isFastText;

		/*
		 * The web element selector type enum,
		 * used for selecting and interacting with web elements
		 */
		protected enum SelectorType
		{
			CSS,
			XPath,
			ID
		}

		protected IDictionary<string, (string selector, SelectorType type)> selectorDict = new Dictionary<string, (string, SelectorType)>();

		// check that the entity appears on the page
		public bool ElementExists(string element)
		{
			try
			{
				FindElementExt(element);
				return true;
			}
			catch
			{
				return false;
			}
		}

		protected IWebElement FindElementExt(IWebElement baseElement, string elementName)
		{
			var selector = selectorDict[elementName];
			var elementSelector = (selector.type == SelectorType.CSS) ?
				By.CssSelector(selector.selector) : By.XPath(selector.selector);
			return baseElement.FindElement(elementSelector);
		}

		protected IWebElement FindElementExt(string elementName)
		{
			var selector = selectorDict[elementName];
			By elementSelector = null;

			switch (selector.type)
			{
				case SelectorType.CSS:
					elementSelector = By.CssSelector(selector.selector);
					break;
				case SelectorType.ID:
					elementSelector = By.Id(selector.selector);
					break;
				case SelectorType.XPath:
					elementSelector = By.XPath(selector.selector);
					break;
			}
			return driver.FindElementExt(elementSelector);
		}

		public By GetWebElementBy(string elementName)
		{
			var selector = selectorDict[elementName];

			return selector.type switch
			{
				SelectorType.CSS => By.CssSelector(selector.selector),
				SelectorType.XPath => By.XPath(selector.selector),
				_ => null,
			};
		}

		public BaseSection(ContextConfiguration contextConfiguration)
		{
			driver = contextConfiguration.WebDriver;
			isFastText = contextConfiguration.SeleniumSettings.FastText;
		}
	}
}
