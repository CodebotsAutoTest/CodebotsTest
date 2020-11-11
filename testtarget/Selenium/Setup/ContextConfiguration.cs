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
using System.Drawing;
using System.Globalization;
using System.IO;
using APITests.Settings;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using Xunit.Abstractions;

namespace SeleniumTests.Setup
{
	public class ContextConfiguration
	{
		private readonly SeleniumSettings _seleniumSettings;

		public ContextConfiguration(ITestOutputHelper testOutputHelper)
		{
			//load in site configuration
			var siteConfiguration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddIniFile("SiteConfig.ini", optional: true, reloadOnChange: false)
				.Build();

			var siteSettings = new SiteSettings();
			siteConfiguration.GetSection("site").Bind(siteSettings);

			var baseUrlFromEnvironment = Environment.GetEnvironmentVariable("BASE_URL");
			BaseUrl = baseUrlFromEnvironment ?? siteSettings.BaseUrl;

			// as soon as the site url is given we will test its connection and immediately fail if necessary
			APITests.Utils.PingServer.TestConnection(BaseUrl);

			//load in the selenium configuration configuration
			var seleniumConfiguration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddIniFile("SeleniumConfig.ini", optional: true, reloadOnChange: false)
				.Build();

			_seleniumSettings = new SeleniumSettings();
			seleniumConfiguration.GetSection("selenium").Bind(_seleniumSettings);
			seleniumConfiguration.GetSection("screensize").Bind(_seleniumSettings);
			seleniumConfiguration.GetSection("screenshot").Bind(_seleniumSettings);
			seleniumConfiguration.GetSection("cultureinfo").Bind(_seleniumSettings);

			CultureInfo = new CultureInfo(_seleniumSettings.Locale);

			//load in base choice configuration
			var baseChoiceConfiguration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddIniFile("BaseChoiceConfig.ini", optional: true, reloadOnChange: false)
				.Build();

			var baseChoiceSettings = new BaseChoiceSettings();
			baseChoiceConfiguration.GetSection("basechoice").Bind(baseChoiceSettings);

			//load in the user configurations
			var userConfiguration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddIniFile("UserConfig.ini", optional: true, reloadOnChange: false)
				.Build();

			var superUserSettings = new UserSettings();
			var testUserSettings = new UserSettings();
			userConfiguration.GetSection("super").Bind(superUserSettings);
			userConfiguration.GetSection("test").Bind(testUserSettings);
			// % protected region % [Add any additional user settings here] off begin
			// % protected region % [Add any additional user settings here] end

			TestUserConfiguration = testUserSettings;
			SuperUserConfiguration = superUserSettings;
			SeleniumSettings = _seleniumSettings;
			BaseChoiceSettings = baseChoiceSettings;

			// % protected region % [Set any other variables here] off begin
			// % protected region % [Set any other variables here] end

			WebDriver = InitialiseWebDriver();
			TestOutputHelper = testOutputHelper;
			WebDriverWait = InitializeWebDriverWait(WebDriver, _seleniumSettings.Timeout, _seleniumSettings.PollInterval);
		}

		public readonly ITestOutputHelper TestOutputHelper;
		public CultureInfo CultureInfo { get; set; }
		public string BaseUrl { get; set; }
		public UserSettings TestUserConfiguration { get; set; }
		public UserSettings SuperUserConfiguration { get; set; }
		public SeleniumSettings SeleniumSettings { get; set; }
		public BaseChoiceSettings BaseChoiceSettings { get; set; }
		public IWebDriver WebDriver { get; set; }
		public IWait<IWebDriver> WebDriverWait { get; set; }
		// % protected region % [Add any additional public variables here] off begin
		// % protected region % [Add any additional public variables here] end

		public void WriteTestOutput(string text) => TestOutputHelper.WriteLine(text);

