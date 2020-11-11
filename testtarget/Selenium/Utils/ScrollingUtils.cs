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
using OpenQA.Selenium.Interactions;

namespace SeleniumTests.Utils
{
	internal static class ScrollingUtils
	{
		/// <summary>
		/// Will find and scroll to an element given an OpenQA.Selenium.By identifier
		/// </summary>
		/// <param name="driver">The webdriver to use to control the page</param>
		/// <param name="elementBy">The OpenQA.Selenium.By element identifier</param>
		public static void scrollToElement(IWebDriver driver, IWebElement element)
		{
			var actions = new Actions(driver);
			actions.MoveToElement(element);
			actions.Perform();
		}

		/// <summary>
		/// Will find and scroll to an element given an OpenQA.Selenium.By identifier
		/// </summary>
		/// <param name="driver">The webdriver to use to control the page</param>
		/// <param name="elementBy">The OpenQA.Selenium.By element identifier</param>
		public static void scrollToElement(IWebDriver driver, By elementBy)
		{
			var element = driver.FindElement(elementBy);
			var actions = new Actions(driver);
			actions.MoveToElement(element);
			actions.Perform();
		}

		/// <summary>
		/// Will scroll the page up or down given an amount of pixels to move by
		/// </summary>
		/// <param name="driver"></param>
		/// <param name="pixelDistance"></param>
		public static void scrollUpOrDown(IWebDriver driver, int pixelDistance)
		{
			var js = (IJavaScriptExecutor)driver;
			js.ExecuteScript($"window.scrollBy(0,{pixelDistance})");
		}
	}
}