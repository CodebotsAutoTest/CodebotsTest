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
import { action } from 'mobx';
import { AsyncComboboxProps, Combobox, SyncComboboxProps } from './Combobox';
import { DropdownProps } from 'semantic-ui-react';

export interface SyncMultiComboboxProps<T, I> extends SyncComboboxProps<T, I> {

}

export interface AsyncMultiComboboxProps<T, I> extends AsyncComboboxProps<T, I> {

}

export type IMultiComboboxProps<T, I> =  SyncMultiComboboxProps<T, I> | AsyncComboboxProps<T, I>;

/**
 * A MultiCombobox is a view that allows allows selection of many elements from a dropdown menu
 */
@observer
export class MultiCombobox<T, I> extends React.Component<IMultiComboboxProps<T, I>> {
	static defaultProps = {
		styles: {}
	}

	public render() {
		// return "Not yet done";
		return (
			<Combobox
				{...this.props}
				inputProps={{
					multiple: true,
					...this.props.inputProps,
				}}
			/>
		);
	}
}