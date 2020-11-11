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
using APITests.Classes;
using APITests.Utils;
using RestSharp;
using Xunit.Abstractions;

// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace APITests.Setup
{
	class WebApi
	{
		private readonly StartupTestFixture _configure;
		private readonly ITestOutputHelper _output;

		public Dictionary<string, string> CommonHeaders { get; } = new Dictionary<string, string>();

		/// <summary>
		/// Create a new test web api.
		/// </summary>
		/// <param name="configure">configuration options for the particular test</param>
		/// <param name="output">test output logger</param>
		public WebApi(StartupTestFixture configure, ITestOutputHelper output)
		{
			_configure = configure;
			_output = output;
		}

		/// <summary>
		/// Create a new test web api.
		/// </summary>
		/// <param name="configure">configuration options for the particular test</param>
		public WebApi(StartupTestFixture configure)
		{
			_configure = configure;
		}

		/// <summary>
		/// Sets the most common headers shared by enpoints of the application.
		/// </summary>
		public void SetDefaultHeaders()
		{
			CommonHeaders["Content-Type"] = "application/json";
			CommonHeaders["Accept"] = "application/json, text/html, */*";
		}

		// % protected region % [Customize ConfigureAuthenticationHeaders here] off begin
		/// <summary>
		/// Configures authentication for the specified user, if none is provided the super account
		/// is used.
		/// </summary>
		/// <param name="userName">The username of the user to configure authentication for</param>
		/// <param name="password">The password of the user to configure authentication for</param>
		public void ConfigureAuthenticationHeaders(string userName = null, string password = null)
		{
			userName ??= _configure.SuperUsername;
			password ??= _configure.SuperPassword;

			var loginToken = new LoginToken(_configure.BaseUrl, userName, password);

			CommonHeaders["Authorization"] = $"{loginToken.TokenType} {loginToken.AccessToken}";
		}
		// % protected region % [Customize ConfigureAuthenticationHeaders here] end

		/// <summary>
		/// Get an end point with the specified parameters and headers.
		/// </summary>
		/// <param name="url">The Endpoint</param>
		/// <param name="param">The Request Body/Parameters</param>
		/// <param name="headers">The Request Headers</param>
		/// <param name="dataFormat">XML or JSon Body</param>
		/// <returns>The Requests Response</returns>
		public IRestResponse Get(
			string url, 
			Dictionary<string,string> param = null, 
			Dictionary<string, string> headers = null,
			DataFormat dataFormat = DataFormat.Json)
		{
			return RequestEndpoint(Method.GET, url, param, headers, dataFormat);
		}

		/// <summary>
		/// Post to an end point with the specified parameters and headers.
		/// </summary>
		/// <param name="url">The Endpoint</param>
		/// <param name="body">The Request Body/Parameters</param>
		/// <param name="headers">The Request Headers</param>
		/// <param name="dataFormat">XML or JSon Body</param>
		/// <param name="files">Files to attatch to the request</param>
		/// <returns>The Requests Response</returns>
		public IRestResponse Post(
			string url,
			RestSharp.JsonObject body = null,
			Dictionary<string, string> headers = null,
			DataFormat dataFormat = DataFormat.Json,
			IEnumerable<FileData> files = null)
		{
			return RequestEndpoint(Method.POST, url, body, headers, dataFormat, files);
		}

		/// <summary>
		/// Post to an end point with the specified parameters and headers.
		/// </summary>
		/// <param name="url">The Endpoint</param>
		/// <param name="body">The Request Body/Parameters</param>
		/// <param name="headers">The Request Headers</param>
		/// <param name="dataFormat">XML or JSon Body</param>
		/// <param name="files">Files to attatch to the request</param>
		/// <returns>The Requests Response</returns>
		public IRestResponse Post(
			string url,
			JsonArray body = null,
			Dictionary<string, string> headers = null,
			DataFormat dataFormat = DataFormat.Json,
			IEnumerable<FileData> files = null)
		{
			return RequestEndpoint(Method.POST, url, body, headers, dataFormat, files);
		}
		
		/// <summary>
		/// Post to an end point with the specified parameters and headers.
		/// </summary>
		/// <param name="url">The Endpoint</param>
		/// <param name="body">The Request Body/Parameters</param>
		/// <param name="headers">The Request Headers</param>
		/// <param name="dataFormat">XML or JSon Body</param>
		/// <param name="files">Files to attatch to the request</param>
		/// <returns>The Requests Response</returns>
		public IRestResponse Post(
			string url,
			Dictionary<string, object> body = null,
			Dictionary<string, string> headers = null,
			DataFormat dataFormat = DataFormat.Json,
			IEnumerable<FileData> files = null)
		{
			return RequestEndpoint(Method.POST, url, body, headers, dataFormat, files);
		}

		/// <summary>
		/// Put request to an end point with the specified parameters and headers.
		/// </summary>
		/// <param name="url">The Endpoint</param>
		/// <param name="param">The Request Body/Parameters</param>
		/// <param name="headers">The Request Headers</param>
		/// <param name="dataFormat">XML or JSon Body</param>
		/// <returns>The Requests Response</returns>
		public IRestResponse Put(
			string url,
			RestSharp.JsonObject body = null,
			Dictionary<string, string> headers = null,
			DataFormat dataFormat = DataFormat.Json)
		{
			return RequestEndpoint(Method.PUT, url, body, headers, dataFormat);
		}

		/// <summary>
		/// Put request to an end point with the specified parameters and headers.
		/// </summary>
		/// <param name="url">The Endpoint</param>
		/// <param name="param">The Request Body/Parameters</param>
		/// <param name="headers">The Request Headers</param>
		/// <param name="dataFormat">XML or JSon Body</param>
		/// <returns>The Requests Response</returns>
		public IRestResponse Put(
			string url,
			JsonArray body = null,
			Dictionary<string, string> headers = null,
			DataFormat dataFormat = DataFormat.Json)
		{
			return RequestEndpoint(Method.PUT, url, body, headers, dataFormat);
		}

		/// <summary>
		/// Delete request to an end point with the specified parameters and headers
		/// </summary>
		/// <param name="url">The Endpoint</param>
		/// <param name="param">The Request Body/Parameters</param>
		/// <param name="headers">The Request Headers</param>
		/// <param name="dataFormat">XML or JSon Body</param>
		/// <returns>The Requests Response</returns>
		public IRestResponse Delete(
			string url,
			Dictionary<string, string> param = null,
			Dictionary<string, string> headers = null,
			DataFormat dataFormat = DataFormat.Json)
		{
			return RequestEndpoint(Method.DELETE, url, param, headers);
		}

		/// <summary>
		/// Perform a specific request type to an end point with the specified parameters and headers,
		/// </summary>
		/// <param name="method">The specified request method type</param>
		/// <param name="url">The Endpoint</param>
		/// <param name="param">The Request Body/Parameters</param>
		/// <param name="headers">The Request Headers</param>
		/// <param name="dataFormat">XML or JSon Body</param>
		/// <param name="files">Files to attatch to the request</param>
		public IRestResponse RequestEndpoint(
			Method method, 
			string url, 
			object param = null, 
			Dictionary<string, string> headers = null, 
			DataFormat dataFormat= DataFormat.Json,
			IEnumerable<FileData> files = null
		)
		{
			// Setup the rest client
			var client = new RestClient
			{
				BaseUrl = new Uri(_configure.BaseUrl + url)
			};

			// Setup the request
			var request = new RestRequest
			{
				Method = method,
			};
		
			// Merge the two dictionaries together
			var requestHeaders = new Dictionary<string, string>();
			if (headers != null)
			{
				foreach (var (key, value) in headers)
				{
					requestHeaders.TryAdd(key, value);
				}
			}
			if (CommonHeaders != null)
			{
				foreach (var (key, value) in CommonHeaders)
				{
					requestHeaders.TryAdd(key, value);
				}
			}

			// Set all the headers to the request
			if (requestHeaders.Count > 0)
			{
				foreach (var (key, value) in requestHeaders)
				{
					request.AddHeader(key, value);
				}
			}

			if (param != null)
			{
				request.RequestFormat = dataFormat;

				switch (dataFormat)
				{
					case DataFormat.Json:
						request.AddJsonBody(param);
						break;
					case DataFormat.Xml:
						request.AddXmlBody(param);
						break;
					case DataFormat.None:
						break;
					default:
						request.AddJsonBody(param);
						break;
				}
			}
			
			if (files != null)
			{
				if (param is Dictionary<string, object> paramDictionary)
				{
					foreach (var paramItem in paramDictionary)
					{
						request.AddParameter(paramItem.Key, paramItem.Value, ParameterType.GetOrPost);
					}
				}
				else
				{
					throw new Exception("The param format is invalid");
				}

				foreach (var file in files)
				{
					request.AddFile(file.Id.ToString(), file.Data, file.Filename);
				}
			}
			
			// Execute the request
			var response = client.Execute(request);

			if (_output != null)
			{
				ApiOutputHelper.WriteRequestResponseOutput(request, response, _output);
			}

			return response;
		}
	}
}