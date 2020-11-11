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
import { observer } from 'mobx-react';
import * as React from 'react';
import { Comparators } from '../ModelCollection/ModelQuery';
import { displayType } from 'Models/CRUDOptions';
import { Button, Display } from '../Button/Button';
import FilterDateRange from './CollectionFilterAttributes/FilterDateRangePicker';
import FilterEnumComboBox from './CollectionFilterAttributes/FilterEnumComboBox';
import _ from 'lodash';
import { ButtonGroup, Alignment } from '../Button/ButtonGroup';

// % protected region % [Add extra imports and exports here] off begin
// % protected region % [Add extra imports and exports here] end

export interface IFilter<T> {
	/** The column name to filter on */
	path: string;
	/** The comparison operator */
	comparison: Comparators | 'range';
	/** The value to filter on */
	value1: string | string[] | Date | number | undefined;
	/** 
	 * The second value to filter
	 * Only valid for 'range' type comparison for now where this represents the end of the range 
	 */
	value2: string | Date | number | undefined;
	/** this is specifically for the model of date range */
	active: boolean;
	/** The display type of the filter */
	displayType: displayType;
	/** The display name of the filter */
	displayName: string;
	/** The function to resolve and return the options of the enum-combobox (for now only enum-combobox) */
	enumResolveFunction?: Array<{display: string, value: string}>;
	// % protected region % [Add extra IFilter props here] off begin
	// % protected region % [Add extra IFilter props here] end
}

export interface ICollectionFilterPanelProps<T> {
	filters: IFilter<T>[];
	onClearFilter: () => void;
	onApplyFilter: () => void;
	onFilterChanged?: () => void;
}

@observer
class CollectionFilterPanel<T> extends React.Component<ICollectionFilterPanelProps<T>> {

	public render() {
		const { 
			filters, 
			onFilterChanged, 
			onApplyFilter, 
			onClearFilter,
		} = this.props;

		if (filters === undefined || !filters.length) {
			return null;
		}
		
		return (
			<>
				<div className="collection-filter-form__container">
					{
						filters.map(filter => {
							switch (filter.displayType) {
								case 'datepicker':
									if (filter.comparison === 'range') {
										return (
											<FilterDateRange
												filter={filter}
												className={'filter-' + filter.path}
												key={'filter-' + filter.path}
												onAfterChange={() => {
													if (onFilterChanged) {
														onFilterChanged();
													}
												}}
											/>
										);
									}
									return '';
								case 'enum-combobox':
									return (
										<FilterEnumComboBox
											filter={filter}
											className={'filter-' + filter.path}
											key={'filter-' + filter.path}
											onAfterChange={() => {
												if (onFilterChanged) {
													onFilterChanged();
												}
											}}
										/>
									);

								// % protected region % [Add extra filter cases here] off begin
								// % protected region % [Add extra filter cases here] end
								default:
									console.error(`The filter display type ${filter.displayType} is not supported.`);
									return '';
							}
						})
					}
				</div>
				<div className="collection-filter-form__actions">
					<ButtonGroup alignment={Alignment.HORIZONTAL}>
						<Button 
							className="clear-filters"
							display={Display.Outline}
							onClick={onClearFilter}
						>
							Clear Filters
						</Button>
						<Button 
							className="apply-filters"
							display={Display.Solid}
							onClick={onApplyFilter}
						>
							Apply Filters
						</Button>
					</ButtonGroup>
				</div>
			</>
		);
	}
}

export default CollectionFilterPanel;
