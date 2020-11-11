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
using Xunit;
using SeleniumTests.PageObjects;

namespace SeleniumTests.Steps.BotWritten.AlertBox
{
	[Binding]
	public sealed class AlertBoxSteps  : BaseStepDefinition
	{
		private readonly ContextConfiguration _contextConfiguration;

		public AlertBoxSteps(ContextConfiguration contextConfiguration)  : base(contextConfiguration)
		{
			_contextConfiguration = contextConfiguration;
			// % protected region % [Add any additional setup options here] off begin
			// % protected region % [Add any additional setup options here] end
		}

		[StepDefinition("I assert the message box reads (.*)")]
		public void GivenINavigateToTheEntityPage(string expectedMessage)
		{
			var messageBoxContents = AlertBoxUtils.ReadAlertBoxMessage(_driver);
			Assert.Equal(messageBoxContents, expectedMessage);
		}

		[StepDefinition("I (.*) the alert")]
		public void AcceptDismissAlert(UserActionType acceptance)
		{
			switch(acceptance)
			{
				case UserActionType.ACCEPT:
					AlertBoxUtils.AlertBoxHandler(_driver, true);
					break;
				case UserActionType.DISMISS:
					AlertBoxUtils.AlertBoxHandler(_driver, false);
					break;
			}
		}

		[StepDefinition("I type (.*) and (.*) the alert")]
		public void TypeAcceptDismissAlert(string text, UserActionType acceptance)
		{
			AlertBoxUtils.WriteToAlertBox(_driver, text);
			switch (acceptance)
			{
				case UserActionType.ACCEPT:
					AlertBoxUtils.AlertBoxHandler(_driver, true);
					break;
				case UserActionType.DISMISS:
					AlertBoxUtils.AlertBoxHandler(_driver, false);
					break;
			}
		}

		[StepDefinition("I expect the alert message to be '(.*)'")]
		public void ExpectAlertMessage(string expectedMessage)
		{
			var displayedMessage = AlertBoxUtils.ReadAlertBoxMessage(_driver);
			Assert.Equal(expectedMessage, displayedMessage);
		}

		[Then(@"I assert that I can see a popup displays a message: (.*)")]
		public void ThenIAssertThatICanSeeTheToasterWithAEntityAdddedSuccessMessage( string expectedSuccessMsg)
		{
			var toaster = new ToasterAlert(_contextConfiguration);
			_driverWait.Until(_ => toaster.ToasterBody.Displayed);
			Assert.Equal(expectedSuccessMsg, toaster.ToasterBody.Text);
		}
	}
}