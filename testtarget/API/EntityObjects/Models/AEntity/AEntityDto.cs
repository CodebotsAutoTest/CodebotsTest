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
using System.Linq;
using ServersideAEntity = Lm2348.Models.AEntity;

namespace APITests.EntityObjects.Models
{
	public class AEntityDto
	{
		public Guid Id { get; set; }
		public DateTime Created { get; set; }
		public DateTime Modified { get; set; }
		public String Dsds { get; set; }

		public ICollection<BEntity> Bssssdasds { get; set; }

		public AEntityDto(AEntity model)
		{
			Id = model.Id;
			Created = model.Created;
			Modified = model.Modified;
			Dsds = model.Dsds;
			Bssssdasds = model.Bssssdasds;
		}

		public AEntityDto(ServersideAEntity model)
		{
			Id = model.Id;
			Created = model.Created;
			Modified = model.Modified;
			Dsds = model.Dsds;
			Bssssdasds = model.Bssssdasds.Select(BEntityDto.Convert).ToList();
		}

		public AEntity GetTesttargetAEntity()
		{
			return new AEntity
			{
				Id = Id,
				Created = Created,
				Modified = Modified,
				Dsds = Dsds,
				Bssssdasds = Bssssdasds,
			};
		}

		public ServersideAEntity GetServersideAEntity()
		{
			return new ServersideAEntity
			{
				Id = Id,
				Created = Created,
				Modified = Modified,
				Dsds = Dsds,
				Bssssdasds = Bssssdasds?.Select(BEntityDto.Convert).ToList(),
			};
		}

		public static ServersideAEntity Convert(AEntity model)
		{
			var dto = new AEntityDto(model);
			return dto.GetServersideAEntity();
		}

		public static AEntity Convert(ServersideAEntity model)
		{
			var dto = new AEntityDto(model);
			return dto.GetTesttargetAEntity();
		}
	}
}