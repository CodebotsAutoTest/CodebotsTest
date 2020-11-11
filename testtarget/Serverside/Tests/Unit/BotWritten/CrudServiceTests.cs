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

using Lm2348.Services;
using Moq;
using Xunit;

namespace ServersideTests.Tests.Unit.BotWritten
{
	public class CrudServiceTests
	{
		[Theory]
		[InlineData(null, null, false)]
		[InlineData(null, 0, false)]
		[InlineData(0, null, false)]
		[InlineData(0, 0, false)]
		[InlineData(1, 1, true)]
		public void TestPaginationValidation(int? pageNum, int? pageSize, bool expectedValidation)
		{
			var paginationOptions = new PaginationOptions
			{
				PageNo = pageNum,
				PageSize = pageSize
			};

			var pagination = new Pagination(paginationOptions);

			Assert.Equal(expectedValidation, pagination.isValid());
		}
	}
}
