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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lm2348.Helpers;
using Lm2348.Models;
using Lm2348.Services.Interfaces;
using Lm2348.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Lm2348.Services
{
	public class IdentityService : IIdentityService
	{
		public bool Fetched { get; set; } = false;

		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IUserService _userService;
		private readonly UserManager<User> _userManager;

		/// <inheritdoc />
		public User User { get; set; }

		/// <inheritdoc />
		public IList<string> Groups { get; set; }

		// % protected region % [Add any extra class variables here] off begin
		// % protected region % [Add any extra class variables here] end

		public IdentityService(
			// % protected region % [Add any extra constructor arguments here] off begin
			// % protected region % [Add any extra constructor arguments here] end
			IHttpContextAccessor httpContextAccessor,
			IUserService userService,
			UserManager<User> userManager)
		{
			_httpContextAccessor = httpContextAccessor;
			_userService = userService;
			_userManager = userManager;
			// % protected region % [Add any extra constructor logic here] off begin
			// % protected region % [Add any extra constructor logic here] end
		}

		/// <inheritdoc />
		public async Task RetrieveUserAsync()
		{
			// % protected region % [Change RetrieveUserAsync here] off begin
			if (Fetched != true)
			{
				User = await _userService.GetUserFromClaim(_httpContextAccessor.HttpContext.User);
				Groups = User == null ? new List<string>() : await _userManager.GetRolesAsync(User);
				Groups.AddRange(SecurityUtilities.GetAllAcls()
					.Where(x => x.IsVisitorAcl && x.Group != null)
					.Select(x => x.Group)
					.ToHashSet());
				Fetched = true;
			}
			// % protected region % [Change RetrieveUserAsync here] end
		}

		// % protected region % [Add any further methods here] off begin
		// % protected region % [Add any further methods here] end
	}
}