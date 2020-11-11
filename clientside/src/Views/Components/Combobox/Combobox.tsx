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
import * as React from "react";
import _ from 'lodash';
import { observer } from 'mobx-react';
import { DisplayType } from '../Models/Enums';
import {
	Dropdown,
	DropdownItemProps,
	DropdownProps,
	Label,
	StrictDropdownProps,
} from 'semantic-ui-react';
import InputWrapper from 'Views/Components/Inputs/InputWrapper';
import classNames from 'classnames';
import { action, computed, observable } from 'mobx';
import Spinner from 'Views/Components/Spinner/Spinner';

// % protected region % [Add extra imports here] off begin
// % protected region % [Add extra imports here] end

export interface ComboboxOption<I> {
	/** The value to display in the combobox item dropdown */
	display: React.ReactNode;
	/** The value to select */
	value: I | undefined;
	/** In a multi combobox can the value be removed */
	isFixed?: boolean;
}

interface InternalComboboxProps<T, I> {
	/** The model to change the attribute of */
	model: T;
	/** The property on the model to change */
	modelProperty: string;
	/**
	 * Gets an identifying property from the value object. This must be a unique value and not an object.
	 * By default this will get the value object itself
	 */
	getOptionValue?: (option: I | undefined) => undefined | boolean | number | string;
	/**
	 * A function to compare the model property to the selected option
	 * @param modelProperty The model property to compare
	 * @param option The option from the combobox
	 */
	optionEqualFunc?: (modelProperty: string | number | boolean | undefined, option: I | undefined) => boolean;
	/** The to display around the combobox */
	label: string;
	/** Weather the label is visible */
	labelVisible?: boolean;
	/** The tooltip to display */
	tooltip?: string;
	/** The display type to use */
	displayType?: DisplayType;
	/** The classname for to combobox */
	className?: string;
	/** Raw props that are passed through to the react-select component */
	inputProps?: DropdownProps;
	/** The placeholder text when the combobox is empty */
	placeholder?: string;
	/** A list of errors that are to be displayed around the combobox */
	errors?: string | string[];
	/** The minimum length of search string with can be searched, default to 1 */
	minSearchLength?: number;
	/** If the combobox is isDisabled */
	isDisabled?: boolean;
	/** If the field is required */
	isRequired?: boolean;
	/** Override of the onChange function. Using this will remove the model binding logic of the component */
	onChange?: (event: React.SyntheticEvent<HTMLElement>, data: DropdownProps) => void;
	/** Action to perform after the onChange method is called */
	onAfterChange?: (event: React.SyntheticEvent<HTMLElement>, data: DropdownProps) => void;
	/** Is the select value clearable **/
	isClearable?: boolean;
}

export interface SyncComboboxProps<T, I> extends InternalComboboxProps<T, I> {
	/**
	 * The options on the dropdown
	 * An array of JSON objects that by default will have the form of {display: string, value: string}
	 *
	 * The key and value properties can be overriden with the getOptionLabel and getOptionValue props
	 */
	options: ComboboxOption<I | undefined>[];
	/** If the combobox is searchable */
	searchable?: boolean | ((options: DropdownItemProps[], value: string) => DropdownItemProps[]);
	/** If the combobox is in a loading state */
	loading?: boolean;
}

export interface AsyncComboboxProps<T, I> extends InternalComboboxProps<T, I> {
	/**
	 * The options on the dropdown
	 * A function that returns an promise resolving to an array of JSON objects that by default will have the form of
	 * {display: string, value: string}
	 *
	 * The key and value properties can be overriden with the getOptionLabel and getOptionValue props
	 */
	options: (input: string) => Promise<ComboboxOption<I | undefined>[]>;
	/**
	 * The initial options that are displayed in the combobox before any search occurs
	 */
	initialOptions?: () => Promise<ComboboxOption<I | undefined>[]>;
}

interface InternalSyncComboboxProps<T, I> extends SyncComboboxProps<T, I> {
	getOptionValue: (option: I | undefined) => undefined | boolean | number | string;
}

interface InternalAsyncComboboxProps<T, I> extends AsyncComboboxProps<T, I> {
	getOptionValue: (option: I | undefined) => undefined | boolean | number | string;
}

export type IComboboxProps<T, I> = AsyncComboboxProps<T, I> | SyncComboboxProps<T, I>;

/**
 * A dropdown selection menu for selecting an item from a list of options
 */
@observer
export class Combobox<T, I> extends React.Component<IComboboxProps<T, I>> {
	static defaultProps = {
		labelVisible: true,
		minSearchLength: 0,
		searchable: true
	};

