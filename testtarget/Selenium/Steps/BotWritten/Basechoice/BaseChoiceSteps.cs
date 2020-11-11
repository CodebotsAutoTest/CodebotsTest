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
using TestDataLib;

namespace SeleniumTests.Steps.BotWritten.Basechoice
{
	[Binding]
	public sealed class BaseChoiceSteps  : BaseStepDefinition
	{
		private readonly ContextConfiguration _contextConfiguration;

		public BaseChoiceSteps (ContextConfiguration contextConfiguration)  : base(contextConfiguration)
		{
			_contextConfiguration = contextConfiguration;
		}

		[ObsoleteAttribute]
		[StepDefinition("I insert valid basechoice of type (.*) with length (.*) to (.*) into element with (.*) of (.*)")]
		public void IInsertValidStringBaseChoiceIntoElement(string baseChoiceType, int min, int max, SelectorPathType selector, string path)
		{
			var elementBy = WebElementUtils.GetElementAsBy(selector, path);
			WaitUtils.elementState(_driverWait, elementBy, ElementState.EXISTS);

			switch (baseChoiceType)
			{
				case "wordystring":
					TypingUtils.TypeElement(_driver, elementBy, DataUtils.RandString(min, max));
					break;
				case "string":
					TypingUtils.TypeElement(_driver, elementBy, DataUtils.RandString(min, max));
					break;
			}
		}

		[ObsoleteAttribute]
		[StepDefinition("I insert valid basechoice of type (.*) into element with (.*) of (.*)")]
		public void IInsertValidBaseChoiceIntoElement(BaseChoiceType baseChoiceType, SelectorPathType selector, string path)
		{
			By elementBy = WebElementUtils.GetElementAsBy(selector, path);
			WaitUtils.elementState(_driverWait, elementBy, ElementState.EXISTS);

			switch (baseChoiceType)
			{
				case BaseChoiceType.INT:
					TypingUtils.TypeElement(_driver, elementBy, DataUtils.RandInt().ToString());
					break;
				case BaseChoiceType.DOUBLE:
					TypingUtils.TypeElement(_driver, elementBy, DataUtils.RandDouble().ToString());
					break;
				case BaseChoiceType.BOOL:
					TypingUtils.TypeElement(_driver, elementBy, DataUtils.RandBool().ToString());
					break;
				case BaseChoiceType.EMAIL:
					TypingUtils.TypeElement(_driver, elementBy, DataUtils.RandEmail());
					break;
			}
		}

		[ObsoleteAttribute("This transformation is only needed if absolute methods above are")]
		[StepArgumentTransformation]
		public static BaseChoiceType TransformStringToBaseChoiceTypeEnum (string baseChoiceType)
		{
			return (baseChoiceType.ToLower()) switch
			{
				"int" => BaseChoiceType.INT,
				"double" => BaseChoiceType.DOUBLE,
				"bool" => BaseChoiceType.BOOL,
				"email" => BaseChoiceType.EMAIL,
				_ => throw new Exception($"{baseChoiceType} enum is not handled"),
			};
		}
	}
}