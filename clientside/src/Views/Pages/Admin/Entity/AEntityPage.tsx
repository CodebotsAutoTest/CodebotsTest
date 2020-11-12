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
import * as Models from 'Models/Entities';
import EntityCRUD from 'Views/Components/CRUD/EntityCRUD';
import { PageWrapper } from 'Views/Components/PageWrapper/PageWrapper';
import PageLinks from '../PageLinks';
import SecuredPage from 'Views/Components/Security/SecuredPage';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

@observer
export default class AEntityPage extends React.Component<RouteComponentProps> {
	// % protected region % [Add any extra attributes here] off begin
	// % protected region % [Add any extra attributes here] end

	public render() {
		let contents = null;

		// % protected region % [Override contents here] off begin
		contents = (
			<PageWrapper {...this.props}>
				<EntityCRUD
					modelType={Models.AEntity}
					{...this.props}
				/>
			</PageWrapper>
		);
		// % protected region % [Override contents here] end

		return (
			<SecuredPage groups={["Super Administrators", ""]}>
				{contents}
			</SecuredPage>
		);
	}
	
	// % protected region % [Add any extra functions here] off begin
	// % protected region % [Add any extra functions here] end
}
