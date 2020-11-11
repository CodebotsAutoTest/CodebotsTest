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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Lm2348.Models {
	public class EntityAttribute : Attribute
	{
		// % protected region % [Add any extra entity attribute fields here] off begin
		// % protected region % [Add any extra entity attribute fields here] end
	}

	public class FileReference : Attribute
	{
		// % protected region % [Add any extra file reference attribute fields here] off begin
		// % protected region % [Add any extra file reference attribute fields here] end
	}

	public class EntityForeignKey : Attribute
	{
		public string Name { get; }
		public string OppositeName { get; }
		public bool Required { get; }
		public Type OppositeEntity { get; }
		// % protected region % [Add any extra entity foreign key fields here] off begin
		// % protected region % [Add any extra entity foreign key fields here] end

		public EntityForeignKey(string name, string oppositeName, bool required, Type oppositeEntity)
		{
			Name = name;
			OppositeName = oppositeName;
			Required = required;
			OppositeEntity = oppositeEntity;
			// % protected region % [Add any extra entity foreign key constructor fields here] off begin
			// % protected region % [Add any extra entity foreign key constructor fields here] end
		}
	}

	public interface IAbstractModel
	{
		Guid Id { get; set; }
		DateTime Created { get; set; }
		DateTime Modified { get; set; }

		Task BeforeSave(EntityState operation, Lm2348DBContext dbContext, IServiceProvider serviceProvider, CancellationToken cancellationToken = default);
		Task AfterSave(EntityState operation, Lm2348DBContext dbContext, IServiceProvider serviceProvider, ICollection<ChangeState> changes, CancellationToken cancellationToken = default);

		// % protected region % [Add any extra abstract model fields here] off begin
		// % protected region % [Add any extra abstract model fields here] end
	}

	public static class AbstractModelExtensions
	{
		public static bool ValidateObjectFields(this object abstractModel, List<ValidationResult> errors)
		{
			var context = new ValidationContext(abstractModel, serviceProvider: null, items: null);
			return Validator.TryValidateObject(abstractModel, context, errors, validateAllProperties: true);
		}
	}

	public class AbstractModelConfiguration
	{
		public static void Configure<T>(EntityTypeBuilder<T> builder)
			where T : class, IAbstractModel
		{
			// % protected region % [Alter base database configurations here] off begin
			// Configuration for a POSTGRES database
			builder
				.Property(e => e.Id)
				.HasDefaultValueSql("uuid_generate_v4()");
			builder
				.Property(e => e.Created)
				.HasDefaultValueSql("CURRENT_TIMESTAMP");
			builder
				.Property(e => e.Modified)
				.HasDefaultValueSql("CURRENT_TIMESTAMP");
			// % protected region % [Alter base database configurations here] end

			// % protected region % [Add any extra abstract model configuration here] off begin
			// % protected region % [Add any extra abstract model configuration here] end
		}
	}
}