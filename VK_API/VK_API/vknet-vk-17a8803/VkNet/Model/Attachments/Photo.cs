using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VkNet.Utils;

namespace VkNet.Model.Attachments
{
	/// <summary>
	/// Фотография.
	/// </summary>
	/// <remarks>
	/// См. описание http://vk.com/dev/photo
	/// </remarks>
	[Serializable]
	public class Photo : MediaAttachment
	{
		static Photo()
		{
			RegisterType(type: typeof(Photo), match: "photo");
		}

		/// <summary>
		/// Идентификатор альбома, в котором находится фотография.
		/// </summary>
		[JsonProperty("album_id")]
		public long? AlbumId { get; set; }

		/// <summary>
		/// Идентификатор пользователя, загрузившего фото (если фотография размещена в
		/// сообществе). Для фотографий, размещенных
		/// от имени сообщества.
		/// </summary>
		[JsonProperty("user_id")]
		public long? UserId { get; set; }

		/// <summary>
		/// Текст описания фотографии.
		/// </summary>
		[JsonProperty("text")]
		public string Text { get; set; }

		/// <summary>
		/// Дата добавления фотографии.
		/// </summary>
		[JsonProperty("date")]
		[JsonConverter(converterType: typeof(UnixDateTimeConverter))]
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// Размеры фотографий.
		/// </summary>
		[JsonProperty("sizes")]
		public ReadOnlyCollection<PhotoSize> Sizes { get; set; }

		/// <summary>
		/// Uri фотографии с максимальным размером 50x50px.
		/// </summary>
		[JsonProperty("photo_50")]
		public Uri Photo50 { get; set; }

		/// <summary>
		/// Uri фотографии с максимальным размером 75x75px.
		/// </summary>
		[JsonProperty("photo_75")]
		public Uri Photo75 { get; set; }

		/// <summary>
		/// Uri фотографии с максимальным размером 100x100px.
		/// </summary>
		[JsonProperty("photo_100")]
		public Uri Photo100 { get; set; }

		/// <summary>
		/// Uri фотографии с максимальным размером 130x130px.
		/// </summary>
		[JsonProperty("photo_130")]
		public Uri Photo130 { get; set; }

		/// <summary>
		/// Uri фотографии с максимальным размером 200x200px.
		/// </summary>
		[JsonProperty("photo_200")]
		public Uri Photo200 { get; set; }

		/// <summary>
		/// Uri фотографии с максимальным размером 604x604px.
		/// </summary>
		[JsonProperty("photo_604")]
		public Uri Photo604 { get; set; }

		/// <summary>
		/// Uri фотографии с максимальным размером 807x807px.
		/// </summary>
		[JsonProperty("photo_807")]
		public Uri Photo807 { get; set; }

		/// <summary>
		/// Uri фотографии с максимальным размером 1280x1024px.
		/// </summary>
		[JsonProperty("photo_1280")]
		public Uri Photo1280 { get; set; }

		/// <summary>
		/// Uri фотографии с максимальным размером  2560x2048px.
		/// </summary>
		[JsonProperty("photo_2560")]
		public Uri Photo2560 { get; set; }

		/// <summary>
		/// Ширина оригинала фотографии в пикселах
		/// </summary>
		[JsonProperty("width")]
		public int? Width { get; set; }

		/// <summary>
		/// Высота оригинала фотографии в пикселах.
		/// </summary>
		[JsonProperty("height")]
		public int? Height { get; set; }

	#region Методы

