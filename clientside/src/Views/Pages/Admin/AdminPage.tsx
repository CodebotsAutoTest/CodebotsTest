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
import { Redirect, RouteComponentProps } from 'react-router';
import { PageWrapper } from 'Views/Components/PageWrapper/PageWrapper';
import { store } from '../../../Models/Store';
import Card from '../../Components/Card/Card';
import CardGroup from '../../Components/Card/CardGroup';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

@observer
export default class AdminPage extends React.Component<RouteComponentProps> {

	public render() {
		// % protected region % [Override contents here] off begin
		return (
			<PageWrapper {...this.props}>
				<div className={'dashboard'}>
					<div className={'dashboard-heading'}>
						<h2>Admin Dashboard</h2>
						<p>Welcome to the admin section. Get started on the home page by going to a featured lesson or
							straight to <a href={'https://codebots.app/academy'} target={'_blank'}>Codebots Academy</a></p>
					</div>
					<div className={'dashboard-cards'}>
						<CardGroup>
							<Card 
								href={'/admin/graphiql'}
								rounded={true} 
								iconOnly={true}
								icon={'icon-globe'}>
								<h4>View GraphiQL</h4>
								<p>View the GraphiQL developer tool to write your api requests.</p>
							</Card>
							<Card
								href={'/api/swagger'}
								icon={'icon-git-merge'}
								iconOnly={true}
								rounded={true}>
								<h4>View OpenAPI</h4>
								<p>View the app's swagger docs and the structure of the API.</p>
							</Card>
						</CardGroup>
						<CardGroup>
							<Card
								href={'https://codebots.app/app-settings-and-details/git'}
								icon={'icon-api'}
								iconOnly={true}
								rounded={true}>
								<h4>View Git</h4>
								<p>Access your source code by cloning from your git repository.</p>
							</Card>
							<Card 
								href={'https://codebots.app/academy'}
								className={'card--learn-more'} 
								rounded={true}>
								<h4>Want to learn more?</h4>
								<p>Codebots academy has a variety of lessons and courses that can help you build your
									app.</p>
								<div className="planet planet--primary"/>
								<div className="planet planet--secondary"/>
								<div className="planet--sub"/>
							</Card>
						</CardGroup>
					</div>
				</div>
			</PageWrapper>
		);
		// % protected region % [Override contents here] end
	}
}
