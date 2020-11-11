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
import { RouteComponentProps } from 'react-router-dom';
import { IIconProps } from '../Helpers/Common';
import { observer } from 'mobx-react';
import { observable, action, computed } from 'mobx';
import classNames from 'classnames';
import NavigationLinks from './NavigationLinks';
// % protected region % [Add extra page imports here] off begin
// % protected region % [Add extra page imports here] end

export enum Orientation {
	VERTICAL,
	HORIZONTAL,
	// % protected region % [Add orientations here] off begin
	// % protected region % [Add orientations here] end
}

export interface ILink extends IIconProps {
	shouldDisplay?: () => boolean;
	path: string;
	label: React.ReactNode;
	onClick?: (event?: any) => void;
	subLinks?: ILink[];
	subLinksFilter?: (link: ILink) => boolean;
	isDisabled?: boolean;
	useATag?: boolean;
	customComponent?: React.ReactNode;
	className?: string;
	// % protected region % [Add extra ILink fields here] off begin
	// % protected region % [Add extra ILink fields here] end
}

export interface INavigationProps<T extends ILink> extends RouteComponentProps {
	className?: string;
	orientation: Orientation;
	linkGroups: Array<Array<T>>;
	filter?: (link: T) => boolean;
	alwaysExpanded?: boolean;
	// % protected region % [Add extra INavigationProps fields here] off begin
	// % protected region % [Add extra INavigationProps fields here] end
}

// % protected region % [Add extra interfaces here] off begin
// % protected region % [Add extra interfaces here] end

@observer
// % protected region % [Customise class implementation here] off begin
class Navigation<T extends ILink> extends React.Component<INavigationProps<T>> {
// % protected region % [Customise class implementation here] end
	// % protected region % [Customise navigation fields here] off begin
	@computed
	private get alwaysExpanded() {
		const { alwaysExpanded, orientation } = this.props;
		if (orientation === Orientation.HORIZONTAL && alwaysExpanded === undefined) {
			return true;
		}
		return alwaysExpanded;
	}

	@observable
	private navCollapsed: boolean = true;
	// % protected region % [Customise navigation fields here] end

	public render() {
		// % protected region % [Customise expand button here] off begin
		const { className, linkGroups, ...routerProps } = this.props;

		let expandButton = null;
		let navClassName = classNames(className, 'nav', this.getOrientationClassName());

		if (!this.alwaysExpanded) {
			navClassName = classNames(navClassName, this.navCollapsed ? 'nav--collapsed' : 'nav--expanded');
			expandButton = (
				<a className={classNames('link-rm-txt-dec expand-icon', this.navCollapsed ? 'icon-menu' : 'icon-menu', 'icon-left')} 
					onClick={this.onClickNavCollapse} />
			);
		}

		return (
			<nav className={navClassName}>
				{linkGroups.map((links, index) => (
					<NavigationLinks
						key={index}
						{...routerProps}
						links={links}
					/>
				))}
				{expandButton}
			</nav>
		);
		// % protected region % [Customise expand button here] end
	}

	private getOrientationClassName = () => {
		// % protected region % [Customise getOrientationClassName here] off begin
		const { orientation } = this.props;
		switch (orientation) {
			case Orientation.HORIZONTAL:
				return 'nav--horizontal';
			case Orientation.VERTICAL:
				return 'nav--vertical';
			default:
				break;
		}
		return '';
		// % protected region % [Customise getOrientationClassName here] end
	};

	@action
	private onClickNavCollapse = () => {
		// % protected region % [Customise onClickNavCollapse here] off begin
		this.navCollapsed = !this.navCollapsed;
		// % protected region % [Customise onClickNavCollapse here] end
	};

	// % protected region % [Add extra methods here] off begin
	// % protected region % [Add extra methods here] end
}

// % protected region % [Customise export here] off begin
export default Navigation;
// % protected region % [Customise export here] end