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
using OpenQA.Selenium.Support.UI;
using SeleniumTests.Setup;
using Xunit;

namespace SeleniumTests.PageObjects
{
	public interface IBasePage
	{
		string Url { get; set; }

		BasePage AssertOnPage();
		bool ElementExists(string element);
		List<IWebElement> GetReadonlyInputFieldAttributes();
		By GetWebElementBy(string elementName);
		BasePage Navigate();
	}

	///<summary>
	///The Base page object, every page is extended from this page, contains information shared across every page
	///</summary>
	public class BasePage : BaseSection, IBasePage
	{
		protected readonly string baseUrl;
		protected IWait<IWebDriver> driverWait;
		public virtual string Url { get; set; }
		protected ContextConfiguration contextConfiguration;

		public BasePage(ContextConfiguration currentContext) : base (currentContext)
		{
			contextConfiguration = currentContext;
			baseUrl = contextConfiguration.BaseUrl;
			driver = contextConfiguration.WebDriver;
			driverWait = contextConfiguration.WebDriverWait;
		}

		 // % protected region % [Add any Methods which can be done on page] off begin
		// % protected region % [Add any Methods which can be done on page] end

		//goto Home -> go's to base URL
		public BasePage Navigate()
		{
			driver.Navigate().GoToUrl(Url);
			return this;
		}

		/* Compares the url of the driver to the url of this page object after stripping
		 * any trailing whitespaces and forward slashes. */
		public BasePage AssertOnPage() {
			var thisUrl = Url.Trim('/');
			driverWait.Until(driver => driver.Url.Trim('/') == thisUrl);
			Assert.Equal(driver.Url.Trim('/'), thisUrl);
			return this;
		}

		protected IEnumerable<IWebElement> GetAllReadOnlyElements() => selectorDict.Where(e => e.Key != "UserPasswordElement" && e.Key != "UserConfirmPasswordElement").Select(e => FindElementExt(e.Key));

		public List<IWebElement> GetReadonlyInputFieldAttributes()
		{
			driverWait.Until(_ => GetAllReadOnlyElements().ToList().Count > 0);
			return GetAllReadOnlyElements().ToList();
		}
	}
}
