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
using APITests.Factories;
using SeleniumTests.Factories;
using SeleniumTests.PageObjects.BotWritten.UserPageObjects;
using SeleniumTests.PageObjects.CRUDPageObject;
using SeleniumTests.Setup;
using SeleniumTests.Utils;
using TechTalk.SpecFlow;
using Xunit;

namespace SeleniumTests.Steps.BotWritten
{
	[Binding]
	public sealed class EntityCreateSteps  : BaseStepDefinition
	{
		private readonly ContextConfiguration _contextConfiguration;
		private readonly GenericEntityPage _genericEntityPage;

		public EntityCreateSteps(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
			_contextConfiguration = contextConfiguration;
			_genericEntityPage = new GenericEntityPage(_contextConfiguration);
			// % protected region % [Add any additional setup options here] off begin
			// % protected region % [Add any additional setup options here] end
		}

		[When("I insert a valid (.*), search for it and delete it")]
		public void IInsertAValidEntityAndSearchForIt(string entityName)
		{
			// Insert the row
			var entityFactory = new EntityFactory(entityName);
			var entity = entityFactory.ConstructAndSave(_testOutputHelper);

			// Search for it using GUID
			_genericEntityPage.SearchInput.SendKeys(entity.Id.ToString());
			_genericEntityPage.SearchButton.Click();
			_driverWait.Until(_ => _genericEntityPage.TotalEntities() == 1);
			Assert.Equal(1,_genericEntityPage.TotalEntities());

			// Delete it
			_genericEntityPage.DeleteTopEntity();
			_driverWait.Until(_ => _genericEntityPage.TotalEntities() == 0);
			Assert.Equal(0, _genericEntityPage.TotalEntities());
		}

		[When("I open the create (.*) form And I fill it with valid data")]
		public void IOpenTheCreateEntityForm(string entityName)
		{
			var page = new GenericEntityPage(entityName, _contextConfiguration);
			page.ClickCreateButton();
			var factory = new EntityDetailFactory(_contextConfiguration);
			factory.ApplyDetails(entityName, true);
		}

		[When(@"I edit the first entity row")]
		public void WhenIEditTheFirstRowAndNavigateToEditPage()
		{
			var entityOnPage = new EntityOnPage(_contextConfiguration, _genericEntityPage.EntityTable);
			_driverWait.Until(x => entityOnPage.EditButton.Displayed);
			entityOnPage.EditButton.ClickWithWait(_driverWait);
		}


		[Given(@"I click to create a (.*)")]
		public void IClickToCreateAnEntity(string entityName)
		{
			var page = new GenericEntityPage(entityName, _contextConfiguration);
			page.ClickCreateButton();
		}

		[Given("I have (.*) valid (.*) entities")]
		public void IHaveValidEntities(int numEntities, string entityName)
		{
			var entityFactory = new EntityFactory(entityName);
			entityFactory.ConstructAndSave(_testOutputHelper, numEntities);
		}

		[StepDefinition("I insert a (.*) into the database")]
		public void InsertEntityToDatabase(string entityName)
		{
			var entityFactory = new EntityFactory(entityName);
			entityFactory.ConstructAndSave(_testOutputHelper);
		}

		[When("I create a (.*) (.*)")]
		public void WhenICreateAValidEntity(string validStr, string entityName)
		{
			bool isValid;

			switch(validStr)
			{
				case "valid":
					isValid = true;
					break;
				case "invalid":
					isValid = false;
					break;
				default:
					throw new Exception("Please specify whether a 'valid' or 'invalid' entity is required");
			}

			var page = new GenericEntityEditPage(entityName, _contextConfiguration);
			var factory = new EntityDetailFactory(_contextConfiguration);
			factory.ApplyDetails(entityName, isValid);
			page.SubmitButton.Click();
		}

		[When("I delete the first (.*)")]
		public void WhenIDeleteTheFirstEntity(string entityName)
		{
			var page = new GenericEntityPage(entityName, _contextConfiguration);
			page.Navigate();
			page.DeleteTopEntity();
		}
	}
}