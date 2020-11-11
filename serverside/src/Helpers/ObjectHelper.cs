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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lm2348.Helpers
{
	public class ObjectHelper
	{
		public static IEnumerable<PropertyInfo> GetCollectionProperties(Type type)
		{
			var props = new List<PropertyInfo>();
			foreach (var prop in type.GetProperties())
			{
				if (typeof(ICollection).IsAssignableFrom(prop.PropertyType))
				{
					props.Add(prop);
				}
			}
			return props;
		}

		public static IEnumerable<PropertyInfo> GetNonCollectionProperties(Type type)
		{
			return type.GetProperties()
				.Where(p => p.PropertyType == typeof(string) ||
				   !typeof(IEnumerable).IsAssignableFrom(p.PropertyType));

		}

		public static IEnumerable<PropertyInfo> GetNonReferenceProperties(Type type)
		{
			return type.GetProperties()
				.Where(p => p.PropertyType == typeof(string) ||
				   (!typeof(IEnumerable).IsAssignableFrom(p.PropertyType))
						&&
					!(
						(p.PropertyType.BaseType.Name.Contains("IdentityUser"))
					  ||(p.PropertyType.BaseType.Name.Contains("OwnerAbstractModel"))
					  ||(p.PropertyType.BaseType.BaseType != null && p.PropertyType.BaseType.Name.Contains("AbstractModel"))
					 )
				);

		}

		public static IEnumerable<T> AsEnumerable<T>(T item)
		{
			yield return item;
		}
	}

	public class JObjectHelper
	{

		public static void ChangeNumericalPropertyNames(JsonReader reader)
		{
			JObject jo = JObject.Load(reader);

			foreach (JProperty jp in jo.Properties().ToList())
			{
				if (Regex.IsMatch(jp.Name, @"^\d"))
				{
					string name = "n" + jp.Name;
					jp.Replace(new JProperty(name, jp.Value));
				}
			}
		}
	}

	public class LowerFirstCharJsonConverter : JsonConverter
	{
		private readonly Type[] _types;

		public LowerFirstCharJsonConverter(params Type[] types)
		{
			_types = types;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			JToken t = JToken.FromObject(value);

			if (t.Type != JTokenType.Object)
			{
				t.WriteTo(writer);
			}
			else
			{
				JObject o = (JObject)t;

				foreach (JProperty jp in o.Properties().ToList())
				{
					string name = Char.ToLowerInvariant(jp.Name[0]) + jp.Name.Substring(1);
					jp.Replace(new JProperty(name, jp.Value));
				}

				o.WriteTo(writer);
			}
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
		}

		public override bool CanRead => false;

		public override bool CanConvert(Type objectType)
		{
			return _types.Any(t => t == objectType);
		}
	}
}
