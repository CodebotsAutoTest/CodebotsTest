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
import * as uuid from 'uuid';
import { observable, runInAction, computed } from 'mobx';
import {
	validator as validatorSymbol,
	attributes as attributesSymbol,
	crudOptions,
	references as referencesSymbol,
	modelName as modelNameSymbol,
	displayName as displayNameSymbol,
	fileAttributes,
	APPLICATION_ID,
} from 'Symbols';
import { store } from './Store';
import gql from 'graphql-tag';
import { lowerCaseFirst } from 'Util/StringUtils';
import { ICRUDOptions, AttributeCRUDOptions } from './CRUDOptions';
import moment from 'moment';
import { IModelAttributeValidationError, IModelValidator, IEntityValidationErrors, IAttributeValidationErrorInfo, PropertyType, ErrorType, IFormFieldValidationError } from 'Validators/Util';
import { IAcl } from './Security/IAcl';
import { IOrderByCondition, IWhereCondition, Comparators } from '../Views/Components/ModelCollection/ModelQuery';
import { getFetchAllConditional, getModelName } from '../Util/EntityUtils';
import { ErrorResponse } from 'apollo-link-error';
import { getTheNetworkError } from 'Util/GraphQLUtils';
import { ICollectionFilterPanelProps } from 'Views/Components/Collection/CollectionFilterPanel';
import _ from 'lodash';
import co from "co";
import { EntityFormMode } from 'Views/Components/Helpers/Common';
import axios from 'axios';
import { SERVER_URL } from 'Constants';
import { DocumentNode } from 'graphql';
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

export type jsonReplacerFn = (input: {[key: string]: any}) => {[key: string]: any};

export interface QueryOptions<TVariables = { [key: string]: any }> {
	query: DocumentNode;
	variables?: TVariables;
	errorPolicy?: 'none' | 'ignore' | 'all';
	fetchResults?: boolean;
	metadata?: any;
	context?: any;
	fetchPolicy?: 'cache-first' | 'network-only' | 'cache-only' | 'no-cache' | 'standby';
	[key: string]: any;
}

export interface IModelType<T extends Model = Model> {
	new (attributes?: Partial<IModelAttributes>): T;
	acls?: IAcl[];
	getOrderByField?: () => IOrderByCondition<T> | undefined;
	fetch(variables?: IConditionalFetchArgs<T>): Promise<T[]>;
}

export interface IModelAttributes {
	id?: string;
	created?: Date;
	modified?: Date;
	// % protected region % [Add any further abstract model properties here] off begin
	// % protected region % [Add any further abstract model properties here] end
}

export type SaveOption = {key: string, value: any, graphQlType: string};
export interface ISaveOptions {
	options?: SaveOption[];
	createOptions?: SaveOption[];
	updateOptions?: SaveOption[];
	graphQlInputType?: string;
	jsonTransformFn?: jsonReplacerFn;
	contentType?: 'application/json' | 'multipart/form-data';
	files?: {attributeName: string, file: Blob}[];
}

interface IBaseFetchArgs<T> {
	id?: string;
	ids?: Array<String>
	orderBy?: Array<IOrderByCondition<T>>;
	skip?: number;
	take?: number;
}

export interface IFetchArgs<T> extends IBaseFetchArgs<T> {
	args?: Array<IWhereCondition<T>>;
}

export interface IConditionalFetchArgs<T> extends IBaseFetchArgs<T> {
	args?: Array<Array<IWhereCondition<T>>>;
}

/**
 * Initialises the attributes array on a target model
 * Cannot be run on the abstract model since that will initialise the same array on all child models
 * @param target {T extends Model} The target class to initialise the attributes object on
 */
function initAttributes(target: any) {
	if (target.getModelName() !== 'Model') {
		if (!target[attributesSymbol]) {
			target[attributesSymbol] = ['id', 'created', 'modified'];
		}
		if (!target[referencesSymbol]) {
			target[referencesSymbol] = [];
		}
		if (!target[fileAttributes]) {
			target[fileAttributes] = [];
		}
	}
}

