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
using Lm2348.Models;

namespace ServersideTests.Helpers.EntityFactory
{
	/// <summary>
	/// A factory for creating modeled entities to provide to the tests. This factory can create attributes and
	/// recursively create required references as well.
	/// </summary>
	/// <typeparam name="T">The type of entity to create</typeparam>
	public interface IEntityFactory<out T> where T : IAbstractModel
	{
		/// <summary>
		/// If trackEntities flag in the constructor is set to true then the generation step shall place all
		/// created entities into this IEnumerable.
		/// </summary>
		IEnumerable<IAbstractModel> GetAllEntities();

		/// <summary>
		/// Should attributes be created by the factory
		/// </summary>
		/// <param name="enabled">Weather to disable or enable attribute generation. Defaults to true</param>
		/// <returns>This entity factory</returns>
		IEntityFactory<T> UseAttributes(bool enabled = true);

		/// <summary>
		/// Should references be created by the factory
		/// </summary>
		/// <param name="enabled">Weather to disable or enable reference generation. Defaults to true</param>
		/// <returns>This entity factory</returns>
		IEntityFactory<T> UseReferences(bool enabled = true);

		/// <summary>
		/// Should an owner id be assigned to each created entity
		/// </summary>
		/// <param name="ownerId">The owner id to assign. Set this to null to disable ownership generation</param>
		/// <returns>This entity factory</returns>
		IEntityFactory<T> UseOwner(Guid? ownerId);

		/// <summary>
		/// Disable generation for the Id attribute
		/// </summary>
		/// <param name="disable">Should this be disabled. Defaults to true.</param>
		/// <returns>This entity factory</returns>
		IEntityFactory<T> DisableIdGeneration(bool disable = true);

		/// <summary>
		/// Disable generation for the Created attribute
		/// </summary>
		/// <param name="disable">Should this be disabled. Defaults to true.</param>
		/// <returns>This entity factory</returns>
		IEntityFactory<T> DisableCreatedGeneration(bool disable = true);

		/// <summary>
		/// Disable generation for the Modified attribute
		/// </summary>
		/// <param name="disable">Should this be disabled. Defaults to true.</param>
		/// <returns>This entity factory</returns>
		IEntityFactory<T> DisableModifiedGeneration(bool disable = true);

		/// <summary>
		/// Should all entities created by this factory be tracked
		/// </summary>
		/// <param name="enabled">Weather to enable or disable entity tracking. Defaults to true</param>
		/// <returns>This entity factory</returns>
		IEntityFactory<T> TrackEntities(bool enabled = true);

		/// <summary>
		/// Freezes the use of an entity to generate when creating references.
		/// This means that the factory will always use this entity for given type in generation.
		/// This will not work for the base list of entities.
		/// </summary>
		/// <param name="entity">The entity to use</param>
		/// <typeparam name="TE">The type of the entity to use</typeparam>
		/// <returns>This entity factory</returns>
		IEntityFactory<T> Freeze<TE>(TE entity)
			where TE : IAbstractModel;

		/// <summary>
		/// Freezes the use of an entity to generate when creating references.
		/// This means that the factory will always use this entity for given type in generation.
		/// This will not work for the base list of entities.
		/// </summary>
		/// <param name="type">The type of the entity to use</param>
		/// <param name="entity">The entity to use</param>
		/// <returns>This entity factory</returns>
		IEntityFactory<T> Freeze(Type type, IAbstractModel entity);

		/// <summary>
		/// Freezes the use of an attribute on an entity. This means that the factory will always use an attribute of
		/// this value when generating the entity.
		/// </summary>
		/// <param name="attributeName">The name of the attribute to freeze</param>
		/// <param name="value">The value of the attribute to freeze</param>
		/// <typeparam name="TE">The type of the entity to freeze the attribute on</typeparam>
		/// <returns>This entity factory</returns>
		/// <remarks>
		/// Note that the value param is typed as object and it is therefore the developer responsibility to ensure that
		/// it is of the correct type.
		/// </remarks>
		IEntityFactory<T> FreezeAttribute<TE>(string attributeName, object value);

		/// <summary>
		/// Freezes the use of an attribute on an entity. This means that the factory will always use an attribute of
		/// this value when generating the entity.
		/// </summary>
		/// <param name="type">The type of the entity to freeze the attribute on</param>
		/// <param name="attributeName">The name of the attribute to freeze</param>
		/// <param name="value">The value of the attribute to freeze</param>
		/// <returns>This entity factory</returns>
		/// <remarks>
		/// Note that the value param is typed as object and it is therefore the developer responsibility to ensure that
		/// it is of the correct type.
		/// </remarks>
		IEntityFactory<T> FreezeAttribute(Type type, string attributeName, object value);

		/// <summary>
		/// Creates a stream of entities.
		/// </summary>
		/// <remarks>
		/// The returned IEnumerable can only be looped over once. If multiple iteration is required then .ToList needs
		/// to be called.
		/// </remarks>
		/// <remarks>
		/// If totalEntites in the constructor was set to null then this will generate an infinite stream of entities.
		/// In this case the number of entities taken from this IEnumerable will need to be limited by a call to .Take.
		/// </remarks>
		/// <returns>The entities described by the factory configuration</returns>
		IEnumerable<T> Generate();
	}
}