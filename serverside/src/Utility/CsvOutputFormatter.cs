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
using System.Collections;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace Lm2348.Utility
{
	public class CsvOutputFormatter : OutputFormatter
	{
		public CsvOutputFormatter()
		{
			SupportedMediaTypes.Clear();
			SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
			SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/csv"));
		}

		public override bool CanWriteResult(OutputFormatterCanWriteContext context)
		{
			return base.CanWriteResult(context) && typeof(IEnumerable).IsAssignableFrom(context.ObjectType);
		}

		public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
		{
			if (context.Object is IEnumerable data)
			{
				await using var writer = new StreamWriter(context.HttpContext.Response.Body);
				await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

				csv.SetIsoDateTimeFormat();

				await csv.WriteRecordsAsync(data);
			}
		}
	}
}