		public IWebDriver InitialiseWebDriver()
		{
			// get the driver type from the configuration
			var driverType = Environment.GetEnvironmentVariable("LM2348_TEST_SELENIUM_WEB_DRIVER")
								?? _seleniumSettings.Webdriver.ToLower();

			switch (driverType)
			{
				case "chrome":
				case "chrome-edge":
					var chromeOptions = new ChromeOptions();

					// % protected region % [The default chrome driver variables are set , change to suit your needs] off begin
					if (_seleniumSettings.Headless)
					{
						chromeOptions.AddArguments("--silent-launch");
						chromeOptions.AddArguments("--no-startup-window");
						chromeOptions.AddArguments("--no-sandbox");
						chromeOptions.AddArguments("--headless");
						chromeOptions.AddArguments("--allow-insecure-localhost");
						chromeOptions.AddArguments("--disable-gpu");
						chromeOptions.AddAdditionalCapability("acceptInsecureCerts", true, true);
					}

					// the screensize is set custom if it is either headless or overwritten flag is active
					if (_seleniumSettings.OverwriteDefault || _seleniumSettings.Headless)
					{
						chromeOptions.AddArguments($"--window-size={_seleniumSettings.Width},{_seleniumSettings.Height}");
					}
					else
					{
						chromeOptions.AddArguments("--start-maximized");
					}

					// % protected region % [The default chrome driver variables are set , change to suit your needs] end

					/*
					* for different chromium browsers we will need to specify the binary
					* path to tell it which version to use
					*/
					string chromeDriverDirectory;
					if (driverType == "chrome")
					{
						chromeDriverDirectory = ".";
					}
					else if (driverType == "chrome-edge")
					{
						chromeDriverDirectory = "./EdgeChromiumDriver";
						var binaryPath = _seleniumSettings.EdgeChromiumPath;
						chromeOptions.BinaryLocation = binaryPath;
					}
					else
					{
						throw new Exception("Could not find chromium driver");
					}

					// chrome options are shared between chromium drivers
					WebDriver = new ChromeDriver(ChromeDriverService.CreateDefaultService(chromeDriverDirectory), chromeOptions, TimeSpan.FromMinutes(3));
					break;
				case "firefox":
					var firefoxOptions = new FirefoxOptions();

					/*
					* this is required to fix a but in dotnet core where there is an implicit timeout waiting
					* for ipv6 to resolve. See: https://github.com/SeleniumHQ/selenium/issues/6597
					*/
					var service = FirefoxDriverService.CreateDefaultService(".");
					service.Host = "::1";

					// % protected region % [The default firefox driver variables are set , change to suit your needs] off begin
					if (_seleniumSettings.Headless)
					{
						firefoxOptions.AddArguments("--silent-launch");
						firefoxOptions.AddArguments("--no-startup-window");
						firefoxOptions.AddArguments("--no-sandbox");
						firefoxOptions.AddArguments("--headless");
						firefoxOptions.AddArguments("--allow-insecure-localhost");
						firefoxOptions.AddArguments("--disable-gpu");
						firefoxOptions.AddAdditionalCapability("acceptInsecureCerts", true, true);
					}

					// % protected region % [The default firefox driver variables are set , change to suit your needs] end

					WebDriver = new FirefoxDriver(service, firefoxOptions);

					// % protected region % [default driver operations for firefox, change to suit your needs] off begin
					if (_seleniumSettings.OverwriteDefault|| _seleniumSettings.Headless)
					{
						WebDriver.Manage().Window.Size = new Size(_seleniumSettings.Width, _seleniumSettings.Height);
					}
					else
					{
						WebDriver.Manage().Window.Maximize();
					}
					// % protected region % [default driver operations for firefox, change to suit your needs] end
					break;
				case "ie":
					WebDriver = new InternetExplorerDriver();

					// % protected region % [Add any internet explorer driver options here] off begin
					// % protected region % [Add any internet explorer driver options here] end
					break;
				case "edge":
					WebDriver = new EdgeDriver();

					// % protected region % [Add any edge driver options here] off begin
					// % protected region % [Add any edge driver options here] end
					break;
				default:
					//default to using a chrome driver which is maximised
					var defaultOptions = new ChromeOptions();
					defaultOptions.AddArguments(new List<string>() {
						"--start-maximized"
						,});
					WebDriver = new ChromeDriver(".", defaultOptions);
					break;
			}

			// % protected region % [Add any additional WebDriver configuration here] off begin
			// % protected region % [Add any additional WebDriver configuration here] end

			return WebDriver;
		}

		public static IWait<IWebDriver> InitializeWebDriverWait(IWebDriver webDriver, int timeout, int pollInterval)
		{
			return  new WebDriverWait(webDriver, TimeSpan.FromMilliseconds(timeout))
			{
				PollingInterval = TimeSpan.FromMilliseconds(pollInterval)
			};
		}
	}
}