export interface IAttributeOptions {
	isReference?: boolean;
	file?: string;
}

/**
 * The attribute decorator for all bot written model properties.
 * This will append to the attribute array and decorate with a mobx observer
 */
export function attribute(options?: IAttributeOptions) {
	// % protected region % [Add to the attribute decorator here] off begin
	// % protected region % [Add to the attribute decorator here] end
	return (target: object, key: string) => {
		// Init attributes array
		initAttributes(target);

		// Add to the attributes array for any attributes
		if (options?.isReference) {
			target[referencesSymbol].push(key);
		} else if (options?.file) {
			target[fileAttributes].push({name: key, blob: options.file});
		} else {
			target[attributesSymbol].push(key);
		}

		// Decorate the property with an observable
		// decorate(target, {[key]: observable});
	};
}

/**
 * @param modelName The name of the model
 * @param displayName The display name of the model for presentation purposes, defaults to the modelName
 */
export function entity(modelName: string, displayName?: string) {
	return (target: object) => {
		target[modelNameSymbol] = modelName;
		target[displayNameSymbol] = displayName === undefined
			? modelName
			: displayName
	}
}

export interface IAttributeGroup {
	id: number;
	name: string;
	order: number;
	showName?: boolean;
}

export class Model implements IModelAttributes {
	/**
	 * The acl entries associated to this entity
	 */
	public static acls: IAcl[] = [];

	public static excludeFromCreate: string[] = [];
	public static excludeFromUpdate: string[] = [];

	/**
	 * Client only id for identifying entities before they are sent up to the server
	 */
	public _clientId = uuid.v4();

	/* The default order by field when the collection is loaded */
	public get orderByField(): IOrderByCondition<Model> | undefined {
		return undefined;
	}

	public static getOrderByField() {
		return this.prototype.orderByField;
	}

	@observable
	public validationErrors: IEntityValidationErrors = {};

	@computed
	public get hasValidationError(): boolean {
		return Object.keys(this.validationErrors).some((prop) => {
			var propErrors = this.validationErrors[prop];
			switch(propErrors.type){
				case PropertyType.OWN:
					return true;
				case PropertyType.REFERENCE:
					return Object.keys(this.validationErrors[prop].errors).length > 0;
				case PropertyType.CHILDREN:
					return (this.validationErrors[prop].errors as Array<IEntityValidationErrors>).some((childErrors) => {
						return Object.keys(this.validationErrors[prop].errors).length > 0;
					})
				default:
					return false;
			}
		})
	}

	// Does not support get reference errors and child entity errors yet for now
	public getErrorsForAttribute = (attributeName: string): string[] => {
		if(!!this.validationErrors[attributeName]){
			if(this.validationErrors[attributeName].type === PropertyType.OWN){
				return _
				.chain(Object.keys((this.validationErrors[attributeName].errors)))
				.map((error)=>{
					return this.validationErrors[attributeName].errors[error];
				})
				.flatMap(errors => {
					return errors;
				}).value();
			}
		}
		return [];
	}

	/**
	 * Graphql query fragment that is always included when fetching this entity
	 */
	public defaultExpands = '';

	/**
	 * Graphql query fragment that is only included when fetched from the crud list.
	 * This normally does not contain any references.
	 * This field will not be used when defaultExpands is used.
	 */
	public listExpands = '';

	public attributeGroups?: IAttributeGroup[];

	@observable
	public id: string;

	@observable
	private _created: Date;
	@computed
	public get created() {
		return this._created;
	}
	public set created(date: Date) {
		if (typeof (date) === 'string') {
			this._created = new Date(date);
		} else {
			this._created = date;
		}
	}

	@observable
	private _modified: Date;
	@computed
	public get modified() {
		return this._modified;
	}
	public set modified(date: Date) {
		if (typeof(date) === 'string') {
			this._modified = new Date(date);
		} else {
			this._modified = date;
		}
	}

	// % protected region % [Add any custom attributes to the abstract model here] off begin
	// % protected region % [Add any custom attributes to the abstract model here] end

