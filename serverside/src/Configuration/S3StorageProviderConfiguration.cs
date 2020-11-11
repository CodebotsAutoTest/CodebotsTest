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
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Lm2348.Configuration
{
	/// <summary>
	/// Configuration for the S3 storage provider
	/// </summary>
	public class S3StorageProviderConfiguration
	{
		/// <summary>
		/// The S3 access key
		/// </summary>
		public string AccessKey { get; set; }

		/// <summary>
		/// The S3 secret key
		/// </summary>
		public string SecretKey { get; set; }

		/// <summary>
		/// The id of the S3 AWS region to get the files from.
		/// The list of all regions is listed here: https://docs.aws.amazon.com/AmazonRDS/latest/UserGuide/Concepts.RegionsAndAvailabilityZones.html
		/// </summary>
		/// <example>ap-southeast-2</example>
		public string RegionEndpoint { get; set; }

		/// <summary>
		/// The name of the bucket to store files in
		/// </summary>
		public string BucketName { get; set; }

		// % protected region % [Add any extra configuration properties here] off begin
		// % protected region % [Add any extra configuration properties here] end
	}

	// % protected region % [Add any extra classes here] off begin
	// % protected region % [Add any extra classes here] end
}