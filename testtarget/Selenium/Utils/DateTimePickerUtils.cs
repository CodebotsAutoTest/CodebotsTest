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
using OpenQA.Selenium;

namespace SeleniumTests.Utils
{
	/// <summary>
	/// <para>Utility class for testing the React Date-Time components.</para>
	/// <para>
	/// Provides useful, reusable methods for interacting with the DatePicker, DateRangePicker, DateTimePicker,
	/// DateTimeRangePicker and TimePicker components.
	/// </para>
	/// <para>Currently private methods may be made public if appropriate.</para>
	/// </summary>
	internal static class DateTimePickerUtils
	{
		/// <summary>
		/// Inputs the given DateTime into the standard date picker opened by clicking on the HTML element with the
		/// given className.
		/// </summary>
		/// <param name="webDriver">
		/// 	currently active Selenium web driver
		/// </param>
		/// <param name="className">
		/// 	.css class name of an element which triggers a Flatpickr - expected to be unique within the DOM
		/// </param>
		/// <param name="date">
		///		DateTime to input
		/// </param>
		public static void EnterDateByClassName(IWebDriver webDriver, string className, DateTime date)
		{
			IJavaScriptExecutor js = (IJavaScriptExecutor)webDriver;
			var script = $@"var input = document.getElementsByClassName('{className}')[0].lastElementChild;
				var setValue = Object.getOwnPropertyDescriptor(window.HTMLInputElement.prototype, 'value').set;
				 setValue.call(input, '{date.ToString("yyyy'-'MM'-'dd")}');
				var e = new Event('input',  {{bubbles: true}} );
				input.dispatchEvent(e);";
			js.ExecuteScript(script);
		}
	}
}