	constructor(attributes?: Partial<IModelAttributes>) {
		this.assignAttributes(attributes);
	}

	public assignAttributes(attributes?: Partial<IModelAttributes>) {
		if (attributes) {
			if (attributes.id) {
				this.id = attributes.id;
			}
			if (attributes.created) {
				this.created = attributes.created;
			}
			if (attributes.modified) {
				this.modified = attributes.modified;
			}
		}
	}

	/**
	 * Validates a model and any submodels
	 * @param path A path object for choosing which nested models to convert to JSON
	 * @example
	 * myModel.validate();
	 * @example
	 * myModel.validate({relation: {subrelation: {}}});
	 */
	public validate(path: {} = {}): Promise<IEntityValidationErrors> {
		let that: Model = this;
		var promise = new Promise<IEntityValidationErrors>((resolve, reject) => {
			//@ts-ignore
			co(function* () {
				let results: IEntityValidationErrors = {};
				if (that[validatorSymbol]) {
					const ownErrorsArrray: IModelAttributeValidationError[] =
						(yield Promise.all(
							(that[validatorSymbol] as IModelValidator[]).map(
								(validator) => validator(that)
							))
						)
						.filter(
							(error: IModelAttributeValidationError) => {
								if(!_.some(path, _.isEmpty)){
									return !!error;
								} else {
									return (!!error && !!error.attributeName && (path as object).hasOwnProperty(error.attributeName)) ? !!error : null;
								}
							}
						);

					const ownErrors = ownErrorsArrray.reduce((errors, error) => {
						if (errors[error.attributeName] === undefined) {
							errors[error.attributeName] = {
								type: PropertyType.OWN,
								errors: { [`${error.errorType}`]: [error.errorMessage] } as IFormFieldValidationError
							}
						}
						else {
							let errs = errors[error.attributeName].errors;
							if(errs[error.errorType] === undefined){
								errs[error.errorType] = [error.errorMessage];
							} else {
								errs[error.errorType].push(error.errorMessage);
							}
						}
						return errors;
					}, {} as IEntityValidationErrors);

					results = { ...results, ...ownErrors };
				}

				const referencesToValidate = Object.keys(path).filter(key => that.references.includes(key));

				for (const reference of referencesToValidate) {
					if (Array.isArray(that[reference])) {
						const childrenErrors: IEntityValidationErrors[] = (yield Promise.all(
							that[reference].map(
								(item: Model) => item.validate(path[reference])
							))
						).map((oneChildErrors: IEntityValidationErrors) => {
							if (Object.keys(oneChildErrors).length > 0) {
								return oneChildErrors;
							}
							return null;
						});

						results[reference] = {
							type: PropertyType.CHILDREN,
							target: that[reference],
							errors: childrenErrors
						} as IAttributeValidationErrorInfo;
					} else {
						const referenceErrors = (yield that[reference].validate(path[reference])) as IEntityValidationErrors;
						if (Object.keys(referenceErrors).length > 0) {
							results[reference] = { type: PropertyType.REFERENCE, target: that[reference], errors: referenceErrors } as IAttributeValidationErrorInfo;
						}
					}
				}

				runInAction(() => {
					that.validationErrors = results;
				});
				return results;
			}).then(function (results: any) {
				resolve(results as IEntityValidationErrors);
			}, function (err: any) {
				reject(err);
			});
		});

		return promise;
	}

	/**
	 * Clear all the validation errors in a model
	 * @example
	 * myModel.clearErrors();
	 */
	public clearErrors(): void {
		runInAction(() => {
			this.validationErrors = {};
			for (const reference of this.references) {
				if (Array.isArray(this[reference])) {
					(this[reference] as Model[]).forEach((model) => {
						if(!!model){
							model.clearErrors();
						}
					});
				} else {
					if(!!this[reference]){
						if(typeof (this[reference] as Model).clearErrors === 'function') {
							(this[reference] as Model).clearErrors();
						}
					}
				}
			}
		});
	}

