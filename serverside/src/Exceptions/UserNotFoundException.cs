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
using System;

namespace Lm2348.Exceptions
{
	public class UserNotFoundException : Exception
	{
		public UserNotFoundException() : base("The user was not found")
		{
			// % protected region % [Add any extra extra constructor 1 options here] off begin
			// % protected region % [Add any extra extra constructor 1 options here] end
		}

		public UserNotFoundException(string message) : base(message)
		{
			// % protected region % [Add any extra extra constructor 2 options here] off begin
			// % protected region % [Add any extra extra constructor 2 options here] end
		}

		public UserNotFoundException(string message, Exception innerException) : base(message, innerException)
		{
			// % protected region % [Add any extra extra constructor 3 options here] off begin
			// % protected region % [Add any extra extra constructor 3 options here] end
		}

		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	}
}