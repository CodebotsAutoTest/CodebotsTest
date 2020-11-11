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
import { Symbols } from 'Symbols';
import { Model } from 'Models/Model';

export type IModelValidator = (model: Model) => Promise<IModelAttributeValidationError | null>;

export function initValidators(target: any, key: string) {
	if (!target[Symbols.validator]) {
		target[Symbols.validator] = Array<IModelValidator>();
	}
	if (!target[Symbols.validatorMap]) {
		target[Symbols.validatorMap] = {};
	}
	if (!target[Symbols.validatorMap][key]) {
		target[Symbols.validatorMap][key] = [];
	}
}

export interface IModelAttributeValidationError {
	errorType: ErrorType;
	attributeName: string;
	errorMessage: string;
	target: Model;
}

export enum ErrorType {
	REQUIRED = 'required',
	EXISTS = 'exists',
	NOT_EXIST = 'notExist',
	LENGTH = 'length',
	INVALID = 'invalid',
	RANGE = 'range',
	UNKNOW = 'unknow'
}

export enum PropertyType {
	OWN = 'own',
	REFERENCE = 'reference',
	CHILDREN = 'children',
}

export type IFormFieldValidationError = {
	[key in ErrorType]?: string[];
};

export interface IAttributeValidationErrorInfo{
	type: PropertyType,
	target?: Model,
	errors: IFormFieldValidationError | IEntityValidationErrors | Array<IEntityValidationErrors>
};

export interface IEntityValidationErrors {
	[prop: string]: IAttributeValidationErrorInfo
}

// todo: not finished
export function getFieldErrorMessages(fieldName: string, fieldErrors: IEntityValidationErrors){
	if(fieldErrors[fieldName]){
		return Object.keys(fieldErrors[fieldName]).map((errorType, i)=>{
			return fieldErrors[fieldName][errorType];
		})
	}else{
		return undefined;
	}
}

// todo: not finished
export function setFieldValidate(fieldName: string, fieldErrors: IEntityValidationErrors, valid?: boolean) {
	if(fieldErrors[fieldName]){
		let fieldError = fieldErrors[fieldName];
		if(fieldError){
			if(valid !== undefined){
				
			}
		}
	}
}