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
import { IUserResult, store } from 'Models/Store';
import axios from 'axios';
import { action, observable } from 'mobx';
import Spinner from '../Spinner/Spinner';
import { Redirect, RouteComponentProps } from 'react-router';
import { SERVER_URL } from 'Constants';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

export interface AuthProps extends Partial<RouteComponentProps> {
	/** If true then don't redirect on login check failure. */
	checkOnly?: boolean
	/** The location that the component will redirect to on login check failure. */
	redirectLocation?: string;
	/** Callback on login check success. */
	onAfterSuccess?: () => void;
	/** Callback on login check failure. Note that if checkOnly is not set to true the user will still be redirected. */
	onAfterError?: () => void;
	// % protected region % [Add any extra props here] off begin
	// % protected region % [Add any extra props here] end
}

/**
 * This component handles the requirement for a user being logged in before they can access the child components.
 *
 * If the user is logged in then the this component will just render the child props.
 *
 * If the user is not flagged as logged in with Javascript then it will send a request to the server to see if they are
 * actually authenticated. If they are then the user is set as logged in, otherwise they are redirected to the login
 * page.
 */
@observer
export default class Auth extends React.Component<AuthProps> {
	// % protected region % [Add any extra fields here] off begin
	// % protected region % [Add any extra fields here] end

	@observable
	// % protected region % [Override requestState here] off begin
	private requestState: 'pending' | 'error' | 'success' = 'pending';
	// % protected region % [Override requestState here] end

	@action
	private onSuccess = (userResult?: IUserResult) => {
		// % protected region % [Override onSuccess here] off begin
		if (userResult) {
			store.setLoggedInUser(userResult);
		}
		this.requestState = 'success';
		this.props.onAfterSuccess?.();
		// % protected region % [Override onSuccess here] end
	};

	@action
	private onError = () => {
		// % protected region % [Override onError here] off begin
		store.clearLoggedInUser();
		this.requestState = 'error';
		this.props.onAfterError?.()
		// % protected region % [Override onError here] end
	}

	public componentDidMount() {
		// % protected region % [Override componentDidMount here] off begin
		// If we are already logged in then we don't need to check again
		if (store.loggedIn) {
			this.onSuccess();
			return;
		}

		// Otherwise send a request to the server to see if the token is valid
		axios.get(`${SERVER_URL}/api/account/me`)
			.then(({data}) => this.onSuccess(data))
			.catch(this.onError);
		// % protected region % [Override componentDidMount here] end
	}

	public render() {
		// % protected region % [Override contents here] off begin
		const { location, children, redirectLocation, checkOnly } = this.props;
		const redirect = redirectLocation ?? location?.pathname ?? store.routerHistory.location.pathname;

		switch (this.requestState) {
			case 'pending':
				return <Spinner />;
			case 'success':
				return this.props.children;
			case 'error':
				if (checkOnly) {
					return children;
				}
				return <Redirect to={{pathname: '/login', search: `?redirect=${redirect}`}} />;
		}
		// % protected region % [Override contents here] end
	}

	// % protected region % [Add any extra functions here] off begin
	// % protected region % [Add any extra functions here] end
}