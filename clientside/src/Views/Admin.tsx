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
import { action } from 'mobx';
import * as AdminPages from './Pages/Admin/Entity';
import Auth from "./Components/Auth/Auth";
import AllUsersPage from './Pages/Admin/AllUsersPage';
import AdminPage from './Pages/Admin/AdminPage';
import Topbar from "./Components/Topbar/Topbar";
import PageLinks from './Pages/Admin/PageLinks';
import Spinner from 'Views/Components/Spinner/Spinner';
import { Redirect, Route, RouteComponentProps, Switch } from 'react-router';
import { store } from "Models/Store";
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

// % protected region % [Customize lazy imports here] off begin
const GraphiQlLazy = React.lazy(() => import("./Pages/Admin/Graphiql"));
// % protected region % [Customize lazy imports here] end

export default class Admin extends React.Component<RouteComponentProps> {
	@action
	private setAppLocation = () => {
		store.appLocation = 'admin';
	}

	public componentDidMount() {
		this.setAppLocation();
	}

	public render() {
		return (
			<>
				<div className="body-container">
					{
					// % protected region % [Modify Topbar] off begin
					}
					<Topbar currentLocation="admin" />
					{
					// % protected region % [Modify Topbar] end
					}
					<div className="admin">
						<Auth {...this.props}>
							<this.adminSwitch />
						</Auth>
					</div>
				</div>
			</>
		);
	}

	private adminSwitch = () => {
		if (!store.userGroups.some(ug => ug.hasBackendAccess)) {
			return <Redirect to="/404" />;
		}

		const path = this.props.match.path === '/' ? '' : this.props.match.path;

		return (
			<>
				{
				// % protected region % [Override contents here] off begin
				}
				<PageLinks {...this.props} />
				{
				// % protected region % [Override contents here] end
				}
				<div className="body-content">
					<Switch>
						{/* These routes require a login to view */}

						{/* Admin entity pages */}
						<Route exact={true} path={`${path}`} component={AdminPage} />
						<Route path={`${path}/User`} component={AllUsersPage} />
						<Route path={`${path}/AEntity`} component={AdminPages.AEntityPage} />
						<Route path={`${path}/BEntity`} component={AdminPages.BEntityPage} />

						{
						// % protected region % [Add any extra page routes here] off begin
						}
						{
						// % protected region % [Add any extra page routes here] end
						}
					</Switch>
				</div>
				{
				// % protected region % [Add any admin footer content here] off begin
				}
				{
				// % protected region % [Add any admin footer content here] end
				}

				{
				// % protected region % [Override graphiql here] off begin
				}
				<Switch>
					<Route path={`${path}/graphiql`}>
						<React.Suspense fallback={<Spinner />}>
							<GraphiQlLazy />
						</React.Suspense>
					</Route>
				</Switch>
				{
				// % protected region % [Override graphiql here] end
				}
			</>
		);
	}
}