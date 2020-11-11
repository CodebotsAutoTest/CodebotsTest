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

using APITests.EntityObjects.Models;
using OpenQA.Selenium;
using SeleniumTests.Setup;

// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace SeleniumTests.PageObjects.BotWritten.UserPageObjects
{
	public abstract class RegisterUserBasePage : BasePage
	{
		public IWebElement EmailInput => FindElementExt("EmailInput");
		public IWebElement PasswordInput => FindElementExt("PasswordInput");
		public IWebElement ConfirmPasswordInput => FindElementExt("ConfirmPasswordInput");
		public IWebElement RegisterButton => FindElementExt("RegisterButton");
		public IWebElement CancelButton => FindElementExt("CancelButton");

		public RegisterUserBasePage(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
			InitializeSelectors();
		}

		private void InitializeSelectors()
		{
			selectorDict.Add("EmailInput", (selector: "div.email > input", type: SelectorType.CSS));
			selectorDict.Add("PasswordInput", (selector: "div.password > input", type: SelectorType.CSS));
			selectorDict.Add("ConfirmPasswordInput", (selector: "div._confirmPassword > input", type: SelectorType.CSS));
			selectorDict.Add("RegisterButton", (selector: "submit_register", type: SelectorType.ID));
			selectorDict.Add("CancelButton", (selector: "cancel_register", type: SelectorType.ID));
		}

		public abstract void Register(UserBaseEntity entity);

		internal void FillRegistrationDetails(string email, string password)
		{
			EmailInput.SendKeys(email);
			PasswordInput.SendKeys(password);
			ConfirmPasswordInput.SendKeys(password);
		}
	}
}
