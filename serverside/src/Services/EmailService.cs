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
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Lm2348.Helpers;
using Lm2348.Services.Interfaces;
using Lm2348.Utility;
// % protected region % [Add any extra email service imports here] off begin
// % protected region % [Add any extra email service imports here] end

namespace Lm2348.Services
{
	/// <summary>
	/// Model for reading EmailAccount from appsettings.json
	/// </summary>
	public class EmailAccount
	{
		/// <summary>
		/// The name or IP address of the host used for SMTP transactions
		/// </summary>
		public string Host { get; set; }
		/// <summary>
		/// The user name associated with the credentials
		/// </summary>
		public string Username { get; set; }
		/// <summary>
		/// The password for the user name associated with the credentials
		/// </summary>
		public string Password { get; set; }
		/// <summary>
		/// The from address attached to the email
		/// </summary>
		public string FromAddress { get; set; }
		/// <summary>
		/// The from address display name of the from address
		/// </summary>
		public string FromAddressDisplayName { get; set; }
		/// <summary>
		/// The port used for SMTP transactions
		/// </summary>
		public int Port { get; set; }
		/// <summary>
		/// Specify whether the System.Net.Mail.SmtpClient uses Secure Sockets Layer (SSL) to encrypt the connection
		/// </summary>
		public bool EnableSsl { get; set; }
		/// <summary>
		/// This is for testing or debuggin purpose only. If set and not empty, for email, the to will be set to this email address, cc and bcc will be set to empty, and a text message as below will be attached at the beginning of the email body:
		/// "This email was originally send to xxx, This email was originally cc to xxx, This email was originally bcc to xxx"
		/// </summary>
		public string RedirectToAddress { get; set; }
		/// <summary>
		/// This is for bypassing sslPolicyErrors if ssl Certificate Validation got errors. But should only be set to true in debugging or other special cases. Otherwise always set to false
		/// </summary>
		public bool BypassCertificateValidation { get; set; }
		/// <summary>
		/// This is for saving the email to a local .json file instead of sending through SMTP
		/// </summary>
		public bool SaveToLocalFile{ get; set; }
	}

	/// <summary>
	/// The parameters object for calling SendEmail function
	/// </summary>
	public class EmailEntity
	{
		/// <summary>
		/// The To email address list
		/// </summary>
		public IEnumerable<string> To;
		/// <summary>
		/// The Bcc email address list
		/// </summary>
		public IEnumerable<string> Bcc;
		/// <summary>
		/// The Cc email address list
		/// </summary>
		public IEnumerable<string> Cc;
		/// <summary>
		/// The Html string which is the email body
		/// </summary>
		public string Body;
		/// <summary>
		/// The Email subject text
		/// </summary>
		public string Subject;
		/// <summary>
		/// The user name associated with the credentials
		/// </summary>
		public string CredentialUser;
		/// <summary>
		/// The user password associated with the credentials
		/// </summary>
		public string CredentialPassword;
		/// <summary>
		/// The name or IP address of the host used for SMTP transactions
		/// </summary>
		public string Host;
		/// <summary>
		/// Currently not being used
		/// </summary>
		public int? Port;
		/// <summary>
		/// Currently not being used
		/// </summary>
		public bool UseSsl;
		/// <summary>
		/// the email attachments, key = file name, value = file stream
		/// </summary>
		public readonly Dictionary<string, Stream> Attachments = new Dictionary<string, Stream>();
	}

	/// <summary>
	/// Service for handling email operations
	/// </summary>
	public class EmailService : IEmailService
	{

		public EmailService(IOptions<EmailAccount> emailAccount)
		{
			EmailAccount = emailAccount.Value;
		}

		private EmailAccount EmailAccount { get; }

		/// <inheritdoc />
		// % protected region % [Configure SendEmail method here] off begin
		public async Task<bool> SendEmail(EmailEntity emailToSend)
		{
			var to = emailToSend.To;
			var cc = emailToSend.Cc;
			var bcc = emailToSend.Bcc;
			var body = emailToSend.Body;

			// redirect email
			if (!string.IsNullOrWhiteSpace(EmailAccount.RedirectToAddress))
			{
				var originalAddressInfo = "This email was originally send to <br/>" + to;
				originalAddressInfo += "This email was originally cc to <br/>" + cc;
				originalAddressInfo += "This email was originally Bcc to <br/>" + bcc;
				body = originalAddressInfo + body;

				to = EmailAccount.RedirectToAddress.Split(",");
				cc = null;
				bcc = null;
			}

			// Create the mail message
			var mailMessage = new MailMessage
			{
				Body = body,
				IsBodyHtml = true,
				From = new MailAddress(EmailAccount.FromAddress, EmailAccount.FromAddressDisplayName, Encoding.UTF8),
				Subject = emailToSend.Subject,
				SubjectEncoding = Encoding.UTF8,
				Priority = MailPriority.Normal
			};

			// Add recipients
			mailMessage.To.AddRange(to.Select(address => new MailAddress(address)));
			if (cc != null)
			{
				mailMessage.CC.AddRange(cc.Select(address => new MailAddress(address)));
			}
			if (bcc != null)
			{
				mailMessage.Bcc.AddRange(bcc.Select(address => new MailAddress(address)));
			}

			var attachments = emailToSend.Attachments
				.Select(attachment => new Attachment(attachment.Value, attachment.Key));
			mailMessage.Attachments.AddRange(attachments);

			//send email
			ServicePointManager.ServerCertificateValidationCallback = CertificateValidationCallBack;
			using var smtp = new SmtpClient
			{
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(EmailAccount.Username, EmailAccount.Password),
				Host = EmailAccount.Host,
				EnableSsl = EmailAccount.EnableSsl
			};

			if (EmailAccount.Port > 0) smtp.Port = EmailAccount.Port;

			smtp.UseDefaultCredentials = false;
			smtp.Credentials = new NetworkCredential(EmailAccount.Username, EmailAccount.Password);
			smtp.Host = EmailAccount.Host;
			smtp.EnableSsl = EmailAccount.EnableSsl;

			if (EmailAccount.Port > 0) smtp.Port = EmailAccount.Port;

			if (EmailAccount.SaveToLocalFile)
			{
				FileWritingUtilities.WriteEmailToLocalFile(mailMessage);
				return true;
			}

			await smtp.SendMailAsync(mailMessage);

			return true;
		}
		// % protected region % [Configure SendEmail method here] end

		private bool CertificateValidationCallBack(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			// If the certificate is a valid, signed certificate, return true.
			return sslPolicyErrors == SslPolicyErrors.None || EmailAccount.BypassCertificateValidation;
		}
		// % protected region % [Add any extra email service methods here] off begin
		// % protected region % [Add any extra email service methods here] end
	}
}
