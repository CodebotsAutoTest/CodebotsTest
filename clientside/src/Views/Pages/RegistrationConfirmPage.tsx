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
import { SERVER_URL } from '../../Constants';
import { RouteComponentProps } from "react-router";
import queryString from "querystring";
import Spinner from 'Views/Components/Spinner/Spinner';
import If from 'Views/Components/If/If';
import axios from 'axios';
import { getErrorMessages } from 'Util/GraphQLUtils';
import { observable, runInAction, action } from 'mobx';
import alert from '../../Util/ToastifyUtils';
import { store } from 'Models/Store';
import { ButtonGroup, Alignment } from 'Views/Components/Button/ButtonGroup';
import { Button, Display, Sizes } from 'Views/Components/Button/Button';
// % protected region % [Add any extra imports or interfaces here] off begin
// % protected region % [Add any extra imports or interfaces here] end

/**
 * Logs the user out of the application
 *
 * Can take a location query param that will redirect after the logout occurs
 */
// % protected region % [Customise class signature and class properties] off begin
@observer
export default class RegistrationConfirmPage extends React.Component<RouteComponentProps> {
	// % protected region % [Customise class signature and class properties] end
	@observable
	private loading: boolean = false;
	@observable
	private confirmed: boolean = false;
	private confirmEmailData: { token: string, email: string };

	public componentDidMount() {
		const { token, username } = queryString.parse(this.props.location.search.substring(1)) || undefined;
		this.confirmEmailData = {
			token: token as string,
			email: username as string
		};
	}

	public render() {
		// % protected region % [Override render here] off begin
		let pageContent = (
			<div className="registration registration-confirm">
				<h2>Confirm your registration</h2>
				<p>Please click the button below to finish your registration</p>
				<ButtonGroup alignment={Alignment.HORIZONTAL} className="confirm-reg-buttons">
					<Button type='submit' className="confirm-registration" display={Display.Solid} sizes={Sizes.Medium} buttonProps={{ id: "confirm_registration" }} onClick={this.onClickConfirm}>Confirm Registration</Button>
				</ButtonGroup>
			</div>
		);

		if (this.confirmed) {
			pageContent = (
				<div className="body-content">
					{this.loading && <Spinner />}
					<If condition={!this.loading}>
						<div className="registration registration-success">
							<h2>Registration successful</h2>
							<p>Your email has been confirmed successfully</p>
							<a className="login-link" onClick={this.onLoginClick}>Login</a>
						</div>
					</If>
				</div>
			);
		}

		return pageContent;
		// % protected region % [Override render here] end
	}

	private onLoginClick = () => {
		// % protected region % [Override onLoginClick here] off begin
		const { redirect } = queryString.parse(this.props.location.search.substring(1));
		if (redirect) {
			store.routerHistory.push(`/login?redirect=${redirect}`);
		}
		store.routerHistory.push('/login');
		// % protected region % [Override onLoginClick here] end
	};

	@action
	private onClickConfirm = () => {
		// % protected region % [Override onClickConfirm here] off begin
		this.loading = true;
		axios.post(
			`${SERVER_URL}/api/register/confirm-email`, this.confirmEmailData)
			.then(({ data }) => {
				runInAction(() => {
					this.loading = false;
					this.confirmed = true;
				})
			})
			.catch(response => {
				runInAction(() => {
					this.loading = false;
				})
				const errorMessages = getErrorMessages(response).map((error: any) => (<p>{error.message}</p>));
				alert(
					<div>
						<h6>Email Confirmation was not successful</h6>
						{errorMessages}
					</div>,
					'error'
				);
			});
		// % protected region % [Override onClickConfirm here] end
	};

	// % protected region % [Add class methods here] off begin
	// % protected region % [Add class methods here] end
}

// % protected region % [Add extra features here] off begin
// % protected region % [Add extra features here] end
