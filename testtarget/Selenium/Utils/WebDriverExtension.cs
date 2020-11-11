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
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests.Utils
{
	public static class ExtendedWebDriver
	{
		private const int timeoutInSeconds = 5; //Change to confid timeout

		public static IWebElement FindElementExt(this IWebDriver driver, By by) // Change to use a new class which is an extended webDriver and add these there as the default method
		{
			if (timeoutInSeconds > 0)
			{
				var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
				return wait.Until(drv => drv.FindElements(by).FirstOrDefault());
			}
		}

		public static IEnumerable<IWebElement> FindElementsExt(this IWebDriver driver, By by)
		{
			if (timeoutInSeconds > 0)
			{
				var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
				wait.Until(drv => drv.FindElements(by).Any());
				return driver.FindElements(by);
			}
		}

		public static void GoToUrlExt(this IWebDriver driver, string url)
		{
			driver.Navigate().GoToUrl(url);
			new WebDriverWait(driver, TimeSpan.FromSeconds(20)).Until(drvr =>
				drvr.Url.Equals(url));
		}
	}
}