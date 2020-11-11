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
import { observable, action, computed } from 'mobx';
import classNames from 'classnames';
import { Interface } from 'readline';

export interface ITabConfig {
	name: string,
	component: React.ReactNode,
	key: string,
	className?: string
};

export interface ITabsProps {
	tabs: Array<ITabConfig>;
	className?: string;
	innerProps?: React.DetailedHTMLProps<React.HTMLAttributes<HTMLElement>, HTMLElement>;
	defaultTab?: number;
	currentTab?: number;
	onTabClicked?: (tabIndex: number, event: React.MouseEvent<HTMLAnchorElement, MouseEvent>) => void;
}

interface ITabHeaderProps {
	tabState: { currentTab: number };
	tabIdx: number;
	tabChanged?: (() => void);
	onTabClicked?: (tabIndex: number, event: React.MouseEvent<HTMLAnchorElement, MouseEvent>) => void;
}

class TabHeader extends React.Component<ITabHeaderProps, any> {
	public render() {
		return <a onClick={this.onTabClicked}>{this.props.children}</a>;
	}

	@action
	private onTabClicked = (event: React.MouseEvent<HTMLAnchorElement, MouseEvent>) => {
		if(this.props.tabState.currentTab !== this.props.tabIdx && this.props.tabChanged){
			this.props.tabChanged();
		}
		this.props.tabState.currentTab = this.props.tabIdx;
		if (this.props.onTabClicked) {
			this.props.onTabClicked(this.props.tabIdx, event);
		}
	}
}

@observer
export default class Tabs extends React.Component<ITabsProps, any> {
	@observable
	public tabState = {
		currentTab: 0,
	};

	@computed
	public get currentTab() {
		return this.props.currentTab || this.tabState.currentTab;
	}
	
	constructor(props: ITabsProps, context: any) {
		super(props, context);
		if (this.props.defaultTab) {
			this.tabState.currentTab = this.props.defaultTab;
		}
	}

	public render(){
		return (
			<>
				<nav {...this.props.innerProps} className={'tabs ' + (this.props.className ? this.props.className : '')}>
					<ul>
						{this.props.tabs.map((tab, idx) => {
							return (
								<li key={tab.key} className={'tab-header' + (this.currentTab === idx ? ' selected' : '')}>
									<TabHeader tabState={this.tabState} tabIdx={idx} onTabClicked={this.props.onTabClicked}>{tab.name}</TabHeader>
								</li>
							);
						})}
					</ul>
				</nav>
				<div className={classNames(this.props.tabs[this.currentTab].className)}>
					{this.props.tabs[this.currentTab].component}
				</div>
			</>
		);
	}
}