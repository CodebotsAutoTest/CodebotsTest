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
using RestSharp;
using System.Net;
using Xunit;
using Xunit.Abstractions;

// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace APITests.Utils
{
	// % protected region % [Configure RequestHelpers type here] off begin
	internal static class RequestHelpers
	{
	// % protected region % [Configure RequestHelpers type here] end

		// % protected region % [Add any further fields here] off begin
		// % protected region % [Add any further fields here] end

		// % protected region % [Configure BasicGetRequest here] off begin
		public static RestRequest BasicGetRequest()
		{
			//setup the request
			var request = new RestRequest { Method = Method.GET, RequestFormat = DataFormat.Json };

			//get the authorization token and adds the token to the request
			request.AddHeader("Content-Type", "application/json");
			request.AddHeader("Accept", "application/json, text/html, */*");

			return request;
		}
		// % protected region % [Configure BasicGetRequest here] end

		// % protected region % [Configure BasicGetRequest here] off begin
		public static RestRequest BasicPostRequest()
		{
			//setup the request
			var request = new RestRequest { Method = Method.POST, RequestFormat = DataFormat.Json };

			//get the authorization token and adds the token to the request
			request.AddHeader("Content-Type", "application/json");
			request.AddHeader("Accept", "application/json, text/html, */*");

			return request;
		}
		// % protected region % [Configure BasicGetRequest here] end

		// % protected region % [Configure BasicGetRequest here] off begin
		public static void ValidateResponse(RestClient client, Method method, RestRequest request, HttpStatusCode expectedResponse)
		{
			request.Method = method;
			var response = client.Execute(request);
			Assert.Equal(expectedResponse, response.StatusCode);
		}
		// % protected region % [Configure BasicGetRequest here] end

		// % protected region % [Configure BasicGetRequest here] off begin
		public static void SendPostRequest(string uri, RestSharp.JsonObject query, ITestOutputHelper output)
		{
			var client = new RestClient {BaseUrl = new Uri(uri)};
			var request = new RestRequest {Method = Method.POST, RequestFormat = DataFormat.Json};
			request.AddParameter("application/json", query, ParameterType.RequestBody);
			request.AddHeader("Content-Type", "application/json");
			request.AddHeader("Accept", "*\\*");
			var response = client.Execute(request);

			ApiOutputHelper.WriteRequestResponseOutput(request, response, output);
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		}
		// % protected region % [Configure BasicGetRequest here] end

		// % protected region % [Add any further request helper methods here] off begin
		// % protected region % [Add any further request helper methods here] end
	}
}