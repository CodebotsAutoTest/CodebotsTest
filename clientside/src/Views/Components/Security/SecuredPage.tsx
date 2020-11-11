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
import { observer } from "mobx-react";
import { Redirect } from 'react-router';
import { store } from 'Models/Store';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

export interface ISecuredPageProps {
	/**
	 * The groups that can access this page.
	 * If this is undefined then the page will be available to all
	 */
	groups?: string[];
	// % protected region % [Add any extra props here] off begin
	// % protected region % [Add any extra props here] end
}

/**
 * A secured page is a component to manage the page access with respect to security groups
 */
@observer
export default class SecuredPage extends React.Component<ISecuredPageProps, any> {
	// % protected region % [Add any extra fields here] off begin
	// % protected region % [Add any extra fields here] end
	public render() {
		// % protected region % [Override contents here] off begin
		if (this.props.groups) {
			const { groups } = this.props;
			if (!groups || !groups.length) {
				return <Redirect to="/404" />;
			}
			if (groups.some(r => store.userGroups.map(ug => ug.name).includes(r))) {
				return this.props.children;
			}
			return <Redirect to="/404" />;
		}

		return this.props.children;
		// % protected region % [Override contents here] end
	}

	// % protected region % [Add any extra functions here] off begin
	// % protected region % [Add any extra functions here] end
}