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
import { ButtonGroup, Alignment } from '../Button/ButtonGroup';

export interface IBottomActionBarProps {
	groups: React.ReactNode[];
}

@observer
export class BottomActionBar<T> extends React.Component<IBottomActionBarProps> {
	public render() {
		return (
			<section aria-label="action-bar" className="action-bar">
				{this.props.groups.map((e, i) => <ButtonGroup alignment={Alignment.HORIZONTAL} key={i}>
					{e}
				</ButtonGroup>)}
			</section>
		);
	}
}