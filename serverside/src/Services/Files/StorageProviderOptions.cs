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
using System.IO;
using Lm2348.Models;
using Microsoft.AspNetCore.Http;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Lm2348.Services.Files
{
	/// <summary>
	/// The options for the GetAsync storage provider function
	/// </summary>
	public class StorageGetOptions
	{
		/// <summary>
		/// The container the file is in
		/// </summary>
		public string Container { get; set; }

		/// <summary>
		/// The name of the file
		/// </summary>
		public string FileName { get; set; }

		// % protected region % [Add any extra StorageGetOptions properties here] off begin
		// % protected region % [Add any extra StorageGetOptions properties here] end
	}

	/// <summary>
	/// The options for the ListAsync storage provider function
	/// </summary>
	public class StorageListOptions
	{
		/// <summary>
		/// The container to list the files in
		/// </summary>
		public string Container { get; set; }

		// % protected region % [Add any extra StorageListOptions properties here] off begin
		// % protected region % [Add any extra StorageListOptions properties here] end
	}

	/// <summary>
	/// The options for the ExistsAsync storage provider function
	/// </summary>
	public class StorageExistsOptions
	{
		/// <summary>
		/// The container the file is in
		/// </summary>
		public string Container { get; set; }

		/// <summary>
		/// The name of the file
		/// </summary>
		public string FileName { get; set; }

		// % protected region % [Add any extra StorageExistsOptions properties here] off begin
		// % protected region % [Add any extra StorageExistsOptions properties here] end
	}

	/// <summary>
	/// The options for the PutAsync storage provider function
	/// </summary>
	public class StoragePutOptions
	{
		/// <summary>
		/// The container to save the file to
		/// </summary>
		public string Container { get; set; }

		/// <summary>
		/// The name to save the file as
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		/// The stream of data to save
		/// </summary>
		public Stream Content { get; set; }

		/// <summary>
		/// Optional. If the file already exists should it be overwritten
		/// </summary>
		public bool Overwrite { get; set; } = true;

		/// <summary>
		/// Optional. The content type of the file
		/// </summary>
		public string ContentType { get; set; } = null;

		/// <summary>
		/// If putting the object into a container that does not exist should it be created automatically
		/// </summary>
		/// <remarks>
		/// This option only has meaning when the underlying storage provider implements physical containers. In the
		/// case of a key value store provider with only namespaced containers the container might be 'created'
		/// regardless of this option.
		/// </remarks>
		public bool CreateContainerIfNotExists { get; set; } = true;

		// % protected region % [Add any extra StoragePutOptions properties here] off begin
		// % protected region % [Add any extra StoragePutOptions properties here] end
	}

	/// <summary>
	/// The options for the DeleteAsync storage provider function
	/// </summary>
	public class StorageDeleteOptions
	{
		/// <summary>
		/// The container the file is in
		/// </summary>
		public string Container { get; set; }

		/// <summary>
		/// The name of the file
		/// </summary>
		public string FileName { get; set; }

		// % protected region % [Add any extra StorageDeleteOptions properties here] off begin
		// % protected region % [Add any extra StorageDeleteOptions properties here] end
	}

	/// <summary>
	/// The options for the ContainerExistsAsync storage provider function
	/// </summary>
	public class StorageContainerExistsOptions
	{
		/// <summary>
		/// The name of the container to check
		/// </summary>
		public string Container { get; set; }

		// % protected region % [Add any extra StorageContainerExistsOptions properties here] off begin
		// % protected region % [Add any extra StorageContainerExistsOptions properties here] end
	}

	/// <summary>
	/// The options for the CreateContainerAsync storage provider function
	/// </summary>
	public class StorageCreateContainerOptions
	{
		/// <summary>
		/// The container to create
		/// </summary>
		public string Container { get; set; }

		// % protected region % [Add any extra StorageCreateContainerOptions properties here] off begin
		// % protected region % [Add any extra StorageCreateContainerOptions properties here] end
	}

	/// <summary>
	/// The options for the DeleteContainerAsync storage provider function
	/// </summary>
	public class StorageDeleteContainerOptions
	{
		/// <summary>
		/// The name of the container to delete
		/// </summary>
		public string Container { get; set; }

		// % protected region % [Add any extra StorageDeleteContainerOptions properties here] off begin
		// % protected region % [Add any extra StorageDeleteContainerOptions properties here] end
	}

	/// <summary>
	/// The options for the OnFetch storage provider function
	/// </summary>
	public class StorageOnFetchOptions
	{
		/// <summary>
		/// The file to fetch
		/// </summary>
		public UploadFile File { get; set; }

		/// <summary>
		/// Should the file have download headers
		/// </summary>
		public bool Download { get; set; }

		/// <summary>
		/// The HttpContext of the request
		/// </summary>
		public HttpContext HttpContext { get; set; }

		// % protected region % [Add any extra StorageOnFetchOptions properties here] off begin
		// % protected region % [Add any extra StorageOnFetchOptions properties here] end
	}

	// % protected region % [Add any extra classes here] off begin
	// % protected region % [Add any extra classes here] end
}