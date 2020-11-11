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
using SeleniumTests.Setup;

// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace SeleniumTests.PageObjects.BotWritten.UserPageObjects
{
	public class RegisterUserSelectionPage : BasePage
	{
		public IWebElement UserTypeDropdown => FindElementExt("UserTypeDropdown");
		public IWebElement ConfirmButton => FindElementExt("ConfirmButton");
		public IWebElement CancelButton => FindElementExt("CancelButton");

		public RegisterUserSelectionPage(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
			InitializeSelectors();
		}

		private void InitializeSelectors()
		{
			selectorDict.Add("UserTypeDropdown", (selector: "//div[contains(@class, 'input-group__dropdown')]//input", type: SelectorType.XPath));
			selectorDict.Add("ConfirmButton", (selector: "confirm_type", type: SelectorType.ID));
			selectorDict.Add("CancelButton", (selector: "cancel_register", type: SelectorType.ID));
		}

		public RegisterUserBasePage Select (UserType userType)
		{
			UserTypeDropdown.Click();

			switch (userType)
			{
				default:
					throw new Exception($"Invalid user type {userType}");
			}
		}
	}
}
