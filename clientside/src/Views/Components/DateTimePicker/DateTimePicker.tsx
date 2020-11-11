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
import * as uuid from 'uuid';
import { action, computed } from 'mobx';
import { observer } from 'mobx-react';
import { DisplayType } from '../Models/Enums';
import InputWrapper, { InputType } from '../Inputs/InputWrapper';
import InputsHelper from '../Helpers/InputsHelper';
import Flatpickr, { DateTimePickerProps } from 'react-flatpickr';
import { BaseOptions } from 'flatpickr/dist/types/options';
import { Instance } from 'flatpickr/dist/types/instance';
import { store } from 'Models/Store';
// % protected region % [Override flatpicker theme here] off begin
import 'flatpickr/dist/themes/material_green.css';
// % protected region % [Override flatpicker theme here] end

/**
 * Root properties inheritted by all Flatpickr wrapper classes.
 *
 * Implementing Classes:
 *	- DatePicker
 *	- DateRangePicker
 *	- DateTimePicker
 *	- DateTimeRangePicker
 *	- TimePicker
 */
export interface IDateTimePickerProps<T> {
	/* Component Props */
	/** Accept direct user input via the input field iff true. */
	allowInput?: boolean;
	/** String format to display when altInput is true. */
	altFormat?: string;
	/** Convert internal representation to an alt format for presentation iff true. */
	altInput?: boolean;
	/** Brings the component into focus on the page after rendering iff true. */
	autoFocus?: boolean;
	/** Name of the styling class for this component. */
	className?: string;
	/** Internal string format. Used in conjunction with altFormat. */
	dateFormat?: string;
	/** Flatpickr core options. See https://flatpickr.js.org/options/ for details. */
	flatpickrOptions?: Partial<BaseOptions>;
	/** Flatpickr base properties. */
	flatpickrProps?: DateTimePickerProps;
	/** CSS display type. */
	displayType?: DisplayType;
	/** Enable the time input field. */
	enableTime?: boolean;
	/** Errors. */
	errors?: string | string[];
	/** Display in the the component-specific human-friendly format.
	 * Abstraction of the altFormat, altInput and dateFormat props. */
	humanFriendly?: boolean;
	/** Component id. */
	id?: string;
	/** Input entry is required before form submission iff true. */
	isRequired?: boolean;
	/** Input field is disabled iff true. */
	isDisabled?: boolean;
	/** Input field is read-only iff true (same as disabled in practice). */
	isReadOnly?: boolean;
	/** Label for the input field. */
	label?: string;
	/** Display the input field label iff true. */
	labelVisible?: boolean;
	/** Maximum (i.e. latest) selectable date.  */
	maxDate?: string;
	/** Minimum (i.e. earliest) selectable date.  */
	minDate?: string;
	/** Flatpickr mode. No wrapper component currently implemented for multiple. */
	mode?: "single" | "multiple" | "range" | "time";
	/** Model datastructure for input state preservation. */
	model: T;
	/** Index of the modelProperty storing the state of the input to this component. */
	modelProperty: string;
	/** Name for the component instance. */
	name?:string;
	/** Display the calendar input field iff false. */
	noCalendar?: boolean;
	/** Placeholder text string. */
	placeholder?: string;
	/** Static input field iff true (same as disabled in practice). */
	staticInput?: boolean;
	/**  */
	subDescription?: string;
	/* Set the time selection field to select from 24hr or 12hr time.
	 * Note: if you wish to display the the time shown by the input
	 * field after selection in 12hr format, please set the humanFriendly
	 * property to true. */
	time_24hr?: boolean;
	/** Tooltip string. */
	tooltip?: string;
	/* Event handlers */
	/** Callback run after an input change is detected. */
	onAfterChange?: (dates: Date[], currentDateString: string, self: Instance, data?: any) => void;
	/** Callback run on input change and loss of focus. */
	onChangeAndBlur?: (event: React.ChangeEvent<HTMLInputElement>)=>void;
}

/**
 * Creates a datetime picker component. Root Flatpickr wrapper class. All other datetime-related
 * components create an instance of this component and modify the requisite properties.
 *
 * Derived Classes:
 *	- DatePicker
 *	- DateRangePicker
 *	- DateTimeRangePicker
 *	- TimePicker
 */
@observer
export class DateTimePicker<T> extends React.Component<IDateTimePickerProps<T>> {
	/* Component instance-specific properties */
	private uuid = uuid.v4();
	private _input?: HTMLInputElement;
	private valueWhenFocused: string = '';

