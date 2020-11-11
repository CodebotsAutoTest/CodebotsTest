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
	public enum StorageProviders
	{
		FILE_SYSTEM,
		S3,
		// % protected region % [Add any extra storage provider enum entries here] off begin
		// % protected region % [Add any extra storage provider enum entries here] end
	}

	/// <summary>
	/// Configuration for the file storage
	/// </summary>
	public class StorageProviderConfiguration
	{
		/// <summary>
		/// The provider to use for file storage
		/// </summary>
		public StorageProviders Provider { get; set; } = StorageProviders.FILE_SYSTEM;

		// % protected region % [Add any extra configuration properties here] off begin
		// % protected region % [Add any extra configuration properties here] end
	}

	// % protected region % [Add any extra classes here] off begin
	// % protected region % [Add any extra classes here] end
}