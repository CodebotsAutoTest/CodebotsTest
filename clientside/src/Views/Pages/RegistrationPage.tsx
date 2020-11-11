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
import { action, observable, runInAction } from 'mobx';
import { store } from 'Models/Store';
import axios from 'axios';
import * as queryString from 'querystring';
import { ButtonGroup, Alignment } from 'Views/Components/Button/ButtonGroup';
import If from 'Views/Components/If/If';
import { Combobox } from 'Views/Components/Combobox/Combobox';
import * as Models from '../../Models/Entities';
import _ from 'lodash';
import { Model } from 'Models/Model';
import { isRequired } from 'Util/EntityUtils';
import { getAttributeComponent } from 'Views/Components/CRUD/Attributes/AttributeFactory';
import alert from '../../Util/ToastifyUtils';
import { SERVER_URL } from 'Constants';
import { getErrorMessages } from 'Util/GraphQLUtils';
import Spinner from 'Views/Components/Spinner/Spinner';
import { EntityFormMode } from 'Views/Components/Helpers/Common';
import { IUserEntity, UserFields } from 'Models/UserEntity';
// % protected region % [Add any extra imports or interfaces here] off begin
// % protected region % [Add any extra imports or interfaces here] end

interface IRegistrationState {
	errors: { [attr: string]: string };
	selectedRegisterType: string,
	modelToRegister: Model & IUserEntity | null,
	emailSending: boolean,
	emailSent: boolean,
	// % protected region % [Add extra registration state properties here] off begin
	// % protected region % [Add extra registration state properties here] end
}

const defaultRegistrationState: IRegistrationState = {
	selectedRegisterType: '',
	modelToRegister: null,
	errors: {},
	emailSending: false,
	emailSent: false,
	// % protected region % [Instantiate extra registration state properties here] off begin
	// % protected region % [Instantiate extra registration state properties here] end
}

// % protected region % [Customise class signature and class properties] off begin
@observer
export default class RegistrationPage extends React.Component<RouteComponentProps> {
	// % protected region % [Customise class signature and class properties] end
	@observable
	private registrationState: IRegistrationState = defaultRegistrationState;

	@observable
	private displayMode: 'select-type' | 'register' = 'register';

	private registerableEntities: {value: string, display: string}[] = [
	];
	
	// % protected region % [Add extra constructor logic here] off begin
	constructor(props: RouteComponentProps, context: any) {
		super(props, context);

		if (this.registerableEntities.length > 1) {
			this.displayMode = 'select-type';
		} else {
			this.displayMode = 'register';
			if (this.registerableEntities.length === 1) {
				this.registrationState.selectedRegisterType = this.registerableEntities[0].display;
				this.registrationState.modelToRegister = this.userEntityFactory(this.registerableEntities[0].value);
			}
		}
	}
	// % protected region % [Add extra constructor logic here] end

	public render() {
		let contents = null;

		if (store.loggedIn) {
			// % protected region % [Override redirect here] off begin
			return <Redirect to="/" />;
			// % protected region % [Override redirect here] end
		}

		// % protected region % [Override contents here] off begin
		const selectRegisterType = (
			<div className="body-content">
				<form className="register register-user-type">
					<h2>What type of user are you?</h2>
					<Combobox
						label='User Type'
						model={this.registrationState}
						options={this.registerableEntities}
						modelProperty='selectedRegisterType'
						isRequired={true}
						errors={this.registrationState.errors['selectedRegisterType']}
					/>
					<ButtonGroup alignment={Alignment.HORIZONTAL} className="select-type-buttons">
						<Button className="confirm-type" display={Display.Solid} sizes={Sizes.Medium} buttonProps={{ id: "confirm_type" }} onClick={this.onTypeConfirmed}>Confirm</Button>
						<Button className="cancel-register" display={Display.Outline} sizes={Sizes.Medium} buttonProps={{ id: "cancel_register" }} onClick={this.onCancelRegisterClicked}>Cancel</Button>
					</ButtonGroup>
				</form>
			</div>
		);

		const entityAttrs = this.getRegisterEntityAttributes();

		const registerNode = (
			<div className="body-content">
				<form className="register" onSubmit={this.onSubmitRegisterClicked}>
					<If condition={this.registerableEntities.length > 1}>
						<a className='change-user-type icon-chevron-left icon-left' onClick={this.onChangeUserType}>Change User Type</a>
					</If>
					<h2>Registration</h2>
					<h5>Registering as a {_.startCase(this.registrationState.selectedRegisterType)}</h5>
					{entityAttrs}
					<ButtonGroup alignment={Alignment.HORIZONTAL} className="register-buttons">
						<Button type='submit' className="submit-register" display={Display.Solid} sizes={Sizes.Medium} buttonProps={{ id: "submit_register" }}>Register</Button>
						<Button className="cancel-register" display={Display.Outline} sizes={Sizes.Medium} buttonProps={{ id: "cancel_register" }} onClick={this.onCancelRegisterClicked}>Cancel</Button>
					</ButtonGroup>
				</form>
			</div>
		);

		const emailSentMessageNode = (
			<div className="registration registration-success">
				<h2>Registration successful</h2>
				<p>Registration for email <span className="bold">{this.registrationState.modelToRegister ? this.registrationState.modelToRegister.email : 'the user'}</span> is successful.</p>
				<p> We already sent you a confirmation email. You have to confirm your email address before you can login</p>
				<a className="login-link" onClick={this.onLoginClick}>Login</a>
			</div>
		);

		contents = (
			<>
				{this.registrationState.emailSending && <Spinner />}
				<If condition={!this.registrationState.emailSent}>
					<If condition={this.displayMode === 'select-type'}>
						{selectRegisterType}
					</If>
					<If condition={this.displayMode === 'register'}>
						{registerNode}
					</If>
				</If>
				<If condition={this.registrationState.emailSent}>
					{emailSentMessageNode}
				</If>
			</>
		);
		// % protected region % [Override contents here] end


		return contents;
	}

