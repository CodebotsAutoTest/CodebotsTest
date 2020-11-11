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
import Collection, {
	ICollectionItemActionProps,
	expandFn,
	showExpandFn,
	ICollectionBulkActionProps,
	ICollectionActionProps,
	ICollectionProps,
} from '../Collection/Collection';
import { Button, Display } from '../Button/Button';
import { observer } from 'mobx-react';
import { RouteComponentProps } from 'react-router';
import { Model, IModelType } from 'Models/Model';
import { getModelName, getModelDisplayName, getAttributeCRUDOptions, exportAll } from 'Util/EntityUtils';
import {observable, action, runInAction, computed} from 'mobx';
import Spinner from '../Spinner/Spinner';
import { ICollectionHeaderProps } from '../Collection/CollectionHeaders';
import ModelQuery, { IWhereCondition, IOrderByCondition, IWhereConditionApi } from '../ModelCollection/ModelQuery';
import { IFilter } from '../Collection/CollectionFilterPanel';
import { PaginationQueryOptions } from 'Models/PaginationData';
import { QueryResult } from 'react-apollo';
import { lowerCaseFirst } from 'Util/StringUtils';
import { SecurityService } from 'Services/SecurityService';
import { OperationVariables } from 'apollo-boost';
import * as _ from 'lodash';
import classNames from 'classnames';
import { confirmModal } from '../Modal/ModalUtils';
import alert from 'Util/ToastifyUtils';
import { IEntityContextMenuActions } from '../EntityContextMenu/EntityContextMenu';
import { convertCaseComparisonToPascalCase } from 'Util/GraphQLUtils';
import { ICollectionFilterPanelProps } from '../Collection/CollectionFilterPanel';
import moment from 'moment';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

export type refetchFn = (variables?: any) => Promise<any>;
export type actionOverrideFn<T extends Model> = (refetchFn: viewActionOptions<T>) => ICollectionItemActionProps<T> | undefined;
export type viewActionOptions<T extends Model> = {
	refetch: refetchFn,
	self: EntityCollection<T>,
}

export interface AdditionalBulkActions<T> extends ICollectionActionProps<T> {
	bulkAction: (mode: 'includedIds' | 'excludedIds', models: T[], event: React.MouseEvent<Element, MouseEvent>) => void;
	// % protected region % [Add any extra AdditionalBulkActions fields here] off begin
	// % protected region % [Add any extra AdditionalBulkActions fields here] end
}

export interface IEntityCollectionProps<T extends Model> extends RouteComponentProps {
	modelType: IModelType;
	expandList?: expandFn<T>;
	showExpandButton?: showExpandFn<T>;
	additionalBulkActions?: AdditionalBulkActions<T>[];
	additionalFilters?: Array<IFilter<T>>;
	perPage?: number;
	orderBy?: IOrderByCondition<T>;
	actionsMore?: IEntityContextMenuActions<T>;
	url?: string;
	additionalTableActions?: Array<ICollectionItemActionProps<T>>;
	conditions?: Array<Array<IWhereCondition<T>>>;
	removeCreatedFilter?: boolean;
	removeModifiedFilter?: boolean;
	filterOrientationRow?: boolean;
	createAction?: (refetchFn: viewActionOptions<T>) => React.ReactNode;
	viewAction?: actionOverrideFn<T>;
	deleteAction?: actionOverrideFn<T>;
	updateAction?: actionOverrideFn<T>;
	disableBulkExport?: boolean;
	disableBulkDelete?: boolean;
	collectionProps?: ICollectionProps<T>;
	disableCreateButtonSecurity?: boolean;
	disableReadButtonSecurity?: boolean;
	disableUpdateButtonSecurity?: boolean;
	disableDeleteButtonSecurity?: boolean;
	// % protected region % [Add any extra props here] off begin
	// % protected region % [Add any extra props here] end
}

export interface ISearch {
	searchTerm: string;
	// % protected region % [Add any extra ISearch fields here] off begin
	// % protected region % [Add any extra ISearch fields here] end
}