		/// <summary>
		/// Разобрать из json.
		/// </summary>
		/// <param name="response"> Ответ сервера. </param>
		/// <returns> </returns>
		public static Photo FromJson(VkResponse response)
		{
			var photo = new Photo
			{
				Id = response[key: "photo_id"] ?? response[key: "pid"] ?? response[key: "id"],
				AlbumId = response[key: "album_id"] ?? response[key: "aid"], OwnerId = response[key: "owner_id"],
				Photo50 = response[key: "photo_50"],
				Photo75 = response[key: "photo_75"] ?? response[key: "src_small"],
				Photo100 = response[key: "photo_100"],
				Photo130 = response[key: "photo_130"] ?? response[key: "src"],
				Photo200 = response[key: "photo_200"],
				Photo604 = response[key: "photo_604"] ?? response[key: "src_big"],
				Photo807 = response[key: "photo_807"] ?? response[key: "src_xbig"],
				Photo1280 = response[key: "photo_1280"] ?? response[key: "src_xxbig"],
				Photo2560 = response[key: "photo_2560"] ?? response[key: "src_xxxbig"], Width = response[key: "width"],
				Height = response[key: "height"], Text = response[key: "text"],
				CreateTime = response[key: "date"] ?? response[key: "created"],
				UserId = Utilities.GetNullableLongId(response: response[key: "user_id"]),
				PostId = Utilities.GetNullableLongId(response: response[key: "post_id"]), AccessKey = response[key: "access_key"],
				PlacerId = Utilities.GetNullableLongId(response: response[key: "placer_id"]), TagCreated = response[key: "tag_created"],
				TagId = response[key: "tag_id"], Likes = response[key: "likes"], Comments = response[key: "comments"],
				CanComment = response[key: "can_comment"], Tags = response[key: "tags"], PhotoSrc = response[key: "photo_src"],
				PhotoHash = response[key: "photo_hash"], SmallPhotoSrc = response[key: "src_small"], Latitude = response[key: "lat"],
				Longitude = response[key: "long"], Sizes = response[key: "sizes"].ToReadOnlyCollectionOf<PhotoSize>(selector: x => x)
			};

			return photo;
		}

	#endregion

	#region опциональные поля

		/// <summary>
		/// Ключ доступа.
		/// </summary>
		[JsonProperty("access_key")]
		public string AccessKey { get; set; }

		/// <summary>
		/// Идентификатор записи, у которой данная фотография является прикреплением???
		/// </summary>
		[JsonProperty("post_id")]
		public long? PostId { get; set; }

		/// <summary>
		/// Идентификатор пользователя, сделавшего отметку
		/// </summary>
		[JsonProperty("placer_id")]
		public long? PlacerId { get; set; }

		/// <summary>
		/// Дата создания отметки
		/// </summary>
		[JsonProperty("tag_created")]
		[JsonConverter(converterType: typeof(UnixDateTimeConverter))]
		public DateTime? TagCreated { get; set; }

		/// <summary>
		/// Идентификатор отметки
		/// </summary>
		[JsonProperty("tag_id")]
		public long? TagId { get; set; }

		/// <summary>
		/// Лайки
		/// </summary>
		[JsonProperty("likes")]
		public Likes Likes { get; set; }

		/// <summary>
		/// Возможность комментирования фотографии
		/// </summary>
		[JsonProperty("can_comment")]
		public bool? CanComment { get; set; }

		/// <summary>
		/// Комментарии
		/// </summary>
		[JsonProperty("comments")]
		public Comments Comments { get; set; }

		/// <summary>
		/// Теги
		/// </summary>
		[JsonProperty("tags")]
		public Tags Tags { get; set; }

		/// <summary>
		/// Источник изображения.
		/// </summary>
		[JsonProperty("photo_src")]
		public Uri PhotoSrc { get; set; }

		/// <summary>
		/// Хеш изображения.
		/// </summary>
		[JsonProperty("photo_hash")]
		public string PhotoHash { get; set; }

		/// <summary>
		/// Географическая широта отметки, заданная в градусах
		/// </summary>
		[JsonProperty("lat")]
		public double? Latitude { get; set; }

		/// <summary>
		/// Географическая долгота отметки, заданная в градусах
		/// </summary>
		[JsonProperty("long")]
		public double? Longitude { get; set; }

		/// <summary>
		/// Uri фотографии с максимальным размером.
		/// </summary>
		[JsonProperty("big_photo_src")]
		public Uri BigPhotoSrc { get; set; }

		/// <summary>
		/// Uri фотографии с минимальным размером.
		/// </summary>
		[JsonProperty("src_small")]
		public Uri SmallPhotoSrc { get; set; }

	#endregion
	}
}