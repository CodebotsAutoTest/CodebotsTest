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
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Lm2348.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Lm2348.Services.Files.Providers
{
	/// <summary>
	/// An implementation of the generic storage provider that saves files onto the file system that is running the
	/// application. This provider is mainly recommended for testing purposes, or if there is a file server attached
	/// to the application server that is running the application
	/// </summary>
	/// <see cref="FileSystemStorageProviderConfiguration">
	/// The appsettings configuration for the file system provider
	/// </see>
	public class FileSystemStorageProvider : IUploadStorageProvider
	{
		private readonly ILogger<FileSystemStorageProvider> _logger;
		private readonly FileSystemStorageProviderConfiguration _configuration;

		/// <summary>
		/// The root folder that the files are stored in
		/// </summary>
		private string RootFolder => _configuration.RootFolder;

		// % protected region % [Add any extra properties here] off begin
		// % protected region % [Add any extra properties here] end

		public FileSystemStorageProvider(
			// % protected region % [Add any extra constructor arguments here] off begin
			// % protected region % [Add any extra constructor arguments here] end
			IOptions<FileSystemStorageProviderConfiguration> configuration,
			ILogger<FileSystemStorageProvider> logger)
		{
			// % protected region % [Override constructor here] off begin
			_logger = logger;
			_configuration = configuration.Value;

			_logger.LogInformation("Using file system provider. Root File {Path}", RootFolder);
			// % protected region % [Override constructor here] end
		}

		/// <inheritdoc />
		public Task<Stream> GetAsync(StorageGetOptions options, CancellationToken cancellationToken = default)
		{
			// % protected region % [Override GetAsync here] off begin
			ValidateFileName(options.Container);
			ValidateFileName(options.FileName);

			_logger.LogDebug("Fetching file {FileName} from container {Container}", options.FileName, options.Container);

			return Task.FromResult(File.OpenRead(GetFileLocation(options.Container, options.FileName)) as Stream);
			// % protected region % [Override GetAsync here] end
		}

		/// <inheritdoc />
		public Task<IEnumerable<string>> ListAsync(StorageListOptions options, CancellationToken cancellationToken = default)
		{
			// % protected region % [Override ListAsync here] off begin
			ValidateFileName(options.Container);

			_logger.LogDebug("Listing contents of container {Container}", options.Container);

			return Task.FromResult(
				Directory.GetFiles(GetContainerLocation(options.Container)).AsEnumerable());
			// % protected region % [Override ListAsync here] end
		}

		/// <inheritdoc />
		public Task<bool> ExistsAsync(StorageExistsOptions options, CancellationToken cancellationToken = default)
		{
			// % protected region % [Override ExistsAsync here] off begin
			ValidateFileName(options.Container);
			ValidateFileName(options.FileName);

			_logger.LogDebug("Checking if file {FileName} from container {Container} exists", options.FileName, options.Container);

			return Task.FromResult(File.Exists(GetFileLocation(options.Container, options.FileName)));
			// % protected region % [Override ExistsAsync here] end
		}

		/// <inheritdoc />
		public async Task PutAsync(StoragePutOptions options, CancellationToken cancellationToken = default)
		{
			// % protected region % [Override PutAsync here] off begin
			ValidateFileName(options.Container);
			ValidateFileName(options.FileName);

			_logger.LogDebug(
				"Writing file {FileName} to container {Container}",
				options.FileName,
				options.Container,
				options.Overwrite);

			if (options.Overwrite == false && File.Exists(GetFileLocation(options.Container, options.FileName)))
			{
				throw new IOException("File already exists");
			}

			var containerLocation = GetContainerLocation(options.Container);
			if (!Directory.Exists(containerLocation))
			{
				if (options.CreateContainerIfNotExists)
				{
					Directory.CreateDirectory(containerLocation);
				}
				else
				{
					throw new IOException("This container does not exist");
				}
			}

			await using var streamWriter = new StreamWriter(GetFileLocation(options.Container, options.FileName), false);
			await options.Content.CopyToAsync(streamWriter.BaseStream, cancellationToken);
			await streamWriter.FlushAsync();
			// % protected region % [Override PutAsync here] end
		}

		/// <inheritdoc />
		public Task DeleteAsync(StorageDeleteOptions options, CancellationToken cancellationToken = default)
		{
			// % protected region % [Override DeleteAsync here] off begin
			ValidateFileName(options.Container);
			ValidateFileName(options.FileName);

			_logger.LogDebug("Deleting file {FileName} from container {Container}", options.FileName, options.Container);

			File.Delete(GetFileLocation(options.Container, options.FileName));

			return Task.CompletedTask;
			// % protected region % [Override DeleteAsync here] end
		}

		/// <inheritdoc />
		public Task<bool> ContainerExistsAsync(StorageContainerExistsOptions options, CancellationToken cancellationToken = default)
		{
			// % protected region % [Override ContainerExistsAsync here] off begin
			_logger.LogDebug("Checking if container {Container} exists", options.Container);
			return Task.FromResult(Directory.Exists(GetContainerLocation(options.Container)));
			// % protected region % [Override ContainerExistsAsync here] end
		}

		/// <inheritdoc />
		public Task CreateContainerAsync(StorageCreateContainerOptions options, CancellationToken cancellationToken = default)
		{
			// % protected region % [Override CreateContainerAsync here] off begin
			ValidateFileName(options.Container);

			var path = GetContainerLocation(options.Container);
			if (!Directory.Exists(path))
			{
				_logger.LogDebug("Creating container {Container}", options.Container);
				Directory.CreateDirectory(path);
			}
			else
			{
				_logger.LogDebug("Attempting to create container {Container}, but it already exists", options.Container);
			}

			return Task.CompletedTask;
			// % protected region % [Override CreateContainerAsync here] end
		}

		/// <inheritdoc />
		public Task DeleteContainerAsync(StorageDeleteContainerOptions options, CancellationToken cancellationToken = default)
		{
			// % protected region % [Override DeleteContainerAsync here] off begin
			ValidateFileName(options.Container);

			_logger.LogDebug("Deleting container {Container}", options.Container);
			Directory.Delete(options.Container, true);

			return Task.CompletedTask;
			// % protected region % [Override DeleteContainerAsync here] end
		}

		/// <inheritdoc />
		public Func<CancellationToken, Task<IActionResult>> OnFetch(StorageOnFetchOptions options)
		{
			// % protected region % [Override OnFetch here] off begin
			return cancellationToken =>
			{
				var readStream = File.OpenRead(GetFileLocation(options.File.Container, options.File.FileId));

				var cd = new ContentDispositionHeaderValue(options.Download ? "attachment" : "inline")
				{
					Name = options.File.FileName,
					FileNameStar = options.File.FileName,
					Size = readStream.Length,
					FileName = options.File.FileName,
				};

				options.HttpContext.Response.Headers["Content-Disposition"] = cd.ToString();

				return Task.FromResult(new FileStreamResult(readStream, options.File.ContentType)
				{
					LastModified = options.File.Modified,
				} as IActionResult);
			};
			// % protected region % [Override OnFetch here] end
		}

		/// <summary>
		/// Validates that a file has a valid filename
		/// </summary>
		/// <param name="fileName">The name of the file to validate</param>
		/// <exception cref="IOException">If the filename is invalid</exception>
		private void ValidateFileName(string fileName)
		{
			// % protected region % [Override ValidateFileName here] off begin
			if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
			{
				_logger.LogInformation("The name {fileName} is an invalid file name", fileName);
				throw new IOException("Invalid path name");
			}
			// % protected region % [Override ValidateFileName here] end
		}

		/// <summary>
		/// Gets the location of a options.Container on the file system
		/// </summary>
		/// <param name="container">The name of the options.Container</param>
		/// <returns>The location of the options.Container</returns>
		private string GetContainerLocation(string container)
		{
			// % protected region % [Override GetContainerLocation here] off begin
			return Path.Combine(Path.GetFullPath(RootFolder), container);
			// % protected region % [Override GetContainerLocation here] end
		}

		/// <summary>
		/// Get the location of a file on the file system
		/// </summary>
		/// <param name="container">The name of the options.Container the file is in</param>
		/// <param name="fileName">The name of the file</param>
		/// <returns>The location of the file</returns>
		private string GetFileLocation(string container, string fileName)
		{
			// % protected region % [Override GetFileLocation here] off begin
			return Path.Combine(GetContainerLocation(container), fileName);
			// % protected region % [Override GetFileLocation here] end
		}

		public void Dispose()
		{
			// % protected region % [Override Dispose here] off begin
			// % protected region % [Override Dispose here] end
		}

		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	}
}