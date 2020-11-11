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
import { Combobox, ComboboxOption } from '../../Combobox/Combobox';
import { computed } from 'mobx';
import { observer } from 'mobx-react';
import { IAttributeProps } from './IAttributeProps';
import { Model } from 'Models/Model';
// % protected region % [Add extra imports here] off begin
// % protected region % [Add extra imports here] end

export interface IAttributeEnumComboboxProps<T extends Model> extends IAttributeProps<T> {
	// % protected region % [Add IAttributeEnumCombobox props here] off begin
	// % protected region % [Add IAttributeEnumCombobox props here] end
}

@observer
export default class AttributeEnumCombobox<T extends Model> extends React.Component<IAttributeEnumComboboxProps<T>> {
	// % protected region % [Add extra options class properties here] off begin
	// % protected region % [Add extra options class properties here] end

	@computed
	private get options() {
		// % protected region % [Customize get options here] off begin
		return this.props.options.enumResolveFunction || [];
		// % protected region % [Customize get options here] end
	}

	public render() {
		// % protected region % [Customize options render here] off begin
		return <Combobox
			model={this.props.model}
			label={this.props.options.name}
			options={this.options}
			modelProperty={this.props.options.attributeName}
			className={this.props.className}
			isDisabled={this.props.isReadonly}
			isRequired={this.props.isRequired}
			errors={this.props.errors}
		/>;
		// % protected region % [Customize options render here] end
	}
}