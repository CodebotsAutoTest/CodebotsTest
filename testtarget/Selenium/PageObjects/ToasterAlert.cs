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
using SeleniumTests.Setup;
using SeleniumTests.Utils;
using SeleniumTests.Enums;

namespace SeleniumTests.PageObjects
{
	internal class ToasterAlert
	{
		protected IWebDriver _driver;
		protected IWait<IWebDriver> _wait;

		public static By ToasterBy => By.XPath("//div[@class='Toastify__toast Toastify__toast--success alert alert__success']");
		public static By ToasterCloseButtonBy => By.XPath("//div[@class='Toastify__toast Toastify__toast--success alert alert__success']/button");

		public IWebElement Toaster => _driver.FindElementExt(By.XPath("//div[@class='Toastify__toast Toastify__toast--success alert alert__success']"));

		private IWebElement ToasterCloseButton => _driver.FindElement(By.XPath("//div[@class='Toastify__toast Toastify__toast--success alert alert__success']/button"));

		public IWebElement ToasterBody => _driver.FindElement(By.XPath("//div[@role='alert' and @class='Toastify__toast-body']"));

		public ToasterAlert(ContextConfiguration contextConfiguration)
		{
			_driver = contextConfiguration.WebDriver;
			_wait = contextConfiguration.WebDriverWait;
		}

		public string GetToasterAlertMessage()
		{
			// Wait for the toaster body to display
			bool isDisplayedToasterBody = WaitUtils.elementState(_wait, By.XPath("//div[@role='alert' and @class='Toastify__toast-body']"), ElementState.VISIBLE);

			if (isDisplayedToasterBody)
			{
				return ToasterBody.Text;
			}
			return string.Empty;
		}

		public void CloseToasterAlert()
		{
			WaitUtils.elementState(_wait, ToasterCloseButtonBy, ElementState.VISIBLE);
			ToasterCloseButton.Click();
		}
	}
}

