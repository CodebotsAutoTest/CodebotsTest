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
import { Model, IModelType } from 'Models/Model';
import { RouteComponentProps } from 'react-router';
import { observer, inject } from 'mobx-react';
import EntityAttributeList from './EntityAttributeList';
import { EntityFormMode } from '../Helpers/Common'
import { Query, QueryResult } from 'react-apollo';
import { getFetchSingleQuery, getModelDisplayName, getModelName } from 'Util/EntityUtils';
import { lowerCaseFirst } from 'Util/StringUtils';

interface IEntityEditProps<T extends Model> extends RouteComponentProps<IEntityEditRouteParams> {
	modelType: IModelType;
	formMode: EntityFormMode;
}

interface IEntityEditRouteParams {
	id?: string;
}

@inject('store')
@observer
class EntityEdit<T extends Model> extends React.Component<IEntityEditProps<T>, any> {
	public render() {
		const { modelType } = this.props;
		const query = getFetchSingleQuery(modelType);
		const modelName = getModelDisplayName(modelType);
		const dataReturnName = lowerCaseFirst(getModelName(modelType));

		const title = `${this.props.formMode === 'create' ? 'Create' : (this.props.formMode === 'edit' ? 'Edit' : 'View')} ${modelName}`;
		const sectionClassName = 'crud__' + this.props.formMode;
		const options = { title, sectionClassName };

		if (this.props.match.params.id === null) {
			throw new Error('Expected id of model to fetch for edit');
		}

		/* Refetch the model */
		return (
			<Query query={query} fetchPolicy="network-only" variables={{ "args": [{ "path": "id", "comparison": "equal", "value": this.props.match.params.id }] }}>
				{({ loading, error, data }: QueryResult) => {
					if (loading) {
						return <div>Loading {modelName}...</div>;
					}
					if (error) {
						return <div>Error Loading {modelName}</div>;
					}
					return (<EntityAttributeList
						{...this.props}
						model={new modelType(data[dataReturnName])}
						{...options}
						formMode={this.props.formMode}
						modelType={this.props.modelType}
					/>);
				}}
			</Query>
		);
	}
}

export default EntityEdit;