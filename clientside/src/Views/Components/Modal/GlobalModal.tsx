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
import { observer } from 'mobx-react';
import * as React from 'react';
import { action, observable } from 'mobx';
import Modal, { IModalProps } from './Modal';

type globalModalState = IModalProps & {children: React.ReactNode};

export interface IGlobalModal {
	/**
	 * Shows a modal on the screen
	 * @param label A label for the modal for screen readers
	 * @param children The content to display inside the modal
	 * @param modalProps
	 */
	show: (label: string, children: React.ReactNode, modalProps?: Partial<IModalProps>) => void;
	/**
	 * Hides the global dialog
	 */
	hide: () => void;
}

/**
 * A global modal that can be called imperatively from the store
 * This component should only be constructed by the top level App component
 */
@observer
export default class GlobalModal extends React.Component implements IGlobalModal {
	/** Defaults for the modal state */
	private get defaultModalState(): globalModalState {
		return {
			label: '',
			children: null,
			isOpen: false,
			className: undefined,
			onRequestClose: this.hide,
		};
	}

	/**
	 * The props of the modal controlled by this component
	 */
	@observable
	private modalState: globalModalState = this.defaultModalState;

	/** @inheritDoc */
	@action
	public show = (label: string, children: React.ReactNode, modalProps: Partial<IModalProps> = {}) => {
		this.modalState = {
			...this.defaultModalState,
			isOpen: true,
			children,
			label,
			...modalProps,
		};
	}

	/** @inheritDoc */
	@action
	public hide = () => {
		this.modalState.isOpen = false;
	}

	public render() {
		return (
			<Modal {...this.modalState}>
				{this.modalState.children}
			</Modal>
		);
	}
}