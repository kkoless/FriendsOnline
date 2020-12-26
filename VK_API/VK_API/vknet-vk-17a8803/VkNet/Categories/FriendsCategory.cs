﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;
using VkNet.Abstractions;
using VkNet.Enums;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;
using VkNet.Exception;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace VkNet.Categories
{
	/// <summary>
	/// Методы для работы с друзьями.
	/// </summary>
	public partial class FriendsCategory : IFriendsCategory
	{
		private readonly IVkApiInvoke _vk;

		/// <summary>
		/// </summary>
		/// <param name="vk"> </param>
		public FriendsCategory(IVkApiInvoke vk)
		{
			_vk = vk;
		}

		/// <summary>
		/// Возвращает список идентификаторов друзей пользователя или расширенную
		/// информацию о друзьях пользователя (при
		/// использовании параметра fields).
		/// </summary>
		/// <param name="params"> Входные параметры выборки. </param>
		/// <param name="skipAuthorization"> Если <c> true </c>, то пропустить авторизацию </param>
		/// <returns>
		/// После успешного выполнения возвращает список идентификаторов (id) друзей
		/// пользователя, если параметр fields не
		/// использовался.
		/// При использовании параметра fields  возвращает список объектов пользователей,
		/// но не более 5000.
		/// </returns>
		/// <remarks>
		/// Страница документации ВКонтакте http://vk.com/dev/friends.get
		/// </remarks>
		public VkCollection<User> Get(FriendsGetParams @params, bool skipAuthorization = false)
		{
			return _vk.Call(methodName: "friends.get", parameters: @params, skipAuthorization: skipAuthorization)
					.ToVkCollectionOf(selector: x => @params.Fields != null ? x : new User { Id = x });
		}

		/// <summary>
		/// Возвращает список идентификаторов друзей текущего пользователя, которые
		/// установили данное приложение.
		/// </summary>
		/// <returns>
		/// После успешного выполнения возвращает список идентификаторов (id) друзей
		/// текущего пользователя, установивших
		/// приложение.
		/// </returns>
		/// <remarks>
		/// Страница документации ВКонтакте http://vk.com/dev/friends.getAppUsers
		/// </remarks>
		[Pure]
		public ReadOnlyCollection<long> GetAppUsers()
		{
			VkResponseArray response = _vk.Call(methodName: "friends.getAppUsers", parameters: VkParameters.Empty);

			return response.ToReadOnlyCollectionOf<long>(selector: x => x);
		}

		/// <summary>
		/// Возвращает список идентификаторов друзей пользователя, находящихся на сайте.
		/// </summary>
		/// <param name="params"> Входные параметры выборки. </param>
		/// <returns>
		/// После успешного выполнения возвращает список идентификаторов (id) друзей,
		/// находящихся сейчас на сайте, у
		/// пользователя с идентификатором uid и входящих в список с идентификатором lid.
		/// При использовании параметра online_mobile=1 также возвращается поле
		/// online_mobile, содержащее список идентификатор
		/// друзей, находящихся на сайте с мобильного устройства.
		/// </returns>
		/// <remarks>
		/// Страница документации ВКонтакте http://vk.com/dev/friends.getOnline
		/// </remarks>
		public FriendOnline GetOnline(FriendsGetOnlineParams @params)
		{
			var result = _vk.Call(methodName: "friends.getOnline", parameters: @params);

			return FriendOnline.FromJson(response: result);
		}

		/// <summary>
		/// Возвращает список идентификаторов общих друзей между парой пользователей.
		/// </summary>
		/// <param name="params"> Входные параметры выборки. </param>
		/// <returns>
		/// После успешного выполнения возвращает список идентификаторов (id) общих друзей
		/// между пользователями с
		/// идентификаторами source_uid и target_uid.
		/// </returns>
		/// <remarks>
		/// Страница документации ВКонтакте http://vk.com/dev/friends.getMutual
		/// </remarks>
		public ReadOnlyCollection<long> GetMutual(FriendsGetMutualParams @params)
		{
			VkResponseArray response = _vk.Call(methodName: "friends.getMutual", parameters: @params);

			return response.ToReadOnlyCollectionOf<long>(selector: x => x);
		}

		/// <inheritdoc />
		[Pure]
		public ReadOnlyCollection<AreFriendsResult> AreFriends(IEnumerable<long> userIds, bool? needSign = null)
		{
			if (userIds == null)
			{
				throw new ArgumentNullException(paramName: nameof(userIds));
			}

			var parameters = new VkParameters
			{
					{ "user_ids", userIds }
					, { "need_sign", needSign }
			};

			return _vk.Call(methodName: "friends.areFriends", parameters: parameters)
					.ToReadOnlyCollectionOf<AreFriendsResult>(selector: x => x);
		}

		/// <summary>
		/// Создает новый список друзей у текущего пользователя.
		/// </summary>
		/// <param name="name">
		/// Название создаваемого списка друзей. строка, обязательный параметр (Строка,
		/// обязательный параметр).
		/// </param>
		/// <param name="userIds">
		/// Идентификаторы пользователей, которых необходимо поместить в созданный список.
		/// список
		/// положительных чисел, разделенных запятыми (Список положительных чисел,
		/// разделенных запятыми).
		/// </param>
		/// <returns>
		/// После успешного выполнения возвращает идентификатор (list_id) созданного списка
		/// друзей.
		/// </returns>
		/// <remarks>
		/// Страница документации ВКонтакте http://vk.com/dev/friends.addList
		/// </remarks>
		public long AddList(string name, IEnumerable<long> userIds)
		{
			VkErrors.ThrowIfNullOrEmpty(expr: () => name);

			var parameters = new VkParameters
			{
					{ "name", name }
					, { "user_ids", userIds }
			};

			var response = _vk.Call(methodName: "friends.addList", parameters: parameters);

			return response[key: "list_id"];
		}

		/// <inheritdoc />
		public bool DeleteList(long listId)
		{
			var parameters = new VkParameters
			{
					{ "list_id", listId }
			};

			return _vk.Call(methodName: "friends.deleteList", parameters: parameters);
		}

		/// <summary>
		/// Возвращает список меток друзей текущего пользователя.
		/// </summary>
		/// <param name="userId">
		/// Идентификатор пользователя. положительное число, по умолчанию идентификатор
		/// текущего пользователя
		/// (Положительное число, по умолчанию идентификатор текущего пользователя).
		/// </param>
		/// <param name="returnSystem">
		/// Возвращать ли системный список публичных меток друзей пользователя. флаг, может
		/// принимать
		/// значения 1 или 0 (Флаг, может принимать значения 1 или 0).
		/// </param>
		/// <returns>
		/// После успешного выполнения возвращает список объектов, каждый из которых
		/// содержит следующие поля:
		/// name — название списка друзей;
		/// id — идентификатор списка друзей.
		/// </returns>
		/// <remarks>
		/// Страница документации ВКонтакте http://vk.com/dev/friends.getLists
		/// </remarks>
		public VkCollection<FriendList> GetLists(long? userId = null, bool? returnSystem = null)
		{
			var parameters = new VkParameters
			{
					{ "user_id", userId }
					, { "return_system", returnSystem }
			};

			return _vk.Call(methodName: "friends.getLists", parameters: parameters).ToVkCollectionOf<FriendList>(selector: x => x);
		}

		/// <summary>
		/// Редактирует существующий список друзей текущего пользователя.
		/// </summary>
		/// <param name="name"> Название списка друзей. строка (Строка). </param>
		/// <param name="listId">
		/// Идентификатор списка друзей. положительное число, обязательный параметр
		/// (Положительное число,
		/// обязательный параметр).
		/// </param>
		/// <param name="userIds">
		/// Идентификаторы пользователей, включенных в список. список положительных чисел,
		/// разделенных
		/// запятыми (Список положительных чисел, разделенных запятыми).
		/// </param>
		/// <param name="addUserIds">
		/// Идентификаторы пользователей, которых необходимо добавить в список. (в случае
		/// если не передан
		/// user_ids) список положительных чисел, разделенных запятыми (Список
		/// положительных чисел, разделенных запятыми).
		/// </param>
		/// <param name="deleteUserIds">
		/// Идентификаторы пользователей, которых необходимо изъять из списка. (в случае
		/// если не
		/// передан user_ids) список положительных чисел, разделенных запятыми (Список
		/// положительных чисел, разделенных
		/// запятыми).
		/// </param>
		/// <returns>
		/// После успешного выполнения возвращает 1.
		/// </returns>
		/// <remarks>
		/// Страница документации ВКонтакте http://vk.com/dev/friends.editList
		/// </remarks>
		public bool EditList(long listId
							, string name = null
							, IEnumerable<long> userIds = null
							, IEnumerable<long> addUserIds = null
							, IEnumerable<long> deleteUserIds = null)
		{
			VkErrors.ThrowIfNumberIsNegative(expr: () => listId);

			var parameters = new VkParameters
			{
					{ "name", name }
					, { "list_id", listId }
					, { "user_ids", userIds }
					, { "add_user_ids", addUserIds }
					, { "delete_user_ids", deleteUserIds }
			};

			return _vk.Call(methodName: "friends.editList", parameters: parameters);
		}

		/// <summary>
		/// Отмечает все входящие заявки на добавление в друзья как просмотренные.
		/// </summary>
		/// <returns>
		/// После успешного выполнения возвращает 1.
		/// </returns>
		/// <remarks>
		/// Страница документации ВКонтакте http://vk.com/dev/friends.deleteAllRequests
		/// </remarks>
		public bool DeleteAllRequests()
		{
			return _vk.Call(methodName: "friends.deleteAllRequests", parameters: VkParameters.Empty);
		}

		/// <summary>
		/// Одобряет или создает заявку на добавление в друзья.
		/// </summary>
		/// <param name="userId">
		/// Идентификатор пользователя, которому необходимо отправить заявку, либо заявку
		/// от которого
		/// необходимо одобрить. положительное число, обязательный параметр (Положительное
		/// число, обязательный параметр).
		/// </param>
		/// <param name="text">
		/// Текст сопроводительного сообщения для заявки на добавление в друзья.
		/// Максимальная длина сообщения —
		/// 500 символов. строка (Строка).
		/// </param>
		/// <param name="follow">
		/// Флаг, может принимать значения 1 или 0 (Флаг, может
		/// принимать значения 1 или 0).
		/// </param>
		/// <param name="captchaSid">
		/// Id капчи (только если для вызова метода необходимо
		/// ввести капчу)
		/// </param>
		/// <param name="captchaKey">
		/// Текст капчи (только если для вызова метода необходимо
		/// ввести капчу)
		/// </param>
		/// <returns>
		/// После успешного выполнения возвращает одно из следующих значений:
		/// 1 — заявка на добавление данного пользователя в друзья отправлена;
		/// 2 — заявка на добавление в друзья от данного пользователя одобрена;
		/// 4 — повторная отправка заявки.
		/// </returns>
		/// <remarks>
		/// Страница документации ВКонтакте http://vk.com/dev/friends.add
		/// </remarks>
		public AddFriendStatus Add(long userId, string text = "", bool? follow = null, long? captchaSid = null, string captchaKey = null)
		{
			var parameters = new VkParameters
			{
					{ "user_id", userId }
					, { "text", text }
					, { "follow", follow }
					, { "captcha_sid", captchaSid }
					, { "captcha_key", captchaKey }
			};

			return _vk.Call(methodName: "friends.add", parameters: parameters);
		}

		/// <summary>
		/// Удаляет пользователя из списка друзей или отклоняет заявку в друзья.
		/// </summary>
		/// <param name="userId">
		/// Идентификатор пользователя, которого необходимо удалить из списка друзей, либо
		/// заявку от которого
		/// необходимо отклонить. положительное число, обязательный параметр (Положительное
		/// число, обязательный параметр).
		/// </param>
		/// <returns>
		/// После успешного выполнения начиная с версии 5.28 возвращается объект с полями:
		/// success — удалось успешно удалить друга
		/// friend_deleted — был удален друг
		/// out_request_deleted  — отменена исходящая заявка
		/// in_request_deleted  — отклонена входящая заявка
		/// suggestion_deleted  — отклонена рекомендация друга
		/// Для версии 5.27 и более старых возвращает одно из следующих значений:
		/// 1 — пользователь удален из списка друзей;
		/// 2 — заявка на добавление в друзья данного пользователя отклонена (входящая или
		/// исходящая);
		/// 3 — рекомендация добавить в друзья данного пользователя удалена.
		/// </returns>
		/// <remarks>
		/// Страница документации ВКонтакте http://vk.com/dev/friends.delete
		/// </remarks>
		public FriendsDeleteResult Delete(long userId)
		{
			VkErrors.ThrowIfNumberIsNegative(expr: () => userId);

			var parameters = new VkParameters
			{
					{ "user_id", userId }
			};

			return _vk.Call(methodName: "friends.delete", parameters: parameters);
		}

		/// <summary>
		/// Редактирует списки друзей для выбранного друга.
		/// </summary>
		/// <param name="userId">
		/// Идентификатор пользователя (из числа друзей), для которого необходимо
		/// отредактировать списки
		/// друзей. положительное число, обязательный параметр (Положительное число,
		/// обязательный параметр).
		/// </param>
		/// <param name="listIds">
		/// Идентификаторы списков друзей, в которые нужно добавить пользователя. список
		/// положительных чисел,
		/// разделенных запятыми (Список положительных чисел, разделенных запятыми).
		/// </param>
		/// <returns>
		/// После успешного выполнения возвращает 1.
		/// </returns>
		/// <remarks>
		/// Страница документации ВКонтакте http://vk.com/dev/friends.edit
		/// </remarks>
		public bool Edit(long userId, IEnumerable<long> listIds)
		{
			VkErrors.ThrowIfNumberIsNegative(expr: () => userId);

			var parameters = new VkParameters
			{
					{ "user_id", userId }
					, { "list_ids", listIds }
			};

			return _vk.Call(methodName: "friends.edit", parameters: parameters);
		}

		/// <summary>
		/// Возвращает список идентификаторов недавно добавленных друзей текущего
		/// пользователя.
		/// </summary>
		/// <param name="count">
		/// Максимальное количество недавно добавленных друзей, которое необходимо
		/// получить. положительное
		/// число, по умолчанию 100, максимальное значение 1000 (Положительное число, по
		/// умолчанию 100, максимальное значение
		/// 1000).
		/// </param>
		/// <returns>
		/// После успешного выполнения возвращает отсортированный в антихронологическом
		/// порядке список идентификаторов (id)
		/// недавно добавленных друзей текущего пользователя.
		/// </returns>
		/// <remarks>
		/// Страница документации ВКонтакте http://vk.com/dev/friends.getRecent
		/// </remarks>
		public ReadOnlyCollection<long> GetRecent(long? count = null)
		{
			VkErrors.ThrowIfNumberIsNegative(expr: () => count);

			var parameters = new VkParameters { { "count", count } };
			VkResponseArray response = _vk.Call(methodName: "friends.getRecent", parameters: parameters);

			return response.ToReadOnlyCollectionOf<long>(selector: x => x);
		}

		/// <summary>
		/// Возвращает информацию о полученных или отправленных заявках на добавление в
		/// друзья для текущего пользователя.
		/// </summary>
		/// <param name="params"> Входные параметры выборки. </param>
		/// <returns>
		/// Если не установлен параметр need_mutual, то в случае успеха возвращает
		/// отсортированный в антихронологическом
		/// порядке по времени подачи заявки список идентификаторов (id) пользователей
		/// (кому или от кого пришла заявка).
		/// Если установлен параметр need_mutual, то в случае успеха возвращает
		/// отсортированный в антихронологическом порядке
		/// по времени подачи заявки массив объектов, содержащих информацию о заявках на
		/// добавление в друзья.
		/// Каждый из объектов содержит поле uid, являющийся идентификатором пользователя.
		/// При наличии общих друзей, в объекте будет содержаться поле mutual, в котором
		/// будет находиться список
		/// идентификаторов общих друзей.
		/// </returns>
		/// <remarks>
		/// Страница документации ВКонтакте http://vk.com/dev/friends.getRequests
		/// </remarks>
		public GetRequestsResult GetRequests(FriendsGetRequestsParams @params)
		{
			const string errorMessage =
					"Для получения расширенной информации используйте метод GetRequestsExtended(FriendsGetRequestsParams @params)";

			if (@params.Extended.HasValue && @params.Extended.Value)
			{
				throw new ParameterMissingOrInvalidException(message: errorMessage);
			}

			if (@params.NeedMutual.HasValue && @params.NeedMutual.Value)
			{
				throw new ParameterMissingOrInvalidException(message: errorMessage);
			}

			return _vk.Call<GetRequestsResult>(methodName: "friends.getRequests", parameters: @params);
		}

		/// <summary>
		/// Возвращает информацию о полученных или отправленных заявках на добавление в
		/// друзья для текущего пользователя.
		/// </summary>
		/// <param name="params"> Входные параметры выборки. </param>
		/// <returns>
		/// Если не установлен параметр need_mutual, то в случае успеха возвращает
		/// отсортированный в антихронологическом
		/// порядке по времени подачи заявки список идентификаторов (id) пользователей
		/// (кому или от кого пришла заявка).
		/// Если установлен параметр need_mutual, то в случае успеха возвращает
		/// отсортированный в антихронологическом порядке
		/// по времени подачи заявки массив объектов, содержащих информацию о заявках на
		/// добавление в друзья.
		/// Каждый из объектов содержит поле uid, являющийся идентификатором пользователя.
		/// При наличии общих друзей, в объекте будет содержаться поле mutual, в котором
		/// будет находиться список
		/// идентификаторов общих друзей.
		/// </returns>
		/// <remarks>
		/// Страница документации ВКонтакте http://vk.com/dev/friends.getRequests
		/// </remarks>
		public VkCollection<FriendsGetRequestsResult> GetRequestsExtended(FriendsGetRequestsParams @params)
		{
			@params.Extended = true;

			return _vk.Call<VkCollection<FriendsGetRequestsResult>>(methodName: "friends.getRequests", parameters: @params);
		}

		/// <summary>
		/// Возвращает список профилей пользователей, которые могут быть друзьями текущего
		/// пользователя.
		/// </summary>
		/// <param name="filter">
		/// Типы предлагаемых друзей, которые нужно вернуть,
		/// перечисленные через запятую.
		/// </param>
		/// <param name="count">
		/// Количество рекомендаций, которое необходимо вернуть. положительное число,
		/// максимальное значение
		/// 500, по умолчанию 500 (положительное число, максимальное значение 500, по
		/// умолчанию 500).
		/// </param>
		/// <param name="offset">
		/// Смещение, необходимое для выбора определённого подмножества списка.
		/// положительное число
		/// (положительное число).
		/// </param>
		/// <param name="fields">
		/// Список дополнительных полей, которые необходимо вернуть. Доступные значения:
		/// nickname,
		/// screen_name, sex, bdate, city, country, timezone, photo_50, photo_100,
		/// photo_200_orig, has_mobile, contacts,
		/// education, online, counters, relation, last_seen, status,
		/// can_write_private_message, can_see_all_posts, can_post,
		/// universities список строк, разделенных через запятую(список строк, разделенных
		/// через запятую).
		/// </param>
		/// <param name="nameCase">
		/// Падеж для склонения имени и фамилии пользователя. Возможные значения:
		/// именительный – nom,
		/// родительный – gen, дательный – dat, винительный – acc, творительный – ins,
		/// предложный – abl. По умолчанию nom.
		/// строка (строка).
		/// </param>
		/// <returns>
		/// После успешного выполнения возвращает список объектов пользователей с
		/// дополнительным полем found_with для
		/// пользователей, найденных через импорт контактов. Для некоторых пользователей,
		/// которые были найдены давно поле
		/// found_with может отсутствовать.
		/// </returns>
		/// <remarks>
		/// Страница документации ВКонтакте http://vk.com/dev/friends.getSuggestions
		/// </remarks>
		public VkCollection<User> GetSuggestions(FriendsFilter filter = null
												, long? count = null
												, long? offset = null
												, UsersFields fields = null
												, NameCase nameCase = null)
		{
			var parameters = new VkParameters
			{
					{ "filter", filter }
					, { "count", count }
					, { "offset", offset }
					, { "fields", fields }
					, { "name_case", nameCase }
			};

			return _vk.Call(methodName: "friends.getSuggestions", parameters: parameters).ToVkCollectionOf<User>(selector: x => x);
		}

		/// <summary>
		/// Возвращает список друзей пользователя, у которых завалидированные или указанные
		/// в профиле телефонные номера входят
		/// в заданный список.
		/// </summary>
		/// <param name="phones">
		/// Список телефонных номеров в формате MSISDN, разделеннных запятыми. Например
		/// +79219876543,+79111234567
		/// Максимальное количество номеров в списке — 1000. список строк, разделенных
		/// через запятую (Список строк, разделенных
		/// через запятую).
		/// </param>
		/// <param name="fields">
		/// Список дополнительных полей, которые необходимо вернуть.
		/// Доступные значения: nickname, screen_name, sex, bdate, city, country, timezone,
		/// photo_50, photo_100,
		/// photo_200_orig, has_mobile, contacts, education, online, counters, relation,
		/// last_seen, status,
		/// can_write_private_message, can_see_all_posts, can_post, universities список
		/// строк, разделенных через запятую
		/// (Список строк, разделенных через запятую).
		/// </param>
		/// <returns>
		/// После успешного выполнения возвращает список объектов пользователей с
		/// дополнительным полем phone, в котором
		/// содержится номер из списка заданных для поиска номеров.
		/// </returns>
		/// <remarks>
		/// Страница документации ВКонтакте http://vk.com/dev/friends.getByPhones
		/// </remarks>
		public ReadOnlyCollection<User> GetByPhones(IEnumerable<string> phones, ProfileFields fields)
		{
			var parameters = new VkParameters
			{
					{ "phones", phones }
					, { "fields", fields }
			};

			return _vk.Call(methodName: "friends.getByPhones", parameters: parameters).ToReadOnlyCollectionOf<User>(selector: x => x);
		}

		/// <summary>
		/// Позволяет искать по списку друзей пользователей.
		/// </summary>
		/// <param name="params"> Входные параметры выборки. </param>
		/// <returns>
		/// После успешного выполнения метод  возвращает список объектов пользователей.
		/// </returns>
		/// <remarks>
		/// Страница документации ВКонтакте http://vk.com/dev/friends.search
		/// </remarks>
		public VkCollection<User> Search(FriendsSearchParams @params)
		{
			return _vk.Call(methodName: "friends.search", parameters: @params).ToVkCollectionOf<User>(selector: x => x);
		}

		/// <summary>
		/// Создает новый список друзей у текущего пользователя.
		/// </summary>
		/// <param name="name"> Название создаваемого списка друзей. </param>
		/// <returns>
		/// После успешного выполнения возвращает идентификатор созданного списка
		/// друзей.
		/// </returns>
		/// <remarks>
		/// Для вызова этого метода Ваше приложение должно иметь права с битовой маской,
		/// содержащей Settings.Friends
		/// Страница документации ВКонтакте http://vk.com/dev/friends.addList
		/// </remarks>
		public long AddList(string name)
		{
			return AddList(name: name, userIds: Enumerable.Empty<long>());
		}

		/// <summary>
		/// Позволяет получить список идентификаторов пользователей, доступных для вызова в
		/// приложении, используя метод JSAPI
		/// callUser.
		/// Подробнее о схеме вызова из приложений.
		/// </summary>
		/// <param name="fields">
		/// Список дополнительных полей, которые необходимо вернуть.
		/// Доступные значения: nickname, domain, sex, bdate, city, country, timezone,
		/// photo_50, photo_100, photo_200_orig,
		/// has_mobile, contacts, education, online, relation, last_seen, status,
		/// can_write_private_message, can_see_all_posts,
		/// can_post, universities список строк, разделенных через запятую (Список строк,
		/// разделенных через запятую).
		/// </param>
		/// <param name="nameCase">
		/// Падеж для склонения имени и фамилии пользователя. Возможные значения:
		/// именительный – nom,
		/// родительный – gen, дательный – dat, винительный – acc, творительный – ins,
		/// предложный – abl. По умолчанию nom.
		/// строка, по умолчанию Nom (Строка, по умолчанию Nom).
		/// </param>
		/// <returns>
		/// После успешного выполнения возвращает список идентификаторов (id) друзей
		/// пользователя, доступных для вызова, если
		/// параметр fields не использовался.
		/// При использовании параметра fields  возвращает список объектов пользователей.
		/// </returns>
		/// <remarks>
		/// Страница документации ВКонтакте http://vk.com/dev/friends.getAvailableForCall
		/// </remarks>
		public VkCollection<User> GetAvailableForCall(ProfileFields fields, NameCase nameCase)
		{
			var parameters = new VkParameters
			{
					{ "fields", fields }
					, { "name_case", nameCase }
			};

			return _vk.Call(methodName: "friends.getAvailableForCall", parameters: parameters)
					.ToVkCollectionOf(selector: x => fields != null ? new User { Id = x } : x);
		}
	}
}