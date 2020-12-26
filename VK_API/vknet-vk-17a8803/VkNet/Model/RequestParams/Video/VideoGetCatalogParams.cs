﻿using System;
using Newtonsoft.Json;
using VkNet.Enums.SafetyEnums;
using VkNet.Utils;
using VkNet.Utils.JsonConverter;

namespace VkNet.Model.RequestParams
{
	/// <summary>
	/// Параметры запроса Video.GetCatalog
	/// </summary>
	[Serializable]
	public class VideoGetCatalogParams
	{
		/// <summary>
		/// Число блоков каталога, информацию о которых необходимо вернуть.
		/// Обратите внимание, параметр распространяется только на блоки other.
		/// </summary>
		[JsonProperty(propertyName: "count")]
		public long? Count { get; set; }

		/// <summary>
		/// Число видеозаписей в каждом блоке.
		/// </summary>
		[JsonProperty(propertyName: "items_count")]
		public long? ItemsCount { get; set; }

		/// <summary>
		/// Параметр для получения следующей страницы результатов.
		/// Значение, необходимое для передачи в этом параметре, возвращается в поле ответа
		/// next.
		/// </summary>
		[JsonProperty(propertyName: "from")]
		public string From { get; set; }

		/// <summary>
		/// 1 — в ответе будут возвращены дополнительные поля profiles и groups,
		/// содержащие информацию о пользователях и сообществах.
		/// По умолчанию: 0.
		/// </summary>
		[JsonProperty(propertyName: "extended")]
		public bool? Extended { get; set; }

		/// <summary>
		/// id блоков, которые необходимо вернуть в ответе.
		/// </summary>
		[JsonProperty(propertyName: "filters")]
		[JsonConverter(typeof(SafetyEnumJsonConverter))]
		public VideoCatalogFilters Filters { get; set; }

		/// <summary>
		/// Привести к типу VkParameters.
		/// </summary>
		/// <param name="p"> Параметры. </param>
		/// <returns> </returns>
		public static VkParameters ToVkParameters(VideoGetCatalogParams p)
		{
			var parameters = new VkParameters
			{
					{ "items_count", p.ItemsCount }
					, { "count", p.Count }
					, { "extended", p.Extended }
					, { "from", p.From }
					, { "filters", p.Filters }
			};

			return parameters;
		}
	}
}