@observer
class EntityCollection<T extends Model> extends React.Component<IEntityCollectionProps<T>, any> {

	@observable
	public search: ISearch = { searchTerm: '' };

	@observable
	public filterConfig: ICollectionFilterPanelProps<T>;

	@observable
	public filterApplied: boolean = false;

	@observable
	public orderBy: IOrderByCondition<T> | undefined;

	// % protected region % [Customize _orderBy method here] off begin
	@computed
	public get _orderBy() {
		if (this.orderBy === undefined) {
			// set the default order by to display the options in reverse creation order
			return { path: 'Created', descending: true };
		}
		return this.orderBy;
	}
	// % protected region % [Customize _orderBy method here] end

	@observable
	public paginationQueryOptions: PaginationQueryOptions = new PaginationQueryOptions();

	@observable
	public allSelectedItemIds: string[] = new Array<string>();

	@observable
	public allExcludedItemIds: string[] = new Array<string>();

	@observable
	public allPagesSelected: boolean = false;

	@computed
	public get security() {
		const {
			modelType,
			disableCreateButtonSecurity,
			disableReadButtonSecurity,
			disableUpdateButtonSecurity,
			disableDeleteButtonSecurity
		} = this.props;
		return {
			create: disableCreateButtonSecurity ? true : SecurityService.canCreate(modelType),
			read: disableReadButtonSecurity ? true : SecurityService.canRead(modelType),
			update: disableUpdateButtonSecurity ? true : SecurityService.canUpdate(modelType),
			delete: disableDeleteButtonSecurity ? true : SecurityService.canDelete(modelType),
		}
	}

	// % protected region % [Customize collectionFilters method here] off begin
	@computed
	public get collectionFilters() {
		let conditions = this.getSearchConditions();
		let filterConditions: IWhereCondition<Model>[][] | undefined;

		if (this.filterApplied) {
			filterConditions = new this.props.modelType().getFilterConditions(this.filterConfig);
		}

		if (!!this.props.conditions && !!this.props.conditions.length) {
			conditions = [...conditions, ...this.props.conditions]
			.map(andCondition => andCondition.map(orCondition => ({
				...orCondition,
				value: Array.isArray(orCondition.value) ? orCondition.value : [orCondition.value],
			})));
		}

		if (filterConditions && !!filterConditions.length) {
			conditions = [...conditions, ...filterConditions]
				.map(andCondition => andCondition.map(orCondition => ({
					...orCondition,
					value: _.isArray(orCondition.value) ? orCondition.value : [orCondition.value],
				})));
		}

		return conditions;
	}
	// % protected region % [Customize collectionFilters method here] end

	// % protected region % [Customize url method here] off begin
	public get url() {
		const { url, match } = this.props;
		return url ?? match.url;
	}
	// % protected region % [Customize url method here] end

	private models: T[] = [];

	// % protected region % [Customize constructor method here] off begin
	constructor(props: IEntityCollectionProps<T>, context: any) {
		super(props, context);
		const { modelType } = this.props;

		this.filterConfig = {
			filters: this.getFilters(),
			onClearFilter: this.onClearFilter,
			onApplyFilter: this.onApplyFilter,
			onFilterChanged: this.onFilterChanged,
		};
		const defaultOrderBy = modelType.getOrderByField ? modelType.getOrderByField() : undefined;
		this.orderBy = defaultOrderBy ? defaultOrderBy : props.orderBy;
	}
	// % protected region % [Customize constructor method here] end

	// % protected region % [Customize componentDidUpdate method here] off begin
	componentDidUpdate() {
		runInAction(() => {
			this.paginationQueryOptions.page = 0;
		});
	}
	// % protected region % [Customize componentDidUpdate method here] end

	// % protected region % [Customize render method here] off begin
	public render() {
		const { perPage, modelType } = this.props;
		runInAction(() => this.paginationQueryOptions.perPage = perPage || 10);
		return (
			<>
				<ModelQuery
					model={modelType}
					pagination={this.paginationQueryOptions}
					orderBy={this._orderBy}
					conditions={this.collectionFilters}
					useListExpands
				>
					{this.renderCollection}
				</ModelQuery>
			</>
		);
	}
	// % protected region % [Customize render method here] end

