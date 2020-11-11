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
using System.Reflection;
using GraphQL.Types;
// % protected region % [Add any extra imports here] off begin
// % protected region % [Add any extra imports here] end

namespace Lm2348.Helpers
{
	public static class ComplexGraphTypeExtensions
	{
		/// <summary>
		/// Removes a field from a Complex GraphQL type
		/// </summary>
		/// <param name="self">The graphql type to remove the field from</param>
		/// <param name="name">The name of the field to remove</param>
		/// <typeparam name="T">The type encapsulated by this graphql type</typeparam>
		public static void RemoveField<T>(this ComplexGraphType<T> self, string name)
		{
			// % protected region % [Customise remove field here] off begin
			var typeInfo = typeof(ComplexGraphType<T>)
				.GetField("_fields", BindingFlags.NonPublic | BindingFlags.Instance);

			if (typeInfo == null)
			{
				return;
			}

			var fields = (List<FieldType>) typeInfo.GetValue(self);
			fields?.RemoveAt(fields.FindIndex(x => x.Name == name));
			// % protected region % [Customise remove field here] end
		}

		// % protected region % [Add any extra methods here] off begin
		// % protected region % [Add any extra methods here] end
	}
}