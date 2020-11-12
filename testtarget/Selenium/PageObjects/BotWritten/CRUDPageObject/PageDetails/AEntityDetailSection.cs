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
using System.Linq;
using System.Collections.Generic;
using APITests.EntityObjects.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumTests.PageObjects.Components;
using SeleniumTests.Setup;
using SeleniumTests.Utils;
using SeleniumTests.Enums;
using SeleniumTests.PageObjects.BotWritten;
// % protected region % [Custom imports] off begin
// % protected region % [Custom imports] end

namespace SeleniumTests.PageObjects.CRUDPageObject.PageDetails
{
	//This section is a mapping from an entity object to an entity create or detailed view page
	public class AEntityDetailSection : BasePage, IEntityDetailSection
	{
		private readonly IWait<IWebDriver> _driverWait;
		private readonly IWebDriver _driver;
		private readonly bool _isFastText;
		private readonly ContextConfiguration _contextConfiguration;

		// reference elements
		private static By BssssdasdsElementBy => By.XPath("//*[contains(@class, 'bssssdasd')]//div[contains(@class, 'dropdown__container')]/a");
		private static By BssssdasdsInputElementBy => By.XPath("//*[contains(@class, 'bssssdasd')]/div/input");

		//FlatPickr Elements

		//Attribute Headers
		private readonly AEntity _aEntity;

		//Attribute Header Titles
		private IWebElement DsdsHeaderTitle => _driver.FindElementExt(By.XPath("//th[text()='dsds']"));

		// Datepickers
		public IWebElement CreateAtDatepickerField => _driver.FindElementExt(By.CssSelector("div.created > input[type='date']"));
		public IWebElement ModifiedAtDatepickerField => _driver.FindElementExt(By.CssSelector("div.modified > input[type='date']"));

		public AEntityDetailSection(ContextConfiguration contextConfiguration, AEntity aEntity = null) : base(contextConfiguration)
		{
			_driver = contextConfiguration.WebDriver;
			_driverWait = contextConfiguration.WebDriverWait;
			_isFastText = contextConfiguration.SeleniumSettings.FastText;
			_contextConfiguration = contextConfiguration;
			_aEntity = aEntity;

			InitializeSelectors();
			// % protected region % [Add any extra construction requires] off begin

			// % protected region % [Add any extra construction requires] end
		}

		// initialise all selectors and grouping them with the selector type which is used
		private void InitializeSelectors()
		{
			// Attribute web elements
			selectorDict.Add("DsdsElement", (selector: "//div[contains(@class, 'dsds')]//input", type: SelectorType.XPath));

			// Reference web elements
			selectorDict.Add("BssssdasdElement", (selector: ".input-group__dropdown.bssssdasds > .dropdown.dropdown__container", type: SelectorType.CSS));

			// Datepicker
			selectorDict.Add("CreateAtDatepickerField", (selector: "//div[contains(@class, 'created')]/input", type: SelectorType.XPath));
			selectorDict.Add("ModifiedAtDatepickerField", (selector: "//div[contains(@class, 'modified')]/input", type: SelectorType.XPath));
		}

		//outgoing Reference web elements

		//Attribute web Elements
		private IWebElement DsdsElement => FindElementExt("DsdsElement");

		// Return an IWebElement that can be used to sort an attribute.
		public IWebElement GetHeaderTile(string attribute)
		{
			return attribute switch
			{
				"dsds" => DsdsHeaderTitle,
				_ => throw new Exception($"Cannot find header tile {attribute}"),
			};
		}

		// Return an IWebElement for an attribute input
		public IWebElement GetInputElement(string attribute)
		{
			switch (attribute)
			{
				case "dsds":
					return DsdsElement;
				default:
					throw new Exception($"Cannot find input element {attribute}");
			}
		}

		public void SetInputElement(string attribute, string value)
		{
			switch (attribute)
			{
				case "dsds":
					SetDsds(value);
					break;
				default:
					throw new Exception($"Cannot find input element {attribute}");
			}
		}

		private By GetErrorAttributeSectionAsBy(string attribute)
		{
			return attribute switch
			{
				"dsds" => WebElementUtils.GetElementAsBy(SelectorPathType.CSS, "div.dsds > div > p"),
				_ => throw new Exception($"No such attribute {attribute}"),
			};
		}

		public List<string> GetErrorMessagesForAttribute(string attribute)
		{
			var elementBy = GetErrorAttributeSectionAsBy(attribute);
			WaitUtils.elementState(_driverWait, elementBy, ElementState.VISIBLE);
			var element = _driver.FindElementExt(elementBy);
			var errors = new List<string>(element.Text.Split("\r\n"));
			// remove the item in the list which is the name of the attribute and not an error.
			errors.Remove(attribute);
			return errors;
		}

		public void Apply()
		{
			// % protected region % [Configure entity application here] off begin
			SetDsds(_aEntity.Dsds);

			if (_aEntity.BssssdasdIds != null)
			{
				SetBssssdasds(_aEntity.BssssdasdIds.Select(x => x.ToString()));
			}
			// % protected region % [Configure entity application here] end
		}

		public List<Guid> GetAssociation(string referenceName)
		{
			switch (referenceName)
			{
				case "bssssdasd":
					return GetBssssdasds();
				default:
					throw new Exception($"Cannot find association type {referenceName}");
			}
		}

		// set associations
		private void SetBssssdasds(IEnumerable<string> ids)
		{
			WaitUtils.elementState(_driverWait, BssssdasdsInputElementBy, ElementState.VISIBLE);
			var bssssdasdsInputElement = _driver.FindElementExt(BssssdasdsInputElementBy);

			foreach(var id in ids)
			{
				bssssdasdsInputElement.SendKeys(id);
				WaitForDropdownOptions();
				bssssdasdsInputElement.SendKeys(Keys.Return);
			}
		}


		// get associations
		private List<Guid> GetBssssdasds()
		{
			var guids = new List<Guid>();
			WaitUtils.elementState(_driverWait, BssssdasdsElementBy, ElementState.VISIBLE);
			var bssssdasdsElement = _driver.FindElements(BssssdasdsElementBy);

			foreach(var element in bssssdasdsElement)
			{
				guids.Add(new Guid (element.GetAttribute("data-id")));
			}
			return guids;
		}

		// wait for dropdown to be displaying options
		private void WaitForDropdownOptions()
		{
			var xpath = "//*/div[@aria-expanded='true']";
			var elementBy = WebElementUtils.GetElementAsBy(SelectorPathType.XPATH, xpath);
			WaitUtils.elementState(_driverWait, elementBy,ElementState.EXISTS);
		}

		private void SetDsds (String value)
		{
			TypingUtils.InputEntityAttributeByClass(_driver, "dsds", value, _isFastText);
			DsdsElement.SendKeys(Keys.Tab);
			DsdsElement.SendKeys(Keys.Escape);
		}

		private String GetDsds =>
			DsdsElement.Text;


		// % protected region % [Add any additional getters and setters of web elements] off begin
		// % protected region % [Add any additional getters and setters of web elements] end
	}
}