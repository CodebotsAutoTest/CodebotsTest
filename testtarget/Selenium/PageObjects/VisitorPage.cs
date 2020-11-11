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

using SeleniumTests.Setup;

namespace SeleniumTests.PageObjects
{
	///<summary>
	///The vistor page object, All pages open to visitors should extend this page
	/// Contains methods shared across all page Objects
	///</summary>
	public abstract class VisitorPage : BasePage
	{
		protected VisitorNavSection visitorNavBar;

		// % protected region % [Add any webelements which are present across all vistor pages] off begin

		// % protected region % [Add any webelements which are present across all vistor pages] end

		public VisitorPage(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
			// % protected region % [Add any additional construction required by visitor pages] off begin

			visitorNavBar = new VisitorNavSection(contextConfiguration);

			// % protected region % [Add any additional construction required by visitor pages] end
		}

		 // % protected region % [Add any Methods which can be done on every visitor page of the site] off begin

		// % protected region % [Add any Methods which can be done on every visitor page of the site] end

	}
}
