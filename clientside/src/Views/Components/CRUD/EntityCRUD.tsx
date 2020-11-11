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
import { Route, RouteComponentProps, Switch } from 'react-router';
import { IModelType, Model } from 'Models/Model';
import EntityCollection, { AdditionalBulkActions, IEntityCollectionProps, viewActionOptions } from './EntityCollection';
import EntityAttributeList from './EntityAttributeList';
import EntityEdit from './EntityEdit';
import { getModelDisplayName } from 'Util/EntityUtils';
import SecuredAdminPage from '../Security/SecuredAdminPage';
import { SecurityService } from 'Services/SecurityService';
import { expandFn, showExpandFn, ICollectionItemActionProps } from '../Collection/Collection';
import { IEntityContextMenuActions } from '../EntityContextMenu/EntityContextMenu';
import { EntityFormMode } from '../Helpers/Common';
import { IFilter } from '../Collection/CollectionFilterPanel';
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

interface IEntityCRUDProps<T extends Model> extends RouteComponentProps {
	/** The type of model to render */
	modelType: IModelType;
	/** Function to determine the expanded content of the list */
	expandList?: expandFn<T>;
	/** Function to determine whether expand button, to display expand list, shows */
	showExpandButton?: showExpandFn<T>;
	/** Number of entities per page */
	perPage?: number;
	/** Context menu actions for each row */
	actionsMore?: IEntityContextMenuActions<T>;
	/** Url suffix to add to the route */
	URLExtension?: string;
	/** Additional actions for the bulk actions menu */
	additionalBulkActions?: Array<AdditionalBulkActions<T>>;
	/** Additional table actions for the collection view */
	additionalTableActions?: Array<ICollectionItemActionProps<T>>;
	/** Additional filters to add to the collection view */
	additionalFilters?: Array<IFilter<T>>;
	/** Remove the view action from the collection */
	removeViewAction?: boolean;
	/** Override for the collection component */
	collectionComponent?: (routeProps: RouteComponentProps) => React.ReactNode;
	/** Override for the create component */
	createComponent?: (routeProps: RouteComponentProps) => React.ReactNode;
	/** If this is set to true then remove the created date filter from the collection view */
	removeCreatedFilter?: boolean;
	/** If this is set to true then remove the modified date filter from the collection view */
	removeModifiedFilter?: boolean;
	/** Change the filter orientation to row */
	filterOrientationRow?: boolean;
	/** Override for the view component */
	viewComponent?: (routeProps: RouteComponentProps) => React.ReactNode;
	/** Override for the edit component */
	editComponent?: (routeProps: RouteComponentProps) => React.ReactNode;
	/** Custom action for view on the collection. If this function returns undefined then it will disable view */
	collectionViewAction?: (options: viewActionOptions<T>) => ICollectionItemActionProps<T> | undefined;
	/** Custom action for create on the collection. If this function returns undefined then it will disable create */
	collectionCreateAction?: (options: viewActionOptions<T>) => React.ReactNode;
	/** Custom action for delete on the collection. If this function returns undefined then it will disable delete */
	collectionDeleteAction?: (options: viewActionOptions<T>) => ICollectionItemActionProps<T> | undefined;
	/** Custom action for update on the collection. If this function returns undefined then it will disable update */
	collectionUpdateAction?: (options: viewActionOptions<T>) => ICollectionItemActionProps<T> | undefined;
	/** Custom props to be passed to the entity collection for when it is being rendered. */
	entityCollectionProps?: Partial<IEntityCollectionProps<T>>;
	/** Override for disabling bulk export on the bulk select all */
	disableBulkExport?: boolean;
	/** Override for disabling bulk delete on the bulk select all */
	disableBulkDelete?: boolean;
	// % protected region % [Add any extra props here] off begin
	// % protected region % [Add any extra props here] end
}

/**
 * This component is used to render a CRUD (create, read, update, delete) view for a specific entity type.
 */
@observer
class EntityCRUD<T extends Model> extends React.Component<IEntityCRUDProps<T>> {
	private url = () => {
		// % protected region % [Override url function here] off begin
		const { URLExtension, match } = this.props;

		if (URLExtension) {
			return `${match.url}/${URLExtension}`;
		}

		return match.url;
		// % protected region % [Override url function here] end
	};

