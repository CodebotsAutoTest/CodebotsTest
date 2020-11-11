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
import { observable, action } from 'mobx';
import axios, { AxiosResponse } from 'axios';

export interface QueryResult {
	data: any;
	error?: string;
	success?: boolean;
	loading: boolean;
}

export interface IAPIQueryProps {
	children: (result: QueryResult) => React.ReactNode;
	url: string;
}

@observer
class APIQuery extends React.Component<IAPIQueryProps> {
	@observable
	private requestState: 'loading' | 'error' | 'done' = 'loading';

	private requestData: any;

	private requestError?: string;

	public componentDidMount = () => {
		const url = this.props.url;

		axios.get(url)
			.then(this.onSuccess)
			.catch(this.onError);
	}

	@action
	private onSuccess = (data: AxiosResponse) => {
		this.requestData = data.data;
		this.requestState = 'done';
	}

	@action
	private onError = (data: AxiosResponse) => {
		this.requestData = data.data;
		this.requestState = 'error';
		this.requestError = data.statusText;
	}

	public render() {
		return this.props.children({
			loading: this.requestState === 'loading', 
			success: this.requestState === 'done', 
			error: this.requestError,
			data: this.requestData,
		});
	}
}

export default APIQuery;