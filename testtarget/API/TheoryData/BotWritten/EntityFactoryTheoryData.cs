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
using APITests.Factories;
using Xunit;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace APITests.TheoryData.BotWritten
{
	public class UserEntityFactorySingleTheoryData : TheoryData<UserEntityFactory>
	{
		public UserEntityFactorySingleTheoryData()
		{
			// % protected region % [Modify UserEntityFactorySingleTheoryData entities here] off begin
			// % protected region % [Modify UserEntityFactorySingleTheoryData entities here] end
		}
	}

	public class EntityFactorySingleTheoryData : TheoryData<EntityFactory, int>
	{
		public EntityFactorySingleTheoryData()
		{
			// % protected region % [Modify UserEntityFactorySingleTheoryData entities here] off begin
			// % protected region % [Modify UserEntityFactorySingleTheoryData entities here] end
		}
	}

	public class NonUserEntityFactorySingleTheoryData : TheoryData<EntityFactory, int>
	{
		public NonUserEntityFactorySingleTheoryData()
		{
			// % protected region % [Modify UserEntityFactorySingleTheoryData entities here] off begin
			// % protected region % [Modify UserEntityFactorySingleTheoryData entities here] end
		}
	}

	public class EntityFactoryTheoryData : TheoryData<EntityFactory>
	{
		public EntityFactoryTheoryData()
		{
			// % protected region % [Modify UserEntityFactorySingleTheoryData entities here] off begin
			// % protected region % [Modify UserEntityFactorySingleTheoryData entities here] end
		}
	}

	public class EntityFactoryMultipleTheoryData : TheoryData<EntityFactory, int>
	{
		public EntityFactoryMultipleTheoryData()
		{
			// % protected region % [Modify UserEntityFactorySingleTheoryData entities here] off begin
			var numEntities = 3;
			// % protected region % [Modify UserEntityFactorySingleTheoryData entities here] end
		}
	}

	// % protected region % [Add any further custom EntityFactoryTheoryData here] off begin
	// % protected region % [Add any further custom EntityFactoryTheoryData here] end

}