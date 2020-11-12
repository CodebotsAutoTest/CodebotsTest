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
using SeleniumTests.Factories;
using SeleniumTests.PageObjects.CRUDPageObject.PageDetails;
using SeleniumTests.Setup;
using SeleniumTests.Utils;

namespace SeleniumTests.PageObjects.CRUDPageObject
{
	public class GenericEntityEditPage : CrudGenericEntityPage
	{
		private readonly string _entityName;

		public IDetailSection detailsSection;

		public GenericEntityEditPage(string entityName, ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
			Url = baseUrl + "/admin/" +entityName +"/edit";
			_entityName = entityName;
			detailsSection = EntityDetailUtils.GetEntityDetailsSection(entityName, contextConfiguration);
		}

		public GenericEntityEditPage(ContextConfiguration contextConfiguration) : base(contextConfiguration){}

		public void Fill()
		{
			var factory = new EntityDetailFactory(contextConfiguration);
			factory.ApplyDetails(_entityName, true);
		}

		public void Cancel() => CancelButton.ClickWithWait(driverWait);
		public void Submit() => SubmitButton.ClickWithWait(driverWait);

		// Created/Modified Datepickers
		public IWebElement CreateAtDatepickerField => driver.FindElementExt(By.XPath("//label[text()='Created']/following-sibling::input[@class='flatpickr-input']"));
		public IWebElement ModifiedAtDatepickerField => driver.FindElementExt(By.XPath("//label[text()='Modified']/following-sibling::input[@class='flatpickr-input']"));
	}
}