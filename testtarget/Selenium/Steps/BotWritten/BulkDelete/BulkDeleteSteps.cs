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
using System.Linq;
using System.Collections.Generic;
using SeleniumTests.PageObjects;
using SeleniumTests.PageObjects.CRUDPageObject;
using SeleniumTests.PageObjects.BotWritten.CRUDPageObject.Modals;
using SeleniumTests.Setup;
using SeleniumTests.Enums;
using TechTalk.SpecFlow;
using Xunit;

namespace SeleniumTests.Steps.BotWritten.BulkDelete
{
	[Binding]
	public class BulkDeleteFeatureSteps  : BaseStepDefinition
	{
		private readonly ContextConfiguration _contextConfiguration;
		private readonly GenericEntityPage _genericEntityPage;
		private readonly ModalOnPage _modalOnPage;
		private int _totalNumSelectedItems;
		private readonly static List<string> ToasterAlertMessages = new List<string> { "All selected items are deleted successfully", "These records could not be deleted because of an association"};

		public BulkDeleteFeatureSteps(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
			_contextConfiguration = contextConfiguration;
			_genericEntityPage = new GenericEntityPage(_contextConfiguration);
			_modalOnPage = new ModalOnPage(_contextConfiguration);
		}

		[Then(@"I assert (.*) items have been selected")]
		public void ThenIAssertItemsHaveBeenSelected(int numSelectedItems)
		{
			_totalNumSelectedItems = _genericEntityPage.NumberOfItemsSelected();
			Assert.Equal(numSelectedItems, _totalNumSelectedItems);
		}

		[When(@"I delete the selected items, and (.*) to confirm")]
		public void WhenIDeleteTheSelectedItemsAndAcceptToConfirm(UserActionType userAction)
		{
			_genericEntityPage.BulkDeleteButton.Click();
			switch (userAction)
			{
				case UserActionType.ACCEPT:
					_modalOnPage.ConfirmDeleteButton.Click();
					break;
				default:
					throw new Exception("Unable to determine required action on Alert box");
			}
		}

		[Then(@"I assert that the alertbox responds to our deletion request")]
		public void ThenIAssertThatTheAlertboxRespondsToOurDeletionRequest()
		{
			var toaster = new ToasterAlert(_contextConfiguration);
			var toasterMessage = toaster.GetToasterAlertMessage();

			// asserts that any of the expected messages appear in the toaster alert box
			var containsMessage = ToasterAlertMessages.Any(x => toasterMessage.Contains(x));
			_contextConfiguration.WriteTestOutput($"Toaster Message Contained: {toasterMessage}");
			Assert.True(containsMessage);
		}

		[StepArgumentTransformation]
		public static UserActionType TransformStringToUserActionTypeEnum(string userAction)
		{
			return (userAction.ToLower()) switch
			{
				"accept" => UserActionType.ACCEPT,
				"confirm" => UserActionType.CONFIRM,
				"cancel" => UserActionType.CANCEL,
				"close" => UserActionType.CLOSE,
				"x close" => UserActionType.X_CLOSE,
				"dismiss" => UserActionType.DISMISS,
				_ => throw new Exception($"{userAction} enum is not handled"),
			};
		}

		public static ModalActionType TransformStringToModalActionTypeEnum(string modalType)
		{
			return (modalType.ToLower()) switch
			{
				"alert" => ModalActionType.ALERT,
				"confirm" => ModalActionType.CONFIRM,
				"custom" => ModalActionType.CUSTOM,
				"imperative" => ModalActionType.IMPERATIVE,
				_ => throw new Exception($"{modalType} enum is not handled"),
			};
		}
	}
}