	private _reFetch?: () => void = undefined;
	public get reFetch() {
		if (this._reFetch) {
			return this._reFetch;
		}
		return function() {};
	}

	private getOptionValue = (option: I | undefined) => this.props.getOptionValue
		? this.props.getOptionValue(option)
		: option as string | number | boolean | undefined;

	public render() {
		return (
			<InputWrapper
				className={classNames('input-group__dropdown', this.props.className)}
				label={this.props.label}
				errors={this.props.errors}
				labelVisible={this.props.labelVisible}
				tooltip={this.props.tooltip}
				displayType={this.props.displayType}
				inputName={this.props.modelProperty}
				isRequired={this.props.isRequired}>
				{
					typeof this.props.options === 'function'
					? <AsyncCombobox
							{...this.props}
							ref={ref => this._reFetch = ref?.reFetch}
							options={this.props.options}
							getOptionValue={this.getOptionValue}
						/>
					: <SyncCombobox
							{...this.props}
							options={this.props.options}
							getOptionValue={this.getOptionValue}
						/>
				}
			</InputWrapper>
		);
	}
}

@observer
class SyncCombobox<T, I> extends React.Component<InternalSyncComboboxProps<T, I>> {
	public render() {
		return <InnerCombobox
			model={this.props.model}
			modelProperty={this.props.modelProperty}
			getOptionValue={this.props.getOptionValue}
			optionResults={this.props.options}
			search={this.props.searchable}
			loading={this.props.loading}
			placeholder={this.props.placeholder}
			clearable={this.props.isClearable}
			onAfterChange={this.props.onAfterChange}
			disabled={this.props.isDisabled}
			onChange={this.props.onChange}
			optionEqualFunc={this.props.optionEqualFunc}
			{...this.props.inputProps}
		/>;
	}
}

type loadingState = 'loading' | 'error' | 'done';

@observer
class AsyncCombobox<T, I> extends React.Component<InternalAsyncComboboxProps<T, I>> {
	@observable
	private requestState: loadingState = 'done';

	@observable
	private initialRequestState: loadingState = 'loading';

	@observable
	private optionResults: ComboboxOption<I | undefined>[] = [];

	private lastSearchQuery = '';

	public reFetch = () => this.searchOptions(this.lastSearchQuery);

	@action
	private searchOptions = (data: string): Promise<void> => {
		this.requestState = 'loading';
		this.lastSearchQuery = data;
		return this.props.options(data)
			.then(this.onSearchSuccess)
			.catch(this.onSearchFail);
	}

	@action
	private onSearchSuccess = (options: ComboboxOption<I | undefined>[], initial?: boolean) => {
		this.optionResults.length = 0;
		this.optionResults.push(...options);
		if (initial) {
			this.initialRequestState = 'done';
		} else {
			this.requestState = 'done';
		}
	};

	@action
	private onSearchFail = (error: any, initial?: boolean) => {
		console.error(error);
		if (initial) {
			this.initialRequestState = 'error';
		} else {
			this.requestState = 'error';
		}
	};

	constructor(props: InternalAsyncComboboxProps<T, I>, context: any) {
		super(props, context);
		if (this.props.initialOptions) {
			this.props.initialOptions()
				.then(_.partial(this.onSearchSuccess, _, true))
				.catch(_.partial(this.onSearchFail, _, true));
		} else {
			this.onSearchSuccess([], true);
		}
	}

	// % protected region % [Add any extra Component lifecycle methods here] off begin
	// % protected region % [Add any extra Component lifecycle methods here] end

	public render() {
		switch (this.initialRequestState) {
			case 'loading':
				return <Spinner />;
			case 'error':
				return <div>There was an error loading the combobox data</div>;
			case 'done':
				return <InnerCombobox
					model={this.props.model}
					modelProperty={this.props.modelProperty}
					getOptionValue={this.props.getOptionValue}
					search={options => options}
					optionResults={this.optionResults}
					onSearchChange={(e, data) => this.searchOptions(data.searchQuery)}
					placeholder={this.props.placeholder}
					loading={this.requestState === 'loading'}
					disabled={this.props.isDisabled}
					minCharacters={this.props.minSearchLength}
					clearable={this.props.isClearable}
					onAfterChange={this.props.onAfterChange}
					onChange={this.props.onChange}
					optionEqualFunc={this.props.optionEqualFunc}
					{...this.props.inputProps}
				/>;
		}
	}
}

