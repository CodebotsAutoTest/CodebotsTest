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
import { AttributeCRUDOptions } from 'Models/CRUDOptions';
import { observer } from 'mobx-react';
import If from '../If/If';
import { action, observable } from 'mobx';

// % protected region % [Add extra imports here] off begin
// % protected region % [Add extra imports here] end

interface IFormErrorsProps {
	error: React.ReactNode;
	detailedErrors?: React.ReactNode;
}

@observer
class FormErrors extends React.Component<IFormErrorsProps> {
	@observable
	public showDetailedErrors: boolean;
	
	public render() {
		const { error } = this.props;
		
		// % protected region % [Add form error logic here] off begin
		// % protected region % [Add form error logic here] end

		// % protected region % [Customize form error dom here] off begin
		return (
			<div className="form-errors">
				{error}
				{this.props.detailedErrors
					? (
						<If condition={!!this.props.detailedErrors}>
							<div>
								<a onClick={() => this.displayErrors()}>{this.showDetailedErrors ? 'Hide' : 'Show'} Detailed Errors</a>
								<If condition={this.showDetailedErrors}>
									<div>{this.props.detailedErrors}</div>
								</If>
							</div>
						</If>
					)
					: null}
			</div>
		// % protected region % [Customize form error dom here] end
		);
	}
	
	@action
	public displayErrors() {
		this.showDetailedErrors = !this.showDetailedErrors;
	}
}

export default FormErrors;