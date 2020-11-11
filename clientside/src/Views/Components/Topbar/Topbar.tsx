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
import { Link } from "react-router-dom";
import { Checkbox } from "../Checkbox/Checkbox";
import { store } from "../../../Models/Store";
import If from "../If/If";

export type uiLocation = 'frontend' | 'admin';
export interface ITopBarProps {
	/** The current location of the application */
	currentLocation: uiLocation;
} 

/**
 * The Topbar component displays the topbar for admins to toggle between the frontend and the backend
 */
@observer
export default class Topbar extends React.Component<ITopBarProps> {
	private buttonLink = ({ location }: {location: uiLocation}) => {
		if (location === 'admin') {
			return <Link to="/" className="icon-right icon-arrow-right-up link-rm-txt-dec active">Frontend</Link>;
		}
		return <Link to="/admin" className="icon-right icon-arrow-right-up link-rm-txt-dec active">Admin</Link>;
	}

	public render() {
		return (
			<If condition={store.hasBackendAccess}>
				<div className="admin__top-bar">
					<ul>
						<If condition={false}>
							<li>
								<Checkbox
									model={store}
									modelProperty="frontendEditMode"
									label="Edit Mode"
									labelVisible={false}
									className="input-group__toggle-switch" />
							</li>
						</If>
						<li>
							<this.buttonLink location={this.props.currentLocation}/>
						</li>
					</ul>
				</div>
			</If>
		)
	}
}