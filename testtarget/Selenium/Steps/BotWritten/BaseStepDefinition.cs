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

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumTests.Setup;
using TechTalk.SpecFlow;
using Xunit.Abstractions;

namespace SeleniumTests.Steps.BotWritten
{
	[Binding]
	public class BaseStepDefinition
	{
		protected readonly IWebDriver _driver;
		protected readonly IWait<IWebDriver> _driverWait;
		protected readonly string _baseUrl;
		protected readonly ITestOutputHelper _testOutputHelper;
		protected readonly ContextConfiguration _contextConfiguration;
		// % protected region % [Declare any additional properties here] off begin
		// % protected region % [Declare any additional properties here] end

		public BaseStepDefinition(ContextConfiguration contextConfiguration)
		{
			_driver = contextConfiguration.WebDriver;
			_driverWait = contextConfiguration.WebDriverWait;
			_baseUrl = contextConfiguration.BaseUrl;
			_testOutputHelper = contextConfiguration.TestOutputHelper;
			_contextConfiguration = contextConfiguration;
			// % protected region % [Add any additional setup options here] off begin
			// % protected region % [Add any additional setup options here] end
		}
	}
}