	/**
	 * Turns the model into JSON. Can convert selected submodels to JSON as well using the path param.
	 * @param path A path object for choosing which nested models to convert to JSON
	 * @param excludeCrudFields Should excludeFromCreate and excludeFromUpdate be respected
	 * @param replacer A function to mutate the JSON result
	 * @returns A JSON form of the model
	 * @example
	 * myModel.toJSON();
	 * @example
	 * myModel.toJSON({relation: {subrelation: {}}});
	 */
	public toJSON(path: {} = {}, excludeCrudFields = false, replacer: jsonReplacerFn | undefined = undefined) {
		const json = {};
		const pathKeys = Object.keys(path);

		let allKeys = _.uniq(this.attributes.concat(this.references).concat(pathKeys));
		if (excludeCrudFields) {
			const excludeList = this.id
				? this.constructor['excludeFromUpdate']
				: this.constructor['excludeFromCreate'];
			allKeys = allKeys.filter((k: string) => excludeList.indexOf(k) === -1);
		}

		for (const key of allKeys) {
			switch (typeof (this[key])) {
				case 'function':
					// We never want functions
					break;
				case 'object':
					// Format dates as strings
					if (this[key] instanceof Date) {
						json[key] = moment(this[key]).format('YYYY-MM-DD HH:mm:ss');
						break;
					}

					// We only want objects if they are defined in the provided path
					const keyIdx = pathKeys.indexOf(key);
					if (keyIdx >= 0) {
						if (Array.isArray(this[key])) {
							json[key] = this[key].map((model: any) => {
								if (typeof model.toJSON === 'function') {
									return model.toJSON(path[key], true);
								}
								return JSON.parse(JSON.stringify(model));
							});
						} else {
							if (typeof this[key].toJSON === 'function') {
								json[key] = this[key].toJSON(path[key], true);
							} else {
								json[key] = JSON.parse(JSON.stringify(this[key]));
							}
						}
					}
					break;
				case 'undefined':
					break;
				default:
					if (!key.startsWith('_')) {
						json[key] = this[key];
					}
			}
		}

		for (const file of this.files) {
			const fileBlob = this[file.blob];
			if (fileBlob instanceof Blob) {
				json[file.name] = uuid.v5(`${this._clientId}.${file.name}`, APPLICATION_ID);
			} else if (this[file.name]) {
				json[file.name] = this[file.name];
			}
		}

		if (replacer) {
			return replacer(json);
		}
		return json;
	}

	public getFiles(
		path: {} = {},
		excludeCrudFields = false,
		result: { attributeName: string, file: Blob }[] = []):
		{ attributeName: string, file: Blob }[] {

		for (const file of this.files) {
			const fileBlob = this[file.blob];
			if (fileBlob instanceof Blob) {
				result.push({attributeName: uuid.v5(`${this._clientId}.${file.name}`, APPLICATION_ID), file: fileBlob});
			}
		}

		const referenceKeys = Object.keys(path);
		for (const key of referenceKeys) {
			const referenceObj = this[key];
			if (referenceObj instanceof Model) {
				referenceObj.getFiles(path[key], excludeCrudFields, result);
			}
		}

		return result;
	}

	/**
	 * Helper method that reads the display metadata of this object and returns it in a typed format.
	 */
	public getAttributeCRUDOptions(): AttributeCRUDOptions[] {
		const attributeDisplay = [];
		for (const [attributeName, displayOptions] of Object.entries(this[crudOptions])) {
			attributeDisplay.push(new AttributeCRUDOptions(attributeName, displayOptions as ICRUDOptions));
		}

		return attributeDisplay;
	}

	/*
	 * Gets all attributes for the model
	 */
	public get attributes(): string[] {
		return [...this[attributesSymbol]];
	}

	public get references(): string[] {
		return [...this[referencesSymbol]];
	}

	public get files(): {name: string, blob: string}[] {
		return [...this[fileAttributes]];
	}

	public static getAttributes(): string[] {
		return this.prototype[attributesSymbol];
	}

	public static getReferences(): string[] {
		return this.prototype[referencesSymbol];
	}

	public static getFiles(): {name: string, blob: string}[] {
		return this.prototype[fileAttributes];
	}

