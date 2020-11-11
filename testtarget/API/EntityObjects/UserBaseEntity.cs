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
using System.Net;
using System.Collections.Generic;
using Lm2348.Models;
using APITests.Setup;
using APITests.Utils;
using RestSharp;
using TestDataLib;
using Xunit;
// % protected region % [Custom imports] off begin
// % protected region % [Custom imports] end

namespace APITests.EntityObjects.Models
{
	public abstract class UserBaseEntity : BaseEntity
	{
		public string EmailAddress = DataUtils.RandEmail();
		public string Password = "abc123A@";
		public string EndpointName;
		private readonly StartupTestFixture _configure = new StartupTestFixture();

		// % protected region % [Customize CreateUser method here] off begin
		public Guid CreateUser(bool isRegistered = true)
		{
			//setup the rest client
			var client = new RestClient
			{
				BaseUrl = new Uri($"{_configure.BaseUrl}/api/graphql")
			};

			//setup the request
			var request = new RestRequest
			{
				Method = Method.POST,
				RequestFormat = DataFormat.Json
			};

			//get the authorization token and adds the token to the request
			var loginToken = new LoginToken(_configure.BaseUrl, _configure.SuperUsername, _configure.SuperPassword);
			var authorizationToken = $"{loginToken.TokenType} {loginToken.AccessToken}";
			request.AddHeader("Authorization", authorizationToken);
			request.AddHeader("Content-Type", "application/json");
			request.AddHeader("Accept", "*\\*");
			var query = QueryBuilder.CreateEntityQueryBuilder(new List<BaseEntity>{this});
			request.AddParameter("text/json", query, ParameterType.RequestBody);
			var response = client.Execute(request);

			//valid ids returned and a valid response
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);

			if (isRegistered)
			{
				// we will confirm their email, they are registered
				var configure = new StartupTestFixture();
				using (var context = new Lm2348DBContext(configure.DbContextOptions, null, null))
				{
					context.Users.FirstOrDefault(x => x.UserName == EmailAddress).EmailConfirmed = true;
					context.SaveChanges();
				}
			}
			return Id;
		}
		// % protected region % [Customize CreateUser method here] end
	}
}