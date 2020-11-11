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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lm2348.Models;
using Microsoft.EntityFrameworkCore;

namespace Lm2348.Utility
{
	public static class ReflectionCache
	{
		public static MethodInfo ILikeMethod = typeof(NpgsqlDbFunctionsExtensions)
			.GetMethod("ILike", new [] {typeof(DbFunctions), typeof(string), typeof(string)});

		private static ConcurrentDictionary<Type, List<PropertyInfo>> FileAttributeCache { get; } =
			new ConcurrentDictionary<Type, List<PropertyInfo>>();

		/// <summary>
		/// Gets all the file attributes for this type. The values for this are cached in a static map for fast
		/// repeated lookups.
		/// </summary>
		/// <param name="entityType">The type to get the file attributes from</param>
		/// <returns>A list of property info representing the file attributes</returns>
		public static List<PropertyInfo> GetFileAttributes(Type entityType)
		{
			if (FileAttributeCache.TryGetValue(entityType, out var properties))
			{
				return properties;
			}

			var attributeInfos = entityType.GetProperties()
				.Where(p => p.GetCustomAttributes<FileReference>().Any())
				.ToList();
			FileAttributeCache.TryAdd(entityType, attributeInfos);

			return attributeInfos;
		}
	}
}
