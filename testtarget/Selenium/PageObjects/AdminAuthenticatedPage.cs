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
	///The Admin Authenticated Page represents the common elements across pages an admin would see
	///</summary>
	// % protected region % [Protected region incase the admin page should not extend the user page] off begin
	public class AdminAuthenticatedPage : UserAuthenticatedPage
	// % protected region % [Protected region incase the admin page should not extend the user page] end
	{
		// % protected region % [Add any web elements which are specific to logged in admins] off begin
		protected AdminNavSection adminNavBar;

		// % protected region % [Add any web elements which are specific to logged in admins] end

		public AdminAuthenticatedPage(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
		// % protected region % [Add any Admin specific constructor ] off begin
		adminNavBar = new AdminNavSection(contextConfiguration);

		// % protected region % [Add any Admin specific constructor ] end
		}

		// % protected region % [Add any methods which can be performed on an admin authenticated page] off begin
		// % protected region % [Add any methods which can be performed on an admin authenticated page] end

	}
}
