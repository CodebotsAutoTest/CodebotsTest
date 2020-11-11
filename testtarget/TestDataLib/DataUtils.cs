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
using System.Net.Mail;
using System.Text;
using AutoFixture;

namespace TestDataLib
{
	public enum CharType
	{
		ALPHABETIC_ONLY,
		SPECIAL_ONLY,
		WORDY_ONLY,
		MIXED,
		FIXTURE_STRING
	}

	public static class DataUtils
	{
		private static readonly byte[] _redCircleImage = File.ReadAllBytes("Resources/RedCircle.svg");
		private readonly static string[] _specialChars = LoadTextFile("Resources/specialChars.txt");
		private readonly static string[] _words = LoadTextFile("Resources/words.txt");

		private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

		// get the words, we need to filter out the bot written warning and comments
		private static bool FilterPlainTextFile(string x)
		{
			return !x.StartsWith("#");
		}

		private static string[] LoadTextFile(string localFilePath)
		{
			return File.ReadAllLines(localFilePath)
				.Select(x => x.Trim())
				.Where(FilterPlainTextFile)
				.ToArray();
		}

		/// <summary>
		/// Returns an SVG image to be used for testing file and image uploading.
		/// </summary>
		/// <returns>A byte array read from the SVG test file</returns>
		public static byte[] GetSVGTestFile()
		{
			return _redCircleImage;
		} 

		/// <summary>
		/// Create a random string in the desired range and options
		/// </summary>
		/// <param name="minLength">The minimum length of the string</param>
		/// <param name="maxLength">The maximum length of the string</param>
		/// <param name="charType">what type of characters will be used for the string</param>
		/// <returns>A random string with length in the desired range and using the char type specified </returns>
		public static string RandString(int minLength, int maxLength, CharType charType = CharType.WORDY_ONLY)
		{
			// Set to maximum possible length, will fail if min is greater than length
			return RandString(RandInt(minLength, maxLength), charType);
		}

		/// <summary>
		/// Create a random string with the desired options
		/// </summary>
		/// <param name="length">The length of the string</param>
		/// <param name="charType">The type of character set that will be created</param>
		/// <returns>A Random string of desired length and character type</returns>
		public static string RandString(int length = 10, CharType charType = CharType.WORDY_ONLY)
		{
			var fixture = new Fixture();
			switch (charType)
			{
				case CharType.FIXTURE_STRING:
					return fixture.Create<string>();
				case CharType.ALPHABETIC_ONLY:
					return new string(Enumerable.Repeat(_chars, length).Select(s => s[RandInt(0, s.Length - 1)]).ToArray());
				case CharType.SPECIAL_ONLY:
					var specialString = new StringBuilder();
					while (specialString.Length < length)
					{
						specialString.Append(_specialChars[RandInt(0, _specialChars.Length - 1)]);
					}
					return specialString.ToString(0, length);
				case CharType.MIXED:
					var result = new StringBuilder();
					for (var i = 0; i < length; i++)
					{
						var choice = RandBool();
						if (choice)
						{
							result.Append(_specialChars[RandInt(0, _specialChars.Length - 1)]);
						}
						else
						{
							result.Append(_chars[RandInt(0, _chars.Length - 1)]);
						}
					}
					return result.ToString(0, length);
				case CharType.WORDY_ONLY:
				default:
					return RandWordyString(length);
			}
		}

		/// <summary>
		/// Return a valid email address that has been randomly generated
		/// </summary>
		/// <returns> A valid email address that has been randomly generated </returns>
		public static string RandEmail(string seed = null)
		{
			var fixture = new Fixture();
			var mailAddressGenerator = new MailAddressGenerator();
			fixture.Customizations.Add(mailAddressGenerator);
			var address = fixture.Create<MailAddress>().Address;
			return seed != null
				? seed + address
				: address;
		}

		/// <summary>
		/// Will return an illegal javascript popup alert in the form of a string
		/// to be used in a text input field to test site security
		/// </summary>
		/// <returns>a confirm window</returns>
		public static string GenerateJavascriptString() => "window.confirm('hello there');";

		/// <summary>
		/// Create a random integer in the given range
		/// </summary>
		/// <param name="min">The minimum value of the integer</param>
		/// <param name="max">The maximum value of the integer</param>
		/// <returns>a random integer in the given range</returns>
		public static int RandInt(int min, int max)
		{
			var fixture = new Fixture();
			var numberSequenceGenerator = new RandomNumericSequenceGenerator(min, max);
			fixture.Customizations.Add(numberSequenceGenerator);
			return fixture.Create<int>();
		}