	public static async fetch<T>(
		variables?: IConditionalFetchArgs<T>,
		expendString?: string,
		apolloOptions?: Partial<QueryOptions<{[key: string]: any}>>,
		useListExpands?: boolean): Promise<T[]> {
		const { data } = await store.apolloClient.query({
			query: getFetchAllConditional(this, expendString, useListExpands),
			variables: variables,
			fetchPolicy: 'network-only',
			...apolloOptions
		});
		return data[lowerCaseFirst(getModelName(this)) + 's'].map((r: any) => new this(r));
	}

	/**
	 * Gets a fetch query with an option for expands
	 * @param expandString A graphql subquery for this entity
	 * @param operationName The name of the graphql operation. Will default to the model name
	 * @param queryName The name of the query query to run
	 * @param useListExpands Should the query only use the list query expands
	 */
	public getFetchWithExpands(expandString: string, operationName?: string, queryName?: string, useListExpands?: boolean) {
		queryName = queryName || lowerCaseFirst(this.getModelName()) + 's';
		const modelsName = operationName || queryName;

		return gql`
			query ${modelsName}($args: [WhereExpressionGraph], $skip:Int, $take:Int, $orderBy: [OrderByGraph], $ids: [ID] ) {
				${modelsName}: ${queryName}(where: $args, skip:$skip, take:$take, orderBy: $orderBy, ids: $ids) {
					${expandString}
					${this.attributes.join('\n')}
					${useListExpands ? this.listExpands : this.defaultExpands}
					${this.files.map(f => f.name).join('\n')}
				}
				count${this.getModelName()}s(where: $args) {
					number
				}
			}`;
	}

	// % protected region % [Customize fetchAllQuery method here] off begin
	/*
	 * Gets all models
	 */
	public get fetchAllQuery() {
		const modelsName = lowerCaseFirst(this.getModelName());

		return gql`
			query ${modelsName}($args: [WhereExpressionGraph], $skip:Int, $take:Int, $orderBy: [OrderByGraph], $ids: [ID] ) {
				${modelsName}s(where: $args, skip:$skip, take:$take, orderBy: $orderBy, ids: $ids) {
					${this.attributes.join('\n')}
					${this.files.map(f => f.name).join('\n')}
					${this.defaultExpands}
				}
				count${this.getModelName()}s(where: $args) {
					number
				}
			}`;
	}
	// % protected region % [Customize fetchAllQuery method here] end

	/*
	 * Gets all models
	 */
	public get fetchSingleQuery() {
		const modelsName = lowerCaseFirst(this.getModelName());

		// $args:[WhereExpressionGraph]
		return gql`
			query ${modelsName} {
				${modelsName} {
					${this.attributes.join('\n')}
					${this.files.map(f => f.name).join('\n')}
					${this.defaultExpands}
				}
			}`;
	}

	public getModelName() {
		// Ignore this since accessing proto is not avaliable in typescript
		// However the model name metadata is stored in the constructor so we need to go though the prototype first
		// @ts-ignore
		return this.__proto__.constructor[modelNameSymbol];
	}

	public getModelDisplayName() {
		// @ts-ignore
		return this.__proto__.constructor[displayNameSymbol];
	}

	/**
	 * Deletes the model from the server
	 */
	public async delete() {
		const functionName = `delete${this.getModelName()}`;

		const modelsName = lowerCaseFirst(this.getModelName());
		const variableName = `${modelsName}Ids`;

		return store.apolloClient
			.mutate({
				mutation: gql`
					mutation delete($${variableName}:[ID]) {
						${functionName}(${variableName}: $${variableName}) {
							id
						}
					}`,
				variables: {[variableName]: [this.id]},
				update: (cache, results) => {

				},
			})
			.then((response) => {
				const data = response.data[functionName][0];
				runInAction(() => {
					Object.assign(this, data);
				});
			})
			.catch((response: ErrorResponse) => {
				const errorMessage = getTheNetworkError(response);
				throw errorMessage;
			});
	}

