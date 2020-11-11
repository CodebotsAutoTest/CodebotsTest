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
import React, {Component} from 'react';
import classNames from 'classnames';

// % protected region % [Add extra page imports here] off begin
// % protected region % [Add extra page imports here] end

// % protected region % [Modify CardGroupProps here] off begin	
interface CardGroupProps {
	layout?: 'horizontal' | 'vertical';
	className?: string;
}
// % protected region % [Modify CardGroupProps here] end

// % protected region % [Add extra implementation here] off begin
// % protected region % [Add extra implementation here] end

export default class CardGroup extends Component<CardGroupProps> {
	
	// % protected region % [Add extra class implementation here] off begin
	// % protected region % [Add extra class implementation here] end
	
	// % protected region % [Modify render here] off begin	
	render() {
		return (
			<div className={classNames('cards-wrapper', this.props.layout ? this.props.layout : 'horizontal', this.props.className)}>
				{this.props.children}
			</div>
		);
	}
	// % protected region % [Modify render here] end
}