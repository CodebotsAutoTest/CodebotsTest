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
import {ToastOptions, toast, TypeOptions } from 'react-toastify';
import classNames from 'classnames';
import 'react-toastify/dist/ReactToastify.css';

/**
 * Show alert as a toast
 * @param errorMsg the text to be printed on the alert toast
 * @param type The type of the toast
 * @param options additional options such as 'autoClose' | 'position'
 */
export default function alert(errorMsg: React.ReactNode, type: TypeOptions = 'info', options: ToastOptions = {}) {
	toast(<p>{errorMsg}</p>, {
		className: classNames('alert', 'alert__' + type),
		type,
		...options,
	});
}