	// % protected region % [Customize renderCollection method here] off begin
	protected renderCollection = (result: QueryResult<any, OperationVariables>) : JSX.Element => {

		const { loading, error, data, refetch } = result;
		if (error) {
			return (
				<div>
					<h2>An unexpected error occurred:</h2>
					{JSON.stringify(error.message)}
				</div>
			);
		}

		const { modelType } = this.props;
		const modelName = getModelName(modelType);

		const tableHeaders = this.getHeaders();
		const tableActions = this.getTableActions(refetch);

		this.models = [];
		const dataModelName = lowerCaseFirst(modelName + 's');
		if (data[dataModelName]) {
			this.models = data[dataModelName].map((e: any) => new modelType(e));
		}

		const countName = `count${modelName}s`;
		let totalRecords = 0;
		if (data[countName]) {
			totalRecords = data[countName]['number'];
		}

		let additionalActions: React.ReactNode[] = [];
		if(this.security.create) {
			additionalActions.push(this.renderCreateButton(refetch));
		}

		let menuCountFunction = () => {
			if(this.allPagesSelected){
				return totalRecords - this.allExcludedItemIds.length;
			}else{
				return this.allSelectedItemIds.length;
			}
		};

		const selectedBulkActions :Array<ICollectionBulkActionProps<T>> = [];
		if (SecurityService.canRead(this.props.modelType) && this.props.disableBulkExport !== true){
			selectedBulkActions.push({
				bulkAction: this.exportItems,
				label: "Export",
				showIcon: true,
				icon: "export",
				iconPos: 'icon-left',
			});
		}
		if (SecurityService.canDelete(this.props.modelType) && this.props.disableBulkDelete !== true) {
			selectedBulkActions.push({
				bulkAction: () => {
					confirmModal('Please confirm', "Are you sure you want to delete all the selected items?").then(()=>{
						let idsToDelete: Array<string> | undefined;
						let conditions: Array<Array<IWhereCondition<Model>>> | undefined;
						if (this.allPagesSelected) {
							conditions = this.getSearchConditions() as Array<Array<IWhereCondition<Model>>>;
							if(!conditions){
								conditions = [];
							}
							let idsCondition = new Array<IWhereCondition<Model>>();
							idsCondition.push({path:'id', comparison:'notIn', value: this.allExcludedItemIds });
							(conditions as Array<Array<IWhereCondition<Model>>>).push(idsCondition);
							idsToDelete = undefined;
						} else {
							idsToDelete = this.allSelectedItemIds;
							conditions = this.getSearchConditions() as Array<Array<IWhereCondition<Model>>>;
						}
						new this.props.modelType().deleteWhere(conditions, idsToDelete).then((result) => {
							if(!!result && result['value'] === true){
								refetch();
								this.cancelAllSelection();
								alert('All selected items are deleted successfully', 'success');
							}
						}).catch((errorMessage) => {
							alert(
								<div className="delete-error">
									<p className="user-error">These records could not be deleted because of an association</p>
									<p className="internal-error-title">Message:</p>
									<p className="internal-error">{errorMessage}</p>
								</div>,
								'error'
							);
						});
					});
				},
				label: "Delete",
				showIcon: true,
				icon: "bin-full",
				iconPos: 'icon-left',
			});
		}

		return(
			<>
				{loading && <Spinner/>}
				<Collection
					selectableItems={true}
					additionalActions={additionalActions}
					headers={tableHeaders}
					actions={tableActions}
					actionsMore={this.props.actionsMore}
					selectedBulkActions={selectedBulkActions.concat(this.props.additionalBulkActions
						? this.props.additionalBulkActions.map(ba => this.mapBulkAction(ba))
						: [])}
					onSearchTriggered={this.onSearchTriggered}
					menuFilterConfig={this.filterConfig}
					collection={this.models}
					pagination={{totalRecords, queryOptions: this.paginationQueryOptions }}
					itemSelectionChanged={this.itemSelectionChanged}
					cancelAllSelection={this.cancelAllSelection}
					menuCountFunction={menuCountFunction}
					expandList={this.props.expandList}
					showExpandButton={this.props.showExpandButton}
					getSelectedItems={this.getSelectedItems}
					onCheckedAllPages={this.onCheckedAllPages}
					idColumn="id"
					dataFields={row => ({
						created: moment(row.created).format('YYYY-MM-DD'),
						modified: moment(row.modified).format('YYYY-MM-DD'),
					})}
					orderBy={this.orderBy}
					filterOrientationRow={this.props.filterOrientationRow}
					{...this.props.collectionProps}
				/>
			</>
		);
	}
	// % protected region % [Customize renderCollection method here] end

