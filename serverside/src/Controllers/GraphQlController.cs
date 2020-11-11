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
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Lm2348.Services.Interfaces;
using GraphQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Lm2348.Controllers
{
	/// <summary>
	/// The controller that manages all GraphQL operations
	/// </summary>
	// % protected region % [Change controller attributes here here] off begin
	[Route("/api/graphql")]
	[ApiController]
	[Authorize(Policy = "AllowVisitorPolicy")]
	// % protected region % [Change controller attributes here here] end
	public class GraphQlController : Controller
	{
		private class PostBody
		{
			public string OperationName { get; set; }
			public string Query { get; set; }
			public JObject Variables { get; set; }
			// % protected region % [Add any extra PostBody fields here] off begin
			// % protected region % [Add any extra PostBody fields here] end
		}

		private class FormBody
		{
			public PostBody Body { get; set; }
			public IFormFileCollection Files { get; set; }
			// % protected region % [Add any FormBody fields here] off begin
			// % protected region % [Add any FormBody fields here] end
		}

		private readonly IGraphQlService _graphQlService;
		private readonly IIdentityService _identityService;
		private readonly ILogger<GraphQlController> _logger;
		// % protected region % [Add any extra class variables here] off begin
		// % protected region % [Add any extra class variables here] end

		public GraphQlController(
			// % protected region % [Add any extra constructor arguments here] off begin
			// % protected region % [Add any extra constructor arguments here] end
			IGraphQlService graphQlService,
			IIdentityService identityService,
			ILogger<GraphQlController> logger)
		{
			_graphQlService = graphQlService;
			_identityService = identityService;
			_logger = logger;
			// % protected region % [Add any extra constructor logic here] off begin
			// % protected region % [Add any extra constructor logic here] end
		}

		/// <summary>
		/// Executor for GraphQL queries
		/// </summary>
		/// <param name="cancellation">Cancellation token for the operation</param>
		/// <returns>The results for the GraphQL query</returns>
		// % protected region % [Change post method attributes here] off begin
		[HttpPost]
		[RequestSizeLimit(100000000)]
		[Authorize(Policy = "AllowVisitorPolicy")]
		// % protected region % [Change post method attributes here] end
		public async Task<ExecutionResult> Post(CancellationToken cancellation)
		{
			// % protected region % [Change post method here] off begin
			await _identityService.RetrieveUserAsync();

			var parsedRequest = await ParsePostBody(cancellation);

			var result = await _graphQlService.Execute(
				parsedRequest.Body.Query,
				parsedRequest.Body.OperationName,
				parsedRequest.Body.Variables.ToInputs(),
				parsedRequest.Files,
				_identityService.User,
				cancellation);

			return RenderResult(result);
			// % protected region % [Change post method here] end
		}

		/// <summary>
		/// Executor for GraphQL queries
		/// </summary>
		/// <param name="query">The graphql query body</param>
		/// <param name="variables">Variables for the graphql query as JSON</param>
		/// <param name="operationName">The name of the graphql operation to run</param>
		/// <param name="cancellation">Cancellation token for the operation</param>
		/// <returns>The results for the GraphQL query</returns>
		// % protected region % [Change get method attributes here] off begin
		[HttpGet]
		[Authorize(Policy = "AllowVisitorPolicy")]
		// % protected region % [Change get method attributes here] end
		public async Task<ExecutionResult> Get(
			[FromQuery]string query,
			[FromQuery]string variables,
			[FromQuery]string operationName,
			CancellationToken cancellation)
		{
			// % protected region % [Change get method here] off begin
			await _identityService.RetrieveUserAsync();

			var jObject = ParseVariables(variables);
			var result = await _graphQlService.Execute(
				query,
				operationName,
				jObject.ToInputs(),
				new FormFileCollection(),
				_identityService.User,
				cancellation);

			return RenderResult(result);
			// % protected region % [Change get method here] end
		}

		/// <summary>
		/// Correctly renders the Graphql result for returning to the user
		/// </summary>
		/// <param name="result">The graphql execution result</param>
		/// <returns>The graphql execution result with better formatting</returns>
		private ExecutionResult RenderResult(ExecutionResult result)
		{
			// % protected region % [Change RenderResult here] off begin
			if (result.Errors?.Count > 0)
			{
				var newEx = new ExecutionErrors();
				foreach (var error in result.Errors)
				{
					var ex = error.InnerException;
					if (ex is PostgresException pgException)
					{
						if (string.IsNullOrWhiteSpace(pgException.MessageText))
						{
							newEx.Add(error);
						}
						else
						{
							newEx.Add(new ExecutionError(pgException.MessageText));
						}
					}
					else
					{
						newEx.Add(error);
					}

					_logger.LogError(
						"Graphql error message: {Error}\nException: {Exception}",
						error.Message,
						ex?.ToString());
				}
				result.Errors = newEx;
				Response.StatusCode = (int)HttpStatusCode.BadRequest;
			}
			return result;
			// % protected region % [Change RenderResult here] end
		}

		/// <summary>
		/// Parses the post body, handling weather it is a multipart form or a json request
		/// </summary>
		/// <param name="cancellation">Cancellation token</param>
		/// <returns>A parsed result for the form</returns>
		private async Task<FormBody> ParsePostBody(CancellationToken cancellation)
		{
			// % protected region % [Change ParsePostBody here] off begin
			// We are using JSON content type
			if (!Request.HasFormContentType)
			{
				using var sr = new StreamReader(Request.Body);
				using var jsonTextReader = new JsonTextReader(sr);
				var jBody = await JObject.LoadAsync(jsonTextReader, cancellation);
				var body = jBody.ToObject<PostBody>();

				return new FormBody
				{
					Body = body,
					Files = new FormFileCollection(),
				};
			}

			// Otherwise we are a multipart form
			var form = await Request.ReadFormAsync(cancellation);

			form.TryGetValue("operationName", out var operationName);
			form.TryGetValue("variables", out var variables);
			form.TryGetValue("query", out var query);

			return new FormBody
			{
				Body = new PostBody
				{
					Query = query.First(),
					Variables = ParseVariables(variables.First()),
					OperationName = operationName.First(),
				},
				Files = form.Files,
			};
			// % protected region % [Change ParsePostBody here] end
		}

		/// <summary>
		/// Parses the variables object for the graphql query
		/// </summary>
		/// <param name="variables">The variables JSON string</param>
		/// <returns>The graphql Inputs object</returns>
		/// <exception cref="Exception">On failing to parse the string</exception>
		private static JObject ParseVariables(string variables)
		{
			// % protected region % [Change ParseVariables here] off begin
			if (variables == null)
			{
				return null;
			}

			try
			{
				return JObject.Parse(variables);
			}
			catch (Exception exception)
			{
				throw new Exception("Could not parse variables.", exception);
			}
			// % protected region % [Change ParseVariables here] end
		}

		// % protected region % [Add any further methods here] off begin
		// % protected region % [Add any further methods here] end
	}
}