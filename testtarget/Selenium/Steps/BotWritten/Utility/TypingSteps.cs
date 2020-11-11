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
using SeleniumTests.Setup;
using SeleniumTests.Utils;
using SeleniumTests.Enums;
using TechTalk.SpecFlow;

namespace SeleniumTests.Steps.BotWritten.Utility
{
	[Binding]
	public sealed class TypingSteps  : BaseStepDefinition
	{
		private readonly ContextConfiguration _contextConfiguration;

		public TypingSteps (ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
			_contextConfiguration = contextConfiguration;
		}

		//Type to elements
		[ObsoleteAttribute]
		[StepDefinition("I clear and type (.*) into the element with (.*) of (.*)")]
		public void ClearAndTypeToElementBy(string text, SelectorPathType selector, string path)
		{
			By elementBy = WebElementUtils.GetElementAsBy(selector, path);
			TypingUtils.ClearAndTypeElement(_driver, elementBy, text);
		}

		[ObsoleteAttribute]
		[StepDefinition("I type (.*) into the element with (.*) of (.*)")]
		public void TypeElementBy(string text, SelectorPathType selector, string path)
		{
			By elementBy = WebElementUtils.GetElementAsBy(selector, path);
			TypingUtils.TypeElement(_driver, elementBy, text);
		}

		[ObsoleteAttribute]
		[StepDefinition("I type the date (.*) into the element with class of (.*)")]
		public void TypeDateWithClass(string date, string className)
		{
			DateTimePickerUtils.EnterDateByClassName(_driver, className, DateTime.Parse(date));
		}
	}
}