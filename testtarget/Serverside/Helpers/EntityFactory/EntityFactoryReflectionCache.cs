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
using Autofac;
using Lm2348.Models;

namespace ServersideTests.Helpers.EntityFactory
{
	public class TypeInfo
	{
		public string Name { get; set; }
		public Type Type { get; set; }
	}

	/// <summary>
	/// Reflection cache for the entity factory. This stored values generated from reflective calls to reduce overhead.
	/// Every function in this class caches is results to make subsequent calls faster
	/// </summary>
	public static class EntityFactoryReflectionCache
	{
		private static ConcurrentDictionary<Type, List<TypeInfo>> ReferenceCache { get; } = new ConcurrentDictionary<Type, List<TypeInfo>>();

		private static ConcurrentDictionary<Type, List<PropertyInfo>> AttributeCache { get; } = new ConcurrentDictionary<Type, List<PropertyInfo>>();

		private static ConcurrentDictionary<Type, List<PropertyInfo>> AllAttributeCache { get; } = new ConcurrentDictionary<Type, List<PropertyInfo>>();

		/// <summary>
		/// Gets the required non collection references for a type.
		/// </summary>
		/// <param name="entityType">The type of the entity to get the references from</param>
		/// <returns>A list of info on the types</returns>
		public static IEnumerable<TypeInfo> GetRequiredReferences(Type entityType)
		{
			if (ReferenceCache.TryGetValue(entityType, out var types))
			{
				return types;
			}

			var referenceTypes = entityType.GetProperties()
				.Where(p => p.GetCustomAttributes<EntityForeignKey>().Any(a => a.Required) &&
							p.PropertyType.IsAssignableTo<IAbstractModel>())
				.Select(p => new TypeInfo {Name = p.Name, Type = p.PropertyType})
				.ToList();

			ReferenceCache.TryAdd(entityType, referenceTypes);

			return referenceTypes;
		}

		/// <summary>
		/// Gets an attribute definition from a type.
		/// </summary>
		/// <param name="entityType">The type of the entity to get the property from</param>
		/// <param name="property">The property to get</param>
		/// <returns></returns>
		public static PropertyInfo GetAttribute(Type entityType, string property)
		{
			if (AttributeCache.TryGetValue(entityType, out var properties))
			{
				return properties.First(p => p.Name == property);
			}

			var propertyInfos = entityType.GetProperties();
			AttributeCache.TryAdd(entityType, propertyInfos.ToList());

			return propertyInfos.First(p => p.Name == property);
		}

		/// <summary>
		/// Gets all attributes for a type
		/// </summary>
		/// <param name="entityType">The type of the entity to get the attributes from</param>
		/// <returns>The list of attributes</returns>
		public static IEnumerable<PropertyInfo> GetAllAttributes(Type entityType)
		{
			if (AllAttributeCache.TryGetValue(entityType, out var properties))
			{
				return properties;
			}

			var attributeInfos = entityType.GetProperties()
				.Where(p => p.GetCustomAttributes<EntityAttribute>().Any())
				.ToList();
			AllAttributeCache.TryAdd(entityType, attributeInfos);
			
			return attributeInfos;
		}
	}
}