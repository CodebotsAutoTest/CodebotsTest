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
using APITests.Setup;
using Newtonsoft.Json.Linq;
using RestSharp;

// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace APITests.Utils
{
	internal static class ClientXsrf
	{
		// % protected region % [Customize GetValidClientAndxsrfTokenPair here] off begin
		public static (RestClient client, string xsrfToken) GetValidClientAndxsrfTokenPair(StartupTestFixture _configure)
		{
			//make a new client
			var client = new RestClient { BaseUrl = new Uri(_configure.BaseUrl + "/api/authorization/login") };

			// setup a cookie container to store cookiers for later
			client.CookieContainer = new System.Net.CookieContainer();

			//setup the request
			var request = new RestRequest { Method = Method.POST, RequestFormat = DataFormat.Json };

			// add header to say what type the content is
			request.AddHeader("Content-Type", "application/json");

			// add valid username and password to the request body
			request.AddJsonBody(new { username = _configure.SuperUsername, password = _configure.SuperPassword });

			// execute the request
			var response = client.Execute(request);

			// check that the response is ok
			if (response.StatusCode != HttpStatusCode.OK)
			{
				var invalidResponse = JObject.Parse(response.Content);
				throw new Exception(invalidResponse["error_description"].ToString());
			}

			// get the returned xsrf token
			var xsrfToken = response.Cookies.FirstOrDefault(cookie => cookie.Name == "XSRF-TOKEN")?.Value;

			// return the client containing cookies and token
			return (client, xsrfToken);
		}
		// % protected region % [Customize GetValidClientAndxsrfTokenPair here] end
	}
}