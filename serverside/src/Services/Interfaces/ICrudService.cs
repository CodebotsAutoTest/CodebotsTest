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
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Lm2348.Graphql.Types;
using Lm2348.Models;
using Lm2348.Models.RegistrationModels;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Lm2348.Services.Interfaces
{
	public interface ICrudService
	{
		/// <summary>
		/// Gets a list of models. Can be paginated
		/// </summary>
		/// <typeparam name="T">The type of the model</typeparam>
		/// <param name="pagination">The pagination options for the query</param>
		/// <param name="auditFields">Extra data that shall be saved to the audit log</param>
		/// <returns>A graphql execution result</returns>
		IQueryable<T> Get<T>(
			Pagination pagination = null,
			object auditFields = null)
			where T : class, IOwnerAbstractModel, new();

		/// <summary>
		/// Gets a single model by an id
		/// </summary>
		/// <typeparam name="T">The type of the model</typeparam>
		/// <param name="id">The id to fetch by</param>
		/// <returns>A graphql execution result</returns>
		IQueryable<T> GetById<T>(Guid id) where T : class, IOwnerAbstractModel, new();

		/// <summary>
		/// Creates a model
		/// </summary>
		/// <typeparam name="T">The type of the model</typeparam>
		/// <param name="model">The model to be created</param>
		/// <param name="options">Any extra option to pass to create</param>
		/// <param name="cancellation">The cancellation token for this operation</param>
		/// <returns>A graphql execution result</returns>
		Task<T> Create<T>(
			T model,
			UpdateOptions options = null,
			CancellationToken cancellation = default)
			where T : class, IOwnerAbstractModel, new();
		
		/// <summary>
		/// Creates a model
		/// </summary>
		/// <typeparam name="T">The type of the model</typeparam>
		/// <param name="models">The model to be created</param>
		/// <param name="options">Any extra option to pass to create</param>
		/// <param name="cancellation">The cancellation token for this operation</param>
		/// <returns>A graphql execution result</returns>
		Task<ICollection<T>> Create<T>(
			ICollection<T> models,
			UpdateOptions options = null,
			CancellationToken cancellation = default)
			where T : class, IOwnerAbstractModel, new();

		/// <summary>
		/// Updates a model
		/// </summary>
		/// <typeparam name="T">The type of the model</typeparam>
		/// <param name="model">The model to update</param>
		/// <param name="options">Extra options to be passed in for the update</param>
		/// <param name="cancellation">The cancellation token for this operation</param>
		/// <returns>The model after it has been updated</returns>
		Task<T> Update<T>(
			T model,
			UpdateOptions options = null,
			CancellationToken cancellation = default)
			where T : class, IOwnerAbstractModel, new();

		/// <summary>
		/// Updates a model
		/// </summary>
		/// <typeparam name="T">The type of the model</typeparam>
		/// <param name="models">The model to update</param>
		/// <param name="options">Extra options to be passed in for the update</param>
		/// <param name="cancellation">The cancellation token for this operation</param>
		/// <returns>The model after it has been updated</returns>
		Task<ICollection<T>> Update<T>(
			ICollection<T> models,
			UpdateOptions options = null,
			CancellationToken cancellation = default)
			where T : class, IOwnerAbstractModel, new();

		/// <summary>
		/// Deletes a model
		/// </summary>
		/// <typeparam name="T">The type of the model</typeparam>
		/// <param name="id">The id of the model to be deleted</param>
		/// <param name="cancellation">The cancellation token for this operation</param>
		/// <returns>A graphql execution result</returns>
		Task<Guid> Delete<T>(Guid id, CancellationToken cancellation = default)
			where T : class, IAbstractModel;

		/// <summary>
		/// Deletes a model
		/// </summary>
		/// <typeparam name="T">The type of the model</typeparam>
		/// <param name="ids">The id of the model to be deleted</param>
		/// <param name="cancellation">The cancellation token for this operation</param>
		/// <returns>A graphql execution result</returns>
		Task<ICollection<Guid>> Delete<T>(List<Guid> ids, CancellationToken cancellation = default)
			where T : class, IAbstractModel;

		/// <summary>
		/// Updates models conditionally in batch
		/// </summary>
		/// <typeparam name="T">The type of the model</typeparam>
		/// <param name="models">The IQueryable to find the models to be queried</param>
		/// <param name="updateMemberInitExpression">The update expression</param>
		/// <param name="cancellation">The cancellation token for this operation</param>
		/// <returns>A graphql execution result</returns>
		Task<BooleanObject> ConditionalUpdate<T>(
			IQueryable<T> models,
			MemberInitExpression updateMemberInitExpression,
			CancellationToken cancellation = default)
			where T : class, IOwnerAbstractModel, new();

		/// <summary>
		/// Deletes models conditionally in batch
		/// </summary>
		/// <typeparam name="T">The type of the model</typeparam>
		/// <param name="models">The IQueryable to find the models to be deleted</param>
		/// <param name="cancellation">The cancellation token for this operation</param>
		/// <returns>A graphql execution result</returns>
		Task<BooleanObject> ConditionalDelete<T>(
			IQueryable<T> models,
			CancellationToken cancellation = default)
			where T : class, IOwnerAbstractModel, new();

		/// <summary>
		/// Creates a user entity
		/// </summary>
		/// <param name="models">The registration models for the users to create</param>
		/// <param name="options">Any extra option to pass to create</param>
		/// <param name="cancellation">The cancellation token for this operation</param>
		/// <typeparam name="TModel">The type of the user to create</typeparam>
		/// <typeparam name="TRegisterModel">The registration type of the user to create</typeparam>
		/// <returns>A list of created users</returns>
		/// <exception cref="AggregateException">On Validation or registration error</exception>
		Task<ICollection<User>> CreateUser<TModel, TRegisterModel>(
			ICollection<TRegisterModel> models,
			UpdateOptions options = null,
			CancellationToken cancellation = default)
			where TModel : User, IOwnerAbstractModel, new()
			where TRegisterModel : IRegistrationModel<TModel>;

		/// <summary>
		/// Gets a file from a storage provider, respecting security
		/// </summary>
		/// <param name="id">The id of the file to retrieve</param>
		/// <param name="cancellation">The cancellation token for this operation</param>
		/// <returns>A task that evaluates to a file</returns>
		/// <exception cref="FileNotFoundException">If the file could not be found</exception>
		/// <exception cref="UnauthorizedAccessException">If the user does not have access to view the file</exception>
		Task<UploadFile> GetFile(Guid id, CancellationToken cancellation = default);

		// % protected region % [Add extra ICrudServide methods here] off begin
		// % protected region % [Add extra ICrudServide methods here] end
	}
}