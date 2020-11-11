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
import * as React from "react";
import classNames from 'classnames';
import { observer } from 'mobx-react';
import { contextMenu, Menu, Item, Submenu } from 'react-contexify';
import { Button } from '../Button/Button';
import { IconPositions } from '../Helpers/Common';
import { MenuItemEventHandler } from 'react-contexify/lib/types';

export interface IContextMenuItemProps {
	/** A label for the action button */
	label?: string;
	/** A class for the action button */
	buttonClass?: string;
	/** An icon class for the action button */
	icon?: string;
	/** The position of the icon */
	iconPos?: IconPositions;
	/** Should the icon be displayed */
	showIcon?: boolean;
	/** Should the label be displayed*/
	showLabel?: boolean;
	/** Is the action an additional action, this is displayed in the more menu */
	isAdditional?: boolean;
	/** A custom button that is displayed, instead of the one provided by the collection */
	customItem?: React.ReactNode;
	/** Callback function on click */
	onClick?: (args: MenuItemEventHandler) => any;
}

export interface IContextMenuItemGroup {
	groupName: string;
	actions: IContextMenuActions;
}

export type IContextMenuActions = Array<IContextMenuItemProps | IContextMenuItemGroup>;

export interface IContextMenuProps {
	id?: string;
	className?: string;
	actions: IContextMenuActions;
	menuId: string;
}

export function isItemGroup(item: IContextMenuItemProps | IContextMenuItemGroup): item is IContextMenuItemGroup {
	return item['groupName'] !== undefined;
}

@observer
export class ContextMenu extends React.Component<IContextMenuProps> {
	
	public render() {
		const menuItems = this.getSubMenu(this.props.actions, this.props.menuId);

		return (
			<Menu id={this.props.menuId} className={classNames(this.props.className)}>
				{menuItems}
			</Menu>
		);
	}

	private getSubMenu = (subActions: Array<IContextMenuItemProps | IContextMenuItemGroup>, parentItemKey: string): React.ReactNode => {
		return subActions.map((menuItem, index) => {
			const itemKey = `${parentItemKey}-${index}`;
			let menuItemNode = undefined;
			if (!isItemGroup(menuItem)) {
				if (!!menuItem.label) {
					const icon = menuItem.showIcon && menuItem.icon && menuItem.iconPos ? { icon: menuItem.icon, iconPos: menuItem.iconPos } : undefined;
					menuItemNode =
						<Button className={menuItem.buttonClass} icon={icon}>
							{menuItem.label}
						</Button>;
				} else {
					menuItemNode = menuItem.customItem || 'undefined';
				}

				return (
					<Item
						onClick={(args: MenuItemEventHandler) => {
							if (menuItem.onClick) {
								menuItem.onClick(args);
							}
						}}
						key={itemKey}
					>
						{menuItemNode}
					</Item>
				)
			} else {
				return (
					<Submenu
						label={menuItem.groupName}
						arrow=">"
						key={itemKey}
					>
						{this.getSubMenu(menuItem.actions, itemKey)}
					</Submenu>
				)
			}
		});
	}

	// Here come the magic
	public handleContextMenu = (e: React.MouseEvent<Element, MouseEvent>) => {
		// always prevent default behavior
		e.preventDefault();

		contextMenu.show({
			id: this.props.menuId,
			event: e,
		});
	}
}