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
import classNames from 'classnames';
import InputWrapper, { LabelPositions, InputType } from '../Inputs/InputWrapper';
import { action, observable } from 'mobx';
import { observer } from 'mobx-react';
import { DisplayType } from '../Models/Enums';
import * as uuid from 'uuid';
import { Tooltip } from '../Tooltip/Tooltip';
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

export enum RadioAlignment {
	VERTICAL = "radio-group--vertical",
	HORIZONTAL = "radio-group--horizontal",
}

export interface IRadioButtonGroupProps<T> {
	id?: string;
	model: T;
	modelProperty: string;
	className?: string;
	name?: string;
	alignment?: RadioAlignment;
	displayType?: DisplayType;
	label?: string;
	tooltip?: string;
	isRequired?: boolean;
	isDisabled?: boolean;
	isReadOnly?: boolean;
	innerProps?: React.DetailedHTMLProps<React.HTMLAttributes<HTMLDivElement>, HTMLDivElement>;
	options: Array<{value: string, display: string}>;
	errors?: string | string[];
	/** Action to perform after the onChange method is called */
	onAfterChange?: (value: string) => void;
	/**
	 * Should the key for the fields be random uuids. This is useful for when there is a chance for non unique values.
	 * If this is not set or is false then the option field is used as the key.
	 */
	uuidKey?: boolean;
	// % protected region % [Add any further props here] off begin
	// % protected region % [Add any further props here] end
}

@observer
export class RadioButtonGroup<T> extends React.Component<IRadioButtonGroupProps<T>, any> {
	private uuid = uuid.v4();

	// % protected region % [Add any further methods and fields here] off begin
	// % protected region % [Add any further methods and fields here] end
	
	// % protected region % [customize the RadioButtonGroup render function] off begin
	public render() {
		const id = this.props.id || this.uuid.toString();
		const { displayType, tooltip, isDisabled, isReadOnly, isRequired, errors } = this.props;
		let classes = this.props.className;
		classes = classNames(classes, 'input-group-wrapper__radio');
		classes =  this.props.alignment? classNames(classes, this.props.alignment): classes;

		const label = this.props.label;
		const tooltipId = `${id}-tooltip`;
		const requiredMark = (label && isRequired)? <span className = "required">*</span> : undefined;
		const labelNode = label ? <p className="input-group__radio-header" aria-describedby={tooltip ? tooltipId : undefined}>{label}{requiredMark}</p> : null;
		const tooltipNode = (label && tooltip) ? <Tooltip id={tooltipId} content={tooltip}></Tooltip> : '';
		const groupName = this.props.name || 'DefaultName';

		if(this.props.innerProps){
			this.props.innerProps['aira-live'] = 'assertive';
		}

		return (
			<InputWrapper isInputGroup={true} wrapperId={id} errors={errors} isRequired={isRequired}
				id = {id}
				className={classes}
				displayType={displayType}
				innerProps={this.props.innerProps}>
				{labelNode}
				{tooltipNode}
				{this.props.options.map(option => 
					<InputWrapper
						inputType={InputType.RADIO}
						key={this.props.uuidKey ? uuid.v4() : option.value}
						label={{text:option.display, position:LabelPositions.AFTER}}
						inputId={option.value}>
						<input
							type="radio"
							name={groupName}
							id={option.value}
							defaultChecked={this.props.model[this.props.modelProperty] === option.value}
							key={this.props.uuidKey ? uuid.v4() : option.value}
							onChange={this.onChecked}
							disabled={isDisabled || isReadOnly}
						/>
					</InputWrapper>,
				)}
			</InputWrapper>
		);
	}
	// % protected region % [customize the RadioButtonGroup render function] end

	@action
	public onChecked = (event: React.ChangeEvent<HTMLInputElement>) => {
		this.props.model[this.props.modelProperty] = event.target.id;
		// If there is any logic to be done after the change of the Radio Button Group, do it here
		if (this.props.onAfterChange) {
			this.props.onAfterChange(event.target.id);
		}
	}
}