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
import { observable, action, runInAction } from 'mobx';
import { observer } from 'mobx-react';
import * as React from 'react';
import { Button, Display } from '../Button/Button';
import { Alignment, ButtonGroup } from '../Button/ButtonGroup';
import If from '../If/If';
import { DatePicker } from '../DatePicker/DatePicker';
import classNames from 'classnames';
import { ICollectionHeaderProps } from './CollectionHeaders';
import { ICollectionBulkActionProps } from './Collection';
import PaginationData from "../../../Models/PaginationData";
import CollectionFilterPanel, { ICollectionFilterPanelProps } from './CollectionFilterPanel';
import SearchForm from '../SearchForm/SearchForm';

// % protected region % [Add extra imports here] off begin
// % protected region % [Add extra imports here] end

interface ICollectionMenuProps<T> {
	selectedItems: T[];
	search?: boolean;
	filterConfig?: ICollectionFilterPanelProps<T>;
	headers?: Array<ICollectionHeaderProps<T>>;
	onSearchTriggered?: (searchTerm: string) => void;
	additionalActions?: React.ReactNode[];
	cancelAllSelection?: () => void;
	totalSelectedItems: number;
	selectedBulkActions?: Array<ICollectionBulkActionProps<T>>;
	pagination?: PaginationData;
	showSelectAll?: boolean;
	onSelectAll?: () => void;
	filterOrientationRow?: boolean;
}

interface ISearch {
	searchTerm: string;
}

@observer
class CollectionMenu<T> extends React.Component<ICollectionMenuProps<T>> {
	@observable
	private exportExpanded: boolean = false;

	@observable
	private search: ISearch = { searchTerm: "" };

	@observable
	private showFilter: boolean = false;

	// % protected region % [Add extra fields here] off begin
	// % protected region % [Add extra fields here] end

	public render() {
		// TODO: handle more buttons and button group rendering properly
		const { filterConfig, totalSelectedItems, selectedBulkActions, search } = this.props;
		const additionalActions = this.props.additionalActions || [];

		// The action buttons

		let bulkActionsBtnGroup = undefined;
		if(totalSelectedItems && selectedBulkActions && selectedBulkActions.length){
			const bulkActionButtons = selectedBulkActions.map((action, actIdx) => {
				const icon = action.showIcon && action.icon && action.iconPos 
					? { icon: action.icon, iconPos: action.iconPos } 
					: undefined;
				return <Button
					key={actIdx}
					className={action.buttonClass}
					icon={icon}
					buttonProps={{
						onClick: event => {
							action.bulkAction(this.props.selectedItems, event);
						}
					}}>
					{action.label}
				</Button>;
			});

			if (bulkActionButtons && bulkActionButtons.length) {
				bulkActionsBtnGroup =
					<ButtonGroup className="collection__selection-actions" alignment={Alignment.HORIZONTAL}>
						{bulkActionButtons}
					</ButtonGroup>;
			}
		}

		let hasFilter = !!filterConfig && !!filterConfig.filters.length;

		return (
			<>
				<section aria-label="collection menu" className="collection__menu">
					<If condition={search} >
						<SearchForm
							model={this.search}
							onSubmit={this.onSearchButtonClick}
							label="A search for entities"
							classNameSuffix="collection" />
					</If>
					<If condition={hasFilter || additionalActions.length > 0} >
						<section className="collection__actions" >
							<If condition={hasFilter} >
								<Button display={Display.Solid} icon={{ icon: "filter", iconPos: 'icon-top' }} onClick={() => { runInAction(() => {
									this.showFilter = !this.showFilter;
									}) }}>Filter</Button>
							</If>
							{this.renderAdditionalActions()}
						</section>
					</If>
				</section>
				<If condition={hasFilter && this.showFilter} >
					<section aria-label="collection filters" className={`collection__filters ${this.props.filterOrientationRow ? 'orientation_row' : ''}`}>
						<CollectionFilterPanel
							filters = {this.props.filterConfig?this.props.filterConfig.filters:[]}
							onApplyFilter={this.props.filterConfig ? this.props.filterConfig.onApplyFilter : () => { }}
							onClearFilter={this.props.filterConfig ? this.props.filterConfig.onClearFilter : () => { }}
							onFilterChanged={(this.props.filterConfig && this.props.filterConfig.onFilterChanged) ? this.props.filterConfig.onFilterChanged : () => { }} />
					</section>
				</If>
				<If condition={totalSelectedItems !== 0}>
					<section aria-label="select options" className={classNames('collection__select-options', totalSelectedItems === 0 ? 'hide' : null)}>
						{bulkActionsBtnGroup}
						<p className="crud__selection--count">
							<span className="selection-count">{totalSelectedItems}</span> items are selected
						</p>
						<If condition={this.props.showSelectAll && !!this.props.pagination}>
							<Button className="crud__selection--select-all" onClick={this.props.onSelectAll} display={Display.Text}>
								Select all {this.props.pagination ? this.props.pagination.totalRecords : null} items
							</Button>
						</If>
						<Button className="crud__selection--cancel" onClick={this.props.cancelAllSelection}>Cancel</Button>
					</section>
				</If>
			</>
		);
	}

	private renderAdditionalActions(): React.ReactNode {
		const additionalActions = this.props.additionalActions || [];
		if (additionalActions.length > 0) {
			return (
				<>
					{additionalActions}
				</>
			);
		}

		return null;
	}

	private onSearchButtonClick = () => {
		const { onSearchTriggered } = this.props;
		if (onSearchTriggered) {
			onSearchTriggered(this.search.searchTerm);
		}
	}

	// % protected region % [Add extra methods here] off begin
	// % protected region % [Add extra methods here] end
}

export default CollectionMenu;