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
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Lm2348.Services.CertificateProvider
{
	public class CertificateSetting
	{
		// % protected region % [Customise CertificateSetting fields here] off begin
		/// <summary>
		/// The name of the certificate file
		/// </summary>
		public string CertFileName { get; set; }
		/// <summary>
		/// The password for the private key of the certificate file
		/// </summary>
		public string PrivateKeyPWD { get; set; }
		/// <summary>
		/// The JwtBearer Authority
		/// </summary>
		public string JwtBearerAuthority { get; set; }
		/// <summary>
		/// The JwtBearer Audience
		/// </summary>
		public string JwtBearerAudience { get; set; }
		// % protected region % [Customise CertificateSetting fields here] end

		// % protected region % [Add any additional CertificateSetting fields here] off begin
		// % protected region % [Add any additional CertificateSetting fields here] end
	}

	public abstract class BaseCertificateProvider: ICertificateProvider
	{
		// % protected region % [Add any additional fields here] off begin
		// % protected region % [Add any additional fields here] end

		public BaseCertificateProvider(CertificateSetting certSetting)
		{
			CertificateSetting = certSetting;
		}

		protected CertificateSetting CertificateSetting { get; }

		public abstract X509Certificate2 ReadX509SigningCert();

		// % protected region % [Add any BaseCertificateProvider methods here] off begin
		// % protected region % [Add any BaseCertificateProvider methods here] end
	}
}
