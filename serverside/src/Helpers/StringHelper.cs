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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lm2348.Helpers
{
	public static class StringHelper
	{
		public static string LowerCaseFirst(this string input)
		{
			var newString = input;
			if (!string.IsNullOrEmpty(newString) && char.IsUpper(newString[0]))
			{
				newString = char.ToLower(newString[0]) + newString.Substring(1);
			}
			return newString;
		}

		public static string UpperCaseFirst(this string input)
		{
			var newString = input;
			if (!string.IsNullOrEmpty(newString) && char.IsLower(newString[0]))
			{
				newString = char.ToUpper(newString[0]) + newString.Substring(1);
			}
			return newString;
		}
	}
}
