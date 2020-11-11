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
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Lm2348.Controllers.Entities;
using Lm2348.Models;
using Lm2348.Services.Interfaces;
using Lm2348.Services.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Lm2348.Controllers
{
	public class MetadataResponse
	{
		public Guid Id { get; set; }
		public DateTime Created { get; set; }
		public DateTime Modified { get; set; }
		public string FileName { get; set; }
		public string ContentType { get; set; }
		public double Length { get; set; }
		// % protected region % [Add any extra MetadataResponse fields here] off begin
		// % protected region % [Add any extra MetadataResponse fields here] end
	}

	[ApiController]
	[Route("/api/files")]
	public class FileController : BaseApiController
	{
		private readonly IUploadStorageProvider _storageProvider;
		private readonly ICrudService _crudService;
		private readonly ILogger<FileController> _logger;
		// % protected region % [Add any extra class fields here] off begin
		// % protected region % [Add any extra class fields here] end

		public FileController(
			IUploadStorageProvider storageProvider,
			ICrudService crudService,
			ILogger<FileController> logger
			// % protected region % [Add any extra constructor arguments here] off begin
			// % protected region % [Add any extra constructor arguments here] end
			)
		{
			_storageProvider = storageProvider;
			_crudService = crudService;
			_logger = logger;
			// % protected region % [Add any extra constructor calls here] off begin
			// % protected region % [Add any extra constructor calls here] end
		}

		/// <summary>
		/// Gets a file
		/// </summary>
		/// <param name="id">The id of the file to get</param>
		/// <param name="cancellationToken">The cancellation token for this operation</param>
		/// <param name="download">Should the file be downloaded</param>
		/// <returns>The requested file</returns>
		// % protected region % [Alter the Get attributes here] off begin
		[HttpGet]
		[Route("{id}")]
		[AllowAnonymous]
		// % protected region % [Alter the Get attributes here] end
		public async Task<IActionResult> Get(
			Guid id,
			CancellationToken cancellationToken,
			[FromQuery]bool download = false
			// % protected region % [Add any extra get arguments here] off begin
			// % protected region % [Add any extra get arguments here] end
			)
		{
			// % protected region % [Alter the Get endpoint here] off begin
			UploadFile file;
			try
			{
				file = await _crudService.GetFile(id, cancellationToken);
				_logger.LogInformation("Fetched file with id: {Id}", file.Id, file);
			}
			catch (FileNotFoundException e)
			{
				return BadRequest(e.Message);
			}
			catch (UnauthorizedAccessException e)
			{
				return Unauthorized(e.Message);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}

			// Check if there is a OnFetch method given by the provider
			var onFetch = _storageProvider.OnFetch(new StorageOnFetchOptions
			{
				Download = download,
				File = file,
				HttpContext = HttpContext,
			});
			if (onFetch != null)
			{
				return await onFetch(cancellationToken);
			}

			// No OnFetch provided, execute the download ourselves
			var fileStream = await _storageProvider.GetAsync(new StorageGetOptions
			{
				Container = file.Container,
				FileName = file.FileId,
			}, cancellationToken);
			SetFileHeaders(new FileDownloadOptions
			{
				ContentType = file.ContentType,
				FileName = file.FileName,
				Length = fileStream.Length,
				Download = download,
			});

			return new FileStreamResult(fileStream, file.ContentType ?? "application/octet-stream");
			// % protected region % [Alter the Get endpoint here] end
		}

		/// <summary>
		/// Gets the metadata for a file
		/// </summary>
		/// <param name="id">The id of the file</param>
		/// <param name="cancellationToken">Cancellation token for the operation</param>
		/// <returns>The file metadata</returns>
		// % protected region % [Alter the metadata attributes here] off begin
		[HttpGet]
		[Route("metadata/{id}")]
		[Produces(typeof(MetadataResponse))]
		[AllowAnonymous]
		// % protected region % [Alter the metadata attributes here] end
		public async Task<IActionResult> Metadata(
			Guid id, 
			CancellationToken cancellationToken
			// % protected region % [Add any extra get arguments here] off begin
			// % protected region % [Add any extra get arguments here] end
			)
		{
			// % protected region % [Alter the metadata endpoint here] off begin
			UploadFile file;
			try
			{
				file = await _crudService.GetFile(id, cancellationToken);
				_logger.LogInformation("Fetched file with id: {Id}", file.Id, file);
			}
			catch (FileNotFoundException e)
			{
				return BadRequest(e.Message);
			}
			catch (UnauthorizedAccessException e)
			{
				return Unauthorized(e.Message);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}

			return Ok(new MetadataResponse
			{
				Id = file.Id,
				Created = file.Created,
				Modified = file.Modified,
				FileName = file.FileName,
				ContentType = file.ContentType,
				Length = file.Length,
			});
			// % protected region % [Alter the metadata endpoint here] end
		}

		// % protected region % [Add any extra endpoints here] off begin
		// % protected region % [Add any extra endpoints here] end
	}
}