	public render() {
		const { match, modelType, collectionComponent, createComponent, editComponent, viewComponent } = this.props;

		// Wrap the pages with secured page component
		// % protected region % [Override read list view here] off begin
		const entityCollectionPage = (pageProps: RouteComponentProps) => {
			return (
				<SecuredAdminPage canDo={SecurityService.canRead(modelType)}>
					<this.renderEntityCollection {...pageProps} />
				</SecuredAdminPage>
			);
		};
		// % protected region % [Override read list view here] end

		// % protected region % [Override create view here] off begin
		const entityCreatePage = (pageProps: RouteComponentProps) => {
			return (
				<SecuredAdminPage canDo={SecurityService.canCreate(modelType)}>
					<this.renderEntityCreate {...pageProps} />
				</SecuredAdminPage>
			);
		};
		// % protected region % [Override create view here] end

		// % protected region % [Override read view here] off begin
		const entityViewPage = (pageProps: RouteComponentProps) => {
			return (
				<SecuredAdminPage canDo={SecurityService.canRead(modelType)}>
					<this.renderEntityView {...pageProps} />
				</SecuredAdminPage>
			);
		};
		// % protected region % [Override read view here] end

		// % protected region % [Override edit view here] off begin
		const entityEditPage = (pageProps: RouteComponentProps) => {
			return (
				<SecuredAdminPage canDo={SecurityService.canUpdate(modelType)}>
					<this.renderEntityEdit {...pageProps} />
				</SecuredAdminPage>
			);
		};
		// % protected region % [Override edit view here] end

		// % protected region % [Override return value here] off begin
		return (
			<div>
				<Switch>
					<Route exact={true} path={`${match.url}`} render={collectionComponent ?? entityCollectionPage} />
					<Route path={`${this.url()}/view/:id`} render={viewComponent ?? entityViewPage} />
					<Route exact={true} path={`${this.url()}/create`} render={createComponent ?? entityCreatePage} />
					<Route path={`${this.url()}/edit/:id`} render={editComponent ?? entityEditPage} />
				</Switch>
			</div>
		);
		// % protected region % [Override return value here] end
	};
	
	// % protected region % [Customize renderEntityCollection here] off begin
	protected renderEntityCollection = (routeProps: RouteComponentProps) => {
		const {
			modelType,
			expandList,
			showExpandButton,
			perPage,
			actionsMore,
			additionalBulkActions,
			additionalFilters,
			additionalTableActions,
			collectionViewAction,
			collectionCreateAction,
			collectionUpdateAction,
			collectionDeleteAction,
			entityCollectionProps,
			filterOrientationRow,
			disableBulkDelete,
			disableBulkExport,
			removeCreatedFilter,
			removeModifiedFilter
		} = this.props;

		return (
			<EntityCollection
				{...routeProps}
				modelType={modelType}
				expandList={expandList}
				showExpandButton={showExpandButton}
				perPage={perPage}
				actionsMore={actionsMore}
				url={this.url()}
				disableBulkDelete={disableBulkDelete}
				disableBulkExport={disableBulkExport}
				additionalBulkActions={additionalBulkActions}
				additionalTableActions={additionalTableActions}
				additionalFilters={additionalFilters}
				filterOrientationRow={filterOrientationRow}
				viewAction={collectionViewAction}
				createAction={collectionCreateAction}
				deleteAction={collectionDeleteAction}
				updateAction={collectionUpdateAction}
				removeCreatedFilter={removeCreatedFilter}
				removeModifiedFilter={removeModifiedFilter}
				{...entityCollectionProps}
			/>
		);
	};
	// % protected region % [Customize renderEntityCollection here] end

	protected renderEntityCreate = (routeProps: RouteComponentProps) => {
		// % protected region % [Override create component render here] off begin
		const { modelType } = this.props;
		const modelDisplayName = getModelDisplayName(modelType);
		return (
			<EntityAttributeList
				{...routeProps}
				model={new modelType()}
				sectionClassName="crud__create"
				title={`Create New ${modelDisplayName}`}
				formMode={EntityFormMode.CREATE}
				modelType={modelType}
			/>
		);
		// % protected region % [Override create component render here] end
	};

	protected renderEntityEdit = (routeProps: RouteComponentProps) => {
		// % protected region % [Override edit component render here] off begin
		const { modelType } = this.props;
		return <EntityEdit {...routeProps} modelType={modelType} formMode={EntityFormMode.EDIT} />;
		// % protected region % [Override edit component render here] end
	};

	protected renderEntityView = (routeProps: RouteComponentProps) => {
		// % protected region % [Override read component render here] off begin
		const { modelType } = this.props;
		return <EntityEdit {...routeProps} modelType={modelType} formMode={EntityFormMode.VIEW} />;
		// % protected region % [Override read component render here] end
	};

	// % protected region % [Add any extra fields here] off begin
	// % protected region % [Add any extra fields here] end
}

// % protected region % [Override default export here] off begin
export default EntityCRUD;
// % protected region % [Override default export here] end
