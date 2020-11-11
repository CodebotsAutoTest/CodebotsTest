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
import { ILink } from 'Views/Components/Navigation/Navigation';
import { RouteComponentProps } from 'react-router';
import { store } from 'Models/Store';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

export function getFrontendNavLinks(pageProps: RouteComponentProps): ILink[][] {
	// % protected region % [Add any logic before displaying page links here] off begin
	// % protected region % [Add any logic before displaying page links here] end
	return [
		[
			// % protected region % [Customise top nav section here] off begin
			{label: "Home", path: '/', icon: "home", iconPos: 'icon-left'}
			// % protected region % [Customise top nav section here] end
		],
		[
			// % protected region % [Customise middle nav section here] off begin
			// % protected region % [Customise middle nav section here] end
		],
		[
			// % protected region % [Customise bottom nav section here] off begin
			{label: "Login", path: '/login', icon: "login", iconPos: 'icon-left', shouldDisplay: () => !store.loggedIn},
			{label: "Logout", path: '/logout', icon: "room", iconPos: 'icon-left', shouldDisplay: () => store.loggedIn}
			// % protected region % [Customise bottom nav section here] end
		],
	];
}
