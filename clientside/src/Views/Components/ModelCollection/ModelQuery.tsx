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
import { QueryResult, Query, OperationVariables } from 'react-apollo';
import { DocumentNode } from 'graphql';
import { Model } from 'Models/Model';
import { PaginationQueryOptions } from 'Models/PaginationData';
import { getFetchAllQuery, getFetchAllConditional } from 'Util/EntityUtils';
import { observer } from 'mobx-react';
import { isOrCondition } from 'Util/GraphQLUtils';
// % protected region % [Add extra imports here] off begin
// % protected region % [Add extra imports here] end

export type Comparators = 'contains'
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
	| 'startsWith'
	// % protected region % [Add extra comparators here] off begin
	// % protected region % [Add extra comparators here] end
	;

export interface IOrderByCondition<T> {
	path: string;
	descending?: boolean;
	// % protected region % [Add any extra order by condition fields here] off begin
	// % protected region % [Add any extra order by condition fields here] end
}

export type CaseComparison = 'CURRENT_CULTURE'
	| 'CURRENT_CULTURE_IGNORE_CASE'
	| 'INVARIANT_CULTURE'
	| 'INVARIANT_CULTURE_IGNORE_CASE'
	| 'ORDINAL'
	| 'ORDINAL_IGNORE_CASE'
	// % protected region % [Add extra case comparisons here] off begin
	// % protected region % [Add extra case comparisons here] end
	;

export type CaseComparisonPascalCase = 'CurrentCulture'
	| 'CurrentCultureIgnoreCase'
	| 'InvariantCulture'
	| 'InvariantCultureIgnoreCase'
	| 'Ordinal'
	| 'OrdinalIgnoreCase'
	// % protected region % [Add extra pascal case comparators here] off begin
	// % protected region % [Add extra pascal case comparators here] end
	;

interface BaseWhereCondition<T> {
	path: string;
	comparison: Comparators;
	value: any;
	// % protected region % [Add any extra base where condition fields here] off begin
	// % protected region % [Add any extra base where condition fields here] end
}

export interface IWhereCondition<T> extends BaseWhereCondition<T> {
	case?: CaseComparison;
	// % protected region % [Add any extra where condition fields here] off begin
	// % protected region % [Add any extra where condition fields here] end
}

export interface IWhereConditionApi<T> extends BaseWhereCondition<T> {
	case?: CaseComparisonPascalCase;
	// % protected region % [Add any extra api where condition fields here] off begin
	// % protected region % [Add any extra api where condition fields here] end
}

export interface IModelQueryVariables<T> {
	skip?: number;
	take?: number;
	args?: Array<IWhereCondition<T>>;
	orderBy: Array<IOrderByCondition<T>>;
	ids?: string[];
	// % protected region % [Add any extra model query variables fields here] off begin
	// % protected region % [Add any extra model query variables fields here] end
}

export interface IModelQueryProps<T extends Model, TData = any> {
	children: (result: QueryResult<TData, OperationVariables>) => JSX.Element | null;
	model: {new(json?: {}): T};
	conditions?: Array<IWhereCondition<T>> | Array<Array<IWhereCondition<T>>>;
	ids?: string[];
	orderBy?: IOrderByCondition<T>;
	customQuery?: DocumentNode;
	pagination: PaginationQueryOptions;
	useListExpands?: boolean;
	expandString?: string;
	// % protected region % [Add any extra model query props here] off begin
	// % protected region % [Add any extra model query props here] end
}

@observer
class ModelQuery<T extends Model,TData = any> extends React.Component<IModelQueryProps<T, TData>> {
	// % protected region % [Add any extra class methods here] off begin
	// % protected region % [Add any extra class methods here] end

	public render() {
		// % protected region % [Customize render here] off begin
		let fetchAllQuery;

		if (isOrCondition(this.props.conditions)) {
			fetchAllQuery = getFetchAllConditional(this.props.model, this.props.expandString, this.props.useListExpands);
		} else {
			fetchAllQuery = getFetchAllQuery(this.props.model, this.props.expandString, this.props.useListExpands);
		}

		return (
			<Query
				fetchPolicy="network-only"
				notifyOnNetworkStatusChange={true}
				query={this.props.customQuery || fetchAllQuery}
				variables={this.constructVariables()}>
				{this.props.children}
			</Query>
		);
		// % protected region % [Customize render here] end
	}

	private constructVariables() {
		// % protected region % [Customize construct variables method here] off begin
		const { conditions, ids, orderBy : orderByProp, pagination } = this.props;
		const { page, perPage } = pagination;

		let orderBy: IOrderByCondition<T> = {
			path: new this.props.model().getDisplayAttribute(),
			descending: false
		};

		if (orderByProp) {
			orderBy = orderByProp;
		}

		return {
			skip: page * perPage,
			take: perPage,
			args: conditions,
			orderBy: [orderBy],
			ids,
		};
		// % protected region % [Customize construct variables method here] end
	}
}

// % protected region % [Customize default export here] off begin
export default ModelQuery;
// % protected region % [Customize default export here] end