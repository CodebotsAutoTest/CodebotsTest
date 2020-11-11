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
using SeleniumTests.Enums;

namespace SeleniumTests.Utils
{
	public static class WebElementUtils
	{
		public static bool CheckWebElementExists(IWebElement webElement)
		{
			try
			{
				return webElement.Displayed;
			}
			catch (NoSuchElementException)
			{
				return false;
			}
		}

		/// <summary>
		/// Check if WebElement is readonly
		/// </summary>
		/// <param name="webElement"></param>
		/// <returns></returns>
		public static bool IsReadonly(IWebElement webElement) => bool.Parse(webElement.GetAttribute("readonly"));

		/// <summary>
		/// Returns an OpenQA.Selenium.By that can be used to locate an IWebElement with an IWebDriver
		/// </summary>
		/// <param name="identifier">XPath, CssSelector, ClassName, Id</param>
		/// <param name="path">The path to the element</param>
		/// <returns></returns>
		public static By GetElementAsBy(SelectorPathType identifier, string path)
		{
			switch (identifier)
			{
				case SelectorPathType.XPATH:
					return By.XPath(path);
				case SelectorPathType.CSS:
				case SelectorPathType.CSS_SELECTOR:
					return By.CssSelector(path);
				case SelectorPathType.ClASSNAME:
					return By.ClassName(path);
				case SelectorPathType.ID:
					return By.Id(path);
				default:
					throw new Exception($"Cannot find Identifier named {identifier}, please check you are using a valid selector name");
			}
		}
	}
}