interface InnerComboboxProps<T, I> extends StrictDropdownProps {
	model: T;
	modelProperty: string;
	optionResults: ComboboxOption<I | undefined>[];
	getOptionValue: (option: I | undefined) => undefined | boolean | number | string;
	onAfterChange?: (event: React.SyntheticEvent<HTMLElement>, data: DropdownProps) => void;
	optionEqualFunc?: (modelProperty: string | number | boolean | undefined, option: I | undefined) => boolean;
}

@observer
class InnerCombobox<T, I> extends React.Component<InnerComboboxProps<T, I>> {
	@computed
	private get options() {
		return this.props.optionResults.map((option) => ({
			text: option.display,
			value: this.props.getOptionValue(option.value),
			isFixed: option.isFixed,
			key: this.props.getOptionValue(option.value),
			as: (props: any) => <div {..._.omit(props, 'isFixed')} data-id={this.props.getOptionValue(option.value)}>
				{props.children}
			</div>
		}));
	}

	@computed
	private get selectedItem() {
		const modelProperty = this.props.model[this.props.modelProperty];

		if (!(modelProperty === null || modelProperty === undefined)) {
			if (this.props.multiple && Array.isArray(modelProperty)) {
				return _.chain(this.props.optionResults)
					.filter(option => {
						return _.some(modelProperty, modelProp => this.optionsEqual(this.props.getOptionValue(modelProp), option));
					})
					.flatMap(option => {
						const value = this.props.getOptionValue(option.value);
						if (value !== undefined) {
							return value;
						}
						return [];
					})
					.value();
			} else {
				// If there is a value already selected in the model then we want that one to be selected
				const value = this.props.optionResults.find(option => {
					return this.optionsEqual(this.props.getOptionValue(modelProperty), option);
				});
				if (value) {
					return this.props.getOptionValue(value.value);
				}
			}
		}

		return undefined;
	}

	private set selectedItem(value: undefined | boolean | number | string | (boolean | number | string)[]) {
		if (Array.isArray(value)) {
			if (!Array.isArray(this.props.model[this.props.modelProperty])) {
				this.props.model[this.props.modelProperty] = [];
			}

			this.props.model[this.props.modelProperty].length = 0;
			const selected = this.props.optionResults
				.filter(option => {
					return _.some(value, modelProp => this.optionsEqual(modelProp, option));
				})
				.map(option => option.value);
			this.props.model[this.props.modelProperty].push(...selected);
		} else {
			const selected = this.props.optionResults.find(option => {
				return this.optionsEqual(value, option);
			});

			if (!(selected === null || selected === undefined)) {
				this.props.model[this.props.modelProperty] = selected.value;
			} else {
				this.props.model[this.props.modelProperty] = undefined;
			}
		}
	}

	@action
	private onChange = (event: React.SyntheticEvent<HTMLElement>, data: DropdownProps) => {
		// If the onChange is overwritten then we should just use that one instead
		if (this.props.onChange) {
			return this.props.onChange(event, data);
		} else {
			const { value } = data;
			this.selectedItem = value;
			// this.props.model[this.props.modelProperty] = value;
		}

		// If there is any logic to be done after the change of the combobox, do it here
		if (this.props.onAfterChange) {
			this.props.onAfterChange(event, data);
		}
	};

	private optionsEqual(modelProp: string | number | boolean | undefined, option: ComboboxOption<I | undefined>) {
		if (this.props.optionEqualFunc) {
			return this.props.optionEqualFunc(modelProp, option.value);
		}
		return this.props.getOptionValue(option.value) === modelProp;
	}

	public render() {
		return <Dropdown
			selection={true}
			disabled={this.props.disabled}
			onChange={this.onChange}
			value={this.selectedItem}
			data-id={Array.isArray(this.selectedItem) ? this.selectedItem.join(",") : this.selectedItem}
			options={this.options}
			className={classNames('dropdown__container', this.props.multiple ? undefined : 'single')}
			renderLabel={(item, index, defaultLabelProps) => {
				return <>
					<Label
						{...defaultLabelProps}
						data-id={item.value}
						content={item.text}
						key={typeof item.value === 'string' || typeof item.value === 'number' ? item.value : index}
						onRemove={item.isFixed ? undefined : defaultLabelProps.onRemove} />
				</>
			}}
			// % protected region % [Add any extra props to Dropdown in InnerCombobox here] off begin
			// % protected region % [Add any extra props to Dropdown in InnerCombobox here] end
			{..._.omit(
				this.props,
				'model',
				'modelProperty',
				'optionResults',
				'getOptionValue',
				'onAfterChange',
				'optionEqualFunc',
				'selection',
				'onChange',
				'value',
				'data-id',
				'options',
				'className',
				'renderLabel',
			)}
		/>;
	}
}