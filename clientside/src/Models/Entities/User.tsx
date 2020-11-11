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
import { CRUD } from '../CRUDOptions';
import { IModelAttributes, Model, attribute, entity } from '../Model';
import { createUser, updateUser, deleteUser } from '../../Views/Components/UserCRUD/UserService';
import { action, observable } from 'mobx';
import { SERVER_URL } from 'Constants';
import * as Validators from '../../Validators';
import axios from 'axios';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

export interface IGroupAttributes extends IModelAttributes {
	name: string;
}

@entity('group')
export class Group extends Model implements IGroupAttributes {
	@attribute()
	@observable
	public name: string;

	constructor(attributes?: Partial<IGroupAttributes>) {
		super(attributes);
		if (attributes) {
			if (attributes.name) {
				this.name = attributes.name;
			}
		}
	}
	
	public getDisplayName(): string {
		return this.name;
	}
}

export interface IUserAttributes extends IModelAttributes {
	email: string;
	password: string;
	groups: string[];
	// % protected region % [Add any extra user attributes here] off begin
	// % protected region % [Add any extra user attributes here] end
}

function getGroups() {
	return axios.get(`${SERVER_URL}/api/account/groups`)
		.then(({ data }) => {
			return data.map((groupName: any) => { return { display: groupName, value: groupName }});
		});
}

// % protected region % [Customise user model name here] off begin
@entity('user')
// % protected region % [Customise user model name here] end
export default class User extends Model implements IUserAttributes {
	// % protected region % [Add any class properties here] off begin
	// % protected region % [Add any class properties here] end

	// % protected region % [Customize User fields here] off begin
	@Validators.Required()
	@Validators.Length(0, 255)
	@Validators.Email()
	@attribute()
	@observable
	@CRUD({name:'Username', displayType: 'displayfield', headerColumn: true, searchable: true})
	public email: string;

	@Validators.Min(6)
	@Validators.Regex(new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])"), "The password must contain an uppercase letter, a number and a symbol")
	@attribute()
	public password: string;
	// % protected region % [Customize User fields here] end

	// % protected region % [Add any extra user fields here] off begin
	// % protected region % [Add any extra user fields here] end

	// % protected region % [Customize user groups here] off begin
	@attribute()
	@observable
	@CRUD({name:'Groups', displayType: 'reference-multicombobox', referenceTypeFunc: () => Group, headerColumn: false, searchable: false, referenceResolveFunction: getGroups})
	public groups: string[];
	// % protected region % [Customize user groups here] end

	constructor(attributes?: Partial<IUserAttributes>) {
		super(attributes);

		if (attributes) {
			if (attributes.email) {
				this.email = attributes.email;
			}
			if (attributes.password) {
				this.password = attributes.password;
			}
			if (attributes.groups) {
				this.groups = attributes.groups;
			}
			// % protected region % [Add any extra user fields to the constructor here] off begin
			// % protected region % [Add any extra user fields to the constructor here] end
		}

		// % protected region % [Add any extra constructor params here] off begin
		// % protected region % [Add any extra constructor params here] end
	}
	
	public async save(relationPath: {} = {}) {
		// % protected region % [Add any logic before save here] off begin
		// % protected region % [Add any logic before save here] end

		if (this.id === undefined) {
			return createUser(this.toJSON(relationPath))
				.then(({ data }) => this.updateUser(data));
		} 
			
		return updateUser(this.toJSON(relationPath))
			.then(({ data }) => this.updateUser(data));
	}

	public async delete() {
		// % protected region % [Add any logic before delete here] off begin
		// % protected region % [Add any logic before delete here] end

		await deleteUser(this.id);
	}

	@action
	private updateUser(data: {}) {
		// % protected region % [Add any logic before update here] off begin
		// % protected region % [Add any logic before update here] end

		Object.assign(this, data);
	}

	public toJSON(path: {} = {}): {} {
		// % protected region % [Overwrite toJSON here] off begin
		return {
			id: this.id,
			email: this.email,
			groups: this.groups,
		};
		// % protected region % [Overwrite toJSON here] end
	}

	// % protected region % [Add additional methods here] off begin
	// % protected region % [Add additional methods here] end
}
