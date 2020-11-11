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
import { AttributeCRUDOptions } from 'Models/CRUDOptions';
import { Password } from '../../Password/Password';
import { IAttributeProps } from './IAttributeProps';

interface IAttributePasswordProps<T extends Model> extends IAttributeProps<T> {
}

class AttributePassword<T extends Model> extends React.Component<IAttributePasswordProps<T>> {
	public render() {
		const { model, options, errors, className, isReadonly, isRequired } = this.props;
		return <Password
			model={model}
			modelProperty={options.attributeName}
			label={options.displayName}
			errors={errors}
			className={className}
			isReadOnly={isReadonly}
			isRequired={isRequired}
			onChangeAndBlur={this.props.onAfterChange}
			inputProps={{
				autoComplete: "new-password"
			}}
		/>;
	}
}

export default AttributePassword;