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
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lm2348.Services.Files;
using Microsoft.AspNetCore.Mvc;

namespace ServersideTests.Helpers.FileProviders
{
	/// <summary>
	/// In memory file storage provider. This store is backed by an in memory dictionary for testing purposes.
	/// </summary>
	public class InMemoryFileProvider : IUploadStorageProvider
	{
		private readonly ConcurrentDictionary<string, byte[]> _contents = new ConcurrentDictionary<string, byte[]>();

		/// <inheritdoc />
		public void Dispose()
		{
		}

		/// <inheritdoc />
		public Task<Stream> GetAsync(StorageGetOptions options, CancellationToken cancellationToken = default)
		{
			var bytes = _contents[GetFileKey(options.Container, options.FileName)];
			return Task.FromResult(new MemoryStream(bytes) as Stream);
		}

		/// <inheritdoc />
		public Task<IEnumerable<string>> ListAsync(StorageListOptions options, CancellationToken cancellationToken = default)
		{
			return Task.FromResult(_contents.Keys
				.Where(x => x.StartsWith($"{options.Container}/")));
		}

		/// <inheritdoc />
		public Task<bool> ExistsAsync(StorageExistsOptions options, CancellationToken cancellationToken = default)
		{
			return Task.FromResult(_contents.ContainsKey(GetFileKey(options.Container, options.FileName)));
		}

		/// <inheritdoc />
		public Task PutAsync(StoragePutOptions options, CancellationToken cancellationToken = default)
		{
			var stream = new MemoryStream();
			options.Content.CopyTo(stream);
			_contents[GetFileKey(options.Container, options.FileName)] = stream.ToArray();
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public Task DeleteAsync(StorageDeleteOptions options, CancellationToken cancellationToken = default)
		{
			_contents.Remove(GetFileKey(options.Container, options.FileName), out _);
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public Task<bool> ContainerExistsAsync(StorageContainerExistsOptions options, CancellationToken cancellationToken = default)
		{
			return Task.FromResult(true);
		}

		/// <inheritdoc />
		public Task CreateContainerAsync(StorageCreateContainerOptions options, CancellationToken cancellationToken = default)
		{
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public async Task DeleteContainerAsync(StorageDeleteContainerOptions options, CancellationToken cancellationToken = default)
		{
			var files = await ListAsync(new StorageListOptions {Container = options.Container}, cancellationToken);
			foreach (var file in files)
			{
				_contents.Remove(file, out _);
			}
		}

		/// <inheritdoc />
		public Func<CancellationToken, Task<IActionResult>> OnFetch(StorageOnFetchOptions options)
		{
			return null;
		}

		private static string GetFileKey(string container, string fileName)
		{
			return $"{container}/{fileName}";
		}
	}
}