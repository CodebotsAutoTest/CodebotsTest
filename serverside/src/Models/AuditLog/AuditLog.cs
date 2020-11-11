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
using Audit.EntityFramework;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lm2348.Models
{
	/// <summary>
	/// A log of all operations that have occured on entities in the database
	/// </summary>
	[AuditIgnore]
	public class AuditLog
	{
		/// <summary>
		/// The id of the audit
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// The user that performed the operation
		/// </summary>
		public string UserId { get; set; }

		/// <summary>
		/// The username of the the user that performed the operation
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// The type of entity that the operation has occured on
		/// </summary>
		public string EntityType { get; set; }

		/// <summary>
		/// The action that has occured on the entity
		/// </summary>
		public string Action { get; set; }

		/// <summary>
		/// The primary key of the entity that has had the operation performed on it
		/// </summary>
		public string TablePk { get; set; }

		/// <summary>
		/// The date and time of the audit in UTC
		/// </summary>
		public DateTime AuditDate { get; set; }

		/// <summary>
		/// The audit data stored as a JSON object
		/// </summary>
		public JObject AuditData { get; set; }

		/// <summary>
		/// The trace identifier of the http context if the log occured in a http request
		/// </summary>
		public string HttpContextId { get; set; }

		/// <summary>
		/// Logs the audit to the logger
		/// </summary>
		/// <param name="logger">The logger to log to</param>
		/// <param name="logLevel">The level to log as, defaults to information</param>
		public void LogAudit(ILogger<AuditLog> logger, LogLevel logLevel = LogLevel.Information)
		{
			logger.Log(
				logLevel,
				"Id: {Id} UserId: {UserId} UserName: {UserName} EntityType: {EntityType} Action: {Action} " +
				"TablePk: {TablePk} AuditDate: {AuditDate} AuditData: {AuditData} HttpContextId: {HttpContextId}",
				Id,
				UserId,
				UserName,
				EntityType,
				Action,
				TablePk,
				AuditDate,
				AuditData?.ToString(),
				HttpContextId);
		}

		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}