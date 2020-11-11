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
using System.Runtime.InteropServices;
using OpenQA.Selenium;
using SeleniumTests.Enums;

namespace SeleniumTests.Utils
{
	internal static class KeyboardUtils
	{
		public static void EnterKeyToWebElement(IWebElement element, KeyboardActionType keyboardActionType)
		{
			switch (keyboardActionType)
			{
				case KeyboardActionType.ENTER:
					element.SendKeys(Keys.Enter);
					break;
				case KeyboardActionType.ESCAPE:
					element.SendKeys(Keys.Escape);
					break;
				case KeyboardActionType.TAB:
					element.SendKeys(Keys.Tab);
					break;
				default:
					throw new Exception($"Key {keyboardActionType} is not defined in util");
			}
		}

		public static void DeleteAllFromWebElement(IWebElement element)
		{
			SelectAllFromWebElement(element);
			element.SendKeys(Keys.Backspace);
		}

		public static void CopyFromWebElement(IWebElement element)
		{
			element.SendKeys(GetCommandControlInput("c"));
		}

		public static void PasteToWebElement(IWebElement element)
		{
			element.SendKeys(GetCommandControlInput("v"));
		}

		public static void SelectAllFromWebElement(IWebElement element)
		{
			element.SendKeys(GetCommandControlInput("a"));
		}

		/// <summary>
		/// Allows for user control input based on operating system, the case of OSX command key must
		/// be handled.
		/// </summary>
		/// <param name="commandKeyPair">The key which is coupled with the command/control key</param>
		/// <returns>command/control key with its paired key</returns>
		public static string GetCommandControlInput(string commandPairedKey)
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				return Keys.Command + commandPairedKey;
			}
			else
			{
				return Keys.Control + commandPairedKey;
			}
		}

		/// <summary>
		/// Sends an integer of any given length as input via the given Selenium Web Driver.
		/// </summary>
		/// <param name="value">integer value to send</param>
		/// <param name="element">any input web element</param>
		public static void SendIntAsDigits(int value, IWebElement element)
		{
			var digitString = value.ToString();
			var digitArray = digitString.ToCharArray();
			foreach (var digit in digitArray)
			{
				element.SendKeys($"{digit}");
			}
		}
	}
}