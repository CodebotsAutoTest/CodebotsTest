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

namespace Lm2348.Services.CertificateProvider
{
	public class InRootFolderCertificateProvider: BaseCertificateProvider
	{
		public InRootFolderCertificateProvider(CertificateSetting certSetting) : base(certSetting){

		}

		public override X509Certificate2 ReadX509SigningCert()
		{
			var certFileName = CertificateSetting.CertFileName;
			var privateKeyPWD = CertificateSetting.PrivateKeyPWD;
			var fileFullPath = Path.Join(Directory.GetCurrentDirectory(), certFileName);
			if (File.Exists(fileFullPath))
			{
				return new X509Certificate2(fileFullPath, privateKeyPWD);
			}
			else
			{
				return null;
			}
		}
	}
}
