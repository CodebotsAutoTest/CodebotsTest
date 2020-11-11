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
using System.Threading;
using System.Threading.Tasks;
using Lm2348.Models;
using Lm2348.Services.Interfaces;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Lm2348.Services
{
	public class Lm2348GraphQlContext
	{
		public Lm2348DBContext DbContext { get; set; }
		public User User { get; set; }
		public IList<string> UserGroups { get; set; }
		public ISecurityService SecurityService { get; set; }
		public UserManager<User> UserManager { get; set; }
		public IUserService UserService { get; set; }
		public ICrudService CrudService { get; set; }
		public IIdentityService IdentityService { get; set; }
		public IServiceProvider ServiceProvider { get; set; }
		public IAuditService AuditService { get; set; }
		public IFormFileCollection Files { get; set; }
	}

	public class GraphQlService : IGraphQlService
	{
		private readonly IDocumentExecuter _executer;
		private readonly ISchema _schema;
		private readonly Lm2348DBContext _dataContext;
		private readonly ISecurityService _securityService;
		private readonly UserManager<User> _userManager;
		private readonly IUserService _userService;
		private readonly ICrudService _crudService;
		private readonly IIdentityService _identityService;
		private readonly IServiceProvider _serviceProvider;
		private readonly IAuditService _auditService;

		public GraphQlService(
			ISchema schema,
			IDocumentExecuter executer,
			Lm2348DBContext dataContext,
			ISecurityService securityService,
			UserManager<User> userManager,
			IUserService userService,
			ICrudService crudService,
			IServiceProvider serviceProvider,
			IIdentityService identityService,
			IAuditService auditService)
		{
			_schema = schema;
			_executer = executer;
			_dataContext = dataContext;
			_securityService = securityService;
			_userManager = userManager;
			_userService = userService;
			_crudService = crudService;
			_identityService = identityService;
			_serviceProvider = serviceProvider;
			_auditService = auditService;
		}

		/// <inheritdoc />
		public async Task<ExecutionResult> Execute(
			string query,
			string operationName,
			Inputs variables,
			IFormFileCollection attachments,
			User user,
			CancellationToken cancellation)
		{
			await _identityService.RetrieveUserAsync();

			var executionOptions = new ExecutionOptions
			{
				Schema = _schema,
				Query = query,
				OperationName = operationName,
				Inputs = variables,
				UserContext = new Lm2348GraphQlContext
				{
					DbContext = _dataContext,
					User = user,
					UserGroups = _identityService.Groups,
					SecurityService = _securityService,
					CrudService = _crudService,
					IdentityService = _identityService,
					UserManager = _userManager,
					UserService = _userService,
					ServiceProvider = _serviceProvider,
					AuditService = _auditService,
					Files = attachments,
				},
				CancellationToken = cancellation,
#if (DEBUG)
				ExposeExceptions = true,
				EnableMetrics = true,
#endif
			};

			var result = await _executer.ExecuteAsync(executionOptions)
				.ConfigureAwait(false);

			return result;
		}
	}
}