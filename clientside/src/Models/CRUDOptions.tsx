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
import * as React from 'react';
import { crudOptions as crudOptions } from 'Symbols';
import { Model } from './Model';
import { Comparators } from "Views/Components/ModelCollection/ModelQuery";
import { transformFunction } from 'Util/AttributeUtils';

export type displayType =
	'hidden' |
	'textfield' |
	'textarea' |
	'password' |
	'checkbox' |
	'form-data'|
	'workflow-data'|
	'datepicker' |
	'timepicker' |
	'datetimepicker' |
	'displayfield' |
	'enum-combobox'|
	'reference-combobox' |
	'reference-multicombobox' |
	'form-tile' |
	'file'
	// % protected region % [Add more display types here] off begin
	// % protected region % [Add more display types here] end
	;

export interface ICRUDOptions {
	name: string;
	/** Classname to render out to the DOM */
	className?: string;
	/** How to display the field on the crud form */
	displayType: displayType;
	/** Weather this is a header to be displayed in the crud list */
	headerColumn?: boolean;
	/** Is the column searchable */
	searchable?: boolean;
	/** What graphql search function is used for the search */
	searchFunction?: Comparators;
	/**
	 * A function that takes the query and transforms it to a valid state for the search.
	 * An example of this would be transforming dates into a format the server can understand.
	 * If this function returns null then the search is not performed on this attribute.
	 */
	searchTransform?: transformFunction;
	/** Anonymous props for the attribute that is being used */
	inputProps?: {[key: string]: any};
	/** A function that can change the display of the element on the crud list */
	displayFunction?: (attribute: any, that: Model) => React.ReactNode;
	onAfterChange?: (model: Model) => void;

	fileAttribute?: string;

	// Reference Dropdown specific
	referenceTypeFunc?: () => {new (json?: {}):  Model};
	referenceResolveFunction?: (search: string | string[], options: {model: Model}) => Promise<Array<{display: string, value: any}>>;
	enumResolveFunction?: Array<{display: string, value: string}>;
	optionEqualFunc?: (modelProperty: Model, option: string) => boolean;
	/**
	 * Weather the reference to assign to the attribute is on a join field or is the entity
	 */
	isJoinEntity?: boolean;
	/** Can default options be removed from the combobox */
	disableDefaultOptionRemoval?: boolean;

	readFieldType?: displayType;
	createFieldType?: displayType;
	updateFieldType?: displayType;

	/** The id of the attribute group which this attribute belongs to */
	groupId?: number;
	/** The order of the attribute with the group which this attribute belongs to */
	order?: number;
	// % protected region % [Add more CRUD Option interface props here] off begin
	// % protected region % [Add more CRUD Option interface props here] end
}

export class AttributeCRUDOptions implements ICRUDOptions {
	public name: string;
	public className?: string;
	public attributeName: string;
	public displayType: displayType;
	public headerColumn: boolean;
	public searchable: boolean;
	public searchFunction: Comparators;
	public searchTransform: transformFunction = (attr: string) => ({query: attr});
	public inputProps?: {[key: string]: any};
	public referenceResolveFunction?: (search: string | string[], options: {model: Model}) => Promise<Array<{display: string, value: string}>>;
	public enumResolveFunction?: Array<{display: string, value: string}>;
	public optionEqualFunc?: (modelProperty: any, option: any) => boolean;
	public isJoinEntity?: boolean = false;
	public displayFunction?: (attribute: any, that: Model) => React.ReactNode;
	public onAfterChange?: (model: Model) => void;

	public fileAttribute?: string;

	public readFieldType?: displayType;
	public createFieldType?: displayType;
	public updateFieldType?: displayType;
	public disableDefaultOptionRemoval?: boolean;


	// Reference Dropdown specific
	public referenceTypeFunc?: () => {new (json?: {}):  Model};
	public isReadonly?: boolean = false;
	/** The id of the attribute group which this attribute belongs to */
	public groupId?: number;
	/** The order of the attribute with the group which this attribute belongs to */
	public order?: number;
	// % protected region % [Add more Attribute CRUD Option properties here] off begin
	// % protected region % [Add more Attribute CRUD Option properties here] end

	constructor(attributeName: string, options: ICRUDOptions) {
		this.attributeName = attributeName;
		this.name = options.name;
		this.className = options.className;
		this.displayType = options.displayType;
		this.headerColumn = !!options.headerColumn;
		this.searchable = !!options.searchable;
		this.referenceTypeFunc = options.referenceTypeFunc;
		this.searchFunction = options.searchFunction || 'contains';
		this.referenceResolveFunction = options.referenceResolveFunction;
		this.enumResolveFunction = options.enumResolveFunction;
		this.optionEqualFunc = options.optionEqualFunc;
		this.isJoinEntity = options.isJoinEntity;
		this.inputProps = options.inputProps;
		this.displayFunction = options.displayFunction;
		this.onAfterChange = options.onAfterChange;
		this.disableDefaultOptionRemoval = options.disableDefaultOptionRemoval;
		if (options.searchTransform) {
			this.searchTransform = options.searchTransform;
		}

		this.readFieldType = options.readFieldType || options.displayType;
		this.createFieldType = options.createFieldType || options.displayType;
		this.updateFieldType = options.updateFieldType || options.displayType;
		this.groupId = options.groupId;
		this.order = options.order;
		this.fileAttribute = options.fileAttribute;
		// % protected region % [Add more Attribute CRUD Option constructor operations here] off begin
		// % protected region % [Add more Attribute CRUD Option constructor operations here] end
	}

	public get displayName() {
		return this.name;
	}

	// % protected region % [Add more CRUD Option member functions here] off begin
	// % protected region % [Add more CRUD Option member functions here] end

}

export function CRUD(options: ICRUDOptions) {
	return (target: object, key: string) => {
		if (!target[crudOptions]) {
			target[crudOptions] = {};
		}
		target[crudOptions][key] = options;
	};
}

// % protected region % [Add more CRUD Option related functions here] off begin
// % protected region % [Add more CRUD Option related functions here] end