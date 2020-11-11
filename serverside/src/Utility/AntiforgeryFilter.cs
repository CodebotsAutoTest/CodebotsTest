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
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Lm2348.Utility
{
	public class AntiforgeryFilterAttribute : Attribute, IFilterFactory, IOrderedFilter
	{
		public bool IsReusable => true;
		public int Order { get; set; } = 1000;

		public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
		{
			return serviceProvider.GetRequiredService<AntiforgeryFilter>();
		}
	}

	public class AntiforgeryFilter : IAsyncAuthorizationFilter, IAntiforgeryPolicy
	{
		private readonly IAntiforgery _antiforgery;
		private readonly ILogger<AntiforgeryFilter> _logger;

		public AntiforgeryFilter(IAntiforgery antiforgery, ILogger<AntiforgeryFilter> logger)
		{
			_antiforgery = antiforgery;
			_logger = logger;
		}

		public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException(nameof(context));
			}

			if (!context.IsEffectivePolicy<IAntiforgeryPolicy>(this))
			{
				_logger.LogInformation("Global antiforgery filter not most effective filter");
				return;
			}

			if (ShouldValidate(context))
			{
				try
				{
					await _antiforgery.ValidateRequestAsync(context.HttpContext);
				}
				catch (AntiforgeryValidationException exception)
				{
					_logger.LogInformation("Invalid antiforgery request {Message}", exception.Message, exception);
					context.Result = new AntiforgeryValidationFailedResult();
				}
			}
		}

		protected virtual bool ShouldValidate(AuthorizationFilterContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException(nameof(context));
			}

			var method = context.HttpContext.Request.Method;
			const string cookieName = ".AspNetCore." + CookieAuthenticationDefaults.AuthenticationScheme;

			return context.HttpContext.Request.Cookies.ContainsKey(cookieName) &&
				context.HttpContext.User.Identity.IsAuthenticated &&
				!string.Equals("GET", method, StringComparison.OrdinalIgnoreCase) &&
				!string.Equals("HEAD", method, StringComparison.OrdinalIgnoreCase) &&
				!string.Equals("TRACE", method, StringComparison.OrdinalIgnoreCase) &&
				!string.Equals("OPTIONS", method, StringComparison.OrdinalIgnoreCase);
		}
	}
}