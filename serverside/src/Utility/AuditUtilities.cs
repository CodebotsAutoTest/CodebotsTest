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
using Audit.Core;
using Audit.EntityFramework;
using Lm2348.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Lm2348.Utility
{
	public static class AuditUtilities
	{
		public static void LogAuditEvent(AuditEvent audit, ILogger<AuditLog> logger)
		{
			var efEvent = audit.GetEntityFrameworkEvent();
			var dbContext = efEvent.GetDbContext() as Lm2348DBContext;
			foreach (var entry in efEvent.Entries)
			{
				var entity = new AuditLog
				{
					Id = Guid.NewGuid(),
					AuditData = JObject.FromObject(new
					{
						Table = entry.Table,
						Action = entry.Action,
						PrimaryKey = entry.PrimaryKey,
						ColumnValues = entry.ColumnValues,
						Values = entry
							.Changes
							?.Where(e => e.NewValue != null)
							.Select(e => new {ColumnName = e.ColumnName, Value = e.NewValue})
							.ToList()
					}),
					EntityType = entry.Table + "Entity",
					AuditDate = DateTime.UtcNow,
					Action = (entry.Action == "Insert") ? "Create" : entry.Action,
					TablePk = entry.PrimaryKey.First().Value.ToString(),
					UserId = dbContext?.SessionUserId,
					UserName = dbContext?.SessionUser,
					HttpContextId = dbContext?.SessionId
				};

				entity.LogAudit(logger);
			}
		}
	}
}