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
using System.Net;
using Newtonsoft.Json.Linq;
using RestSharp;

// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace APITests.Utils
{
	public class LoginToken
	{
		// % protected region % [Configure LoginToken fields here] off begin
		public string TokenType { get; set; }
		public string AccessToken { get; set; }
		public string ExpiresIn { get; set; }
		// % protected region % [Configure LoginToken fields here] end

		// % protected region % [Configure LoginToken implementation here] off begin
		public LoginToken(string baseUrl, string username, string password)
		{
			var client = new RestClient {BaseUrl = new Uri(baseUrl + "/api/authorization/connect/token")};

			//setup the request
			var request = new RestRequest {Method = Method.POST, RequestFormat = DataFormat.Json};

			//add to the body the email and password for the register request
			request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

			//set the parameter to use the correct content type, username, password and grant type2
			request.AddParameter("application/x-www-form-urlencoded",
				$"username={username}&password={password}&grant_type=password",
				ParameterType.RequestBody);

			// execute the request
			var response = client.Execute(request);

			if (response.StatusCode != HttpStatusCode.OK)
			{
				var invalidResponse = JObject.Parse(response.Content);
				throw new Exception(invalidResponse["error_description"].ToString());
			}

			var loginResponse = JObject.Parse(response.Content);
			TokenType = loginResponse["token_type"].ToString();
			AccessToken = loginResponse["access_token"].ToString();
			ExpiresIn = loginResponse["expires_in"].ToString();
		}
		// % protected region % [Configure LoginToken implementation here] end
	
		// % protected region % [Add any further methods here] off begin
		// % protected region % [Add any further methods here] end
	}
}
