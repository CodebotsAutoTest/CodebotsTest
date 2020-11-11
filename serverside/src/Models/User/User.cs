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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Lm2348.Enums;
using Lm2348.Security;
using Lm2348.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.AspNetCore.Identity;
using Audit.EntityFramework;
// % protected region % [Add extra imports here] off begin
// % protected region % [Add extra imports here] end

namespace Lm2348.Models {
	public class User : IdentityUser<Guid>, IOwnerAbstractModel
	{
		public override Guid Id { get; set; }
		public virtual DateTime Created { get; set; }
		public virtual DateTime Modified { get; set; }
		public virtual Guid Owner { get; set; }

		// % protected region % [Add extra fields here] off begin
		// % protected region % [Add extra fields here] end

		public virtual async Task BeforeSave(EntityState operation, Lm2348DBContext dbContext, IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
		{
			// % protected region % [Add any before save logic here] off begin
			// % protected region % [Add any before save logic here] end
		}

		public virtual async Task AfterSave(EntityState operation, Lm2348DBContext dbContext, IServiceProvider serviceProvider, ICollection<ChangeState> changes, CancellationToken cancellationToken = default)
		{
			// % protected region % [Add any after save logic here] off begin
			// % protected region % [Add any after save logic here] end
		}

		public virtual IEnumerable<IAcl> Acls => new List<IAcl>
		{
			// % protected region % [Add any further ACL entries here] off begin
			// % protected region % [Add any further ACL entries here] end
		};

		public async virtual Task<int> CleanReference<T>(
			string reference,
			IEnumerable<T> models,
			Lm2348DBContext dbContext,
			CancellationToken cancellation = default)
			where T : IOwnerAbstractModel
		{
			// % protected region % [Customise clean reference logic here] off begin
			return 0;
			// % protected region % [Customise clean reference logic here] end
		}

		[Required]
		[EntityAttribute]
		public override string UserName { get; set; }

		[Email]
		[EntityAttribute]
		public override string Email { get; set; }

		public string Discriminator { get; set; }

		// Materialise the password hash in the subclass so it can be ignored by the audit logs
		[AuditIgnore]
		public override string PasswordHash { get; set; }

		// % protected region % [Add any user functions or properties here] off begin
		// % protected region % [Add any user functions or properties here] end
	}
	public class Group : IdentityRole<Guid>
	{
		/// <summary>
		/// Do the users in this group have access to the administration backend of the application
		/// </summary>
		public bool? HasBackendAccess { get; set; }

		// % protected region % [Add any group functions or properties here] off begin
		// % protected region % [Add any group functions or properties here] end
	}

	public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			// % protected region % [Alter base user database configurations here] off begin
			builder
				.Property(e => e.Id)
				.HasDefaultValueSql("uuid_generate_v4()");

			builder.HasDiscriminator(u => u.Discriminator);
			builder.HasIndex(e => e.Discriminator);
			// % protected region % [Alter base user database configurations here] end

			// % protected region % [Add any user configuration options here] off begin
			// % protected region % [Add any user configuration options here] end
		}
	}

	public class GroupConfiguration : IEntityTypeConfiguration<Group>
	{
		public void Configure(EntityTypeBuilder<Group> builder)
		{
			// % protected region % [Alter base group database configurations here] off begin
			builder
				.Property(e => e.Id)
				.HasDefaultValueSql("uuid_generate_v4()");
			// % protected region % [Alter base group database configurations here] end

			// % protected region % [Add any group configuration options here] off begin
			// % protected region % [Add any group configuration options here] end
		}
	}

	// % protected region % [Add any extra user related code here] off begin
	// % protected region % [Add any extra user related code here] end
}