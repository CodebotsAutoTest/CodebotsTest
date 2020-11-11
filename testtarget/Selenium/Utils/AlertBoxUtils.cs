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

namespace SeleniumTests.Utils
{
	internal static class AlertBoxUtils
	{
		/// <summary>
		/// Check if an alert box is currently present on the page
		/// </summary>
		/// <param name="webDriver"></param>
		/// <returns></returns>
		public static bool AlertBoxIsPresent(IWebDriver webDriver)
		{
			try
			{
				var alertBox = webDriver.SwitchTo().Alert();
				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Returns the message contained in the current alert box
		/// </summary>
		/// <param name="webDriver"></param>
		/// <returns></returns>
		public static string ReadAlertBoxMessage(IWebDriver webDriver)
		{
			var alertBox = webDriver.SwitchTo().Alert();
			var alertBoxMessage = alertBox.Text;
			return alertBoxMessage;
		}

		/// <summary>
		/// Writes a message to the current alert box
		/// </summary>
		/// <param name="webDriver"></param>
		/// <param name="message"></param>
		public static void WriteToAlertBox(IWebDriver webDriver, string message)
		{
			var alertBox = webDriver.SwitchTo().Alert();
			alertBox.SendKeys(message);
		}

		/// <summary>
		/// Accepts of Dismissed the current alert box
		/// </summary>
		/// <param name="webDriver"></param>
		/// <param name="confirmation"></param>
		public static void AlertBoxHandler(IWebDriver webDriver, bool confirmation)
		{
			var alertBox = webDriver.SwitchTo().Alert();
			if (confirmation)
			{
				alertBox.Accept();
			}
			else
			{
				alertBox.Dismiss();
			}
			webDriver.SwitchTo().DefaultContent();
		}
	}
}