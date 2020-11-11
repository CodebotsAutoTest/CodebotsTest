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
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Lm2348.Helpers
{
    public class GraphQLHelper
    {
		public static List<IDictionary<string, Object>> getDataFromGraphQLResult(object graphQLData, List<string> columns = null)
		{
			var resultList = new List<IDictionary<string, Object>>();
			var graphData = (graphQLData as Dictionary<string, object>).First();
			if (graphData.Equals(new KeyValuePair<string, object>()))
			{
				return null;
			}

            var entities = graphData.Value;
			foreach (var e in entities as List<object>)
			{
				var destEntity = new ExpandoObject() as IDictionary<string, Object>;
				var eDic = (e as Dictionary<string, object>);

				foreach(var field in eDic)
				{
					object oValue;
					string strValue;

					if (columns.Count > 0 && columns.Contains(field.Key))
					{
						eDic.TryGetValue(field.Key, out oValue);
						strValue = oValue == null ? "" : oValue.ToString();
						destEntity.Add(field.Key, strValue);
					}
				}
				resultList.Add(destEntity);
			}

			return resultList;
		}



	}

}
