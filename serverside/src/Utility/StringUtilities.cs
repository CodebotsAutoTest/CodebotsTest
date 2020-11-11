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

namespace Lm2348.Utility
{
	public static class StringUtilities
	{
		/// <summary>
		/// Method for converting field value to camel Case
		/// </summary>
		/// <param name="field"></param>
		/// <returns></returns>
		public static string ConvertToCamelCase(this string field) => char.ToLowerInvariant(field[0]) + field.Substring(1);

		/// <summary>
		/// Method for converting field value to Pascal Case
		/// </summary>
		/// <param name="field"></param>
		/// <returns></returns>
		public static string ConvertToPascalCase(this string field) => char.ToUpperInvariant(field[0]) + field.Substring(1);

		// % protected region % [Add extra StringUtilities methods here] off begin
		// % protected region % [Add extra StringUtilities methods here] end
	}
}
