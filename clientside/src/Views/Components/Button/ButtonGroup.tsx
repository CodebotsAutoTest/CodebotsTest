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
import classNames from 'classnames';

export enum Alignment {
	VERTICAL = "btn-group--vertical",
	HORIZONTAL = "btn-group--horizontal",
}

export enum Sizing {
	GROW = "btn-group--grow-elements",
	STATIC = "btn-group--static-elements",
}

export interface IButtonGroupProps {
	className?: string;
	alignment?: Alignment;
	sizing?: Sizing;
	innerProps?: React.DetailedHTMLProps<React.HTMLAttributes<HTMLElement>, HTMLElement>;
}


@observer
export class ButtonGroup extends React.Component<IButtonGroupProps, any> {
	public render() {
		const innerProps = this.props.innerProps || {};
		const className = `btn-group ${innerProps.className ? innerProps.className : ''}`;
		let classes = className;

		const {alignment, sizing} = this.props; 
		classes = classNames(classes, alignment ? alignment : '');
		classes = classNames(classes, sizing ? sizing : '');
		classes = classNames(classes, this.props.className);

		return (
			<section {...this.props.innerProps} className={classes}>
				{this.props.children}
			</section>
		);
	}
}