	// % protected region % [Customize getSelectedItems method here] off begin
	public getSelectedItems = () => {
		return this.models.filter(model => {
			if (this.allPagesSelected) {
				return !this.allExcludedItemIds.some(id => {
					return model.id === id;
				});
			} else {
				return this.allSelectedItemIds.some(id => {
					return model.id === id;
				});
			}
		});
	};
	// % protected region % [Customize getSelectedItems method here] end

	// % protected region % [Customize onCheckedAllPages method here] off begin
	@action
	public onCheckedAllPages = (checked: boolean) => {
		this.allPagesSelected = checked;
		if(checked){
			this.allExcludedItemIds = [];
			let changedIds = this.models.map(item => item.id);
			if (checked) {
				this.allSelectedItemIds = _.union(this.allSelectedItemIds, changedIds);
			} else {
				this.allSelectedItemIds = _.pull(this.allSelectedItemIds, ...changedIds);
			}
			let selectedItems = (new Array<T>());
			selectedItems.push(...this.models);
			return selectedItems;
		}else{
			this.allSelectedItemIds = [];
			let changedIds = this.models.map(item => item.id);
			if (!checked) {
				this.allExcludedItemIds = _.union(this.allSelectedItemIds, changedIds);
			} else {
				this.allExcludedItemIds = _.pull(this.allSelectedItemIds, ...changedIds);
			}
			return [];
		}
	}
	// % protected region % [Customize onCheckedAllPages method here] end

	// % protected region % [Customize itemSelectionChanged method here] off begin
	@action
	public itemSelectionChanged = (checked: boolean, changedItems: Model[]) => {
		let changedIds = changedItems.map(item => item.id);
		if (this.allPagesSelected) {
			if (!checked) {
				this.allExcludedItemIds = _.union(this.allExcludedItemIds, changedIds);
			} else {
				this.allExcludedItemIds = _.pull(this.allExcludedItemIds, ...changedIds);
			}
		} else {
			if (checked) {
				this.allSelectedItemIds = _.union(this.allSelectedItemIds, changedIds);
			} else {
				this.allSelectedItemIds = _.pull(this.allSelectedItemIds, ...changedIds);
			}
		}
		return this.models.filter((m: Model) => {
			if (this.allPagesSelected) {
				return !this.allExcludedItemIds.some(id => {
					return m.id === id;
				}) ;
			} else {
				return this.allSelectedItemIds.some(id => {
					return m.id === id;
				}) ;
			}
		});
	}
	// % protected region % [Customize itemSelectionChanged method here] end

	// % protected region % [Customize renderCreateButton method here] off begin
	public renderCreateButton(refetch: refetchFn): React.ReactNode {
		const { modelType, createAction } = this.props;
		if (createAction) {
			return createAction({refetch: refetch, self: this});
		} else {
			const modelDisplayName = getModelDisplayName(modelType);
			return (
				<Button
					key="create"
					className={classNames(Display.Solid)}
					icon={{icon: 'create', iconPos: 'icon-right'}}
					buttonProps={{ onClick: () => { this.props.history.push(`${this.url}/create`); } }}>
					Create {modelDisplayName}
				</Button>
			);
		}
	}
	// % protected region % [Customize renderCreateButton method here] end