	/**
	 * Deletes the models from the server by conditions
	 */
	public async deleteWhere(conditions?: Array<IWhereCondition<Model>> | Array<Array<IWhereCondition<Model>>>, ids?: string[]) {
		return new Promise<object>((resolve, reject) => {
			const functionName = `delete${this.getModelName()}sConditional`;

			return store.apolloClient
				.mutate({
					mutation: gql`
						mutation deleteModelsConditional($args: [[WhereExpressionGraph]], $ids:[ID]) {
							${functionName}(conditions: $args, ids: $ids){
								value
							}
						}`,
					variables: {
						"args": conditions,
						"ids": ids || null
					},
					update: (cache, results) => {

					},
				})
				.then((response) => {
					const result = response.data[functionName];
					resolve(result);
				})
				.catch((response: ErrorResponse) => {
					const errorMessage = getTheNetworkError(response);
					reject(errorMessage);
				});
		})
	}

	/**
	 * Updates the models from the server by conditions
	 */
	public async updateWhere(conditions?: Array<IWhereCondition<Model>> | Array<Array<IWhereCondition<Model>>>, modelValuesToUpdate?: Model, ids?: string[]) {
		return new Promise<object>((resolve) => {
			const functionName = `update${this.getModelName()}sConditional`;

			return store.apolloClient
				.mutate({
					mutation: gql`
						mutation updateModelsConditional($where: [[WhereExpressionGraph]], $ids:[ID], $modalValuesToUpdate: ${this.getModelName()}Input) {
							${functionName}(conditions: $args, ids: $ids, filedsToUpdate: $modelValuesToUpdate){
								valule
							}
						}`,
					variables: {
						"where": conditions,
						"modelValuesToUpdate": modelValuesToUpdate,
						"ids": ids || null
					},
					update: (cache, results) => {

					},
				})
				.then((response) => {
					const result = response.data[functionName];
					resolve(result);
				});
		})
	}

	/**
	 * Saves the model to the server
	 * @param relationPath The relation path to be sent to the toJSON method
	 * @param options
	 */
	public async save(relationPath: {} = {}, options: ISaveOptions = {}) {
		const variables = options.options ?? [];
		const createOptions = options.createOptions ?? [];
		const updateOptions = options.updateOptions ?? [];
		const contentType = options.contentType ?? 'application/json';
		const files = options.files ?? [];

		// Before we save, we run this overwriteable method.
		this.beforeSave();

		let functionName: string;
		let jsonModel = this.toJSON(relationPath, true, options.jsonTransformFn);

		if (this.id === undefined) {
			functionName = `create${this.getModelName()}`;
			variables.push(...createOptions);
		} else {
			functionName = `update${this.getModelName()}`;
			variables.push(...updateOptions);
		}

		const modelsName = lowerCaseFirst(this.getModelName());
		const graphQlInputType = options.graphQlInputType || `[${this.getModelName()}Input]`;

		const mutation = gql`
			mutation ${functionName}($${modelsName}:${graphQlInputType}${variables.map(v => `,$${v.key}:${v.graphQlType}`).join(',')}) {
				${functionName}(${modelsName}s: $${modelsName}${variables.map(v => `,${v.key}: $${v.key}`)}) {
					${this.attributes.join('\n')}
					${this.files.map(f => f.name).join('\n')}
				}
			}` as DocumentNode;

		const queryVariables = {
			[modelsName]: [jsonModel],
			...variables.reduce((a, n) => ({[n.key]: n.value, ...a}), {})
		};

		switch (contentType) {
			case 'application/json':
				return store.apolloClient
					.mutate({
						mutation: mutation,
						variables: queryVariables,
						update: (cache, results) => {

						},
					})
					.then((response) => {
						const data = response.data[functionName][0];
						runInAction(() => {
							this.assignAttributes(data);
						});
					});
			case 'multipart/form-data':
				const data = new FormData();
				data.append('variables', JSON.stringify(queryVariables));
				data.append('operationName', functionName);
				data.append('query', mutation.loc?.source.body ?? '');

				for (const file of this.getFiles(relationPath, true)) {
					data.append(file.attributeName, file.file);
				}

				return axios({
					method: 'POST',
					url: `${SERVER_URL}/api/graphql`,
					data: data,
				}).then((response) => {
					const data = response.data.data[functionName][0];
					runInAction(() => {
						this.assignAttributes(data);
					});
				});
			default:
				return Promise.reject("Invalid content type");

		}
	}

