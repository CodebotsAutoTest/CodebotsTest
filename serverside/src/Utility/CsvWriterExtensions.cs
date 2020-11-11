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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Lm2348.Utility
{
	public static class CsvWriterExtensions
	{
		/// <summary>
		/// Outputs a queryable as a csv with no headers
		/// </summary>
		/// <param name="writer">The writer to output with</param>
		/// <param name="records">The records to output</param>
		/// <param name="cancellationToken">Cancellation token for the operation</param>
		/// <typeparam name="T">The type to output</typeparam>
		/// <returns>Task that resolves when the writer has completed</returns>
		/// <exception cref="CsvHelperException">On output failure</exception>
		public static async Task WriteQueryableAsync<T>(
			this CsvWriter writer, 
			IQueryable<T> records, 
			CancellationToken cancellationToken = default)
		{
			try
			{
				await foreach (var record in records.AsAsyncEnumerable().WithCancellation(cancellationToken))
				{
					writer.WriteRecord(record);
					await writer.NextRecordAsync();
				}
			}
			catch (Exception ex)
			{
				throw ex as CsvHelperException ?? new WriterException(writer.Context, "An unexpected error occurred.", ex);
			}
		}

		/// <summary>
		/// Sets the datetime format for the CSV writer
		/// </summary>
		/// <param name="writer">The writer to set the format on</param>
		/// <param name="format">The date time format</param>
		public static void SetDateTimeFormat(this CsvWriter writer, string format)
		{
			writer.Configuration.TypeConverterOptionsCache.GetOptions<DateTime>().Formats = new[] {format};
			writer.Configuration.TypeConverterOptionsCache.GetOptions<DateTime?>().Formats = new[] {format};
		}

		/// <summary>
		/// Sets the datetime format for the CSV writer to ISO format
		/// </summary>
		/// <param name="writer">The writer to set the format on</param>
		public static void SetIsoDateTimeFormat(this CsvWriter writer)
		{
			writer.SetDateTimeFormat("yyyy-MM-ddTHH:mm:ss");
		}

		// % protected region % [Add any extra functions here] off begin
		// % protected region % [Add any extra functions here] end
	}
}