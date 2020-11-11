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

namespace Lm2348.Helpers
{
	public static class CollectionExtensions
	{
		public static void AddRange<T>(this ICollection<T> target, IEnumerable<T> items)
		{
			foreach (var item in items)
			{
				target.Add(item);
			}
		}

		/// <summary>
		/// Returns the item in a collection that has the maximum value for a given property.
		/// </summary>
		/// <param name="target">The collection to search</param>
		/// <param name="accessor">Function to access the property to use for comparison</param>
		/// <param name="defaultValue">The default value to return if no matching item is found</param>
		/// <typeparam name="T">The type of the collection</typeparam>
		/// <typeparam name="TR">The type of the property</typeparam>
		/// <returns>An object from the collection or defaultValue</returns>
		public static T MaxByOrDefault<T, TR>(this IEnumerable<T> target, Func<T, TR> accessor, T defaultValue = default)
			where TR : IComparable<TR> 
			=> CompareByOrDefault(target, accessor, (x, y) => x.CompareTo(y) < 0, defaultValue);
		
		/// <summary>
		/// Returns the item in a collection that has the minimum value for a given property.
		/// </summary>
		/// <param name="target">The collection to search</param>
		/// <param name="accessor">Function to access the property to use for comparison</param>
		/// <param name="defaultValue">The default value to return if no matching item is found</param>
		/// <typeparam name="T">The type of the collection</typeparam>
		/// <typeparam name="TR">The type of the property</typeparam>
		/// <returns>An object from the collection or defaultValue</returns>
		public static T MinByOrDefault<T, TR>(this IEnumerable<T> target, Func<T, TR> accessor, T defaultValue = default)
			where TR : IComparable<TR> 
			=> CompareByOrDefault(target, accessor, (x, y) => x.CompareTo(y) > 0, defaultValue);

		/// <summary>
		/// Compares each item in a collection using a comparer and returns the item with the most extreme value
		/// </summary>
		/// <param name="target">The collection to search</param>
		/// <param name="accessor">Function to access the property to use for comparison</param>
		/// <param name="comparer">The comparing function used to find the extreme value</param>
		/// <param name="defaultValue">The default value to return if no matching item is found</param>
		/// <typeparam name="T">The type of the collection</typeparam>
		/// <typeparam name="TR">The type of the property</typeparam>
		/// <returns>An object from the collection or defaultValue</returns>
		private static T CompareByOrDefault<T, TR>(this IEnumerable<T> target, Func<T, TR> accessor, Func<TR, TR, bool> comparer, T defaultValue)
			where TR : IComparable<TR>
		{
			var first = true;
			T minValue = default;
			foreach (var value in target)
			{
				if (first)
				{
					minValue = value;
					first = false;
				}
				else
				{
					var x = accessor(value);
					if (comparer(accessor(minValue), x))
					{
						minValue = value;
					}
				}
			}
			return first ? defaultValue : minValue;
		}
	}
}