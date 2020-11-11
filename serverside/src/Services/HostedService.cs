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
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Lm2348.Services
{
	public abstract class HostedService : IHostedService
	{
		private Task _executingTask;
		private CancellationTokenSource _cts;

		public Task StartAsync(CancellationToken cancellationToken)
		{
			// Create a linked token so we can trigger cancellation outside of this token's cancellation
			_cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

			// Store the task we're executing
			_executingTask = ExecuteAsync(_cts.Token);

			// If the task is completed then return it, otherwise it's running
			return _executingTask.IsCompleted ? _executingTask : Task.CompletedTask;
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			// Stop called without start
			if (_executingTask == null)
			{
				return;
			}

			// Signal cancellation to the executing method
			_cts.Cancel();

			// Wait until the task completes or the stop token triggers
			await Task.WhenAny(_executingTask, Task.Delay(-1, cancellationToken));

			// Throw if cancellation triggered
			cancellationToken.ThrowIfCancellationRequested();
		}

		// Derived classes should override this and execute a long running method until
		// cancellation is requested
		protected abstract Task ExecuteAsync(CancellationToken cancellationToken);
	}
}
