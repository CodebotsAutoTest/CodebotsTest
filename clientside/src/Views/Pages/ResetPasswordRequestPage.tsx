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
import { Button, Display, Sizes, Colors } from '../Components/Button/Button';
import { action, observable } from 'mobx';
import { TextField } from '../Components/TextBox/TextBox';
import { store } from 'Models/Store';
import { SERVER_URL } from 'Constants';
import axios from 'axios';
import { ButtonGroup, Alignment } from 'Views/Components/Button/ButtonGroup';
import _ from 'lodash';
import { isEmail } from 'Validators/Functions/Email';
import alert from '../../Util/ToastifyUtils';
import { getErrorMessages } from 'Util/GraphQLUtils';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

interface IResetRequestState {
	username: string; 
	errors: { [attr: string]: string };
	// % protected region % [Add extra reset password request state properties here] off begin
	// % protected region % [Add extra reset password request state properties here] end
}

const defaultResetRequestState: IResetRequestState = {
	username: '',
	errors: {},
	// % protected region % [Instantiate extra reset password request state properties here] off begin
	// % protected region % [Instantiate extra reset password request state properties here] end
};

@observer
export default class ResetPasswordRequestPage extends React.Component<RouteComponentProps> {
	@observable
	private ResetRequestState: IResetRequestState = defaultResetRequestState;

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
					<TextField 
						id="username"
						className="username"
						model={this.ResetRequestState}
						modelProperty="username"
						label="Email Address"
						inputProps={{ autoComplete: 'username' }}
						isRequired={true}
						errors={this.ResetRequestState.errors['username']} />
					<ButtonGroup alignment={Alignment.HORIZONTAL} className="reset-pwd-buttons">
						<Button type='submit' display={Display.Solid} sizes={Sizes.Medium} buttonProps={{ id: "reset_password" }}>Reset Password</Button>
						<Button className="cancel-reset-pwd" display={Display.Outline} sizes={Sizes.Medium} colors={Colors.Secondary} buttonProps={{ id: "cancel_reset_pwd" }} onClick={this.onCancelResetClicked}>Cancel</Button>
					</ButtonGroup>
				</form>
			</div>
		);
		// % protected region % [Override contents here] end
		return contents;
	}

	@action
	private onResetPasswordClicked = (event: React.FormEvent<HTMLFormElement>) => {
		// % protected region % [Override onResetPasswordClicked here] off begin
		event.preventDefault();

		this.ResetRequestState.errors = {};

		if (!this.ResetRequestState.username) {
			this.ResetRequestState.errors['username'] = "Email Address is required";
		} else if (!isEmail(this.ResetRequestState.username)) {
			this.ResetRequestState.errors['username'] = "This is not a valid email address";
		}
		
		if (Object.keys(this.ResetRequestState.errors).length > 0) {
			return;
		} else {
			axios.post(
				`${SERVER_URL}/api/account/reset-password-request`,
				{
					username: this.ResetRequestState.username,
				})
				.then(({ data }) => {
					this.onResetPasswordSent();
				})
				.catch(response => {
					const errorMessages = getErrorMessages(response).map((error: any) => (<p>{error.message}</p>));
					alert(
						<div>
							<h6>Sending request failed</h6>
							{errorMessages}
						</div>,
						'error'
					);
				});
		}
		// % protected region % [Override onResetPasswordClicked here] end
	};

	@action
	private onCancelResetClicked = (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
		// % protected region % [Override onCancelResetClicked here] off begin
		store.routerHistory.push(`/login`);
		// % protected region % [Override onCancelResetClicked here] end
	};

	@action
	private onResetPasswordSent = () => {
		// % protected region % [Override onResetPasswordSent here] off begin
		alert(`Reset Password Email has been sent to ${this.ResetRequestState.username}`, 'success');
		store.routerHistory.push(`/login`);
		// % protected region % [Override onResetPasswordSent here] end
	};

	// % protected region % [Add class methods here] off begin
	// % protected region % [Add class methods here] end
}