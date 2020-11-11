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
import * as moment from 'moment'
import { CaseComparison } from '../Views/Components/ModelCollection/ModelQuery';
import _ from 'lodash';

export type transformFunction = (attr: any) => IStandardisedOption | null;
export interface IStandardisedOption {
	query: string;
	extraOptions?: {[option: string]: any};
}

/**
 * Converts a date string to a date string of format YYYY-MM-DD HH:mm:ss
 * @param attr The date string
 * @returns A date string in the format of YYYY-MM-DD HH:mm:ss or null if the provided date was not valid
 */
export function standardiseDate(attr: string): IStandardisedOption | null {
	const formats = [
		"DD-MM-YYYY 00:00:00",
		"DD-MM-YYYY HH:mm:ss",
		"DD/MM/YYYY 00:00:00",
		"DD/MM/YYYY HH:mm:ss",
		"YYYY-MM-DD 00:00:00",
		"YYYY-MM-DD HH:mm:ss",
		"YYYY/MM/DD 00:00:00",
		"YYYY/MM/DD HH:mm:ss",
		"MM-DD-YYYY 00:00:00",
		"MM-DD-YYYY HH:mm:ss",
		"MM/DD/YYYY 00:00:00",
		"MM/DD/YYYY HH:mm:ss",
	];
	const momentDate = moment(attr, formats);

	// Some invalid dates won't be marked invalid but just exist in year 0
	if (momentDate.isValid() && momentDate.year() !== 0) {
		const dateOnly = momentDate.hours() === 0
			&& momentDate.minutes() === 0
			&& momentDate.seconds() === 0;

		if (dateOnly) {
			return {
				query: momentDate.format('YYYY-MM-DD'),
			}
		}
		return {
			query: momentDate.format('YYYY-MM-DD HH:mm:ss'),
		}
	}
	return null;
}

/**
 * Determines if an input is an int for the purposes of search
 * @param attr The query string to check if it is an int
 */
export function standardiseInteger(attr: string): IStandardisedOption | null {
	const value = Number(attr);
	if (isNaN(value) || !Number.isInteger(value)) {
		return null;
	}

	const maxInt = 2147483647;
	const minInt = -2147483648;

	if (value > maxInt || value < minInt) {
		return null;
	}

	return {query: attr};
}

/**
 * Determines if an input is an float for the purposes of search
 * @param attr The query string to check if it is a float
 */
export function standardiseFloat(attr: string): IStandardisedOption | null {
	if (isNaN(Number(attr))) {
		return null;
	}
	return {query: attr};
}

/**
 * Determines if an input is an bool for the purposes of search
 * @param attr The query string to check if it is a bool
 */
export function standardiseBoolean(attr: string): IStandardisedOption | null {
	if (['true', 'false'].indexOf(attr) >= 0) {
		return {query: attr};
	}
	return null;
}

/**
 * Returns a search query for a string that is case insensitive
 * @param attr The string to search for
 */
export function standardiseString(attr: string): IStandardisedOption | null {
	return {
		query: `%${attr}%`,
		extraOptions: {
			case: 'INVARIANT_CULTURE_IGNORE_CASE'
		}
	}
}

/**
 * Returns a search query for a complete Uuid
 * @param attr The string to search for
 */
export function standardiseUuid(attr: string): IStandardisedOption | null {
	const regex = /^[0-9A-F]{8}-[0-9A-F]{4}-[4][0-9A-F]{3}-[89AB][0-9A-F]{3}-[0-9A-F]{12}$/i;
	if (!attr.match(regex)) {
		return null;
	}

	return {query: attr}
}

/**
 * Returns a search query for a string that is case insensitive
 * @param attr The string to search for
 */
export function standardiseEnum(attr: string, enumOptions: {} ): IStandardisedOption | null {
	const enumKey = _.invert(enumOptions)[attr];
	if (!enumKey) {
		return null;
	} else {
		return {
			query: enumKey
		}
	}
}