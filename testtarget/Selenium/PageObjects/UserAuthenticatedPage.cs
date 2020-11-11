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

using SeleniumTests.PageObjects.BotWritten.UserPageObjects;
using SeleniumTests.Setup;
using SeleniumTests.Utils;

namespace SeleniumTests.PageObjects
{
	///<summary>
	/// The User Authenticated Page represents the common elements across pages a logged in user would see
	///</summary>
	public class UserAuthenticatedPage : VisitorPage
	{
		// % protected region % [Add any web elements which are specific to logged in users] off begin

		protected UserNavSection userNavBar;

		// % protected region % [Add any web elements which are specific to logged in users] end

		public UserAuthenticatedPage(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
			// % protected region % [Add any project specific setup required for the user auth page] off begin

			userNavBar = new UserNavSection(contextConfiguration);

			// % protected region % [Add any project specific setup required for the user auth page] end
		}

			// % protected region % [Add any project specific navigation methods are are applicable to a logged in user] off begin

			// % protected region % [Add any project specific navigation methods are are applicable to a logged in user] end

		public LoginPage Logout()
		{
			driver.GoToUrlExt(baseUrl + "logout");
			return new LoginPage(contextConfiguration);
		}
	}
}