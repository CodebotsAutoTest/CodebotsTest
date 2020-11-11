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
using Lm2348.Services;
using Lm2348.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Lm2348.Utility
{
	public class XsrfActionFilterAttribute : Attribute, IFilterFactory, IOrderedFilter
	{
		public bool IsReusable => true;
		public int Order { get; set; } = 1100;

		public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
		{
			return serviceProvider.GetRequiredService<XsrfActionFilter>();
		}
	}

	public class XsrfActionFilter : IActionFilter
	{
		private readonly IXsrfService _xsrfService;

		public XsrfActionFilter(IXsrfService xsrfService)
		{
			_xsrfService = xsrfService;
		}

		public void OnActionExecuting(ActionExecutingContext context)
		{
			_xsrfService.AddXsrfToken(context.HttpContext);
		}

		public void OnActionExecuted(ActionExecutedContext context)
		{
		}
	}
}