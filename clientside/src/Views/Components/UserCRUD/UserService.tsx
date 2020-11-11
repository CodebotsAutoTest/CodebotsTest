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
import axios from 'axios';
import User from 'Models/Entities/User';
import { SERVER_URL } from 'Constants';

export const getUsers = async (): Promise<User[]> => {
	const response = await axios.get(`${SERVER_URL}/api/account`);
	const usersJson = response.data as Array<{}>;
	return usersJson.map(userData => new User(userData));
};

export const getUser = async (id: string): Promise<User> => {
	const response = await axios.get(`${SERVER_URL}/api/account/${id}`);
	return new User({
		...response.data,
		groups: response.data.groups.map((g: any) => g.name)
	});
};

export const createUser = async (userJson: {}) => {
	const response = await axios.post(`${SERVER_URL}/api/account`, userJson);
	return response.data;
};

export const updateUser = async (userJson: {}) => {
	const response = await axios.put(`${SERVER_URL}/api/account`, userJson);
	return response.data;
};

export const deleteUser = async (id: string) => {
	return await axios.delete(`${SERVER_URL}/api/account?id=${id}`);
};