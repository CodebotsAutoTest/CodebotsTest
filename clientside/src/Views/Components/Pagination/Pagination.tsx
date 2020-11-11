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
import { Button, Display } from '../Button/Button';
import { TextField } from '../TextBox/TextBox';
import { observer } from 'mobx-react';
import { observable } from 'mobx';
import IPaginationData from 'Models/PaginationData';
// % protected region % [Add extra imports here] off begin
// % protected region % [Add extra imports here] end

export interface IPaginationProps {
	// % protected region % [Customize IPaginationProps here] off begin
	pagination: IPaginationData;
	showGoToPageBox?: boolean;
	onPageChange?: () => void;
	// % protected region % [Customize IPaginationProps here] end
}

enum validPageOptions {
	HIGH,
	VALID,
	LOW,
}

@observer
class Pagination extends React.Component<IPaginationProps> {
	// % protected region % [Add extra options class properties here] off begin
	// % protected region % [Add extra options class properties here] end

	@observable
	private pageTextBoxModel = { page: '1' };

	public render() {
		// % protected region % [Customize options render here] off begin
		const { queryOptions } = this.props.pagination;
		const { page } = queryOptions;

		return (
			<nav aria-label="pagination pagination__collections">
				<ul className="collection__pagination">
					<li>{this.renderFirstButton()}</li>
					<li>{this.renderPreviousButton()}</li>
					<li>
						<span className="pagination__page-number">{page + 1} of {this.totalPages()}</span>
					</li>
					<li>{this.renderNextButton()}</li>
					<li>{this.renderLastButton()}</li>
				</ul>
			</nav >
		);
		// % protected region % [Customize options render here] end
	}

	public renderFirstButton() {
		// % protected region % [Customize renderFirstButton here] off begin
		const { page } = this.props.pagination.queryOptions;
		let isFirstPage = (page === 0);
		return (
			<Button
				disabled={isFirstPage}
				onClick={this.firstPage}
				display={Display.Text}
				labelVisible={false}
				icon={{ icon: "chevrons-left", iconPos: 'icon-left' }}
			>
				First
			</Button>
		);
		// % protected region % [Customize renderFirstButton here] end
	}

	public renderNextButton() {
		// % protected region % [Customize renderNextButton here] off begin
		const { page } = this.props.pagination.queryOptions;
		const noNextPage = (page >= ((this.totalPages()) - 1));
		return (
			<Button
				onClick={this.nextPage}
				display={Display.Text}
				disabled={noNextPage}
				labelVisible={false}
				icon={{ icon: "chevron-right", iconPos: 'icon-right' }}
			>
				Next
			</Button>
		);
		// % protected region % [Customize renderNextButton here] end
	}

	public renderPreviousButton() {
		// % protected region % [Customize renderPreviousButton here] off begin
		const { page } = this.props.pagination.queryOptions;
		const noPreviousPage = (page < 1);
		return (
			<Button
				onClick={this.previousPage}
				display={Display.Text}
				disabled={noPreviousPage}
				labelVisible={false}
				icon={{ icon: "chevron-left", iconPos: 'icon-left' }}
			>
				Previous
			</Button>
		);
		// % protected region % [Customize renderPreviousButton here] end
	}

	public renderLastButton() {
		// % protected region % [Customize renderLastButton here] off begin
		const { page } = this.props.pagination.queryOptions;
		const isLastPage = (page >= ((this.totalPages()) - 1));
		return (
			<Button
				onClick={this.lastPage}
				display={Display.Text}
				disabled={isLastPage}
				labelVisible={false}
				icon={{ icon: "chevrons-right", iconPos: 'icon-right' }}
			>
				Last
			</Button>
		);
		// % protected region % [Customize renderLastButton here] end
	}

	public renderGoToPageBox() {
		// % protected region % [Customize renderGoToPageBox here] off begin
		if (this.props.showGoToPageBox) {
			return (
				<form className="paginator__go-to-pg" onSubmit={this.onGoToPageFormSubmit}>
					<TextField model={this.pageTextBoxModel} modelProperty="page" label="Go to page" inputProps={{ type: 'number' }} />
				</form>
			);
		}

		return null;
		// % protected region % [Customize renderGoToPageBox here] end
	}

	private firstPage = () => {
		// % protected region % [Customize firstPageMethod here] off begin
		this.gotoPage(0);
		// % protected region % [Customize firstPageMethod here] end
	}

	private previousPage = () => {
		// % protected region % [Customize previousPageMethod here] off begin
		const { queryOptions } = this.props.pagination;
		this.gotoPage(queryOptions.page - 1);
		// % protected region % [Customize previousPageMethod here] end
	}

	private nextPage = () => {
		// % protected region % [Customize nextPageMethod here] off begin
		const { queryOptions } = this.props.pagination;
		this.gotoPage(queryOptions.page + 1);
		// % protected region % [Customize nextPageMethod here] end
	}

	private lastPage = () => {
		// % protected region % [Customize lastPage method here] off begin
		this.gotoPage(this.totalPages() - 1);
		// % protected region % [Customize lastPage method here] end
	}

	private onGoToPageFormSubmit = (event: React.FormEvent<HTMLFormElement>) => {
		// % protected region % [Customize onGoToPageFormSubmit here] off begin
		event.preventDefault();

		const n = Number.parseInt(this.pageTextBoxModel.page, 10);
		if (!isNaN(n)) {
			this.gotoPage(n - 1);
		}
		// % protected region % [Customize onGoToPageFormSubmit here] end
	}

	public gotoPage = (pageNo: number) => {
		// % protected region % [Customize gotoPage method here] off begin
		const { queryOptions } = this.props.pagination;

		const validPage = this.isValidPage(pageNo);

		if (validPage === validPageOptions.HIGH) {
			queryOptions.gotoPage(this.totalPages() - 1);
		} else if (validPage === validPageOptions.LOW) {
			queryOptions.gotoPage(0);
		}

		queryOptions.gotoPage(pageNo);

		if (this.props.onPageChange) {
			this.props.onPageChange();
		}
		// % protected region % [Customize gotoPage method here] end
	}

	private isValidPage = (pageNo: number): validPageOptions => {
		// % protected region % [Customize isValidPage method here] off begin
		if (pageNo >= this.totalPages()) {
			return validPageOptions.HIGH;
		} else if (pageNo < 0) {
			return validPageOptions.LOW;
		}
		return validPageOptions.VALID;
		// % protected region % [Customize isValidPage method here] end
	}

	private totalPages () {
		// % protected region % [Customize totalPages method here] off begin
		const { queryOptions, totalRecords } = this.props.pagination;
		const { perPage } = queryOptions;

		if (totalRecords > 0) {
			return Math.ceil(totalRecords / perPage);
		}
		return 1;
		// % protected region % [Customize totalPages method here] end
	}
}

export default Pagination;