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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Lm2348.Services.Files;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Lm2348.Models
{
	/// <summary>
	/// A class that represents a file that is saved to a storage provider like the file system or a cloud blob store.
	/// </summary>
	[Table("__Files")]
	public class UploadFile : IAbstractModel
	{
		public Guid Id { get; set; }
		public DateTime Created { get; set; }
		public DateTime Modified { get; set; }

		/// <summary>
		/// The container or bucket that the file is to be placed in
		/// </summary>
		[Required]
		[EntityAttribute]
		public string Container { get; set; }
		
		/// <summary>
		/// The id of the file that is saved to the storage provider
		/// </summary>
		[Required]
		[EntityAttribute]
		public string FileId { get; set; }
		
		/// <summary>
		/// The name of the file as it was uploaded to the server
		/// </summary>
		[Required]
		[EntityAttribute]
		public string FileName { get; set; }
		
		/// <summary>
		/// The content type of the file
		/// </summary>
		public string ContentType { get; set; }
		
		/// <summary>
		/// The length of the file in bytes
		/// </summary>
		[EntityAttribute]
		public long Length { get; set; }

		public async Task BeforeSave(EntityState operation, Lm2348DBContext dbContext, IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
		{
			return;
		}

		public async Task AfterSave(EntityState operation, Lm2348DBContext dbContext, IServiceProvider serviceProvider, ICollection<ChangeState> changes, CancellationToken cancellationToken = default)
		{
			if (operation == EntityState.Deleted)
			{
				try
				{
					var storageProvider = serviceProvider.GetRequiredService<IUploadStorageProvider>();
					await storageProvider.DeleteAsync(new StorageDeleteOptions
					{
						Container = Container,
						FileName = FileId,
					});
				}
				catch (Exception exception)
				{
					// Ignore errors to not destroy the operation since it is non critical for this to succeed.
					Log.Error(
						"Failed to delete file from storage provider. Error: {exception}",
						exception,
						this);
				}
			}
			return;
		}
	}
}