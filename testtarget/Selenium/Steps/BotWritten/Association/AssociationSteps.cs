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
using Xunit;
using TechTalk.SpecFlow;
using APITests.EntityObjects.Models;
using APITests.Factories;
using SeleniumTests.PageObjects.CRUDPageObject;
using SeleniumTests.Setup;
using SeleniumTests.Utils;

namespace SeleniumTests.Steps.BotWritten.Association
{
	[Binding]
	public sealed class AssociationSteps  : BaseStepDefinition
	{
		private readonly ContextConfiguration _contextConfiguration;
		private readonly List<BaseEntity> _targetEntities;
		private readonly List<BaseEntity> _sourceEntities;
		private string _initialAssociation;

		public AssociationSteps(ContextConfiguration contextConfiguration)  : base(contextConfiguration)
		{
			_contextConfiguration = contextConfiguration;
			_targetEntities = new List<BaseEntity>();
			_sourceEntities = new List<BaseEntity>();
		}

		[Given(@"I create (.*) (.*)'s each associated with (.*) (.*) using (.*)")]
		public void GivenICreateEntityEachAssociatedWithEntity(int numTargetEntities, string targetEntityName, int numSourceEntities, string referenceOppositeName, string association)
		{
			_initialAssociation = association;
			var sharedReferences = new Dictionary<string, ICollection<Guid>>(){ {referenceOppositeName + "Id", new List<Guid>()}};
			var targetEntityFactory = new EntityFactory(targetEntityName);
			var sourceEntityName = targetEntityFactory.Construct().References.FirstOrDefault(x => x.OppositeName == referenceOppositeName)?.EntityName;
			var sourceEntityFactory = new EntityFactory(sourceEntityName);

			for (var i = 0; i < numSourceEntities; i++)
			{
				var entity = sourceEntityFactory.Construct();
				entity.Configure(BaseEntity.ConfigureOptions.CREATE_ATTRIBUTES_AND_REFERENCES);
				entity.Save();
				sharedReferences[referenceOppositeName + "Id"].Add(entity.Id);
				_sourceEntities.Add(entity);
			}

			for (var i = 0; i < numTargetEntities; i++)
			{
				var entity = targetEntityFactory.Construct();
				entity.Configure(BaseEntity.ConfigureOptions.CREATE_ATTRIBUTES_AND_REFERENCES);
				entity.SetReferences(sharedReferences);
				entity.Save();
				_targetEntities.Add(entity);
			}
		}

		[Then(@"I validate each (.*) has (.*) (.*) associations using (.*)")]
		public void ThenIValidateEachEntityHasEntityAssociations(string referenceOppositeName, int numAssociations, string targetEntityName, string association)
		{
			if (targetEntityName is null)
			{
				throw new ArgumentNullException(nameof(targetEntityName));
			}

			// form a list of expected guids to compare with the ones that are read off the page
			ICollection<Guid> expectedGuids = new List<Guid>();
			ICollection<Guid> searchGuids = new List<Guid>();

			if (association == _initialAssociation)
			{
				_targetEntities.ForEach(x => searchGuids.Add(x.Id));
				_sourceEntities.ForEach(x => expectedGuids.Add(x.Id));
			}
			else
			{
				_targetEntities.ForEach(x => expectedGuids.Add(x.Id));
				_sourceEntities.ForEach(x => searchGuids.Add(x.Id));
			}

			// Navigate to entity page
			var page = new GenericEntityPage(referenceOppositeName, _contextConfiguration);
			page.Navigate();

			foreach (var searchGuid in searchGuids)
			{
				// Search for entity using GUID
				page.SearchInput.ClickWithWait(_driverWait);
				page.SearchInput.SendKeys(searchGuid.ToString());
				page.SearchButton.Click();
				_driverWait.Until(_ => page.TotalEntities() == 1);
				page.GetAllEntites().FirstOrDefault()?.EditItem();

				// Get the guids from the page
				var entityDetailsSection = EntityDetailUtils.GetEntityDetailsSection(referenceOppositeName, _contextConfiguration);
				var associations = entityDetailsSection.GetAssociation(association.Replace(" ", "").ToLower());

				// Assert the guids on the page match the expected guids
				foreach (var guid in expectedGuids)
				{
					Assert.Contains(guid, associations);
				}
				Assert.Equal(numAssociations, associations.Count);
				new GenericEntityEditPage(_contextConfiguration).Cancel();
			}
		}
	}
}
