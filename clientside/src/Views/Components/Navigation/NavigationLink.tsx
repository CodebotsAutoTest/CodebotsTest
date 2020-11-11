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
import { RouteComponentProps, matchPath } from "react-router";
import * as React from 'react';
import classNames from 'classnames';
import { Link } from 'react-router-dom';
import { observer } from 'mobx-react';
import { computed, action, observable } from 'mobx';
import { IIconProps } from '../Helpers/Common';
import { ILink } from './Navigation';
import NavigationLinks from './NavigationLinks';
import If from '../If/If';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

interface INavigationLinkProps extends RouteComponentProps, IIconProps {
	path: string;
	className?: string;
	label: React.ReactNode;
	isParent?: boolean;
	subLinks?: ILink[];
	subLinksFilter?: (link: ILink) => boolean;
	onClick?: (event: React.MouseEvent<HTMLLIElement, MouseEvent>) => void;
	isActive?: boolean;
	isDisabled?: boolean;
	useATag?: boolean;
	customComponent?: React.ReactNode;
	// % protected region % [Add any extra props here] off begin
	// % protected region % [Add any extra props here] end
}

@observer
class NavigationLink extends React.Component<INavigationLinkProps> {
	static defaultProps = {
		iconPos: 'left',
		// % protected region % [Add any extra default props here] off begin
		// % protected region % [Add any extra default props here] end
	}

	// % protected region % [Add any extra class fields here] off begin
	// % protected region % [Add any extra class fields here] end

	private liRef: HTMLLIElement | null = null;

	componentDidMount() {
		document.addEventListener('click', this.handleDocumentClick);
		// % protected region % [Change componentDidMount here] off begin
		// % protected region % [Change componentDidMount here] end
	}
	componentWillMount() {
		document.removeEventListener('click', this.handleDocumentClick);
		// % protected region % [Change componentWillMount here] off begin
		// % protected region % [Change componentWillMount here] end
	}
	componentWillUnmount() {
		document.removeEventListener('click', this.handleDocumentClick);
		// % protected region % [Change componentWillUnmount here] off begin
		// % protected region % [Change componentWillUnmount here] end
	}

	@observable
	private subLinksExpanded: boolean = false;

	@computed
	private get icon() {
		if (this.props.icon) {
			return `icon-${this.props.icon}`;
		}
		return undefined;
	}

	@computed
	private get iconPos() {
		if (this.icon) {
			return this.props.iconPos;
		}
		return undefined;
	}

	public render() {
		// % protected region % [Customise render here] off begin
		const { path, label, subLinks, subLinksFilter, ...routerProps } = this.props;
		let subLinksNode = null;
		if (this.props.isParent && !!this.props.subLinks) {
			const ulClazz = classNames('nav__sublinks', this.subLinksExpanded ? 'nav__sublinks--visible' : '');
			subLinksNode = (
				<NavigationLinks
					{...routerProps}
					className={ulClazz}
					links={subLinks || []}
					filter={subLinksFilter}
				/>
			);
		}

		const isActive = this.props.isActive ? this.props.isActive : (this.isActive(path));

		let textNode = !!this.icon ? <span>{label}</span> : label;

		return (
			<li
				ref={(ref: any) => {
					if (!!ref) {
						this.liRef = ref;
					}
				}}
				className={classNames({ 'nav__parent-link--active': this.subLinksExpanded && this.props.isParent, 'active': isActive && !this.props.isParent }, this.props.className)} key={path}
				onClick={this.onClick}
			>
				{
					(this.props.isParent || this.props.isDisabled)
						? <a className={classNames(this.icon, this.iconPos)} aria-label={typeof label === "string" ? label : undefined}>{textNode}</a>
						: this.props.customComponent !== undefined
						? this.props.customComponent
						: this.props.useATag
						? <a href={path} className={classNames(this.icon, this.iconPos)} target={'_blank'} aria-label={typeof label === "string" ? label : undefined}>{textNode}</a>
						: <Link to={path} className={classNames(this.icon, this.iconPos)} aria-label={typeof label === "string" ? label : undefined}>{textNode}</Link>
				}
				{subLinksNode}
			</li>
		);
		// % protected region % [Customise render here] end
	}

	@action
	private onClick = (event: React.MouseEvent<HTMLLIElement, MouseEvent>) => {
		// % protected region % [Customise onClick here] off begin
		if (this.props.isParent) {
			this.subLinksExpanded = !this.subLinksExpanded;
		}
		if (this.props.onClick) {
			this.props.onClick(event);
		}
		// % protected region % [Customise onClick here] end
	}

	private isActive = (path: string) => {
		// % protected region % [Customise isActive here] off begin
		return !!matchPath(this.props.location.pathname, { path, exact: true });
		// % protected region % [Customise isActive here] end
	}

	@action
	private handleDocumentClick = (e: any) => {
		// % protected region % [Customise handleDocumentClick here] off begin
		if (this.subLinksExpanded && !!this.liRef && (!this.liRef.contains(e.target) || (!!this.liRef.lastElementChild && this.liRef.lastElementChild.contains(e.target)))) {
			this.subLinksExpanded = !this.subLinksExpanded;
		}
		// % protected region % [Customise handleDocumentClick here] end
	}
}

// % protected region % [Customise export here] off begin
export default NavigationLink;
// % protected region % [Customise export here] end