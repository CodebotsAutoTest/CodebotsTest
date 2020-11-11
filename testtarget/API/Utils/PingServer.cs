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
using RestSharp;

namespace APITests.Utils
{
	public static class PingServer
	{
		/// <summary>
		/// Pings the server to determine if it can get a connection
		/// </summary>
		public static void TestConnection(string url)
		{
			//setup the rest client
			var client = new RestClient
			{
				BaseUrl = new Uri($"{url}")
			};

			//setup the request
			var request = new RestRequest
			{
				Method = Method.GET,
				RequestFormat = DataFormat.Json
			};

			// execute the request
			var response = client.Execute(request);

			if (response.StatusCode != HttpStatusCode.OK)
			{
				throw new Exception($"Could not reach {url}, Response Code: {response.StatusCode}");
			}
		}
	}
}