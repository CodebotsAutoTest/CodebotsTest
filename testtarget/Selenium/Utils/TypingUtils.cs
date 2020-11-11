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

namespace SeleniumTests.Utils
{
	internal static class TypingUtils
	{
		public static void InputEntityAttributeByClass(IWebDriver driver, string className, string inputString, bool isFastText)
		{
			var Element = driver.FindElementExt(By.XPath($"//div[contains(@class, '{className}')]//input"));
			KeyboardUtils.DeleteAllFromWebElement(Element);

			if (isFastText)
			{
				var js = (IJavaScriptExecutor)driver;
				var script = $@"var input = document.getElementsByClassName('{className}')[0].getElementsByTagName('input')[0];
					var setValue = Object.getOwnPropertyDescriptor(window.HTMLInputElement.prototype, 'value').set;
					setValue.call(input, '{inputString}');
					var e = new Event('input',  {{bubbles: true}} );
					input.dispatchEvent(e);";
				js.ExecuteScript(script);
			}
			else
			{
				Element.SendKeys(inputString);
			}
		}

		public static void ClearAndTypeElement(IWebElement element, string text)
		{
			KeyboardUtils.DeleteAllFromWebElement(element);
			element.SendKeys(text);
		}

		public static void ClearAndTypeElement(IWebDriver driver, By elementBy, string text)
		{
			var element = driver.FindElement(elementBy);
			ClearAndTypeElement(element, text);
		}

		public static void TypeElement(IWebElement element, string text)
		{
			element.SendKeys(text);
		}

		public static void TypeElement(IWebDriver driver, By elementBy, string text)
		{
			var element = driver.FindElement(elementBy);
			TypeElement(element, text);
		}
	}
}