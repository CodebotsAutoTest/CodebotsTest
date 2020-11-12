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
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lm2348.Enums;
using Lm2348.Security;
using Lm2348.Security.Acl;
using Lm2348.Validators;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;
// % protected region % [Add any further imports here] off begin
// % protected region % [Add any further imports here] end

namespace Lm2348.Models {
	// % protected region % [Configure entity attributes here] off begin
	[Table("A")]
	// % protected region % [Configure entity attributes here] end
	public class AEntity : IOwnerAbstractModel	{
		[Key]
		public Guid Id { get; set; }
		public Guid Owner { get; set; }
		public DateTime Created { get; set; }
		public DateTime Modified { get; set; }

		// % protected region % [Customise Dsds here] off begin
		[EntityAttribute]
		public String Dsds { get; set; }
		// % protected region % [Customise Dsds here] end

		// % protected region % [Add any further attributes here] off begin
		// % protected region % [Add any further attributes here] end

		public AEntity()
		{
			// % protected region % [Add any constructor logic here] off begin
			// % protected region % [Add any constructor logic here] end
		}

		[NotMapped]
		public IEnumerable<IAcl> Acls => new List<IAcl>
		{
			// % protected region % [Override ACLs here] off begin
			new SuperAdministratorsScheme(),
			// % protected region % [Override ACLs here] end
			// % protected region % [Add any further ACL entries here] off begin
			// % protected region % [Add any further ACL entries here] end
		};

		/// <summary>
		/// Incoming one to many reference
		/// </summary>
		/// <see cref="Lm2348.Models.BEntity"/>
		[EntityForeignKey("Bssssdasds", "A", false, typeof(BEntity))]
		public ICollection<BEntity> Bssssdasds { get; set; }

		public async Task BeforeSave(
			EntityState operation,
			Lm2348DBContext dbContext,
			IServiceProvider serviceProvider,
			CancellationToken cancellationToken = default)
		{
			// % protected region % [Add any before save logic here] off begin
			// % protected region % [Add any before save logic here] end
		}

		public async Task AfterSave(
			EntityState operation,
			Lm2348DBContext dbContext,
			IServiceProvider serviceProvider,
			ICollection<ChangeState> changes,
			CancellationToken cancellationToken = default)
		{
			// % protected region % [Add any after save logic here] off begin
			// % protected region % [Add any after save logic here] end
		}

		public async Task<int> CleanReference<T>(
			string reference,
			IEnumerable<T> models,
			Lm2348DBContext dbContext,
			CancellationToken cancellation = default)
			where T : IOwnerAbstractModel
		{
			var modelList = models.Cast<AEntity>().ToList();
			var ids = modelList.Select(t => t.Id).ToList();

			switch (reference)
			{
				case "Bssssdasds":
					var bssssdasdIds = modelList.SelectMany(x => x.Bssssdasds.Select(m => m.Id)).ToList();
					var oldbssssdasd = await dbContext.BEntity
						.Where(m => m.AId.HasValue && ids.Contains(m.AId.Value))
						.Where(m => !bssssdasdIds.Contains(m.Id))
						.ToListAsync(cancellation);

					foreach (var bssssdasd in oldbssssdasd)
					{
						bssssdasd.AId = null;
					}

					dbContext.BEntity.UpdateRange(oldbssssdasd);
					return oldbssssdasd.Count;
				// % protected region % [Add any extra clean reference logic here] off begin
				// % protected region % [Add any extra clean reference logic here] end
				default:
					return 0;
			}
		}
		// % protected region % [Add any further references here] off begin
		// % protected region % [Add any further references here] end
	}
}