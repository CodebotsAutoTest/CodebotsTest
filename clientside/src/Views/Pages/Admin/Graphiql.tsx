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
import Cookies from 'js-cookie';
import { SERVER_URL } from 'Constants';
// @ts-ignore
import GraphiQL from 'graphiql';
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

const graphiQLFetcher = (graphQLParams: {}) => {
	// % protected region % [Customise GraphiQL fetcher here] off begin
	const token = Cookies.get('XSRF-TOKEN');
	return fetch(`${SERVER_URL}/api/graphql`, {
		method: 'post',
		headers: {
			'Content-Type': 'application/json',
			'X-XSRF-TOKEN': token ? token : '',
		},
		body: JSON.stringify(graphQLParams),
	}).then(response => response.json());
	// % protected region % [Customise GraphiQL fetcher here] end
}

export default function GraphiQl() {
	// % protected region % [Customise GraphiQL component here] off begin
	return (
		<div className="graphiql-content-container body-content">
			<GraphiQL fetcher={graphiQLFetcher}/>
		</div>
	);
	// % protected region % [Customise GraphiQL component here] end
}