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
using SeleniumTests.Enums;
using SeleniumTests.Setup;
using SeleniumTests.Utils;

namespace SeleniumTests.PageObjects.BotWritten.Forms
{
	public class FormsSubmissionPage : BasePage
	{
		public IWebElement OpenFormButton => FindElementExt("OpenFormButton");
		public IWebElement SubmitButton => FindElementExt("SubmitButton");

		public FormsSubmissionPage(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
			InitializeSelectors();
		}
		private void InitializeSelectors()
		{
			selectorDict.Add("OpenFormButton", (selector: "//button[contains(text(),'Open Form')]", type: SelectorType.XPath));
			selectorDict.Add("SubmitButton", (selector: "//button[contains(text(),'Submit')]", type: SelectorType.XPath));
		}

		public bool SlideExists(string slide)
		{
			return WaitUtils.elementState(driverWait, By.XPath($"//h3[contains(text(),'{slide}')]"), ElementState.EXISTS );
		}
		
		public bool QuestionExists(string question)
		{
			return WaitUtils.elementState(driverWait, By.XPath($"//div[@data-name='{question}']"), ElementState.EXISTS );
		}

		public void AnswerTextQuestion(string question, string answer)
		{
			var input = driver.FindElement(By.XPath($"//div[@data-name='{question}']//input"));
			input.SendKeys(answer);
		}
	}
}