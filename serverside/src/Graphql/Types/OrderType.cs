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
using GraphQL.Types;

namespace Lm2348.Graphql.Types
{
	/// <summary>
	/// An object that contains an ordering condition
	/// </summary>
	public class OrderBy
	{
		public string Path { get; set; }
		public bool? Descending { get; set; }
	}

	/// <summary>
	/// The graphql type for the ordering condition
	/// </summary>
	public class OrderGraph : InputObjectGraphType<OrderBy>
	{
		public OrderGraph()
		{
			Field(x => x.Path)
				.Description("The field to order by");
			Field(x => x.Descending,true)
				.Description("Weather or not the field is descending");
		}
	}
}