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
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests.Utils
{
	internal static class NavigationUtils
	{
		/// <summary>
		/// Navigates to a given url and wait for the page to load
		/// </summary>
		/// <param name="driver"></param>
		/// <param name="wait"></param>
		/// <param name="url"></param>
		public static void GoToUrl(IWebDriver driver, IWait<IWebDriver> wait, string url)
		{
			driver.Navigate().GoToUrl(url);
			WaitUtils.waitForPage(wait);
		}

		/// <summary>
		/// Refreshes the current page and wait for the reload to finish
		/// </summary>
		/// <param name="driver"></param>
		/// <param name="wait"></param>
		public static void RefreshPage(IWebDriver driver, IWait<IWebDriver> wait)
		{
			driver.Navigate().Refresh();
			WaitUtils.waitForPage(wait);
		}
	}
}