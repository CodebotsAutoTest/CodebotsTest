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
import Collection from 'Views/Components/Collection/Collection'
import { mount } from "enzyme";
import React from 'react';
import { observable, computed, runInAction } from 'mobx';
import IPaginationData, { PaginationQueryOptions } from 'Models/PaginationData';
import _ from 'lodash'
import { observer } from 'mobx-react';

interface TestCollectionProps{
	paginationSettings: IPaginationData,
	data: {id: string, title: string, description: string}[],
}

@observer
class TestCollection extends React.Component<TestCollectionProps>{
	@observable
	private collectionData = this.props.data

	@observable
	private paginationSettings: IPaginationData = this.props.paginationSettings;

	@computed
	private get paginatedData() {
			let page = this.paginationSettings.queryOptions.page;
			let perPage = this.paginationSettings.queryOptions.perPage;
			return _.take(_.drop(this.collectionData, (page) * perPage), perPage);
	}

	render() {
		return <Collection
			pagination = {this.paginationSettings}
			collection={this.paginatedData}
			headers={[
				{displayName: "id", name: "id"},
				{displayName: "title", name: "title"},
				{displayName: "description", name: "description"},
			]} />
	}
}

const data = [
	{id: "1", title: "Hello 1", description: "world 1"},
	{id: "2", title: "Hello 2", description: "world 2"},
	{id: "3", title: "Hello 3", description: "world 3"},
	{id: "4", title: "Hello 4", description: "world 4"},
	{id: "5", title: "Hello 5", description: "world 5"},
	{id: "6", title: "Hello 6", description: "world 6"},
	{id: "7", title: "Hello 7", description: "world 7"},
	{id: "8", title: "Hello 8", description: "world 8"},
	{id: "9", title: "Hello 9", description: "world 9"},
	{id: "10", title: "Hello 10", description: "world 10"},
	{id: "11", title: "Hello 11", description: "world 11"},
	{id: "12", title: "Hello 12", description: "world 12"},
	{id: "13", title: "Hello 13", description: "world 13"},
	{id: "14", title: "Hello 14", description: "world 14"},
	{id: "15", title: "Hello 15", description: "world 15"},
	{id: "16", title: "Hello 16", description: "world 16"},
];


describe('Collection Component', () => {
	let paginationQuery: PaginationQueryOptions;
	let totalPages: number;
	let paginationSettings: IPaginationData
	let props: { paginationSettings: IPaginationData, data: {id: string, title: string, description: string;}[]}

	beforeEach(() => {
		paginationQuery = new PaginationQueryOptions()
		runInAction(() => paginationQuery.perPage = 3);
		paginationSettings = { queryOptions: paginationQuery, totalRecords: data.length }
		props = {
			paginationSettings,
			data
		}
		totalPages = Math.ceil(data.length / paginationQuery.perPage);
	})

	it(`Should have the correct items displayed on the first page`, () => {
		const component = mount(<TestCollection {...props}/>);
		expect(component.find(".collection__item")).toHaveLength(paginationSettings.queryOptions.perPage);
	});
	it('Should have the correct number of items displayed on the second page', () => {
		const component = mount(<TestCollection {...props}/>);
		expect(component.find('span.pagination__page-number').text()).toEqual( `1 of ${totalPages}`);
		component.find('button.icon-chevron-right').simulate('click');
		component.setState({ forceUpdateCall: 1 });
		expect(component.find('span.pagination__page-number').text()).toEqual( `2 of ${totalPages}`);
		expect(component.find(".collection__item")).toHaveLength(paginationSettings.queryOptions.perPage);
	});
	it('Should end on the last page', () => {
		const component = mount(<TestCollection {...props}/>);

		// go to the last page
		for (let index = 0; index < totalPages; index++) {
			component.find('button.icon-chevron-right').simulate('click');
		}

		component.setState({ forceUpdateCall: 1 });
		expect(component.find('span.pagination__page-number').text()).toEqual( `${totalPages} of ${totalPages}`);
		expect(component.find(".collection__item")).toHaveLength(data.length % paginationSettings.queryOptions.perPage);
	});
	it('Should not be able to go before the first page', () => {
		const component = mount(<TestCollection {...props}/>);
		// start on page 1 and click three times
		component.find('button.icon-chevron-left').simulate('click');
		component.find('button.icon-chevron-left').simulate('click');
		component.find('button.icon-chevron-left').simulate('click');
		component.setState({ forceUpdateCall: 1 });
		expect(component.find('span.pagination__page-number').text()).toEqual( `1 of ${totalPages}`);
		expect(component.find(".collection__item")).toHaveLength(paginationSettings.queryOptions.perPage);
	});
	it('Should be able to go back pages', () => {
		const component = mount(<TestCollection {...props}/>);
		// start on page 1
		component.find('button.icon-chevron-right').simulate('click');
		component.find('button.icon-chevron-left').simulate('click');
		expect(component.find('span.pagination__page-number').text()).toEqual(`1 of ${totalPages}`);
		// should still be on on the last page
		component.setState({ forceUpdateCall: 1 });
		expect(component.find(".collection__item")).toHaveLength(paginationSettings.queryOptions.perPage);
	});
	it('Should be able to jump to the last page', () => {
		const component = mount(<TestCollection {...props}/>);
		component.find('button.icon-chevrons-right').simulate('click');
		expect(component.find('span.pagination__page-number').text()).toEqual(`${totalPages} of ${totalPages}`);
	});
	it('Should be able to jump to the first page', () => {
		const component = mount(<TestCollection {...props}/>);
		component.find('button.icon-chevrons-right').simulate('click');
		component.find('button.icon-chevrons-left').simulate('click');
		expect(component.find('span.pagination__page-number').text()).toEqual(`1 of ${totalPages}`);
	});
});
