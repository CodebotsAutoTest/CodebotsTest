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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using BoDi;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using Xunit.Abstractions;

namespace SeleniumTests.Setup
{
	[Binding]
	public class SpecflowScenarioContext : IDisposable
	{
		private readonly IObjectContainer _objectContainer;
		private readonly ContextConfiguration _contextConfiguration;
		private readonly IWebDriver _webDriver;
		private readonly ITestOutputHelper _output;

		public SpecflowScenarioContext(ITestOutputHelper testOutputHelper, IObjectContainer objectContainer)
		{
			// we need to get the test name out so we can save a screenshot at the end of the test with the name
			_output = testOutputHelper;
			var test = (ITest)_output.GetType().GetField("test", BindingFlags.NonPublic | BindingFlags.Instance)
								 .GetValue(_output);
			TestName = test.DisplayName;

			_contextConfiguration = new ContextConfiguration(testOutputHelper);
			_objectContainer = objectContainer;

			// register the context configuration so it may be injected into the given steps
			_objectContainer.RegisterInstanceAs(_contextConfiguration, typeof(ContextConfiguration));
			_webDriver = _contextConfiguration.WebDriver;
		}

		public string TestName { get; set; }

		/// <summary>
		/// Action to perform before scenario is setup
		/// </summary>
		/// <remarks>
		/// Required for proper disposal of this context
		/// </remarks>
		[BeforeScenario]
		public void BeforeScenarioSetup()
		{
			_output.WriteLine($"Running Test: {TestName}");
		}

		public static void ScreenShotTest(SpecflowScenarioContext context)
		{
			if (context._contextConfiguration.SeleniumSettings.EnableScreenshots)
			{
				var ss = ((ITakesScreenshot)context._webDriver).GetScreenshot();
				var screenshotFileName = context.TestName.Replace(" ", "-");
				screenshotFileName = string.Concat(screenshotFileName.Split(Path.GetInvalidFileNameChars()));
				screenshotFileName = $"{string.Concat(screenshotFileName.Take(25))}-{CalculateMD5Hash(screenshotFileName).Substring(0,6)}";

				var screenshotPath = $"../../../TestResults/{screenshotFileName}.png";

				context._contextConfiguration.WriteTestOutput($"Wrote test screenshot output to: {Path.GetFullPath(screenshotPath)}");
				ss.SaveAsFile(screenshotPath, ScreenshotImageFormat.Png);
			}
		}

		public void Dispose()
		{
			if (_contextConfiguration.SeleniumSettings.EnablePostTestScreenshot)
			{
				try
				{
					ScreenShotTest(this);
				}
				catch (Exception ex)
				{
					_output.WriteLine($"Caught Exception While taking screenshot: {ex}");
				}
			}

			if (_contextConfiguration.WebDriver != null)
			{
				_contextConfiguration.WebDriver.Quit();
				_contextConfiguration.WebDriver = null;
				_contextConfiguration.WebDriverWait = null;
			}
		}

		[BeforeTestRun]
		public static void BeforeTestRun()
		{
			CleanTestResults();
		}

		/// <summary>
		/// Before a test run we usually want to clean out the screenshots
		/// from the last set of tests as they are no longer relevant. If we
		/// do not have the test results directory then we want to create it
		/// </summary>
		private static void CleanTestResults()
		{
			var di = new DirectoryInfo("../../../TestResults");

			if (di.Exists)
			{
				foreach (var file in di.GetFiles())
				{
					if (file.Extension == ".png")
					{
						file.Delete();
					}
				}
			}
			else
			{
				di.Create();
			}
		}

		private static string CalculateMD5Hash(string input)
		{
			// calculate MD5 hash from input
			var md5 = MD5.Create();
			var inputBytes = Encoding.ASCII.GetBytes(input);
			var hash = md5.ComputeHash(inputBytes);

			// convert byte array to hex string
			var sb = new StringBuilder();
			for (var i = 0; i < hash.Length; i++)
			{
				sb.Append(hash[i].ToString("x2"));
			}
			return sb.ToString();
		}
	}
}