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
import { action, observable, runInAction } from 'mobx';
import * as uuid from 'uuid';
import InputWrapper, { InputType, LabelPositions } from '../Inputs/InputWrapper';
import { DisplayType } from '../Models/Enums'
import InputsHelper from '../Helpers/InputsHelper';

interface ICbCheckboxProps<T> {
	model: T;
	modelProperty: string;
	id?: string;
	name?: string;
	className?: string;
	displayType?: DisplayType;
	label?: string;
	labelVisible?: boolean;
	isRequired?: boolean;
	isDisabled?: boolean;
	isReadOnly?: boolean;
	tooltip?: string;
	subDescription?: string;
	onChecked?: ((event: React.ChangeEvent<HTMLInputElement>, checked: boolean) => void);
	onAfterChecked?: ((event: React.ChangeEvent<HTMLInputElement>, checked: boolean) => void);
	inputProps?: React.DetailedHTMLProps<React.InputHTMLAttributes<HTMLInputElement>, HTMLInputElement>;
	errors?: string | string[];
}

@observer
export class Checkbox<T> extends React.Component<ICbCheckboxProps<T>, any> {
	public static defaultProps = {};
	private uuid = uuid.v4();

	public render() {
		const {name, className, displayType, label, isRequired, isDisabled, isReadOnly, tooltip, subDescription, errors} = this.props;
		const id = this.props.id || this.uuid.toString();
		const fieldId = `${id}-field`;

		const labelVisible = (this.props.labelVisible === undefined) ? true : this.props.labelVisible;
		const ariaLabel = !labelVisible ? label : undefined;

		const ariaDescribedby = InputsHelper.getAriaDescribedBy(id, tooltip, subDescription);

		return (
			<InputWrapper id={id} inputType={InputType.CHECKBOX} className={className} displayType={displayType} isRequired={isRequired} tooltip={tooltip} subDescription={subDescription} inputId={fieldId} label={label ? { text: label, position: LabelPositions.AFTER } : undefined} labelVisible={labelVisible} errors={errors}>
				<input
					type="checkbox"
					id={fieldId}
					name={name}
					onChange={this.onChecked}
					checked={this.props.model[this.props.modelProperty] || false}
					disabled={isDisabled || isReadOnly}
					aria-label={ariaLabel}
					aria-describedby={ariaDescribedby}
					{...this.props.inputProps}
				/>
			</InputWrapper>
		);
	}

	@action
	public onChecked = (event: React.ChangeEvent<HTMLInputElement>) => {
		if (this.props.onChecked) {
			return this.props.onChecked(event, event.target.checked);
		}

		this.props.model[this.props.modelProperty] = event.target.checked;

		return this.props.onAfterChecked?.(event, event.target.checked);
	}
}