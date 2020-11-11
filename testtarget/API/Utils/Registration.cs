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
using System.Text.RegularExpressions;
using APITests.Exceptions;
using APITests.Setup;
using APITests.EntityObjects.Models;
using Newtonsoft.Json.Linq;
using RestSharp;
using Xunit.Abstractions;

namespace APITests.Utils
{
	public class Registration
	{
		private readonly StartupTestFixture _configure = new StartupTestFixture();

		public Registration(UserBaseEntity userBaseEntity, ITestOutputHelper testOutputHelper)
		{
			var endpointName = userBaseEntity.EndpointName;
			var clientxsrf = ClientXsrf.GetValidClientAndxsrfTokenPair(_configure);
			var client = clientxsrf.client;
			client.BaseUrl = new Uri(_configure.BaseUrl + $"/api/register/{endpointName}");
			var request = new RestRequest { Method = Method.POST, RequestFormat = DataFormat.Json };
			request.AddHeader("X-XSRF-TOKEN", clientxsrf.xsrfToken);
			request.AddHeader("Content-Type", "application/json");
			request.AddParameter("query", userBaseEntity.ToJson(), ParameterType.RequestBody);
			var response = client.Execute(request);

			ApiOutputHelper.WriteRequestResponseOutput(request, response, testOutputHelper);

			var errorsList = new List<Exception>();

			if (response.StatusCode != System.Net.HttpStatusCode.OK)
			{
				errorsList.Add(new UnexpectedResponseCodeException($"Expected Status Code: {System.Net.HttpStatusCode.OK}, Got: {response.StatusCode}"));
			}

			// get the list of error messages
			var errorMessages = JObject.Parse(response?.Content)["errors"]?.Select(x => x["message"]?.ToString()).ToList() ?? new List<string>();

			// get new exceptions for each of these so an aggregate exception can be thrown
			foreach (var errorMessage in errorMessages)
			{
				var accountExistsPattern = Regex.Matches(errorMessage, "User name '(.*)' is already taken.");
				if (accountExistsPattern.Any())
				{
					errorsList.Add(new UserExistsException(errorMessage));
				}
				switch (errorMessage)
				{
					case "Passwords must have at least one non alphanumeric character.":
						errorsList.Add(new PasswordNonAlphanumericException(errorMessage));
						break;
					case "Passwords must have at least one digit ('0'-'9').":
						errorsList.Add(new PasswordAtLeastOneDigitException(errorMessage));
						break;
					case "Passwords must have at least one uppercase ('A'-'Z').":
						errorsList.Add(new PasswordAtLeastOneUpperCaseCharacterException(errorMessage));
						break;
					case "The Email field is not a valid e-mail address.":
					case "Email is not a valid email":
						errorsList.Add(new InvalidEmailAddressException(errorMessage));
						break;
					case "This account is not yet activated":
						errorsList.Add(new AccountNotActivatedException(errorMessage));
						break;
					case "Passwords must be at least 6 characters.":
						errorsList.Add(new PasswordLengthException(errorMessage));
						break;
					// % protected region % [Add any further cases here] off begin
					// % protected region % [Add any further cases here] end
					default:
						errorsList.Add( new Exception($"An error returned which could not be handled, error message: {errorMessage}"));
						break;
				}
			}

			if (errorMessages.Any())
			{
				throw new AggregateException(errorsList);
			}
		}
	}
}