	private getRegisterEntityAttributes = () => {
		// % protected region % [Override getRegisterEntityAttributes here] off begin
		if (!this.registrationState.modelToRegister) {
			return null;
		} else {
			let attributeOptions = this.registrationState.modelToRegister.getAttributeCRUDOptions();
			const model = this.registrationState.modelToRegister;
			return attributeOptions
				.filter(attributeOption => {
					return isRequired(model, attributeOption.attributeName) ||
						(UserFields as string[]).indexOf(attributeOption.attributeName) >= 0
				})
				.map(attributeOption =>
					getAttributeComponent(
						attributeOption,
						model,
						model.getErrorsForAttribute(attributeOption.attributeName),
						EntityFormMode.CREATE,
						isRequired(model, attributeOption.attributeName),
						undefined,
						model ? model.validate.bind(model) : undefined
					));
		}
		// % protected region % [Override getRegisterEntityAttributes here] end
	};

	@action
	private onSubmitRegisterClicked = async (event: React.FormEvent<HTMLFormElement>) => {
		// % protected region % [Override onSubmitRegisterClicked here] off begin
		event.preventDefault();

		this.registrationState.errors = {};
		if (this.registrationState.modelToRegister) {
			await this.registrationState.modelToRegister.validate();
			if (this.registrationState.modelToRegister.hasValidationError) {
				return;
			}
			this.submitRegister();
		}
		// % protected region % [Override onSubmitRegisterClicked here] end
	};

	@action
	private submitRegister = () => {
		// % protected region % [Override submitRegister here] off begin
		if (this.registrationState.modelToRegister) {
			const userType = this.registrationState.modelToRegister.getModelName();
			const data = this.registrationState.modelToRegister.toJSON({password: {}});

			this.registrationState.emailSending = true;

			axios.post(
				`${SERVER_URL}/api/register/${_.kebabCase(userType)}`, data)
				.then(({ data }) => {
					this.onRegistrationEmailSentSuccess();
				})
				.catch(response => {
					runInAction(() => {
						this.registrationState.emailSending = false;
					});
					const errorMessages = getErrorMessages(response).map((error: any) => (<p>{error.message}</p>));
					alert(
						<div>
							<h6>Registration is not successful</h6>
							{errorMessages}
						</div>,
						'error'
					);
				});
		}
		// % protected region % [Override submitRegister here] end
	};

	@action
	private onCancelRegisterClicked = (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
		// % protected region % [Override onCancelRegisterClicked here] off begin
		event.preventDefault();
		this.resetRegistration();
		const { redirect } = queryString.parse(this.props.location.search.substring(1));
		store.routerHistory.push(`/login?${!!redirect ? `redirect=${redirect}` : ''}`);
		// % protected region % [Override onCancelRegisterClicked here] end
	};

	@action
	private onTypeConfirmed = (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
		// % protected region % [Override onTypeConfirmed here] off begin
		event.preventDefault();

		this.registrationState.errors = {};
		if (this.registerableEntities.length > 1 && !this.registrationState.selectedRegisterType) {
			this.registrationState.errors['selectedRegisterType'] = "You need to choose the registration type";
		}
		if (Object.keys(this.registrationState.errors).length > 0) {
			return;
		} else {
			let entityToRegister = null;
			if (this.registerableEntities.length > 1 && !!this.registrationState.selectedRegisterType) {
				entityToRegister = this.registrationState.selectedRegisterType;
			} else if (this.registerableEntities.length === 1) {
				entityToRegister = this.registerableEntities[0].value;
			}
			this.displayMode = 'register';
			this.registrationState.modelToRegister = this.userEntityFactory(entityToRegister);
		}
		// % protected region % [Override onTypeConfirmed here] end
	};

	private userEntityFactory = (entityToRegister: string | null): Model & IUserEntity | null => {
		// % protected region % [Override userEntityFactory here] off begin
		switch(entityToRegister){
			default:
				return null;
		}
		// % protected region % [Override userEntityFactory here] end
	};

	@action
	private onChangeUserType = (e: React.MouseEvent<HTMLAnchorElement, MouseEvent>) => {
		// % protected region % [Override onChangeUserType here] off begin
		e.preventDefault();
		this.resetRegistration();
		this.displayMode = 'select-type';
		// % protected region % [Override onChangeUserType here] end
	};

	private resetRegistration = () => {
		// % protected region % [Override resetRegistration here] off begin
		this.registrationState = defaultRegistrationState;
		// % protected region % [Override resetRegistration here] end
	};

	@action
	private onRegistrationEmailSentSuccess = () => {
		// % protected region % [Override onRegistrationEmailSentSuccess here] off begin
		this.registrationState.emailSending = false;
		this.registrationState.emailSent = true;
		// % protected region % [Override onRegistrationEmailSentSuccess here] end
	};

	private onLoginClick = () => {
		// % protected region % [Override onLoginClick here] off begin
		const { redirect } = queryString.parse(this.props.location.search.substring(1));
		store.routerHistory.push(`/login?redirect=${redirect}`);
		// % protected region % [Override onLoginClick here] end
	};

	// % protected region % [Add class methods here] off begin
	// % protected region % [Add class methods here] end
}

// % protected region % [Add extra features here] off begin
// % protected region % [Add extra features here] end
