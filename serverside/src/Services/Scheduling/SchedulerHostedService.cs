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
using NCrontab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Lm2348.Services.Scheduling
{

	public class SchedulerHostedService : HostedService
	{
		public event EventHandler<UnobservedTaskExceptionEventArgs> UnobservedTaskException;

		private readonly List<SchedulerTaskWrapper> _scheduledTasks = new List<SchedulerTaskWrapper>();
		// % protected region % [Add any extra dependencies here] off begin
		// % protected region % [Add any extra dependencies here] end

		public SchedulerHostedService(IEnumerable<IScheduledTask> scheduledTasks, IServiceProvider serviceProvider)
		{
			// % protected region % [Add any extra dependency assignments here] off begin
			// % protected region % [Add any extra dependency assignments here] end

			var referenceTime = DateTime.UtcNow;

			foreach (var scheduledTask in scheduledTasks)
			{
				_scheduledTasks.Add(new SchedulerTaskWrapper
				{
					// % protected region % [Configure the parser of the schedule string to support down to 1 minute or 1 second] off begin
					Schedule = CrontabSchedule.Parse(scheduledTask.Schedule),
					// % protected region % [Configure the parser of the schedule string to support down to 1 minute or 1 second] end
					Task = scheduledTask,
					NextRunTime = referenceTime
				});
			}
		}

		protected override async Task ExecuteAsync(CancellationToken cancellationToken)
		{
			// % protected region % [Add any extra processes before the scheduled tasks running here] off begin
			// % protected region % [Add any extra processes before the scheduled tasks running here] end
			
			while (!cancellationToken.IsCancellationRequested)
			{
				await ExecuteOnceAsync(cancellationToken);

				// % protected region % [Configure the interval of the loop to ckeck the timer] off begin
				var timeSpan = TimeSpan.FromMinutes(1);
				// % protected region % [Configure the interval of the loop to ckeck the timer] end

				await Task.Delay(timeSpan, cancellationToken);
			}
		}

		private async Task ExecuteOnceAsync(CancellationToken cancellationToken)
		{

			var taskFactory = new TaskFactory(TaskScheduler.Current);
			var referenceTime = DateTime.UtcNow;

			var tasksThatShouldRun = _scheduledTasks.Where(t => t.ShouldRun(referenceTime)).ToList();

			foreach (var taskThatShouldRun in tasksThatShouldRun)
			{
				taskThatShouldRun.Increment();

				await taskFactory.StartNew(
					async () =>
					{
						try
						{
							await taskThatShouldRun.Task.ExecuteAsync(cancellationToken);
						}
						catch (Exception ex)
						{
							var args = new UnobservedTaskExceptionEventArgs(
								ex as AggregateException ?? new AggregateException(ex));

							UnobservedTaskException?.Invoke(this, args);

							if (!args.Observed)
							{
								throw;
							}
						}
					},
					cancellationToken);
			}
		}

		private class SchedulerTaskWrapper
		{
			public CrontabSchedule Schedule { get; set; }
			public IScheduledTask Task { get; set; }

			public DateTime LastRunTime { get; set; }
			public DateTime NextRunTime { get; set; }

			public void Increment()
			{
				LastRunTime = NextRunTime;
				NextRunTime = Schedule.GetNextOccurrence(NextRunTime);
			}

			public bool ShouldRun(DateTime currentTime)
			{
				return NextRunTime <= currentTime && LastRunTime != NextRunTime;
			}
		}
	}
}
