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
import { default as ApolloClient, NetworkStatus } from 'apollo-boost';
import { QueryOptions } from 'apollo-client/core/watchQueryOptions';
import { ApolloQueryResult } from 'apollo-client/core/types';
import { store } from 'Models/Store';
import { SERVER_URL } from 'Constants';

export const pollingInterval = 100;
export const pollingTimeout = 1000;

let storedGraphqlResponses: {[key: string]: {}} = {};

export function setupGraphQlMocking(graphqlResponses: {[key: string]: {}}) {
	storedGraphqlResponses = {...storedGraphqlResponses, ...graphqlResponses};

	store.apolloClient = new ApolloClient({
		uri: `${SERVER_URL}/api/graphql`,
	});

	store.apolloClient.query = (options: QueryOptions): Promise<ApolloQueryResult<any>> => {
		return new Promise<ApolloQueryResult<any>>((resolve, reject) => {
			const queryName = options.query.definitions[0]?.['name']?.['value'];
			if (!queryName){
				return reject('Query name undefined');
			}
			const graphqlResponse = storedGraphqlResponses[queryName];
			if (!graphqlResponse){
				return reject('Mocked response is undefined');
			}
			return resolve({
				data: graphqlResponse,
				loading: false,
				stale: false,
				networkStatus: NetworkStatus.ready
			})
		})
	};
}