	/* Component pre-render and construction functions. */

	/**
	 * Compiles the options to be passed to the Flatpickr component from the properties passed
	 * to this component.
	 */
	@computed
	private get flatpickerOptions(): Partial<BaseOptions> {
		/* Flatpickr custom options. See https://flatpickr.js.org/options/ for docs. */
		const options = {
			allowInput: this.props.allowInput ?? true,
			enableTime: ((this.props.enableTime === undefined) ? true : this.props.enableTime),
			maxDate: this.props.maxDate,
			minDate: this.props.minDate,
			mode: (this.props.mode || "single"),
			noCalendar: this.props.noCalendar,
			time_24hr: this.props.time_24hr,
		};

		/* Pass in the class-specific formatting if the humanReadable option was specified
		 * to the Component constructor of a wrapping class. */
		if (this.props.humanFriendly) {
			return {
				...options,
				altInput: true,
				altFormat: (this.props.altFormat || "h:i K j F, Y"),
				dateFormat: (this.props.dateFormat || "Y-m-d H:i"),
			};
		}

		return options;
	}

	/**
	 * Returns the Flatpickr HTML for this instance.
	 */
	private flatpickr = (id: string, fieldId: string, labelVisible: boolean): React.ReactNode => {
		const ariaLabel = !labelVisible ? this.props.label : undefined;
		const ariaDescribedby = InputsHelper.getAriaDescribedBy(
			id, this.props.tooltip, this.props.subDescription);

		/* Type check the value in the model before passing it to the Flatpickr component. */
		let value = this.props.model[this.props.modelProperty];
		if (! (value instanceof Date || (value instanceof Array && value.length > 0
			&& value[0] instanceof Date))) {
			value = undefined;
		}

		const isEditable = !(this.props.isDisabled || this.props.staticInput || this.props.isReadOnly);

		/* Flatpickr component: handles all datetime picker logic */
		return <Flatpickr
			aria-label={ariaLabel}
			aria-describedby={ariaDescribedby}
			disabled={!isEditable}
			value={value}
			id={fieldId}
			className={isEditable ? 'enabled' : 'disabled'}
			name={this.props.name}
			options={{
				onReady: (dates, currentDateString, self) => {
					self.calendarContainer?.classList?.add(store.appLocation);
				},
				...this.flatpickerOptions,
				...this.props.flatpickrOptions,
			}}
			placeholder={this.props.placeholder
				? this.props.placeholder : (this.props.label ? this.props.label : undefined)}
			type="date"
			onChange={this.onChange}
			{...this.props.flatpickrProps}
		/>
	}

	/* Component lifecycle functions. */

	componentDidMount() {
		if(this.props.autoFocus && this._input){
			this._input.focus();
		}
	}

	public render() {
		const id = this.props.id || this.uuid.toString();
		const fieldId = `${id}-field`;
		const labelVisible = (this.props.labelVisible === undefined) ? true : this.props.labelVisible;

		return (
			<InputWrapper inputType={InputType.DATE} id={id} inputId={fieldId}
				className={this.props.className} displayType={this.props.displayType}
				staticInput={this.props.staticInput} isRequired={this.props.isRequired}
				tooltip={this.props.tooltip} subDescription={this.props.subDescription}
				label={this.props.label} labelVisible={labelVisible} errors={this.props.errors}>
				{
					/* Render DateTimePicker */
					this.flatpickr(id, fieldId, labelVisible)
				}
			</InputWrapper>
		);
	}

	/* Render/post-render action functions. */

	@action
	private onChange = (dates: Date[], currentDateString: string, self: Instance, data?: any) => {
		if (this.props.mode === 'range') {
			this.props.model[this.props.modelProperty] = dates;
		} else {
			this.props.model[this.props.modelProperty] = dates[0];
		}
		if (this.props.onAfterChange) {
			this.props.onAfterChange(dates, currentDateString, self, data);
		}
	}

	@action
	private onFocus = (event: React.ChangeEvent<HTMLInputElement>) => {
		this.valueWhenFocused = event.target.value;
	}

	@action
	private onBlur = (event: React.ChangeEvent<HTMLInputElement>) => {
		if(this.valueWhenFocused !== event.target.value && this.props.onChangeAndBlur){
			this.props.onChangeAndBlur(event);
		}
	}
}