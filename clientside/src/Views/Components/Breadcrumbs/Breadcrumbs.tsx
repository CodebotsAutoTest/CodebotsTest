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
import { Link } from 'react-router-dom';
import classNames from 'classnames';
import { IIconProps } from '../Helpers/Common';
import If from '../If/If';

export interface IBreadCrumbsTag {
	label: string;
	link?: string;
	className?: string;
	isHomeTag?: boolean;
	isCurrentTag?: boolean;
}

export interface IBreadcrumbsProps {
	tags: IBreadCrumbsTag[];
	className?: string;
	icon?: IIconProps,
}

@observer
export class Breadcrumbs extends React.Component<IBreadcrumbsProps, any> {
	constructor(props: IBreadcrumbsProps, context: any) {
		super(props, context);
	}

	private getTagNode = (tag: IBreadCrumbsTag) => {
		let className = classNames(tag.className)

		if (!tag.isHomeTag) {
			className = classNames(
				className,
				`icon-${(this.props.icon && this.props.icon) ? this.props.icon.icon : 'chevron-right'}`,
				(this.props.icon && this.props.icon) ? this.props.icon.iconPos : 'icon-left'
			);
		}

		if (tag.isCurrentTag) {
			return (
				<li className={className}>
					{tag.label}
				</li>
			);
		} else {
			return (
				<li className={className}>
					<Link to={tag.link ? tag.link : '#'}>
						{tag.label}
					</Link>
				</li>
			);
		}
	}

	render() {
		const className = classNames('breadcrumbs', this.props.className);
		return (
			<ul className={className}>
				{
					this.props.tags.map((tag, index) => {
						return this.getTagNode(
							{
								...tag,
								isHomeTag: (index === 0),
								isCurrentTag: (this.props.tags.length === (index + 1))
							});
					})
				}
			</ul>
		);
	}
}
