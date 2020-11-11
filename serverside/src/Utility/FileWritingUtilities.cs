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
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lm2348.Utility
{
	public static class FileWritingUtilities
	{
		public static void WriteEmailToLocalFile(MailMessage mailMessage)
		{
			var data = new
			{
				Recipients = mailMessage.To.Select(x => x.Address),
				Subject = mailMessage.Subject,
				Body = mailMessage.Body,
			};

			var savePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Test", "Emails");
			var fileName = $"{data.Recipients.FirstOrDefault()}-{data.Subject}.json";

			Directory.CreateDirectory(savePath);
			File.WriteAllText(Path.Combine(savePath, fileName), JsonConvert.SerializeObject(data));
		}
	}
}