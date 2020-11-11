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
using SeleniumTests.Enums;
using SeleniumTests.Setup;
using SeleniumTests.Utils;
using TechTalk.SpecFlow;
using Xunit;

namespace SeleniumTests.Steps.BotWritten.AlertBox
{
	[Binding]
	public sealed class ConfirmationBoxSteps  : BaseStepDefinition
	{
		private readonly ContextConfiguration _contextConfiguration;

		public ConfirmationBoxSteps(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
			_contextConfiguration = contextConfiguration;
			// % protected region % [Add any additional setup options here] off begin
			// % protected region % [Add any additional setup options here] end
		}

		[Then("I (.*) the confirmation box")]
		public void IPerformActionOnConfirmationBox(UserActionType userAction)
		{
			switch (userAction) {
				case UserActionType.ACCEPT:
					AlertBoxUtils.AlertBoxHandler(_driver, true);
					break;
				case UserActionType.DISMISS:
				case UserActionType.CLOSE:
						AlertBoxUtils.AlertBoxHandler(_driver, false);
					break;
				default:
					throw new Exception("Unable to determine required action on Alert box");
			}
		}
	}
}