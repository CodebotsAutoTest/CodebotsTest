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
using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;
using Lm2348.Models;
using Lm2348.Validators;

namespace ServersideTests.Helpers.EntityFactory
{
	public static class EntityFactoryUtils
	{
		/// <summary>
		/// Creates a new entity factory given a specific type
		/// </summary>
		/// <param name="type">The type of entity the factory is to create</param>
		/// <returns>The new entity factory</returns>
		public static IEntityFactory<IAbstractModel> Create(Type type)
		{
			var baseType = typeof(EntityFactory<>);
			var genericType = baseType.MakeGenericType(type);
			return (IEntityFactory<IAbstractModel>) Activator.CreateInstance(genericType);
		}
	}

	/// <summary>
	/// A factory for creating modeled entities to provide to the tests. This factory can create attributes and
	/// recursively create required references as well.
	/// </summary>
	/// <typeparam name="T">The type of entity to create</typeparam>
	public class EntityFactory<T> : IEntityFactory<T> where T : class, IAbstractModel, new()
	{
		private readonly Dictionary<Type, IAbstractModel> _frozenEntities = new Dictionary<Type, IAbstractModel>();
		private readonly Dictionary<Type, List<(string, object)>> _frozenAttributes = new Dictionary<Type, List<(string, object)>>();
		private readonly int? _totalEntities;
		private bool _trackEntities;
		private bool _useAttributes;
		private bool _useReferences;
		private bool _disableIdGeneration = false;
		private bool _disableCreatedGeneration = false;
		private bool _disableModifiedGeneration = false;
		private Guid? _ownerId;

		/// <summary>
		/// If trackEntities flag in the constructor is set to
		/// </summary>
		protected EntityEnumerable<T> EntityEnumerable { get; } = new EntityEnumerable<T>();

		/// <summary>
		/// Creates an entity factory to create modelled entities
		/// </summary>
		/// <param name="totalEntities">
		/// The total number of entities to create. If this value is null then it will create an infinite stream.
		/// </param>
		/// <exception cref="ArgumentException">
		/// If the totalEntities value is less than 1
		/// </exception>
		public EntityFactory(int? totalEntities = 1)
		{
			if (totalEntities <= 0)
			{
				throw new ArgumentException("Total entities cannot be less than one");
			}
			_totalEntities = totalEntities;
		}

		/// <summary>
		/// Creates an entity factory to create an infinite stream of modelled entities.
		/// </summary>
		public EntityFactory()
		{
			_totalEntities = 1;
		}

		/// <inheritdoc />
		public IEnumerable<IAbstractModel> GetAllEntities()
		{
			return EntityEnumerable.AllEntities;
		}

		/// <inheritdoc />
		public IEntityFactory<T> UseAttributes(bool enabled = true)
		{
			_useAttributes = enabled;
			return this;
		}

		/// <inheritdoc />
		public IEntityFactory<T> UseReferences(bool enabled = true)
		{
			_useReferences = enabled;
			return this;
		}

		/// <inheritdoc />
		public IEntityFactory<T> UseOwner(Guid? ownerId)
		{
			_ownerId = ownerId;
			return this;
		}

		/// <inheritdoc />
		public IEntityFactory<T> DisableIdGeneration(bool disable = true)
		{
			_disableIdGeneration = disable;
			return this;
		}

		/// <inheritdoc />
		public IEntityFactory<T> DisableCreatedGeneration(bool disable = true)
		{
			_disableCreatedGeneration = disable;
			return this;
		}

		/// <inheritdoc />
		public IEntityFactory<T> DisableModifiedGeneration(bool disable = true)
		{
			_disableModifiedGeneration = disable;
			return this;
		}

		/// <inheritdoc />
		public IEntityFactory<T> TrackEntities(bool enabled = true)
		{
			_trackEntities = enabled;
			return this;
		}

		/// <inheritdoc />
		public IEntityFactory<T> Freeze<TE>(TE entity)
			where TE : IAbstractModel
		{
			return Freeze(typeof(TE), entity);
		}

		/// <inheritdoc />
		public IEntityFactory<T> Freeze(Type type, IAbstractModel entity)
		{
			_frozenEntities.Add(type, entity);
			return this;
		}

		/// <inheritdoc />
		public IEntityFactory<T> FreezeAttribute<TE>(string attributeName, object value)
		{
			return FreezeAttribute(typeof(TE), attributeName, value);
		}

		/// <inheritdoc />
		public IEntityFactory<T> FreezeAttribute(Type type, string attributeName, object value)
		{
			if (_frozenAttributes.TryGetValue(type, out var frozenAttrs))
			{
				frozenAttrs.Add((attributeName, value));
			}
			else
			{
				_frozenAttributes[type] = new List<(string, object)>
				{
					(attributeName, value)
				};
			}

			return this;
		}

