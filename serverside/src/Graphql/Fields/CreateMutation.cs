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
using System.Linq;
using System.Threading.Tasks;
using Lm2348.Helpers;
using Lm2348.Models;
using Lm2348.Models.RegistrationModels;
using Lm2348.Services;
using GraphQL;
using GraphQL.Types;
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace Lm2348.Graphql.Fields
{
	public class CreateMutation
	{
		/// <summary>
		/// Makes a Create mutation that will save new entities to the database
		/// </summary>
		/// <param name="name">The name of the model to create</param>
		/// <typeparam name="TModel">The type of the model to create</typeparam>
		/// <returns>A function that takes a graphql context and returns a list of created models</returns>
		public static Func<ResolveFieldContext<object>, Task<object>> CreateCreateMutation<TModel>(string name)
			where TModel : class, IOwnerAbstractModel, new()
		{
			// % protected region % [Override CreateCreateMutation here] off begin
			return async context =>
			{
				var graphQlContext = (Lm2348GraphQlContext) context.UserContext;
				var crudService = graphQlContext.CrudService;
				var models = context.GetArgument<List<TModel>>(name.ToCamelCase() + "s");
				List<string> mergeReferences = null;

				if (context.HasArgument("mergeReferences"))
				{
					mergeReferences = context.GetArgument<List<string>>("mergeReferences");
				}

				try
				{
					if (models == null)
					{
						throw new AggregateException(new Exception("No entities provided to save, aborting!"));
					}

					return await crudService.Create(models, new UpdateOptions
					{
						MergeReferences = mergeReferences,
						Files = graphQlContext.Files,
					});
				}
				catch (AggregateException exception)
				{
					context.Errors.AddRange(
						exception.InnerExceptions.Select(error => new ExecutionError(error.Message)));
					return new List<TModel>();
				}
			};
			// % protected region % [Override CreateCreateMutation here] end
		}

		/// <summary>
		/// Makes a Create mutation new user entities to the database
		/// </summary>
		/// <param name="name">The name of the model to create</param>
		/// <typeparam name="TModel">The type of the model to create</typeparam>
		/// <typeparam name="TRegisterModel">The type of the registration model</typeparam>
		/// <typeparam name="TGraphQlRegisterModel">The graphql register model</typeparam>
		/// <returns>A function that takes a graphql context and returns a list of created models</returns>
		public static Func<ResolveFieldContext<object>, Task<object>> CreateUserCreateMutation<TModel, TRegisterModel, TGraphQlRegisterModel>(string name)
			where TModel : User, IOwnerAbstractModel, new()
			where TRegisterModel : IRegistrationModel<TModel>
			where TGraphQlRegisterModel : TRegisterModel
		{
			// % protected region % [Override CreateUserCreateMutation here] off begin
			return async context =>
			{
				var graphQlContext = (Lm2348GraphQlContext) context.UserContext;
				var crudService = graphQlContext.CrudService;
				var models = context.GetArgument<List<TGraphQlRegisterModel>>(name.ToCamelCase() + "s");
				var mergeReferences = context.GetArgument<List<string>>("mergeReferences");

				try
				{
					if (models == null)
					{
						throw new AggregateException(new Exception("No entities provided to save, aborting!"));
					}

					return await crudService.CreateUser<TModel, TGraphQlRegisterModel>(models,new UpdateOptions
					{
						MergeReferences = mergeReferences,
						Files = graphQlContext.Files,
					});
				}
				catch (AggregateException exception)
				{
					context.Errors.AddRange(
						exception.InnerExceptions.Select(error => new ExecutionError(error.Message)));
					return new List<TModel>();
				}
			};
			// % protected region % [Override CreateUserCreateMutation here] end
		}
	}
}