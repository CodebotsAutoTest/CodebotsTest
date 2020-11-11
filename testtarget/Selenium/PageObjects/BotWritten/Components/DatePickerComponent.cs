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
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SeleniumTests.Setup;
using SeleniumTests.Utils;

// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace SeleniumTests.PageObjects.Components
{
	internal interface IDatePickerComponent
	{
		void SetDate(DateTime date);
		void SetDateRange(DateTime startDate, DateTime endDate);
	}

	public class DatePickerComponent : BaseSection, IDatePickerComponent
	{
		internal enum Months
		{
			January = 1,
			February = 2,
			March = 3,
			April = 4,
			May = 5,
			June = 6,
			July = 7,
			August = 8,
			September = 9,
			October = 10,
			November = 11,
			December = 12
		};

		internal IWebDriver _driver;
		public IWebElement DatePickerElement { get; set; }
		public IWebElement TriggeredCalendar => FindElementExt("TriggeredCalendar");
		public IWebElement DatePickerYearElement => FindElementExt(TriggeredCalendar, "YearElement");
		public IWebElement DatePickerMonthElement => FindElementExt(TriggeredCalendar, "MonthElement");

		public DatePickerComponent(ContextConfiguration contextConfiguration, string className) : base(contextConfiguration)
		{
			_driver = contextConfiguration.WebDriver;
			DatePickerElement = _driver.FindElement(By.CssSelector($".{className}"));
			initializeSelectors();
		}

		public DatePickerComponent(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
			_driver = contextConfiguration.WebDriver;
			initializeSelectors();
		}

		public DatePickerComponent(ContextConfiguration contextConfiguration, IWebElement datePickerElement) : base(contextConfiguration)
		{
			DatePickerElement = datePickerElement;
			_driver = contextConfiguration.WebDriver;
			initializeSelectors();
		}

		private void initializeSelectors()
		{
			selectorDict.Add("TriggeredCalendar", (selector: "//div[contains(@class, 'flatpickr-calendar') and contains(@class ,'open')]", type: SelectorType.XPath));
			selectorDict.Add("YearElement", (selector: ".numInput.cur-year", type: SelectorType.CSS));
			selectorDict.Add("MonthElement", (selector: ".flatpickr-monthDropdown-months", type: SelectorType.CSS));
		}

		/// <summary>
		/// Inputs the given date as year, month and day into the date picker
		/// by clicking on the web element with the given className.
		/// </summary>
		/// <param name="date">Date to input</param>
		public void SetDate(DateTime date)
		{
			DatePickerElement.Click();
			SelectCalendarYear(date.Year);
			SelectCalendarMonth(date.Month);
			SelectCalendarDay(date.Day);
			new Actions(_driver).SendKeys(Keys.Escape).Perform();
		}

		/// <summary>
		/// Inputs the given date range into the date picker by
		/// clicking on the web element with the given className.
		/// </summary>
		/// <param name="startDate">Start Date to input</param>
		/// <param name="endDate">End Date to input</param>
		public void SetDateRange(DateTime startDate, DateTime endDate)
		{
			SetDate(startDate);
			SetDate(endDate);
		}

		internal void SelectCalendarYear(int year)
		{
			DatePickerYearElement.Clear();
			DatePickerYearElement.Click();
			KeyboardUtils.SendIntAsDigits(year, DatePickerYearElement);
		}

		internal void SelectCalendarMonth(int month)
		{
			var calendarMonths = DatePickerMonthElement.FindElements(By.CssSelector(".flatpickr-monthDropdown-month"));
			DatePickerMonthElement.Click();
			calendarMonths.FirstOrDefault(x => x.Text == ((Months) month).ToString())?.Click();
		}

		internal void SelectCalendarDay(int day)
		{
			var calendarDays = TriggeredCalendar.FindElements(By.XPath("//*[not(contains(@class,'prevMonthDay') or contains(@class,'nextMonthDay')) and (contains(@class,'flatpickr-day'))]"));
			foreach (var dayElement in calendarDays)
			{
				if (!dayElement.Text.Equals(day.ToString()))
				{
					continue;
				}
				dayElement.Click();
				break;
			}
		}
	}
}