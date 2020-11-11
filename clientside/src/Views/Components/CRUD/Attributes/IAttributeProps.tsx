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
import { Model } from 'Models/Model';
import { AttributeCRUDOptions } from 'Models/CRUDOptions';
import { EntityFormMode } from 'Views/Components/Helpers/Common';

export interface IAttributeProps<T extends Model> {
	/** The model to bind the component to */
	model: T;
	/** The crud options for this attribute */
	options: AttributeCRUDOptions;
	/** The class name for this field */
	className?: string;
	/** If this field is readonly */
	isReadonly?: boolean;
	/** If this field is required */
	isRequired?: boolean;
	/** A list of validation errors for this field */
	errors?: string[];
	/** The call back function to trigger after the model got changed */
	onAfterChange?: (event: any) => void;
	/** The mode the CRUD form is in */
	formMode: EntityFormMode;
}