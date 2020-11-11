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
import classNames from 'classnames';
import { IFilter } from '../CollectionFilterPanel';
import { observer } from 'mobx-react';
import { computed } from 'mobx';
import { SyncComboboxProps } from 'Views/Components/Combobox/Combobox';
import { MultiCombobox } from 'Views/Components/Combobox/MultiCombobox';

interface IFilterEnumComboBoxProps<T, I> extends Partial<SyncComboboxProps<T, I>> {
	filter: IFilter<T>;
	className?: string;
}

@observer
class FilterEnumComboBox<T, I> extends React.Component<IFilterEnumComboBoxProps<T, I>> {
	@computed
	private get options() {
		return this.props.filter.enumResolveFunction || [];
	}

	public render() {
		const { filter, className} = this.props;

		return <MultiCombobox
			model={filter}
			modelProperty="value1"
			label={filter.displayName}
			className={classNames('collection-filter-enum-combobox', className)}
			options={this.options}
			isClearable={true}
			onAfterChange={(event, data) => {
				filter.active = !!filter.value1 && ((filter.value1 as string[]).length > 0);
				if (this.props.onAfterChange) {
					this.props.onAfterChange(event, data);
				}
			}}
		/>;
	}
}

export default FilterEnumComboBox;