	// % protected region % [Customize GetHeaders method here] off begin
	public getHeaders = (): Array<ICollectionHeaderProps<T>> => {
		const attributeOptions = getAttributeCRUDOptions(this.props.modelType);
		return attributeOptions.filter(attributeOption => attributeOption.headerColumn)
			.map(attributeOption => {
					const headers: ICollectionHeaderProps<T> = {
						name: attributeOption.attributeName,
						displayName: attributeOption.displayName,
						sortable: true,
						sortClicked: () => {
							if (this.orderBy && this.orderBy.path === attributeOption.attributeName) {
								if (this.orderBy.descending) {
									const descending = !this.orderBy.descending;
									runInAction(() => this.orderBy = { path: attributeOption.attributeName, descending });
								} else if (!this.orderBy.descending) {
									runInAction(() => this.orderBy = undefined);
								}
								return this.orderBy;
							} else {
								runInAction(() => this.orderBy = { path: attributeOption.attributeName, descending: true });
								return this.orderBy;
							}
						},
					};
					if (attributeOption.displayFunction) {
						headers.transformItem = (item: any) => {
							if (attributeOption.displayFunction) {
								return attributeOption.displayFunction(item[attributeOption.attributeName], item);
							}
							return item[attributeOption.name];
						};
					}
					return headers;
				}
			);
	}
	// % protected region % [Customize GetHeaders method here] end

	// % protected region % [Customize getFilters method here] off begin
	public getFilters = (): Array<IFilter<T>> => {
		const { additionalFilters, removeCreatedFilter, removeModifiedFilter } = this.props;
		let filters = new Array<IFilter<T>>();

		if (!removeCreatedFilter) {
			filters.push({
				path: "created",
				comparison: "range",
				value1: undefined,
				value2: undefined,
				active: false,
				displayType: "datepicker",
				displayName: "Range of Date Created"
			} as IFilter<T>);
		}

		if (!removeModifiedFilter) {
			filters.push({
				path: "modified",
				comparison: "range",
				value1: undefined,
				value2: undefined,
				displayType: "datepicker",
				displayName: "Range of Date Last Modified",
			} as IFilter<T>);
		}

		filters = [...filters ,..._.cloneDeep(additionalFilters) || []];

		const enumFilters = this.getEnumFilters();

		filters = [...filters, ...enumFilters];

		return filters;
	};
	// % protected region % [Customize getFilters method here] end

	// % protected region % [Customise retrieving enum filters logic] off begin
	public getEnumFilters = () => {
		const attributeOptions = getAttributeCRUDOptions(this.props.modelType);
		return attributeOptions
			.filter(attributeOption => attributeOption.displayType === 'enum-combobox')
			.map(attributeOption => {
				return {
					path: attributeOption.attributeName,
					comparison: 'equal',
					value1: [] as string[],
					displayName: attributeOption.displayName,
					displayType: 'enum-combobox',
					enumResolveFunction: attributeOption.enumResolveFunction
				} as IFilter<T>;
			});
	}
	// % protected region % [Customise retrieving enum filters logic] end

	// % protected region % [Customize onClearFilter method here] off begin
	@action
	public onClearFilter = () => {
		this.filterConfig.filters = this.getFilters();
		this.filterApplied = false;
	};
	// % protected region % [Customize onClearFilter method here] end

	// % protected region % [Customize onApplyFilter method here] off begin
	@action
	public onApplyFilter = () => {
		this.filterApplied = true;
	};
	// % protected region % [Customize onApplyFilter method here] end

	// % protected region % [Customize onFilterChanged method here] off begin
	@action
	public onFilterChanged = () => {
		this.filterApplied = false;
	}
	// % protected region % [Customize onFilterChanged method here] end

	// % protected region % [Customize mapBulkAction method here] off begin
	public mapBulkAction = (bulkAction: AdditionalBulkActions<T>): ICollectionBulkActionProps<T> => {
		const rootFn = bulkAction.bulkAction;

		return {
			...bulkAction,
			bulkAction: (models, event) => {
				const mode = this.allPagesSelected ? 'includedIds' : 'excludedIds';
				return rootFn(mode, models, event);
			}
		};
	};
	// % protected region % [Customize mapBulkAction method here] end

