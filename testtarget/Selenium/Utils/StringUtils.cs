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

using System.Linq;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace SeleniumTests.Utils
{
	public static class StringUtil
	{
		public static string Capitalize(this string field) => char.ToUpperInvariant(field[0]) + field.Substring(1);
		public static string Capitalize(this string[] sentence)=> sentence.Select(e => e.ToLower().Capitalize()).Aggregate("", (c, n) => $"{c} {n}").Trim();
		public static string RemoveWordsSpacing(this string field) => field.Replace(" ", "");

		public static IList<string> GetTableData(this Table table)
		{
			// return table values
			return table.Rows.Select(row => row.Select(cell => cell.Value).FirstOrDefault()).ToList();
		}
	}
}