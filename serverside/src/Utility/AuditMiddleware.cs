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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lm2348.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
// % protected region % [Add any extra audit middleware imports here] off begin
// % protected region % [Add any extra audit middleware imports here] end

namespace Lm2348.Utility
{
	public class AuditMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<AuditMiddleware> _logger;
		private readonly ILogger<AuditLog> _auditLogger;
		// % protected region % [Add any extra audit middleware variables here] off begin
		// % protected region % [Add any extra audit middleware variables here] end

		public AuditMiddleware(
			RequestDelegate next,
			ILogger<AuditMiddleware> logger,
			ILogger<AuditLog> auditLogger
			// % protected region % [Add any extra audit middleware constructor args here] off begin
			// % protected region % [Add any extra audit middleware constructor args here] end
			)
		{
			_next = next;
			_logger = logger;
			_auditLogger = auditLogger;
			// % protected region % [Add any extra audit middleware constructor calls here] off begin
			// % protected region % [Add any extra audit middleware constructor calls here] end
		}

		public async Task InvokeAsync(HttpContext context)
		{
			// % protected region % [Perform incoming actions here] off begin
			// % protected region % [Perform incoming actions here] end

			await _next.Invoke(context);

			// % protected region % [Perform outgoing actions here] off begin
			try
			{
				foreach (var log in GetLogs(context))
				{
					log.LogAudit(_auditLogger);
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e.ToString());
			}
			// % protected region % [Perform outgoing actions here] end
		}

		private static List<AuditLog> GetLogs(HttpContext context)
		{
			if (!context.Items.ContainsKey("AuditLogs"))
			{
				return new List<AuditLog>();
			}

			if (context.Items["AuditLogs"] is IList item)
			{
				return item.Cast<AuditLog>().ToList();
			}

			return new List<AuditLog>();
		}

		// % protected region % [Add any extra audit middleware methods here] off begin
		// % protected region % [Add any extra audit middleware methods here] end
	}
}
