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
import { observer } from 'mobx-react';
import { action } from 'mobx';
import InputWrapper, { InputType } from '../Inputs/InputWrapper';
import { DisplayType } from '../Models/Enums';
import * as uuid from 'uuid';
import classNames from 'classnames';
import InputsHelper from '../Helpers/InputsHelper';

interface IPasswordProps<T> {
	model: T;
	modelProperty: string;
	id?: string;
	name?:string;
	className?: string;
	displayType?: DisplayType;
	label?: string;
	labelVisible?: boolean;
	isRequired?: boolean;
	isDisabled?: boolean;
	isReadOnly?: boolean;
	tooltip?: string;
	subDescription?: string;
	placeholder?: string;
	clickToClear?: boolean;
	errors?: string | string[];
	onAfterChange?: (event: React.ChangeEvent<HTMLInputElement>) => void;
	onChangeAndBlur?: (event: React.ChangeEvent<HTMLInputElement>) => void;
	inputProps?: React.InputHTMLAttributes<Element>;
}

@observer
export class Password<T> extends React.Component<IPasswordProps<T>, any> {
	public static defaultProps = {
		inputProps: {},
		className: '',
	};
	private uuid = uuid.v4();
	private valueWhenFocused: string = '';

	public render() {
		const { model, modelProperty, name, className, displayType, label, isRequired, isDisabled, isReadOnly, tooltip, subDescription, clickToClear, placeholder, errors } = this.props;
		const id = this.props.id || this.uuid.toString();
		const fieldId = `${id}-field`;

		const labelVisible = (this.props.labelVisible === undefined) ? true : this.props.labelVisible;
		const ariaLabel = !labelVisible ? label : undefined;

		const ariaDescribedby = InputsHelper.getAriaDescribedBy(id, tooltip, subDescription);
		
		return (
			<InputWrapper inputType={InputType.PASSWORD} id={id} inputId={fieldId}  className={className} displayType={displayType} isRequired={isRequired} tooltip={tooltip} subDescription={subDescription} label={label} labelVisible={labelVisible} errors={errors}>
				<input 
					type="password"
					name={name}
					id={fieldId}
					value={model[modelProperty] ? model[modelProperty] : ''}
					onChange={this.onChange}
					onBlur={this.onBlur}
					placeholder={placeholder ? placeholder : (label ? label : undefined)}
					disabled={isDisabled}
					readOnly={isReadOnly}
					aria-label={ariaLabel}
					aria-describedby={ariaDescribedby}
				{...this.props.inputProps} />
			</InputWrapper>
		);
	}

	@action
	private onChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		this.props.model[this.props.modelProperty] = event.target.value;
		if (this.props.onAfterChange) {
			this.props.onAfterChange(event);
		}
	}

	@action
	private onBlur = (event: React.ChangeEvent<HTMLInputElement>) => {
		if(this.valueWhenFocused !== event.target.value && this.props.onChangeAndBlur){
			this.props.onChangeAndBlur(event);
		}
	}

	@action
	private onClickToClear = (event: React.MouseEvent<HTMLButtonElement>) => {
		this.props.model[this.props.modelProperty] = '';
	}
}