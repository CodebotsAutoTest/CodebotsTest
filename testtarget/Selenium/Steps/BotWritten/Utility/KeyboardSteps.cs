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
using SeleniumTests.Setup;
using SeleniumTests.Utils;
using SeleniumTests.Enums;
using TechTalk.SpecFlow;

namespace SeleniumTests.Steps.BotWritten.Utility
{
	[Binding]
	public sealed class KeyboardSteps  : BaseStepDefinition
	{
		private readonly ContextConfiguration _contextConfiguration;

		public KeyboardSteps(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
			_contextConfiguration = contextConfiguration;
		}

		[ObsoleteAttribute]
		[StepDefinition("I press the (.*) key on element with (.*) of (.*)")]
		public void PresssKeyOnElementWith(KeyboardActionType keyName, SelectorPathType selector, string path)
		{
			var elementBy = WebElementUtils.GetElementAsBy(selector, path);
			var element = _driver.FindElement(elementBy);
			KeyboardUtils.EnterKeyToWebElement(element, keyName);
		}

		[ObsoleteAttribute]
		[StepDefinition("I copy from element with (.*) of (.*)")]
		public void CopyFromElement(SelectorPathType selector, string path)
		{
			var elementBy = WebElementUtils.GetElementAsBy(selector, path);
			var element = _driver.FindElement(elementBy);
			KeyboardUtils.CopyFromWebElement(element);
		}

		[ObsoleteAttribute]
		[StepDefinition("I paste to element with (.*) of (.*)")]
		public void PasteToElement(SelectorPathType selector, string path)
		{
			var elementBy = WebElementUtils.GetElementAsBy(selector, path);
			var element = _driver.FindElement(elementBy);
			KeyboardUtils.PasteToWebElement(element);
		}

		[ObsoleteAttribute]
		[StepDefinition("I select all in element with (.*) of (.*)")]
		public void SelectAllInElement(SelectorPathType selector, string path)
		{
			var elementBy = WebElementUtils.GetElementAsBy(selector, path);
			var element = _driver.FindElement(elementBy);
			KeyboardUtils.SelectAllFromWebElement(element);
		}

		[ObsoleteAttribute("Will be used when the above step defs are no longer absolute")]
		[StepArgumentTransformation]
		public static KeyboardActionType TransformStringToKeyboardActionTypeEnum(string keyboardAction)
		{
			return (keyboardAction.ToLower()) switch
			{
				"enter" => KeyboardActionType.ENTER,
				"escape" => KeyboardActionType.ESCAPE,
				"tab" => KeyboardActionType.TAB,
				_ => throw new Exception($"{keyboardAction} enum is not handled"),
			};
		}
	}
}