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
import { History } from 'history';
import { default as ApolloClient } from 'apollo-boost';
import { action, computed, observable } from 'mobx';
import { IGlobalModal } from '../Views/Components/Modal/GlobalModal';
// % protected region % [Add any extra store imports here] off begin
// % protected region % [Add any extra store imports here] end

// % protected region % [Change the group return result as needed] off begin
export interface IGroupResult {
	name: string;
	hasBackendAccess: boolean;
}
// % protected region % [Change the group return result as needed] end

// % protected region % [Change The user return result as needed] off begin
export interface IUserResult {
	id: string;
	email: string;
	groups: IGroupResult[];
}
// % protected region % [Change The user return result as needed] end

/**
 * A global singleton store that contains a global state of data
 */
export class Store {
	@observable
	private user?: IUserResult;

	/**
	 * The current location in the application
	 */
	@observable
	public appLocation: 'frontend' | 'admin' = 'frontend';

	/**
	 * The router history object for React Router
	 */
	public routerHistory: History;

	/**
	 * The client for Apollo
	 */
	public apolloClient: ApolloClient<{}>;

	/**
	 * The global modal that is stored in the app and can be called imperatively
	 */
	public modal: IGlobalModal;

	/**
	 * This signifies weather we are logged in or not
	 * Only ever set this value to true if there is a value set in this.token
	 */
	@computed
	public get loggedIn() {
		// % protected region % [Customise the loggedIn getter here] off begin
		return this.user !== undefined;
		// % protected region % [Customise the loggedIn getter here] end
	}

	/**
	 * The user Id of the logged-in user
	 */
	@computed
	public get userId(): string | undefined {
		// % protected region % [Customise the userId getter here] off begin
		return this.user ? this.user.id : undefined;
		// % protected region % [Customise the userId getter here] end
	};
	
	/**
	 * The email of the current logged in user
	 */
	@computed
	public get email(): string | undefined {
		// % protected region % [Customise the email getter here] off begin
		return this.user ? this.user.email : undefined;
		// % protected region % [Customise the email getter here] end
	}

	/**
	 * The groups that the logged in user are a part of
	 */
	@computed
	public get userGroups(): IGroupResult[] {
		// % protected region % [Customise the userGroups getter here] off begin
		if (this.user) {
			return [...this.user.groups];
		}
		return [];
		// % protected region % [Customise the userGroups getter here] end
	};

	/**
	 * Does this user have access to the backend admin views
	 */
	@computed
	public get hasBackendAccess() {
		// % protected region % [Customise the hasBackendAccess getter here] off begin
		if (this.user) {
			return this.user.groups.some(ug => ug.hasBackendAccess);
		}
		return false;
		// % protected region % [Customise the hasBackendAccess getter here] end
	};

	/**
	 * Is the frontend in edit mode
	 */
	@observable
	public frontendEditMode = false;
	

	/**
	 * Sets the current logged in user in the store
	 * @param userResult
	 */
	@action
	public setLoggedInUser(userResult: IUserResult) {
		// % protected region % [Customise the setLoggedInUser here] off begin
		this.user = userResult;
		// % protected region % [Customise the setLoggedInUser here] end
	}

	/**
	 * Clears the logged in user data from the store
	 */
	@action clearLoggedInUser() {
		// % protected region % [Customise the clearLoggedInUser here] off begin
		this.user = undefined;
		// % protected region % [Customise the clearLoggedInUser here] end
	}

	// % protected region % [Add any extra store methods or properties here] off begin
	// % protected region % [Add any extra store methods or properties here] end
}

export const store = new Store();