	// % protected region % [Customize exportItems method here] off begin
	public exportItems = () => {
		let conditions: IWhereConditionApi<Model>[][];
		if (this.allPagesSelected && this.collectionFilters) {
			conditions = [
				...this.collectionFilters.map(andCondition => andCondition.map(orCondition => {
					if (orCondition.case) {
						return {
							...orCondition,
							case: convertCaseComparisonToPascalCase(orCondition.case),
							value: [orCondition.value],
						}
					}
					return orCondition as IWhereConditionApi<Model>;
				})),
				[{
					path: "id",
					comparison: "notIn",
					value: this.allExcludedItemIds,
				}]
			];
		} else {
			conditions = [[
				{
					path: "id",
					comparison: "in",
					value: this.allSelectedItemIds
				}
			]];
		}
		return exportAll(this.props.modelType, conditions);
	}
	// % protected region % [Customize exportItems method here] end

	public getTableActions = (refetch: refetchFn) => {
		const tableActions: Array<ICollectionItemActionProps<T>> = [];
		// % protected region % [Customise custom table actions here] off begin
		const { viewAction, updateAction, deleteAction, additionalTableActions } = this.props;

		const updateTableActions = (override: actionOverrideFn<T> | undefined, defaultAction: ICollectionItemActionProps<T>) => {
			if (override) {
				const action = override({refetch: refetch, self: this});
				if (action) {
					tableActions.push(action);
				}
			} else {
				tableActions.push(defaultAction);
			}
		};

		if (this.security.read) {
			updateTableActions(viewAction, {
				action: (item) => {
					this.props.history.push({ pathname: `${this.url}/view/${item['id']}` });
				},
				label: "View",
				showIcon: true,
				icon: "look",
				iconPos: 'icon-top',
			});
		}
		if (this.security.update) {
			updateTableActions(updateAction, {
				action: (item) => {
					this.props.history.push({ pathname: `${this.url}/edit/${item['id']}` });
				},
				label: "Edit",
				showIcon: true,
				icon: "edit",
				iconPos: 'icon-top',
			});
		}
		if (this.security.delete) {
			updateTableActions(deleteAction, {
				action: (item) => {
					confirmModal('Please confirm', "Are you sure you want to delete this item?").then(() => {
						new this.props.modelType(item).delete().then(() => {
							refetch();
							alert('Deleted successfully', 'success');
						}).catch((errorMessage) => {
							alert(
								<div className="delete-error">
									<p className="user-error">This record could not be deleted because of an association</p>
									<p className="internal-error-title">Message:</p>
									<p className="internal-error">{errorMessage}</p>
								</div>
								, 'error'
							);
						});
					});
				},
				label: "Delete",
				showIcon: true,
				icon: "bin-full",
				iconPos: 'icon-top',
			});
		}
		if (additionalTableActions) {
			tableActions.push(...additionalTableActions);
		}
		// % protected region % [Customise custom table actions here] end
		return tableActions;
	}

	// % protected region % [Customize onSearchTriggered method here] off begin
	@action
	public onSearchTriggered = (searchTerm: string) => {
		this.search.searchTerm = searchTerm;
	}
	// % protected region % [Customize onSearchTriggered method here] end

	// % protected region % [Customize cancelAllSelection method here] off begin
	@action
	public cancelAllSelection = () => {
		this.allPagesSelected = false;
		this.allSelectedItemIds = [];
		this.allExcludedItemIds = [];
	}
	// % protected region % [Customize cancelAllSelection method here] end

	// % protected region % [Customize getSearchConditions method here] off begin
	public getSearchConditions() {
		return new this.props.modelType().getSearchConditions(this.search.searchTerm);
	}
	// % protected region % [Customize getSearchConditions method here] end

	// % protected region % [Add any extra EntityCollection fields here] off begin
	// % protected region % [Add any extra EntityCollection fields here] end
}

export default EntityCollection;