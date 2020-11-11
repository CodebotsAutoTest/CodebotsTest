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
import { observer } from 'mobx-react';
import If from 'Views/Components/If/If';
import { ButtonGroup } from 'Views/Components/Button/ButtonGroup';
import { Button, Display } from 'Views/Components/Button/Button';
// % protected region % [Add extra imports here] off begin
// % protected region % [Add extra imports here] end

export interface IFormProps {
	/** Use the default Submit button or not */
	submitButton?: boolean;
	/** Use the default Cancel button or not */
	cancelButton?: boolean;
	/**
	 * Action Groups
	 * If specified, the default submit and cancel action buttons will not display unconditionally
	 * Then you need to use this actionGroups to specify all the actions button
	 */
	actionGroups?: React.ReactNode[];
	/** The callback function when submit event is triggered */
	onSubmit?: React.FormEventHandler<Element>;
	/** The callback function when cencel button is pressed */
	onCancel?: React.FormEventHandler<Element>;
	// % protected region % [Add extra form props here] off begin
	// % protected region % [Add extra form props here] end
}

@observer
export class Form extends React.Component<IFormProps> {
    // % protected region % [Modify render method] off begin
	render() {
		let actionGroups: React.ReactNode[] | undefined;
		if (this.props.actionGroups) {
			actionGroups = this.props.actionGroups;
		} else {
			actionGroups = [
				<If condition={this.props.cancelButton}>
					<Button className="cancel btn--md" type='button' display={Display.Outline} buttonProps={{ onClick: this.props.onCancel }}>Cancel</Button>
				</If>,
				<If condition={this.props.submitButton}>
					<Button className="submit btn--md" type='submit' display={Display.Solid}>Submit</Button>
				</If>
			];
		}

		return (
			<form onSubmit={this.props.onSubmit}>
				<div className="crud__form-container">
					{this.props.children}
				</div>
				<ButtonGroup>
					{actionGroups.map((node, i) => <React.Fragment key={i}>{node}</React.Fragment>)}
				</ButtonGroup>
			</form>
		);
	}
	// % protected region % [Modify render method] end
}