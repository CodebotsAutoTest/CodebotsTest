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

// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace SeleniumTests.PageObjects.ResetPasswordPageObject
{
	public class ResetPasswordPage : BasePage
	{
		// % protected region % [Customise ResetPasswordPage web elements here] off begin
		public IWebElement NewPasswordInput => FindElementExt("NewPasswordInput");
		public IWebElement ConfirmPasswordInput => FindElementExt("ConfirmPasswordInput");
		public IWebElement ConfirmButton => FindElementExt("ConfirmButton");
		// % protected region % [Customise ResetPasswordPage web elements here] end

		// % protected region % [Add any extra fields here] off begin
		// % protected region % [Add any extra fields here] end

		public ResetPasswordPage(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
			// % protected region % [Customise constructor logic here] off begin
			InitializeSelectors();
			// % protected region % [Customise constructor logic here] end
		}

		private void InitializeSelectors()
		{
			// % protected region % [Customise InitializeSelectors logic here] off begin
			selectorDict.Add("NewPasswordInput", (selector: "new_password-field", type: SelectorType.ID));
			selectorDict.Add("ConfirmPasswordInput", (selector: "confirm_password-field", type: SelectorType.ID));
			selectorDict.Add("ConfirmButton", (selector: "confirm_reset_password", type: SelectorType.ID));
			// % protected region % [Customise InitializeSelectors logic here] end
		}

		public void EnterNewPasswordAndSubmit(string password)
		{
			NewPasswordInput.SendKeys(password);
			ConfirmPasswordInput.SendKeys(password);
			ConfirmButton.Click();
		}
		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	
	}
}