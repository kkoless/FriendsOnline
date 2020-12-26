﻿using System;
using System.Diagnostics;
using VkNet.Enums;
using VkNet.Utils;

namespace VkNet.Model
{
	/// <summary>
	/// Определяет тип объекта
	/// </summary>
	[DebuggerDisplay(value: "Id = {Id}, Type = {Type}")]
	[Serializable]
	public class VkObject
	{
		/// <summary>
		/// Идентификатор объекта
		/// </summary>
		public long? Id { get; set; }

		/// <summary>
		/// Тип объекта
		/// </summary>
		public VkObjectType Type { get; set; }

		/// <summary>
		/// Разобрать из json.
		/// </summary>
		/// <param name="response"> Ответ сервера. </param>
		/// <returns> </returns>
		public static VkObject FromJson(VkResponse response)
		{
			var obj = new VkObject
			{
					Id = Utilities.GetNullableLongId(response: response[key: "object_id"])
			};

			string type = response[key: "type"];

			switch (type)
			{
				case "group":

				{
					obj.Type = VkObjectType.Group;

					break;
				}
				case "user":

				{
					obj.Type = VkObjectType.User;

					break;
				}
				case "application":

				{
					obj.Type = VkObjectType.Application;

					break;
				}
				default:

				{
					return obj;
				}
			}

			return obj;
		}
	}
}