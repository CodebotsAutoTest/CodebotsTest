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
using System.Threading;
using System.Threading.Tasks;
using Lm2348.Models;
using GraphQL;
using Microsoft.AspNetCore.Http;

namespace Lm2348.Services.Interfaces
{
	public interface IGraphQlService
	{
		/// <summary>
		/// Executes a graphql query
		/// </summary>
		/// <param name="query">The query to execute</param>
		/// <param name="operationName">The name of the graphql operation to execute</param>
		/// <param name="variables">Variables to pass into the query</param>
		/// <param name="attachments">The files that are attached to this request</param>
		/// <param name="user">The user to perform the operation</param>
		/// <param name="cancellation">A cancellation token</param>
		/// <returns>The result of the query in a task</returns>
		Task<ExecutionResult> Execute(
			string query,
			string operationName,
			Inputs variables,
			IFormFileCollection attachments,
			User user,
			CancellationToken cancellation);
	}
}