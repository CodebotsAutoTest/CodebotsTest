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
import classNames from 'classnames';
import { IIconProps } from '../Helpers/Common';
// % protected region % [Add extra imports here] off begin
// % protected region % [Add extra imports here] end

export enum Display {
	Solid = 'btn--solid',
	Outline = 'btn--outline',
	Text = 'btn--text',
	None = '',
	// % protected region % [Add extra Display literals here] off begin
	// % protected region % [Add extra Display literals here] end
}

export enum Colors {
	Primary = 'btn--primary',
	Secondary = 'btn--secondary',
	Warning = 'btn--warning',
	Error = 'btn--error',
	Success = 'btn--success',
	None = '',
	// % protected region % [Add extra Color literals here] off begin
	// % protected region % [Add extra Color literals here] end
}

export enum Sizes {
	ExtraSmall = 'btn--xsm',
	Small = 'btn--sm',
	Medium = 'btn--md',
	Large = 'btn--lg',
	ExtraLarge = 'btn--xlg',
	None = '',
	// % protected region % [Add extra Sizes literals here] off begin
	// % protected region % [Add extra Sizes literals here] end
}

export enum Widths {
	auto = 'btn--auto-width',
	fullWidth = 'btn--full-width',
	// % protected region % [Add extra widths literals here] off begin
	// % protected region % [Add extra widths literals here] end
}

export enum QuickTypes {
	Primary = 'btn--primary',
	Secondary = 'btn--secondary',
	None = '',
	// % protected region % [Add extra QuickTypes literals here] off begin
	// % protected region % [Add extra QuickTypes literals here] end
}

export interface ICbButtonProps {
	type?: 'submit' | 'reset' | 'button';
	display?: Display;
	colors?: Colors;
	sizes?: Sizes;
	widths?: Widths;
	quickTypes?: QuickTypes;
	buttonProps?: React.ButtonHTMLAttributes<Element>;
	className?: string;
	disabled?: boolean;
	onClick?: ((event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void);
	icon?: IIconProps;
	labelVisible?: false;
	// % protected region % [Add extra ICbButtonProps fields here] off begin
	// % protected region % [Add extra ICbButtonProps fields here] end
}

@observer
export class Button extends React.Component<ICbButtonProps, any> {
	static defaultProps: Partial<ICbButtonProps> = {
		colors: Colors.Primary,
		// % protected region % [Add extra default props here] off begin
		// % protected region % [Add extra default props here] end
	}

	// % protected region % [Add extra methods and fields here] off begin
	// % protected region % [Add extra methods and fields here] end

	public render() {
		// % protected region % [Override render here] off begin
		const icon = this.props.icon ? `btn--icon icon-${this.props.icon.icon} ${this.props.icon.iconPos}` : '';
		const quickTypes = this.props.quickTypes ? this.props.quickTypes : '';
		const display = this.props.display ? this.props.display : '';
		const colors = this.props.colors ? this.props.colors : '';
		const sizes = this.props.sizes ? this.props.sizes : '';
		const widths = this.props.widths ? this.props.widths : '';
		const className = this.props.className ? this.props.className : '';
		const { disabled } = this.props;

		const clazz = ['btn', icon, quickTypes, display, colors, sizes, widths, className];

		const onClick = this.props.onClick || (this.props.buttonProps ? this.props.buttonProps.onClick : () => {});

		const labelVisible = (this.props.labelVisible === undefined) ? true : this.props.labelVisible;

		return (
			<button
				type={this.props.type || 'button'}
				onClick={onClick}
				className={classNames(clazz)}
				aria-label={!labelVisible ? (typeof(this.props.children) === 'string'? this.props.children : undefined) : undefined}
				disabled={disabled}
				{...this.props.buttonProps}
			>
				{labelVisible ? this.props.children : null}
			</button>
		);
		// % protected region % [Override render here] end
	}
}