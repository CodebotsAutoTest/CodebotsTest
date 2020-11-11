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
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Lm2348.Services.Files
{
	/// <summary>
	/// Generic storage provider interface for the application. This interface is used to interact with any type of
	/// runtime file storage interaction such as image upload.
	/// </summary>
	public interface IUploadStorageProvider : IDisposable
	{
		/// <summary>
		/// Gets a file from the storage provider
		/// </summary>
		/// <param name="options">The options for the operation</param>
		/// <param name="cancellationToken">Cancellation token for the operation</param>
		/// <returns></returns>
		Task<Stream> GetAsync(StorageGetOptions options, CancellationToken cancellationToken = default);

		/// <summary>
		/// Lists the files in a container
		/// </summary>
		/// <param name="options">The options for the operation</param>
		/// <param name="cancellationToken">Cancellation token for the operation</param>
		/// <returns>A list of file names</returns>
		Task<IEnumerable<string>> ListAsync(StorageListOptions options, CancellationToken cancellationToken = default);

		/// <summary>
		/// Checks weather a file exists
		/// </summary>
		/// <param name="options">The options for the operation</param>
		/// <param name="cancellationToken">Cancellation token for the operation</param>
		/// <returns>True if the file exists, otherwise false</returns>
		Task<bool> ExistsAsync(StorageExistsOptions options, CancellationToken cancellationToken = default);

		/// <summary>
		/// Saves a file to a container in the storage provider
		/// </summary>
		/// <param name="options">The options for the operation</param>
		/// <param name="cancellationToken">Cancellation token for the operation</param>
		/// <returns>A task that completes when the file is saved</returns>
		/// <throws cref="IOException">
		/// If overwrite is set to false and the file already exists
		/// </throws>
		Task PutAsync(StoragePutOptions options, CancellationToken cancellationToken = default);

		/// <summary>
		/// Deletes a file from the storage provider
		/// </summary>
		/// <param name="options">The options for the operation</param>
		/// <param name="cancellationToken">Cancellation token for the operation</param>
		/// <returns>A task that completes when the file is deleted</returns>
		Task DeleteAsync(StorageDeleteOptions options, CancellationToken cancellationToken = default);

		/// <summary>
		/// Checks if a container exists
		/// </summary>
		/// <param name="options">The options for the operation</param>
		/// <param name="cancellationToken">Cancellation token for the operation</param>
		/// <returns>True if the container exists, false otherwise</returns>
		/// <remarks>
		/// Some file providers such as key value stores do not implement containers as real structures but instead as
		/// namespaced file names. In these cases this call may check if there are any files that exist in the
		/// namespaced container instead.
		/// </remarks>
		Task<bool> ContainerExistsAsync(StorageContainerExistsOptions options, CancellationToken cancellationToken = default);

		/// <summary>
		/// Creates a container.
		/// </summary>
		/// <param name="options">The options for the operation</param>
		/// <param name="cancellationToken">Cancellation token for the operation</param>
		/// <returns>A task that completes when the container is created</returns>
		/// <remarks>
		/// Some file providers such as key value stores do not implement containers as real structures but instead as
		/// namespaced file names. In these cases this call can potentially be a noop.
		/// </remarks>
		Task CreateContainerAsync(StorageCreateContainerOptions options, CancellationToken cancellationToken = default);

		/// <summary>
		///
		/// </summary>
		/// <param name="options">The options for the operation</param>
		/// <param name="cancellationToken">Cancellation token for the operation</param>
		/// <returns>A task that completes when the container is deleted</returns>
		/// <remarks>
		/// Some file providers such as key value stores do not implement containers as real structures but instead as
		/// namespaced file names. In these cases this call may simply delete all the namespaced files.
		/// </remarks>
		Task DeleteContainerAsync(StorageDeleteContainerOptions options, CancellationToken cancellationToken = default);

		/// <summary>
		/// A provider specific implementation of the main GET endpoint of the file controller.
		/// </summary>
		/// <param name="options">The options for the operation</param>
		/// <returns>A function that resolves to an IActionResult or null to use the default implementation.</returns>
		/// <remarks>
		/// If this function returns null then the controller will fall back to the logic of reading the file using
		/// GetAsync and then writing the file to the response using a FileStreamResult.
		/// </remarks>
		Func<CancellationToken, Task<IActionResult>> OnFetch(StorageOnFetchOptions options);

		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	}
}