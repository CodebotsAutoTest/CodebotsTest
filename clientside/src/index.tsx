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
import 'graphiql/graphiql.css';
import 'react-toastify/dist/ReactToastify.css';
import 'react-contexify/dist/ReactContexify.min.css';
import 'semantic-ui-css/semantic.min.css';
import './scss/main.scss';
import * as React from 'react';
import * as ReactDOM from 'react-dom';
import App from './App';
import { store } from './Models/Store';
import * as Models from 'Models/Entities'
// % protected region % [Add any extra index imports here] off begin
// % protected region % [Add any extra index imports here] end

// Add the store to the global scope for easy debugging from the console
window['store'] = store;
window['Models'] = Models;

// % protected region % [Add any extra index fields here] off begin
// % protected region % [Add any extra index fields here] end

// % protected region % [Override render here] off begin
ReactDOM.render(<App />, document.getElementById('root') as HTMLElement);
// % protected region % [Override render here] end
