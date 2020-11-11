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
using APITests.Factories;
using Xunit;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace APITests.TheoryData.BotWritten
{
	public class PasswordInvalidTheoryData : TheoryData<UserEntityFactory, string, string>
	{
		public PasswordInvalidTheoryData()
		{
			var passwordMustContainDigitError = "Passwords must have at least one digit ('0'-'9').";
			var passwordMustContainUppercaseError = "Passwords must have at least one uppercase ('A'-'Z').";
			var passwordMustContainNonAlphanumericError = "Passwords must have at least one non alphanumeric character.";
			var passwordLengthError = "Passwords must be at least 6 characters.";

			// % protected region % [Add any further password error here] off begin
			// % protected region % [Add any further password error here] end

			// % protected region % [Modify PasswordInvalidTheoryData entities here] off begin
			// % protected region % [Modify PasswordInvalidTheoryData entities here] end

			// % protected region % [Add any further test cases here] off begin
			// % protected region % [Add any further test cases here] end
		}
	}

	public class UsernameInvalidTheoryData : TheoryData<UserEntityFactory, string, string>
	{
		public UsernameInvalidTheoryData()
		{
			// % protected region % [Modify UsernameInvalidTheoryData entities here] off begin
			var InvalidEmailError = "Email is not a valid email";

			// % protected region % [Modify UsernameInvalidTheoryData entities here] end
		}
	}
}