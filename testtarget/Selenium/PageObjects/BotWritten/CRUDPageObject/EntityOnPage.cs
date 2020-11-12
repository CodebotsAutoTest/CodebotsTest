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
using SeleniumTests.PageObjects.BotWritten.CRUDPageObject.Modals;
using SeleniumTests.Setup;

namespace SeleniumTests.PageObjects.CRUDPageObject
{
	public class EntityOnPage : BaseSection
	{
		protected IWebElement ListElement { get; set; }
		protected ModalOnPage _modalOnPage;

		public EntityOnPage(ContextConfiguration contextConfiguration, IWebElement listElement) : base(contextConfiguration)
		{
			ListElement = listElement;
			_modalOnPage = new ModalOnPage(contextConfiguration);
		}

		public readonly By DeleteButtonBy = By.XPath(".//*[contains(@class,'icon-bin-full')]");
		public IWebElement DeleteButton => ListElement.FindElement(DeleteButtonBy);
		public readonly By EditButtonBy = By.XPath(".//*[contains(@class,'icon-edit')]");
		public IWebElement EditButton => ListElement.FindElement(EditButtonBy);
		public readonly By SelectCheckboxBy = By.XPath(".//td[@class='select-box']//input");
		public IWebElement SelectCheckbox => ListElement.FindElement(SelectCheckboxBy);
		public readonly By ViewButtonBy = By.XPath(".//*[contains(@class,'icon-look')]");
		public IWebElement ViewButton => ListElement.FindElement(ViewButtonBy);

		public void SelectItem(bool select)
		{
			if (SelectCheckbox.Selected != select)
			{
				SelectCheckbox.Click();
			}
		}

		public void EditItem() => EditButton.Click();

		public void DeleteItem()
		{
			DeleteButton.Click();
			_modalOnPage.ConfirmDeleteButton.Click();
		}
	}
}