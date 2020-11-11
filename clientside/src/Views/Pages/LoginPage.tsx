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
import { RouteComponentProps } from 'react-router';
import { Redirect } from 'react-router';
import { Button, Display, Colors, Sizes } from '../Components/Button/Button';
import { action, observable, runInAction } from 'mobx';
import { TextField } from '../Components/TextBox/TextBox';
import { IUserResult, store } from 'Models/Store';
import { SERVER_URL } from 'Constants';
import axios from 'axios';
import * as queryString from 'querystring';
import { ButtonGroup, Alignment } from 'Views/Components/Button/ButtonGroup';
import { Password } from 'Views/Components/Password/Password';
import _ from 'lodash';
import { isEmail } from 'Validators/Functions/Email';
import alert from '../../Util/ToastifyUtils';
import { getErrorMessages } from 'Util/GraphQLUtils';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

// % protected region % [Customise ILoginState here] off begin
interface ILoginState {
	username: string;
	password: string;
	errors: { [attr: string]: string };
}
// % protected region % [Customise ILoginState here] end

// % protected region % [Customise defaultLoginState here] off begin
const defaultLoginState: ILoginState = {
	username: '',
	password: '',
	errors: {},
};
// % protected region % [Customise defaultLoginState here] end

// % protected region % [Add any extra constants here] off begin
// % protected region % [Add any extra constants here] end

@observer
// % protected region % [Override class signature here] off begin
export default class LoginPage extends React.Component<RouteComponentProps> {
// % protected region % [Override class signature here] end
	@observable
	private loginState: ILoginState = defaultLoginState;

	// % protected region % [Add any extra fields here] off begin
	// % protected region % [Add any extra fields here] end

	// % protected region % [Override render here] off begin
	public render() {
		let contents = null;

		if (store.loggedIn) {
			return <Redirect to="/" />;
		}

		contents = (
			<div className="body-content">
				<form className="login" onSubmit={this.onLoginClicked}>
					<h2>Login</h2>
					<TextField
						id="login_username"
						className="login-username"
						model={this.loginState}
						modelProperty="username"
						label="Email Address"
						inputProps={{ autoComplete: 'username', type: "email" }}
						isRequired={true}
						errors={this.loginState.errors['username']} />
					<Password
						id="login_password"
						className="login-password"
						model={this.loginState}
						modelProperty="password"
						label="Password"
						inputProps={{ autoComplete: "current-password" }}
						isRequired={true}
						errors={this.loginState.errors['password']} />
					<ButtonGroup alignment={Alignment.HORIZONTAL} className="login-buttons">
						<Button type='submit' className="login-submit" display={Display.Solid} sizes={Sizes.Medium} buttonProps={{ id: "login_submit" }}>Login</Button>
					</ButtonGroup>
					<p>
						<a className='link-forgotten-password link-rm-txt-dec' onClick={this.onForgottenPasswordClick}>Forgot your password? </a>
					</p>
				</form>
			</div>
		);
		return contents;
	}
	// % protected region % [Override render here] end

	// % protected region % [Override onLoginClicked here] off begin
	@action
	private onLoginClicked = (event: React.FormEvent<HTMLFormElement>) => {
		event.preventDefault();

		this.loginState.errors = {};

		if (!this.loginState.username) {
			this.loginState.errors['username'] = "Email Address is required";
		} else if (!isEmail(this.loginState.username)) {
			this.loginState.errors['username'] = "This is not a valid email address";
		}
		if (!this.loginState.password) {
			this.loginState.errors['password'] = "Password is required";
		}

		if (Object.keys(this.loginState.errors).length > 0) {
			return;
		} else {
			axios.post(
				`${SERVER_URL}/api/authorization/login`,
				{
					username: this.loginState.username,
					password: this.loginState.password,
				})
				.then(({ data }) => {
					this.onLoginSuccess(data);
				})
				.catch(response => {
					const errorMessages = getErrorMessages(response).map((error: any) => {
						const message = typeof error.message === 'object' 
							? JSON.stringify(error.message)
							: error.message;
						return (<p>{message}</p>);
					});
					alert(
						<div>
							<h6>Login failed</h6>
							{errorMessages}
						</div>,
						'error'
					);
				});
		}
	};
	// % protected region % [Override onLoginClicked here] end

	// % protected region % [Override onStartRegisterClicked here] off begin
	@action
	private onStartRegisterClicked = (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
		const { redirect } = queryString.parse(this.props.location.search.substring(1));
		store.routerHistory.push(`/register?${!!redirect ? `redirect=${redirect}` : ''}`);
	};
	// % protected region % [Override onStartRegisterClicked here] end

	// % protected region % [Override login success logic here] off begin
	@action
	private onLoginSuccess = (userResult: IUserResult) => {
		store.setLoggedInUser(userResult);

		const { redirect } = queryString.parse(this.props.location.search.substring(1));

		if (redirect && !Array.isArray(redirect)) {
			store.routerHistory.push(redirect);
		} else {
			store.routerHistory.push('/');
		}
	};
	// % protected region % [Override login success logic here] end

	// % protected region % [Override onForgottenPasswordClick here] off begin
	@action
	private onForgottenPasswordClick = (e: React.MouseEvent<HTMLAnchorElement, MouseEvent>) => {
		store.routerHistory.push(`/reset-password-request`);
	};
	// % protected region % [Override onForgottenPasswordClick here] end

	// % protected region % [Add class methods here] off begin
	// % protected region % [Add class methods here] end
}

// % protected region % [Add additional exports here] off begin
// % protected region % [Add additional exports here] end
