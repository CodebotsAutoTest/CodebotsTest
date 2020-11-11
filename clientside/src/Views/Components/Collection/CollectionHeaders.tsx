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
import { Checkbox } from '../Checkbox/Checkbox';
import If from '../If/If';
import { ICollectionItemActionProps, actionFilterFn } from './Collection';
import { observable, runInAction } from 'mobx';
import { IOrderByCondition } from '../ModelCollection/ModelQuery';
import { DisplayType } from '../Models/Enums';
// % protected region % [Add extra imports and exports here] off begin
// % protected region % [Add extra imports and exports here] end

type nameFn = (name: string) => (string | React.ReactNode);
type transformFn<T> = (item: T, name: string) => (string | React.ReactNode);

export interface ICollectionHeaderProps<T> {
	name: string;
	displayName: string | nameFn;
	sortable?: boolean;
	transformItem?: transformFn<T>;
	nullValue?: string;
	sortClicked?: (event: React.MouseEvent<HTMLTableHeaderCellElement, MouseEvent>) => IOrderByCondition<T> | undefined | void;
	// % protected region % [Add extra header props here] off begin
	// % protected region % [Add extra header props here] end
}

export interface ICollectionHeaderPropsPrivate<T> extends ICollectionHeaderProps<T> {
	headerName?: string | React.ReactNode;
	// % protected region % [Add extra private header props here] off begin
	// % protected region % [Add extra private header props here] end
}

export interface ICollectionHeadersProps<T> {
	headers: Array<ICollectionHeaderPropsPrivate<T>>;
	actions?: Array<ICollectionItemActionProps<T>> | actionFilterFn<T>;
	selectableItems?: boolean;
	allChecked: boolean;
	onCheckedAll?: (event: React.ChangeEvent<HTMLInputElement>, checked: boolean) => void
	/** The default order by condition */
	orderBy?: IOrderByCondition<T> | undefined;
	// % protected region % [Add extra headers props here] off begin
	// % protected region % [Add extra headers props here] end
}

@observer
export default class CollectionHeaders<T> extends React.Component<ICollectionHeadersProps<T>> {
	@observable
	private orderBy: IOrderByCondition<T> | undefined | void;

	// % protected region % [Add extra class fields here] off begin
	// % protected region % [Add extra class fields here] end

	constructor(props: ICollectionHeadersProps<T>, context: any){
		// % protected region % [Customise constructor here] off begin
		super(props, context);
		const { orderBy } = this.props;
		this.orderBy = orderBy;
		// % protected region % [Customise constructor here] end
	}
	
	public render() {
		// % protected region % [Customise render here] off begin
		const { selectableItems, headers, actions } = this.props;

		return (
			<thead>
				<tr className="list__header">
					<If condition={selectableItems}>
						<th className="select-box">
							{this.renderSelectAllCheckbox()}
						</th>
					</If>
					{headers.map((header, idx) => {
						return (
							<th key={idx} scope="col" onClick={
								event => {
									runInAction(() => {
										if (header.sortClicked) {
											this.orderBy = header.sortClicked(event);
										}
									});
								}
							} 
								className={header.sortable ? ((!this.orderBy || this.orderBy.path !== header.name) ? 'sortable' : (this.orderBy.descending ? "sortable--des" : "sortable--asc")) : ''}>
								{header.headerName ? header.headerName : `Column ${idx}`}
							</th>
						);
					})}
					<If condition={actions != null}>
						<th scope="col" className="list__header--actions"></th>
					</If>
				</tr>
			</thead>
		);
		// % protected region % [Customise render here] end
	}

	public renderSelectAllCheckbox() {
		// % protected region % [Customize initial renderSelectAllCheckbox here] off begin
		const { allChecked, onCheckedAll } = this.props;
		const checkboxDisplayType = DisplayType.INLINE;
		// % protected region % [Customize initial renderSelectAllCheckbox here] end

		return (
			<Checkbox
				label="Select All"
				modelProperty="checked"
				name="selectall"
				model={{}}
				displayType={checkboxDisplayType}
				inputProps={{
					checked: allChecked,
					onChange: event => {
						runInAction(() => {
							if (onCheckedAll) {
								onCheckedAll(event, event.target.checked);
							}
						});
					},
				}}
			/>
		);
	}
}

// % protected region % [Add methods here] off begin
// % protected region % [Add methods here] end