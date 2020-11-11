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
import _ from 'lodash';
import AwesomeDebouncePromise from 'awesome-debounce-promise';
import { store } from 'Models/Store';
import { IModelAttributes, Model } from 'Models/Model';
import { Combobox } from 'Views/Components/Combobox/Combobox';
import { getFetchAllConditional, getModelName, isRequired } from 'Util/EntityUtils';
import { lowerCaseFirst } from 'Util/StringUtils';
import { action, computed, observable } from 'mobx';
import { IAttributeProps } from './IAttributeProps';
import { DropdownProps } from 'semantic-ui-react';

interface IAttributeReferenceComboboxProps<T extends Model> extends IAttributeProps<T> {
	/** A function that returns an object that can construct a model */
	referenceType: { new(json?: {}): Model };
	/**
	 * If this is true then changing the option in the combobox will assign the entire entity to the property in the
	 * model as opposed to just the id
	 */
	fetchReferenceEntity?: boolean;
	/** A function to compare an option value with a value */
	optionEqualFunc?: (modelProperty: Model, option: string) => boolean;
	/** Is the combobox readonly */
}

class AttributeReferenceCombobox<T extends Model> extends React.Component<IAttributeReferenceComboboxProps<T>> {
	@observable
	private internalOptions: Model[] = [];

	@computed
	public get isRequired() {
		return isRequired(this.props.model, this.props.options.attributeName);
	}

	@action
	private setInternalOptions = (options: Model[]) => {
		this.internalOptions = options;
	}

	@action
	private onChange = (event: React.SyntheticEvent<HTMLElement>, data: DropdownProps) => {
		const { value } = data;
		if (value !== undefined && value !== null) {
			if (this.props.fetchReferenceEntity) {
				this.props.model[this.props.options.attributeName] = _.find(this.internalOptions, x => x.id === value);
			} else {
				this.props.model[this.props.options.attributeName] = value;
			}
		}

		if (this.props.onAfterChange) {
			this.props.onAfterChange(event);
		}
	}

	private fetchOptions = async (query?: string | string[]) => {
		const { referenceType } = this.props;

		if (Array.isArray(query)) {
			return [];
		}

		const modelName = getModelName(referenceType);
		const dataReturnName = lowerCaseFirst(modelName) + "s";

		return store.apolloClient
			.query({
				query: getFetchAllConditional(this.props.referenceType),
				variables: {
					args: query ? new this.props.referenceType().getSearchConditions(this.props.fetchReferenceEntity ? query['id'] : query) : null,
					take: 10,
				},
				fetchPolicy: 'network-only',
			})
			.then(({ data }) => {
				let associatedObjects: any[] = data[dataReturnName];
				if (this.props.model.id !== null && this.props.model.id !== undefined) {
					associatedObjects = associatedObjects.filter((d: IModelAttributes) => d.id !== this.props.model.id);
				}
				let existingValue = undefined;
				if (this.props.fetchReferenceEntity) {
					existingValue = this.props.model[this.props.options.attributeName];
				} else {
					existingValue = this.props.model[this.props.options.attributeName.slice(0, -2)];
				}

				if (existingValue) {
					associatedObjects = _.unionBy(associatedObjects, [existingValue], x => x.id);
				}
				let comboOptions: Array<{ display: string, value: string }> = [];
				if (associatedObjects) {
					const optionObjects = associatedObjects.map(obj => new referenceType(obj));
					this.setInternalOptions(optionObjects);
					comboOptions = optionObjects
						.map(obj => ({ display: obj.getDisplayName(), value: obj.id }));
				}

				return comboOptions;
			});
	}

	private getOptions = () => {
		return AwesomeDebouncePromise(this.fetchOptions, 500);
	}

	public render() {
		return <Combobox
			model={this.props.model}
			modelProperty={this.props.options.attributeName}
			label={this.props.options.displayName}
			options={this.getOptions()}
			errors={this.props.errors}
			className={this.props.className}
			placeholder={this.isRequired ? undefined : '-'}
			onChange={this.onChange}
			isDisabled={this.props.isReadonly}
			isRequired={this.props.isRequired}
			onAfterChange={this.props.onAfterChange}
			initialOptions={this.fetchOptions}
			isClearable={!this.props.isRequired}
			optionEqualFunc={(modelProperty, option) => {
				if (this.props.fetchReferenceEntity && modelProperty) {
					return modelProperty['id'] === option
				}
				return modelProperty === option;
			}}
		/>;
	}
}

export default AttributeReferenceCombobox;