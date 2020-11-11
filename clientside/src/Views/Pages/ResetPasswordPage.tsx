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
import { Button, Display, Sizes } from '../Components/Button/Button';
import { action, observable } from 'mobx';
import { store } from 'Models/Store';
import { SERVER_URL } from 'Constants';
import axios from 'axios';
import { ButtonGroup, Alignment } from 'Views/Components/Button/ButtonGroup';
import _ from 'lodash';
import alert from '../../Util/ToastifyUtils';
import { Password } from 'Views/Components/Password/Password';
import * as queryString from 'querystring';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

interface IResetPasswordState {
	token: string;
	username: string;
	password: string;
	confirmPassword: string,
	errors: { [attr: string]: string };
	// % protected region % [Add extra reset password state properties here] off begin
	// % protected region % [Add extra reset password state properties here] end
}

const defaultResetPasswordState: IResetPasswordState = {
	token:'',
	username: '',
	password: '',
	confirmPassword:'',
	errors: {},
	// % protected region % [Instantiate extra reset password state properties here] off begin
	// % protected region % [Instantiate extra reset password state properties here] end
};

@observer
export default class ResetPasswordPage extends React.Component<RouteComponentProps> {
	@observable
	private resetPasswordState: IResetPasswordState = defaultResetPasswordState;

	constructor(props:RouteComponentProps, context: any){
		super(props, context);
		let queryParams = this.props.location.search.substring(1);
		const { token, username } = queryString.parse(queryParams);
		this.resetPasswordState.token = token as string;
		this.resetPasswordState.username = username as string;
	}

	public render() {
		let contents = null;

		if (store.loggedIn) {
			// % protected region % [Override redirect here] off begin
			return <Redirect to="/" />;
			// % protected region % [Override redirect here] end
		}

		// % protected region % [Override contents here] off begin
		contents = (
			<div className="body-content">
				<form className="reset-password" onSubmit={this.onResetPasswordClicked}>
					<h2>Reset Password</h2>
					<Password
						id="new_password"
						className="new-password"
						model={this.resetPasswordState}
						modelProperty="password"
						label="New Password"
						isRequired={true}
						errors={this.resetPasswordState.errors['password']} />
					<Password
						id="confirm_password"
						className="confirm-password"
						model={this.resetPasswordState}
						modelProperty="confirmPassword"
						label="Confirm Password"
						isRequired={true}
						errors={this.resetPasswordState.errors['confirmPassword']} />
					<ButtonGroup alignment={Alignment.HORIZONTAL} className="confirm-pwd-buttons">
						<Button type='submit' className="confirm-reset-password" display={Display.Solid} sizes={Sizes.Medium} buttonProps={{ id: "confirm_reset_password" }}>Confirm Password</Button>
					</ButtonGroup>
				</form>
			</div>
		);
		// % protected region % [Override contents here] end
		return contents;
	}

	@action
	private onResetPasswordClicked = (event: React.FormEvent<HTMLFormElement>) => {
		// % protected region % [Override onLoginClicked here] off begin
		event.preventDefault();

		this.resetPasswordState.errors = {};

		if (!this.resetPasswordState.password) {
			this.resetPasswordState.errors['password'] = "Password is required";
		} else if (this.resetPasswordState['password'].length < 6) {
			this.resetPasswordState.errors['password'] = "The minimum length of password is 6";
		} else if (!this.resetPasswordState['confirmPassword']){
			this.resetPasswordState.errors['confirmPassword'] = "Confirm password is required";
		} else if (this.resetPasswordState['confirmPassword'] !== this.resetPasswordState.password) {
			this.resetPasswordState.errors['confirmPassword'] = "Password and confirm password does not match";
		}

		if (Object.keys(this.resetPasswordState.errors).length > 0) {
			return;
		} else {
			axios.post(
				`${SERVER_URL}/api/account/reset-password`,
				{
					token: this.resetPasswordState.token,
					username: this.resetPasswordState.username,
					password: this.resetPasswordState.password,
				})
				.then(({ data }) => {
					this.onConfirmPasswordSent();
				})
				.catch(error => {
					const errorMsgs = error.response.data.errors.map((error: any) => (<p>{error.message}</p>));
					alert(
						<div>
							<h6>Password could not be changed: </h6>
							{errorMsgs}
						</div>,
						'error'
					);
				});
		}
		// % protected region % [Override onLoginClicked here] end
	};

	@action
	private onConfirmPasswordSent = () => {
		// % protected region % [Override login success logic here] off begin
		alert(`Your password has been reset`, 'success');
		store.routerHistory.push(`/login`);
		// % protected region % [Override login success logic here] end
	};

	// % protected region % [Add class methods here] off begin
	// % protected region % [Add class methods here] end
}