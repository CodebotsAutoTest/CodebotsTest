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
import { observer } from "mobx-react";
import { action, computed, observable } from 'mobx';
import { Button } from "Views/Components/Button/Button";

export interface IAccordionConfig {
	name: string;
	component: React.ReactNode;
	key: string;
	afterTitleContent?: React.ReactNode;
	disabled?: boolean;
}

export interface IAccordionGroupProps {
	accordions: Array<IAccordionConfig>;
}

@observer
export class AccordionSection extends React.Component<IAccordionConfig> {
	@observable
	private showContents = false;

	@computed
	private get isOpen() {
		return this.showContents && !this.props.disabled;
	}

	@action
	public toggleContents = (isOpen?: boolean) => {
		if (isOpen === undefined) {
			this.showContents = !this.showContents;
		} else {
			this.showContents = isOpen;
		}
	};

	private get contentClassName() {
		return this.isOpen ?
			'accordion__info accordion__info--expanded' :
			'accordion__info accordion__info--collapsed';
	};

	public render() {
		return (
			<section className={this.isOpen ? 'accordion active' : 'accordion'}>
				<Button icon={{icon: 'chevron-up', iconPos: 'icon-right'}} onClick={() => this.toggleContents()}>{this.props.name}</Button>
				{this.props.afterTitleContent}
				<div className={this.contentClassName}>
					{this.isOpen ? this.props.component : null}
				</div>
			</section>
		)
	}
}

export default class AccordionGroup extends React.Component<IAccordionGroupProps> {
	public render() {
		return(
			<>
				{this.props.accordions.map(accordion =>
					<AccordionSection {...accordion}/>
				)}
			</>
		)
	}
}