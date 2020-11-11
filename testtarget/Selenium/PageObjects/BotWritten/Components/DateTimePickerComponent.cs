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
using SeleniumTests.Enums;
using SeleniumTests.Setup;
using SeleniumTests.Utils;

// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace SeleniumTests.PageObjects.Components
{
	public class DateTimePickerComponent : IDatePickerComponent, ITimePickerComponent
	{
		public DatePickerComponent datePickerComponent;
		public TimePickerComponent timePickerComponent;

		public IWebElement DateTimePickerElement { get; set; }
		internal IWebElement TriggeredCalendar => datePickerComponent.TriggeredCalendar;
		internal IWebElement DatePickerYearElement => datePickerComponent.DatePickerYearElement;
		internal IWebElement DatePickerMonthElement => datePickerComponent.DatePickerMonthElement;

		public DateTimePickerComponent(ContextConfiguration contextConfiguration, IWebElement dateTimePickerElement)
		{
			DateTimePickerElement = dateTimePickerElement;
			datePickerComponent = new DatePickerComponent(contextConfiguration, dateTimePickerElement);
			timePickerComponent = new TimePickerComponent(contextConfiguration, dateTimePickerElement);
		}

		public DateTimePickerComponent(ContextConfiguration contextConfiguration, string className)
		{
			WaitUtils.elementState(contextConfiguration.WebDriverWait, By.CssSelector($".{className}"), ElementState.EXISTS);
			DateTimePickerElement = contextConfiguration.WebDriver.FindElement(By.CssSelector($".{className}"));
			datePickerComponent = new DatePickerComponent(contextConfiguration, className);
			timePickerComponent = new TimePickerComponent(contextConfiguration, className);
		}

		public void SetDate(DateTime dateTime) => datePickerComponent.SetDate(dateTime);

		public void SetDateRange(DateTime startDate, DateTime endDate)
		{
			datePickerComponent.SetDateRange(startDate, endDate);
		}

		/// <summary>
		/// Sets a date and a time
		/// </summary>
		/// <param name="dateTime">The datetime that should be set</param>
		public void SetDateTime(DateTime dateTime)
		{
			SetDate(dateTime);
			SetTime(dateTime);
		}

		/// <summary>
		/// Sets a date time range
		/// </summary>
		/// <param name="startDateTime">The starting date of the range</param>
		/// <param name="endDateTime">The end date of the range</param>
		public void SetDateTimeRange(DateTime startDateTime, DateTime endDateTime)
		{
			SetDate(startDateTime);
			SetTime(startDateTime);
			SetDate(endDateTime);
			SetTime(endDateTime);
		}

		/// <summary>
		/// Inputs the given time as hour, minute and AM/PM into the time picker
		/// by clicking on the web element with the given className.
		/// </summary>
		/// <param name="time">Time to input</param>
		public void SetTime(DateTime time) => timePickerComponent.SetTime(time);

		public void Close() => timePickerComponent.Close();
	}
}