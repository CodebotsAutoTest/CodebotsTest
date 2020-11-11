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
using System.Net;
using APITests.Setup;
using APITests.EntityObjects.Models;
using RestSharp;
using Xunit;

namespace APITests.Utils
{
	internal static class CreateEntityUtil
	{
		// % protected region % [Customize CreateEntities here] off begin
		public static void CreateEntities(List<BaseEntity> entityList)
		{
			var configure = new StartupTestFixture();

			//setup the rest client
			var client = new RestClient
			{
				BaseUrl = new Uri($"{configure.BaseUrl}/api/graphql")
			};

			//setup the request
			var request = new RestRequest
			{
				Method = Method.POST,
				RequestFormat = DataFormat.Json
			};

			//get the authorization token and adds the token to the request
			var loginToken = new LoginToken(configure.BaseUrl, configure.SuperUsername, configure.SuperPassword);
			var authorizationToken = $"{loginToken.TokenType} {loginToken.AccessToken}";
			request.AddHeader("Authorization", authorizationToken);

			request.AddHeader("Content-Type", "application/json");
			request.AddHeader("Accept", "*\\*");

			var query = QueryBuilder.CreateEntityQueryBuilder(entityList);

			request.AddParameter("text/json", query, ParameterType.RequestBody);

			// execute the request
			var response = client.Execute(request);

			//valid ids returned and a valid response
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		}
		// % protected region % [Customize CreateEntities here] end
	}
}