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
import { IAcl } from '../IAcl';

export class SuperAdministratorScheme implements IAcl {
	public group?: string;
	public isVisitorAcl = false;
	public constructor() {
		this.group = 'Super Administrators';
	}
	public canRead(): boolean {
		// % protected region % [Override read rule contents here here] off begin
		return true;
		// % protected region % [Override read rule contents here here] end
	}
	public canCreate(): boolean {
		// % protected region % [Override create rule contents here here] off begin
		return true;
		// % protected region % [Override create rule contents here here] end
	}
	public canUpdate(): boolean {
		// % protected region % [Override update rule contents here here] off begin
		return true;
		// % protected region % [Override update rule contents here here] end
	}
	public canDelete(): boolean {
		// % protected region % [Override delete rule contents here here] off begin
		return true;
		// % protected region % [Override delete rule contents here here] end
	}
}