	public beforeSave() {
		// Do nothing. This function is here to be overridden
	}

	public saveFromCrud(formMode?: EntityFormMode) {
		return this.save();
	}

	/**
	 * Method to return display name for this model - default is id but can be overridden in sub classes
	 */
	public getDisplayName() {
		return this.id;
	}

	/**
	 * Method to return the display attribute for this model - default is id but can be overridden in sub classes
	 */
	public getDisplayAttribute() {
		return 'id';
	}

	/**
	 * Method to return default search condition for searching the string of searchTerm parameter
	 */
	public getSearchConditions<T extends Model>(searchTerm?: string): Array<Array<IWhereCondition<T>>> | undefined {
		if (searchTerm && searchTerm.trim() !== '') {
			const crudOptions = this.getAttributeCRUDOptions();
			const values: IWhereCondition<T>[] = _
				.chain(crudOptions)
				.filter(attributeOption => attributeOption.searchable)
				.flatMap(attributeOption => {
					const valid = attributeOption.searchTransform(searchTerm.trim());
					if (valid) {
						return [{option: attributeOption, search: valid.query, extraOptions: valid.extraOptions}];
					}
					return [];
				})
				.map(attr => {
					return {
						"path": attr.option.attributeName,
						"comparison": attr.option.searchFunction,
						"value": attr.search,
						...attr.extraOptions,
					};
				})
				.value();

			// If the search is a uuid then we should search on the id of the entity.
			// The following is the regex to match a uuid
			const regex = /^[0-9A-F]{8}-[0-9A-F]{4}-[4][0-9A-F]{3}-[89AB][0-9A-F]{3}-[0-9A-F]{12}$/i;
			if (searchTerm.match(regex)) {
				values.push({
					"path": 'id',
					"comparison": 'equal',
					"value": searchTerm,
				});
			}

			return [values];
		}
		return [];
	}

	/**
	 * Method to convert collection filter array into where condition for collection query
	 */
	public getFilterConditions<T>(filterConfig: ICollectionFilterPanelProps<T>): IWhereCondition<T>[][] | undefined {
		if (filterConfig && filterConfig.filters && filterConfig.filters.some(filter => filter.active === true)) {
			let filterConditions = _
				.chain(filterConfig.filters)
				.filter(filter => filter.active)
				.flatMap(filter => {
					if (filter.comparison === 'range') {
						if (filter.displayType === 'datepicker') {
							return [
								{ ...filter, path: filter.path, comparison: 'greaterThanOrEqual' as Comparators, value1: filter.value1 },
								{ ...filter, path: filter.path, comparison: 'lessThanOrEqual' as Comparators, value1: moment(filter.value2 as Date).add('day', 1).format('YYYY-MM-DD')}
							];
						} else {
							return [
								{ ...filter, path: filter.path, comparison: 'greaterThanOrEqual' as Comparators, value1: filter.value1 },
								{ ...filter, path: filter.path, comparison: 'lessThanOrEqual' as Comparators, value1: filter.value2 }
							];
						}
					}
					else {
						return [filter];
					}
				})
				.map(filter => {
					if (filter.displayType === 'enum-combobox' && Array.isArray(filter.value1)) {
						return filter.value1.map(element => {
							return { path: filter.path, comparison: filter.comparison, value: element } as IWhereCondition<T>
						});
					} else {
						return [{
							path: filter.path,
							comparison: filter.comparison,
							value: filter.value1,
						}] as IWhereCondition<T>[];
					}
				}).value();
			return filterConditions;
		}
		return undefined;
	}

	// % protected region % [Add additional methods here] off begin
	// % protected region % [Add additional methods here] end
}
