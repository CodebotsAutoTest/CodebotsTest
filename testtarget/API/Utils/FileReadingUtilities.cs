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
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace APITests.Utils
{
	public class Email
	{
		public List<string> Recipients { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
		public string Token { get; set; }
		public string Link { get; set; }
	}

	public static class FileReadingUtilities
	{
		public static Email ReadRegistrationEmail(string username) => ReadEmail(username, "Confirm Account");
		public static Email ReadPasswordResetEmail(string username) => ReadEmail(username, "Reset Password");

		private static Email ReadEmail(string username, string subject)
		{
			// read the email
			var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"../../../../../API/src/bin/Debug/netcoreapp3.0/Test/Emails/{username}-{subject}.json");
			if (!FileExists(filePath))
			{
				throw new Exception($"Reset Password Email for {username} does not exist");
			}
			var reader = new StreamReader(filePath);
			var email = JsonConvert.DeserializeObject<Email>(reader.ReadToEnd());
			reader.Dispose();
			File.Delete(filePath);

			email.Token = new Regex(@"(?<=token=)(.*)(?=&username)").Matches(email.Body).FirstOrDefault()?.Value;
			email.Link = new Regex(@"(?<=href=\"")(.*)(?=\"")").Matches(email.Body).FirstOrDefault()?.Value;
			return email;
		}

		private static bool FileExists(string filePath)
		{
			var referenceTime = DateTime.Now;
			while (!File.Exists(filePath) && referenceTime.AddSeconds(5) > DateTime.Now)
			{
				System.Threading.Thread.Sleep(10);
			}

			System.Threading.Thread.Sleep(100);
			return File.Exists(filePath);
		}
	}
}