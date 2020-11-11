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

using System.Linq;
using System.Collections.Generic;
using OpenQA.Selenium;
using SeleniumTests.Setup;

namespace SeleniumTests.PageObjects.BotWritten.Forms
{
	public class FormsBuildPage : BasePage
	{
		public IWebElement NewSlideButton => FindElementExt("NewSlideButton");
		public IWebElement SlideOptionsButton => FindElementExt("SlideOptionsButton");
		public IWebElement SlideNameInput => FindElementExt("SlideNameInput");
		public IWebElement SaveSlideNameButton => FindElementExt("SaveSlideNameButton");
		public IWebElement AddQuestionButton => FindElementExt("AddQuestionButton");
		public IWebElement QuestionOptionsButton => FindElementExt("QuestionOptionsButton");
		public IWebElement QuestionNameInput => FindElementExt("QuestionNameInput");
		public IWebElement SaveQuestionNameButton => FindElementExt("SaveQuestionNameButton");
		public IWebElement SaveAndPublishButton => FindElementExt("SaveAndPublishButton");

		public FormsBuildPage(ContextConfiguration contextConfiguration) : base(contextConfiguration)
		{
			InitializeSelectors();
		}

		private void InitializeSelectors()
		{
			selectorDict.Add("NewSlideButton", (selector: "div.slide-builder__list > .icon-plus", type: SelectorType.CSS));
			selectorDict.Add("SlideOptionsButton", (selector: "section.accordion > .icon-more-horizontal", type: SelectorType.CSS));
			selectorDict.Add("SlideNameInput", (selector: "div.slide-builder__list > div > input", type: SelectorType.CSS));
			selectorDict.Add("SaveSlideNameButton", (selector: "div.slide-builder__list > button", type: SelectorType.CSS));
			selectorDict.Add("AddQuestionButton", (selector: "div.form-designer-add-question > button", type: SelectorType.CSS));
			selectorDict.Add("QuestionOptionsButton", (selector: "div.form__question-container > section >button.icon-edit", type: SelectorType.CSS));
			selectorDict.Add("QuestionNameInput", (selector: "div.slide-builder__list--edit-question > div > input", type: SelectorType.CSS));
			selectorDict.Add("SaveQuestionNameButton", (selector: "div.slide-builder__list--edit-question > div > input", type: SelectorType.CSS));
			selectorDict.Add("SaveAndPublishButton", (selector: "section.action-bar > section > button.btn--solid", type: SelectorType.CSS));
		}

		public List<string> GetSlideNamesWithWait()
		{
			var success = driverWait.Until(x => GetSlideNames().Count > 0);
			contextConfiguration.TestOutputHelper.WriteLine(success
				? $"Successfully found question names"
				: $"Failed to find any question names");
			return GetSlideNames();
		}
		public List<string> GetSlideNames()
			=> driver.FindElements(By.CssSelector("div.slide-builder__list > section > button.icon-right"))
				.Select(x => x.Text)
				.ToList();

		public List<string> GetQuestionNamesWithWait()
		{
			var success = driverWait.Until(x => GetQuestionNames().Count > 0);
			contextConfiguration.TestOutputHelper.WriteLine(success
				? $"Successfully found question names"
				: $"Failed to find any question names");
			return GetQuestionNames();
		}

		public List<string> GetQuestionNames()
			=> driver.FindElements(By.CssSelector(".form__question-container"))
				.Select(x => x.GetAttribute("data-name"))
				.ToList();

		private IEnumerable<IWebElement> ContextMenuOptions()
			=> driver.FindElements(By.CssSelector("div.react-contexify__item__content > button"));

		public bool CreateNewSlide(string slideName)
		{
			NewSlideButton.Click();
			SlideOptionsButton.Click();
			var editSlideButton = ContextMenuOptions().FirstOrDefault(x => x.Text == "Edit Slide");
			if (editSlideButton == null)
			{
				return false;
			}

			editSlideButton.Click();
			SlideNameInput.Clear();
			SlideNameInput.SendKeys(slideName);
			SaveSlideNameButton.Click();
			return true;
		}

		public bool CreateNewQuestion(string questionName)
		{
			AddQuestionButton.Click();
			var addTextQuestionOption = ContextMenuOptions().FirstOrDefault(x => x.Text == "Textbox");
			if (addTextQuestionOption == null)
			{
				return false;
			}
			addTextQuestionOption.Click();
			QuestionOptionsButton.Click();
			QuestionNameInput.Clear();
			QuestionNameInput.SendKeys(questionName);
			SaveQuestionNameButton.Click();
			return true;
		}
	}
}
