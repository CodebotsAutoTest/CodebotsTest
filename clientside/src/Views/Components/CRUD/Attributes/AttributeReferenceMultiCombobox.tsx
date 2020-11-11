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
import { Model } from 'Models/Model';
import { getFetchAllQuery, getModelName } from 'Util/EntityUtils';
import { lowerCaseFirst } from 'Util/StringUtils';
import { observer } from 'mobx-react';
import { action, computed, observable, runInAction } from 'mobx';
import { MultiCombobox } from '../../Combobox/MultiCombobox';
import { store } from 'Models/Store';
import _ from 'lodash';
import AwesomeDebouncePromise from 'awesome-debounce-promise';
import { IAttributeProps } from './IAttributeProps';
import { ComboboxOption } from 'Views/Components/Combobox/Combobox';
import { crudId } from 'Symbols';

export type dropdownData = Array<{ display: string, value: any }>;
type state = 'loading' | 'error' | 'success';

export interface IAttributeReferenceComboboxProps<T extends Model> extends IAttributeProps<T> {
	/** A function that returns the type of model that is on the combobox */
	referenceType: { new(json?: {}): Model };
	/** A function to override loading of the data into the dropdown */
	referenceResolveFunction?: (search: string | string[], options: {model: T}) => Promise<dropdownData>;
	/** Should data be loaded immediately on placing this element into the DOM */
	// synchronousLoad?: boolean;
	/** A function to compare an option value with a value */
	optionEqualFunc?: (modelProperty: Model, option: string) => boolean;
	/** Is the entity in this combobox a join entity */
	isJoinEntity?: boolean;
	/** Can default options be removed from the combobox */
	disableDefaultOptionRemoval?: boolean;
}

/**
 * A dropdown menu that populates itself with references to other objects
 */
@observer
export default class AttributeReferenceMultiCombobox<T extends Model> extends React.Component<IAttributeReferenceComboboxProps<T>> {
	static defaultProps: Partial<IAttributeReferenceComboboxProps<Model>> = {
		optionEqualFunc: (modelProperty, option) => modelProperty.id === option,
	};

	@observable
	public requestState: {state: state, data?: dropdownData} = {state: 'loading'};

	@observable
	public options: T[] = [];

	@computed
	private get modelName() {
		const modelName = getModelName(this.props.referenceType);
		return lowerCaseFirst(modelName) + "s";
	}

	@computed
	private get joinProp() {
		return this.props.options.attributeName.substring(0, this.props.options.attributeName.length - 1);
	}

	private _defaultOptions: Model[] = [];

	@computed
	private get defaultOptions(): ComboboxOption<Model>[] {
		return this._defaultOptions.map(r => {
			r[crudId] = r.id;
			return {
				display: this.props.isJoinEntity ? r[this.joinProp].getDisplayName() : r.getDisplayName(),
				value: r,
				isFixed: this.props.disableDefaultOptionRemoval,
			}
		});
	}

	@computed
	public get resolveFunc() {
		if (this.props.referenceResolveFunction) {
			return _.partial(this.props.referenceResolveFunction, _, {model: this.props.model});
		}

		const query = getFetchAllQuery(this.props.referenceType);
		return () => store.apolloClient.query({query: query, fetchPolicy: 'network-only'})
			.then((data) => {
				const associatedObjects: any[] = data[this.modelName];
				let comboOptions: dropdownData = [];
				if (associatedObjects) {
					comboOptions = associatedObjects.map(obj => new this.props.referenceType(obj))
						.map(obj => ({ display: obj.getDisplayName(), value: obj }));
				}
				return comboOptions;
			});
	}

	constructor(props: IAttributeReferenceComboboxProps<T>, context: any) {
		super(props, context);

		const modelProp: Model[] | undefined = this.props.model[this.props.options.attributeName];
		if (Array.isArray(modelProp)) {
			this._defaultOptions = [...modelProp];
		}
	}

	public mutateOptions = (query: string | string[]): Promise<ComboboxOption<Model>[]> => {
		const { isJoinEntity } = this.props;
		return this.resolveFunc(query)
			.then(e => {
				if (this.props.model.id !== null && this.props.model.id !== undefined) {
					e = e.filter(d => d.value.id !== this.props.model.id);
				}
				runInAction(() => {
					this.options = _.unionBy(
						this.options,
						this.props.model[this.props.options.attributeName],
						e.map(x => x.value),
						isJoinEntity ? this.joinProp + 'Id' : 'id',
					);
				});
				return e.map(x => {
					const option = {
						display: isJoinEntity ? x.value[this.joinProp].getDisplayName() : x.value.getDisplayName(),
						value: x.value,
						isFixed: false,
					};

					if (this.props.disableDefaultOptionRemoval) {
						if (_.find(this.defaultOptions, x.value)) {
							option.isFixed = true;
						}
					}

					return option;
				});
			});
	}

	public getOptions = () => {
		return AwesomeDebouncePromise(this.mutateOptions, 500);
	}

	@action
	private updateData = (data: dropdownData) => {
		this.requestState = {
			state: 'success',
			data,
		};
	}

	@action
	private errorData = () => {
		this.requestState = {
			state: 'error',
		};
	}

	@action
	private getInitialOption = (): Promise<ComboboxOption<Model>[]> => {
		const { isJoinEntity } = this.props;
		return this.mutateOptions('')
			.then(d => {
				return _.unionBy(d, this.defaultOptions, option =>
					this.props.isJoinEntity
						? option.value ? option.value[[this.props.options.attributeName.slice(0, -1)] + 'Id'] : undefined
						: (option.value ? option.value.id : undefined)
				);
			})
			.catch(e => this.defaultOptions);
	};

	public render() {
		return <MultiCombobox<T, Model>
			label={this.props.options.displayName}
			model={this.props.model}
			modelProperty={this.props.options.attributeName}
			options={this.getOptions()}
			errors={this.props.errors}
			className={this.props.className}
			isDisabled={this.props.isReadonly}
			onAfterChange={this.props.onAfterChange}
			isRequired={this.props.isRequired}
			initialOptions={this.getInitialOption}
			getOptionValue={option => {
				return this.props.isJoinEntity
					? (option ? option[this.props.options.attributeName.slice(0, -1)].id : undefined)
					: (option ? option.id : undefined)
			}}
			inputProps={{
				header: 'Type to search for entities'
			}}
		/>;
	}
}
