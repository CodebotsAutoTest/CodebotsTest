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

namespace Lm2348.Models
{
	public class UserDto : ModelDto<User>
	{
		public Guid Owner { get; set; }

		public string UserName { get; set; }

		public string Email { get; set; }

		public bool EmailConfirmed { get; set; }

		public string Discriminator { get; set; }

		// % protected region % [Add extra UserDto properties] off begin
		// % protected region % [Add extra UserDto properties] end

		public UserDto(User model)
		{
			LoadModelData(model);
		}

		public UserDto() { }

		public override User ToModel()
		{
			return new User
			{
				Id = Id,
				UserName = UserName,
				Email = Email,
				EmailConfirmed = EmailConfirmed,
				Discriminator = Discriminator,
				// % protected region % [Add extra ToModel fields] off begin
				// % protected region % [Add extra ToModel fields] end
			};
		}

		public override ModelDto<User> LoadModelData(User model)
		{
			Id = model.Id;
			UserName = model.UserName;
			Email = model.Email;
			EmailConfirmed = model.EmailConfirmed;
			Discriminator = model.Discriminator;
			// % protected region % [Add extra LoadModelData properties] off begin
			// % protected region % [Add extra LoadModelData properties] end
			return this;
		}
	}
}