		/// <summary>
		/// Create a random integer value
		/// </summary>
		/// <returns>a random integer value</returns>
		public static int RandInt() => new Fixture().Create<int>();

		/// <summary>
		/// Create a random boolean value
		/// </summary>
		/// <returns>a random boolean value</returns>
		public static bool RandBool() => new Fixture().Create<bool>();

		/// <summary>
		/// Create a random double
		/// </summary>
		/// <returns>a random double value</returns>
		public static double RandDouble()
		{
			var fixture = new Fixture();
			fixture.Customize<double>(c => c.FromFactory<int>(i => i * 1.33));
			return fixture.Create<double>();
		}

		/// <summary>
		/// Create a random date time
		/// </summary>
		/// <returns>Random datetime object</returns>
		public static DateTime RandDatetime() => new Fixture().Create<DateTime>();

		/// <summary>
		/// Create a random date and time in the given range
		/// </summary>
		/// <returns>Random value maximum of two years ago</returns>
		public static DateTime RandDatetime(DateTime minDate, DateTime maxDate)
		{
			var fixture = new Fixture();
			var dateTimeSequenceGenerator = new RandomDateTimeSequenceGenerator(minDate, maxDate);
			fixture.Customizations.Add(dateTimeSequenceGenerator);
			return fixture.Create<DateTime>();
		}

		/// <summary>
		/// Create a random date and time in the past
		/// </summary>
		/// <returns>Random value maximum of two years ago</returns>
		public static DateTime RandHistoricDateTime()
		{
			var fixture = new Fixture();
			var dateTimeSequenceGenerator = new RandomDateTimeSequenceGenerator(DateTime.Now.AddYears(-2), DateTime.Now.AddDays(-1));
			fixture.Customizations.Add(dateTimeSequenceGenerator);
			return fixture.Create<DateTime>();
		}

		/// <summary>
		/// Create a random date and time in the future
		/// </summary>
		/// <returns>Random value max of two years in the future</returns>
		public static DateTime RandFutureDateTime()
		{
			var fixture = new Fixture();
			var dateTimeSequenceGenerator = new RandomDateTimeSequenceGenerator(DateTime.Now.AddDays(1), DateTime.Now.AddYears(2));
			fixture.Customizations.Add(dateTimeSequenceGenerator);
			return fixture.Create<DateTime>();
		}

		/// <summary>
		/// Creates a string with a random set of predefined words
		/// </summary>
		/// <param name="minLength">The minimum length of the string</param>
		/// <param name="maxLength">The maximum length of the string</param>
		/// <returns>A wordy string within the desired word range</returns>
		public static string RandWordyString(int minLength, int maxLength)
		{
			return RandWordyString(RandInt(minLength, maxLength));
		}

		/// <summary>
		/// Create a random wordy string at the desired or default length
		/// </summary>
		/// <param name="length">Number of characters in the string</param>
		/// <returns>A random string at the desired length</returns>
		private static string RandWordyString(int length = 10)
		{
			// create the string builder we will be using
			var wordyWords = new StringBuilder();
			for (var i = 0; i < length; i = wordyWords.Length)
			{
				// append the words and spacing between words
				wordyWords.Append(_words[RandInt(0,_words.Length - 1)]);

				if (wordyWords.Length < length - 1)
				{
					wordyWords.Append(" ");
				}
			}
			return wordyWords.ToString(0, length);
		}

		/// <summary>
		/// Will return an array of a specific length with random numbers that total the input number
		/// </summary>
		/// <param name="numNumbers"></param>
		/// <param name="numbersSum"></param>
		/// <returns></returns>
		public static int[] RandIntegerSum(int numNumbers, int numbersSum)
		{
			var outputInts = new int[numNumbers];

			foreach(var index in Enumerable.Range(0,numNumbers))
			{
				var maxRngValue = numbersSum - outputInts.Sum() - numNumbers + index + 1;
				outputInts[index] = RandInt(1, maxRngValue);
			}

			outputInts[numNumbers - 1] = numbersSum - outputInts.Sum();
			return outputInts;
		}

		// % protected region % [Set any data util methods here] off begin
		// % protected region % [Set any data util methods here] end
	}
}