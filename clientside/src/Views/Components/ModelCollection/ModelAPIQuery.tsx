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
import _ from 'lodash';
import axios, { AxiosResponse } from 'axios';
import { Model } from 'Models/Model';
import { PaginationQueryOptions } from 'Models/PaginationData';
import { getModelName } from 'Util/EntityUtils';
import { observer } from 'mobx-react';
import { SERVER_URL } from 'Constants';
import { observable, action, runInAction, computed } from 'mobx';
import { lowerCaseFirst } from 'Util/StringUtils';
import { modelName as modelNameSymbol } from 'Symbols';
import { IWhereCondition } from './ModelQuery';

type Comparators =
	'contains'
	| 'endsWith'
	| 'equal'
	| 'greaterThan'
	| 'greaterThanOrEqual'
	| 'in'
	| 'notIn'
	| 'lessThan'
	| 'lessThanOrEqual'
	| 'like'
	| 'notEqual'
	| 'startsWith';

export interface IOrderByCondition<T> {
	path: string;
	descending?: boolean;
}

export interface IModelAPIQueryVariables<T> {
	skip?: number;
	take?: number;
	args?: Array<IWhereCondition<T>>;
	orderBy: Array<IOrderByCondition<T>>;
	ids?: string[];
}

export interface QueryResult {
	data: any;
	error?: string;
	success?: boolean;
	loading: boolean;
	refetch: () => void;
}

export interface ApiQueryParams {
	[key: string]: Date | boolean | string | number
}

export interface IModelAPIQueryProps<T extends Model, TData = any> {
	children: (result: QueryResult) => React.ReactNode;
	model: { new(json?: {}): T };
	conditions?: Array<IWhereCondition<T>> | Array<Array<IWhereCondition<T>>>;
	moreParams?: ApiQueryParams
	ids?: string[];
	searchStr?: string;
	orderBy?: IOrderByCondition<T>;
	url: string;
	pagination: PaginationQueryOptions;
}

@observer
class ModelAPIQuery<T extends Model, TData = any> extends React.Component<IModelAPIQueryProps<T, TData>> {
	@observable
	private requestState: 'loading' | 'error' | 'done' = 'loading';

	private oldProps: IModelAPIQueryProps<T, TData>;

	private requestData: { data: Array<T>, totalCount: number };

	private requestError?: string;

	public componentDidMount = () => {
		this.oldProps = _.cloneDeep(this.props);
		this.makeQuery();
	}

	public componentDidUpdate() {
		if (
			!_.isEqual(this.props, this.oldProps)
			||
			!!this.props.pagination && !!this.oldProps.pagination && this.props.pagination.page !== this.oldProps.pagination.page
		) {
			// if this query is not made due to the pageNo change, then the pageNo should be reset to 0
			// in case the new query result doesn't have this page no
			if (!!this.props.pagination && !!this.oldProps.pagination && this.props.pagination.page === this.oldProps.pagination.page) {
				runInAction(() => {
					this.props.pagination.page = 0;
				});
			}
			this.makeQuery();
			this.oldProps = _.cloneDeep(this.props);
		}
	}

	@action
	private makeQuery = () => {
		this.requestState = 'loading';

		const modelName: string = this.props.model[modelNameSymbol];
		const lowerModelName = lowerCaseFirst(modelName);
		const url = this.props.url || `${SERVER_URL}/api/${lowerModelName}`;

		axios.get(url, { params: this.constructVariables })
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
		const modelName = getModelName(this.props.model);

		if (this.props.pagination.page < 0) {
			return null;
		}

		return this.props.children({
			loading: this.requestState === 'loading',
			success: this.requestState === 'done',
			error: this.requestError,
			refetch: action(() => this.requestState === 'loading'),
			data: {
				[`${lowerCaseFirst(modelName)}s`]: this.requestData ? this.requestData.data : [],
				[`count${modelName}s`]: { number: this.requestData ? this.requestData.totalCount : 0 }
			},
		});
	}

	@computed
	private get constructVariables() {
		const { orderBy: orderByProp, pagination, moreParams } = this.props;
		const { page, perPage } = pagination;

		let orderBy: IOrderByCondition<T> = { path: new this.props.model().getDisplayAttribute(), descending: false };
		if (orderByProp) {
			orderBy = orderByProp;
		}

		return {
			pageNo: (!isNaN(page) && page >= 0) ? (page + 1) : undefined,  // mathcing the backend pagination which starts from page 1
			pageSize: perPage || undefined,
			searchStr: this.props.searchStr || undefined,
			orderBy: orderBy.path || undefined,
			descending: orderBy.descending,
			...moreParams
		};
	}
}

export default ModelAPIQuery;