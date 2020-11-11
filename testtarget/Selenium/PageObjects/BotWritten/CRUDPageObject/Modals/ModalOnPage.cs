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
using SeleniumTests.Utils;

namespace SeleniumTests.PageObjects.BotWritten.CRUDPageObject.Modals
{
	 public class ModalOnPage
	{
		private readonly IWebDriver _driver;

		public ModalOnPage(ContextConfiguration contextConfiguration)
		{
			_driver = contextConfiguration.WebDriver;
		}

		public IWebElement ModalElement => _driver.FindElementExt(By.XPath("//*[contains(@class,'ReactModal__Content ReactModal__Content--after-open modal-content confirm-modal')]"));
		public IWebElement CloseModalButtton => _driver.FindElementExt(By.ClassName("modal--close"));
		public IWebElement ConfirmDeleteButton => _driver.FindElementExt(By.ClassName("modal--confirm"));
		public IWebElement CancelDeleteButton => _driver.FindElementExt(By.ClassName("modal--cancel"));
	}
}