		/// <inheritdoc />
		public IEnumerable<T> Generate()
		{
			if (_totalEntities.HasValue)
			{
				for (var i = 0; i < _totalEntities.Value; i++)
				{
					yield return GenerateEntity();
				}
			}
			else
			{
				while (true)
				{
					yield return GenerateEntity();
				}
			}
		}

		/// <summary>
		/// Generates the individual entities
		/// </summary>
		/// <returns>A new entity</returns>
		protected T GenerateEntity()
		{
			var entity = new T();

			if (_trackEntities)
			{
				EntityEnumerable.AllEntities.Add(entity);
			}

			if (_useAttributes)
			{
				AddAttribute(entity, DateTime.Now, DateTime.Now);
			}

			if (_ownerId.HasValue)
			{
				AddOwnerToModel(entity);
			}

			if (_useReferences)
			{
				CreateAndAddReferences(entity);
			}

			return entity;
		}

		/// <summary>
		/// Adds attributes to an entity
		/// </summary>
		/// <param name="entity">The entity to add attributes to</param>
		/// <param name="created">The created date to add</param>
		/// <param name="modified">The modified date to add</param>
		/// <param name="basePropertiesOnly">Should only common model properties be added</param>
		protected void AddAttribute(
			IAbstractModel entity,
			DateTime? created = null,
			DateTime? modified = null,
			bool basePropertiesOnly = false)
		{
			if (!_disableIdGeneration)
			{
				entity.Id = Guid.NewGuid();
			}

			if (!_disableCreatedGeneration)
			{
				entity.Created = created ?? DateTime.Now;
			}

			if (!_disableModifiedGeneration)
			{
				entity.Modified = modified ?? DateTime.Now;
			}

			var fixture = new Fixture();
			var context = new SpecimenContext(fixture);

			if (basePropertiesOnly)
			{
				return;
			}

			foreach (var attr in EntityFactoryReflectionCache.GetAllAttributes(entity.GetType()))
			{
				var valueSet = false;
				if (_frozenAttributes.TryGetValue(entity.GetType(), out var frozenAttrs))
				{
					var frozenValue = frozenAttrs.FirstOrDefault(x => x.Item1 == attr.Name);
					if (frozenValue != default)
					{
						attr.SetValue(entity, frozenValue.Item2);
						valueSet = true;
					}
				}

				if (!valueSet)
				{
					var customAttributes = attr.GetCustomAttributes().ToList();
					if (customAttributes.Contains(new EmailAttribute()))
					{
						attr.SetValue(entity, TestDataLib.DataUtils.RandEmail());
					}
					else
					{
						attr.SetValue(entity, fixture.Create(attr.PropertyType, context));
					}
				}
			}
		}

		/// <summary>
		/// Adds an owner to a model
		/// </summary>
		/// <param name="entity">The entity to add the owner id to</param>
		protected void AddOwnerToModel(IAbstractModel entity)
		{
			if (entity is IOwnerAbstractModel ownerEntity && _ownerId.HasValue)
			{
				ownerEntity.Owner = _ownerId.Value;
			}
		}

		/// <summary>
		/// Creates an adds any required non collection references to an entity.
		/// </summary>
		/// <param name="entity">The entity to add references to</param>
		/// <param name="visited">
		/// A list of already visited entities. This is used for recursive reference generation and not needed for the
		/// initial call of the function.
		/// </param>
		protected void CreateAndAddReferences(
			IAbstractModel entity,
			List<(Type, string)> visited = null)
		{
			var references = EntityFactoryReflectionCache.GetRequiredReferences(entity.GetType());
			var entityType = entity.GetType();

			if (visited == null)
			{
				visited = new List<(Type, string)>();
			}

			foreach (var reference in references)
			{
				// Detect any loops in the relation list and break if any are found
				if (visited.Contains((entityType, reference.Name)))
				{
					continue;
				}

				if (!_frozenEntities.TryGetValue(reference.Type, out var referenceEntity))
				{
					// Create foreign references and assign them attributes
					referenceEntity = (IAbstractModel)Activator.CreateInstance(reference.Type);
					if (_useAttributes)
					{
						AddAttribute(referenceEntity);
					}

					if (_ownerId.HasValue)
					{
						AddOwnerToModel(referenceEntity);
					}

					// Add the reference to the entity
					EntityFactoryReflectionCache.GetAttribute(entityType, reference.Name)
						.SetValue(entity, referenceEntity);

					// Try to add the reference id to the entity
					try
					{
						EntityFactoryReflectionCache.GetAttribute(entityType, reference.Name + "Id")
							.SetValue(entity, referenceEntity.Id);
					}
					catch
					{
						// Ignore if the Id cannot be set
					}
				}

				if (_trackEntities)
				{
					EntityEnumerable.AllEntities.Add(referenceEntity);
				}

				visited.Add((entityType, reference.Name));

				// Recursively create references for those references
				CreateAndAddReferences(referenceEntity, visited);
			}
		}
	}
}