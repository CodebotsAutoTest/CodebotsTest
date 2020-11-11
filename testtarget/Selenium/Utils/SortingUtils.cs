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
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace SeleniumTests.Utils
{
	internal static class SortingUtils
	{
		private const string NONE = "None";
		private const string ASCENDING = "ascending";
		private const string DESCENDING = "descending";

		// Could have used enum but list is preferred since "contains" is used for string comparison
		private static readonly List<string> intDataTypes = new List<string> { "int" };
		private static readonly List<string> dateTimeTypes = new List<string> { "Date", "Time", "DateTime" };

		public static List<string> AssertSorted(CultureInfo cultureInfo, string attributeType, List<string> sortingList, string sortOrder)
		{
			 var sortedList = new List<string>();
			// Get all Empty strings or "None" items
			var sortedWithNones = sortingList.Where(s => s == NONE).ToList();

			// Apply apply sorting scenario based on sorting order
			if (dateTimeTypes.Contains(attributeType))
			{
				switch (sortOrder)
				{
					case ASCENDING:
						sortedList = SortDateTimeAscending(sortingList,cultureInfo);
						break;
					case DESCENDING:
						sortedList = SortDateTimeDescending(sortingList, cultureInfo);
						break;
					default:
						throw new Exception($"Could not find {sortOrder} type");
				}
			}
			else if (intDataTypes.Contains(attributeType))
			{
				switch(sortOrder)
				{
					case ASCENDING:
					sortedList = SortIntegerAscending(sortingList);
					break;
					case DESCENDING:
					sortedList = SortIntegerDescending(sortingList);
					break;
					default:
						throw new Exception($"Could not find {sortOrder} type");
				}
			}
			else
			{
				switch (sortOrder)
				{
					case ASCENDING:
						sortedList = SortStringAscending(sortingList);
						break;
					case DESCENDING:
						sortedList = SortStringDescending(sortingList);
						break;
					default:
						throw new Exception($"Could not find {sortOrder} type");
				}
			}

			sortedWithNones.AddRange(sortedList);
			return sortedWithNones;
		}

		/// <summary>
		/// Filter out all non-alphanumeric contents. Return Date, Time, or DateTime as is
		/// </summary>
		/// <param name="attributeType"></param>
		/// <param name="originalList"></param>
		/// <param name="sortOrder"></param>
		/// <returns></returns>
		public static List<string> FilterOutNonAlphanumeric(string attributeType, List<string> originalList)
		{
			// Regex pattern for all alphanumeric character including "\"
			var rg = new Regex(@"^[a-zA-Z0-9\s,/\\]*$");

			return dateTimeTypes.Contains(attributeType) ? originalList : originalList.Where(word => rg.IsMatch(word) && !string.IsNullOrEmpty(word)).ToList();
		}

		private static List<string> SortStringAscending(List<string> strList)
		{
			return strList.Where(s => s != NONE).Select(x => x).OrderBy(x => x).ToList();
		}

		private static List<string> SortStringDescending(List<string> strList)
		{
			return strList.Where(s => s != NONE).Select(x => x).OrderByDescending(x => x).ToList();
		}

		private static List<string> SortDateTimeAscending(List<string> dtList, CultureInfo cultureInfo)
		{
			return dtList.Where(x => x != NONE).OrderBy(x => Convert.ToDateTime(x, cultureInfo)).ToList();
		}

		private static List<string> SortDateTimeDescending(List<string> dtList, CultureInfo cultureInfo)
		{
			return dtList.Where(x => x != NONE).OrderByDescending(x => Convert.ToDateTime(x, cultureInfo)).ToList();
		}

		private static List<string> SortIntegerAscending(List<string> dtList)
		{
			return dtList.Where(s => s != NONE).Select(x => x).OrderBy(x => int.Parse(x)).ToList();
		}

		private static List<string> SortIntegerDescending(List<string> dtList)
		{
			return dtList.Where(s => s != NONE).Select(x => x).OrderByDescending(x => int.Parse(x)).ToList();
		}
	}
}