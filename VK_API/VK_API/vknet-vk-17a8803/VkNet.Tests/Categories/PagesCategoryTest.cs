﻿using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using VkNet.Categories;
using VkNet.Enums;
using VkNet.Model.RequestParams;

namespace VkNet.Tests.Categories
{
	[TestFixture]
	[SuppressMessage("ReSharper", "PublicMembersMustHaveComments")]
	[ExcludeFromCodeCoverage]
	public class PagesCategoryTest : BaseTest
	{
		private PagesCategory GetMockedPagesCategory(string url, string json)
		{
			Json = json;
			Url = url;

			return new PagesCategory(Api);
		}

		[Test]
		public void ClearCache()
		{
			const string url = "https://api.vk.com/method/pages.clearCache";

			const string json =
					@"{
					'response': 1
				  }";

			var db = GetMockedPagesCategory(url, json);

			var cache = db.ClearCache(new Uri("https://www.vk.com/dev/groups.addLink"));

			Assert.That(cache, Is.True);
		}

		[Test]
		public void Get1_NormalCase()
		{
			const string url = "https://api.vk.com/method/pages.get";

			const string json =
					@"{
					'response': {
					  'id': 50050492,
					  'group_id': 103292418,
					  'title': 'Свежие новости',
					  'current_user_can_edit': 1,
					  'current_user_can_edit_access': 1,
					  'who_can_view': 1,
					  'who_can_edit': 0,
					  'edited': 1444643546,
					  'created': 1444643546,
					  'views': 1,
					  'editor_id': 32190123,
					  'creator_id': 32190123,
					  'view_url': 'http://m.vk.com/page-103292418_50050492?api_view=bdf796b3489e4adbc46be1cb81863e'
					}
				  }";

			var db = GetMockedPagesCategory(url, json);

			var page = db.Get(new PagesGetParams
			{
					OwnerId = -103292418
					, Title = "Свежие новости"
			});

			Assert.That(page.Id, Is.EqualTo(50050492));
			Assert.That(page.GroupId, Is.EqualTo(103292418));
			Assert.That(page.Title, Is.EqualTo("Свежие новости"));
			Assert.That(page.CurrentUserCanEdit, Is.EqualTo(true));
			Assert.That(page.CurrentUserCanEditAccess, Is.EqualTo(true));

			Assert.That(page.WhoCanEdit, Is.EqualTo(PageAccessKind.OnlyAdministrators));
			Assert.That(page.WhoCanView, Is.EqualTo(PageAccessKind.OnlyMembers));
			Assert.That(page.Edited, Is.EqualTo("1444643546"));
			Assert.That(page.Created, Is.EqualTo("1444643546"));
			Assert.That(page.EditorId, Is.EqualTo(32190123));
			Assert.That(page.CreatorId, Is.EqualTo(32190123));

			Assert.That(page.ViewUrl
					, Is.EqualTo("http://m.vk.com/page-103292418_50050492?api_view=bdf796b3489e4adbc46be1cb81863e"));
		}

		[Test]
		public void Get2_NormalCase()
		{
			const string url = "https://api.vk.com/method/pages.get";

			const string json =
					@"{
					'response': {
					  'id': 50050492,
					  'group_id': 103292418,
					  'title': 'Свежие новости',
					  'current_user_can_edit': 1,
					  'current_user_can_edit_access': 1,
					  'who_can_view': 1,
					  'who_can_edit': 0,
					  'edited': 1444643546,
					  'created': 1444643546,
					  'views': 1,
					  'editor_id': 32190123,
					  'creator_id': 32190123,
					  'view_url': 'http://m.vk.com/page-103292418_50050492?api_view=bdf796b3489e4adbc46be1cb81863e'
					}
				  }";

			var db = GetMockedPagesCategory(url, json);

			var page = db.Get(new PagesGetParams
			{
					OwnerId = -103292418
					, PageId = 50050492
			});

			Assert.That(page.Id, Is.EqualTo(50050492));
			Assert.That(page.GroupId, Is.EqualTo(103292418));
			Assert.That(page.Title, Is.EqualTo("Свежие новости"));
			Assert.That(page.CurrentUserCanEdit, Is.EqualTo(true));
			Assert.That(page.CurrentUserCanEditAccess, Is.EqualTo(true));

			Assert.That(page.WhoCanEdit, Is.EqualTo(PageAccessKind.OnlyAdministrators));
			Assert.That(page.WhoCanView, Is.EqualTo(PageAccessKind.OnlyMembers));
			Assert.That(page.Edited, Is.EqualTo("1444643546"));
			Assert.That(page.Created, Is.EqualTo("1444643546"));
			Assert.That(page.EditorId, Is.EqualTo(32190123));
			Assert.That(page.CreatorId, Is.EqualTo(32190123));

			Assert.That(page.ViewUrl
					, Is.EqualTo("http://m.vk.com/page-103292418_50050492?api_view=bdf796b3489e4adbc46be1cb81863e"));
		}

		[Test]
		public void GetHistory_NormalCase()
		{
			const string url = "https://api.vk.com/method/pages.getHistory";

			const string json =
					@"{
					'response': [
						{
						'id': 184657345,
						'length': 3,
						'date': 1444651212,
						'editor_id': 32190123,
						'editor_name': 'Максим Инютин'
						}, {
						'id': 184657327,
						'length': 3,
						'date': 1444651093,
						'editor_id': 32190123,
						'editor_name': 'Максим Инютин'
						}, {
						'id': 184657321,
						'length': 3,
						'date': 1444651048,
						'editor_id': 32190123,
						'editor_name': 'Максим Инютин'
						}, {
						'id': 184657309,
						'length': 3,
						'date': 1444650925,
						'editor_id': 32190123,
						'editor_name': 'Максим Инютин'
						}, {
						'id': 184657308,
						'length': 3,
						'date': 1444650923,
						'editor_id': 32190123,
						'editor_name': 'Максим Инютин'
						}, {
						'id': 184657135,
						'length': 4,
						'date': 1444644359,
						'editor_id': 32190123,
						'editor_name': 'Максим Инютин'
						}
					]
				  }";

			var db = GetMockedPagesCategory(url, json);

			var histories = db.GetHistory(50050492, 103292418);

			Assert.That(histories, Is.Not.Null);
		}

		[Test]
		public void GetTitles_NormalCase()
		{
			const string url = "https://api.vk.com/method/pages.getTitles";

			const string json =
					@"{
					'response': [{
						'id': 50010549,
						'title': 'Условия оплаты и доставки',
						'group_id': 103292418,
						'created': 1443433192,
						'edited': 1444653608,
						'who_can_view': 2,
						'who_can_edit': 0,
						'views': 0,
						'creator_id': 32190123,
						'creator_name': 'Максим Инютин',
						'editor_id': 32190123,
						'editor_name': 'Максим Инютин'
						}, {
						'id': 50050492,
						'title': 'Свежие новости',
						'group_id': 103292418,
						'created': 1444643546,
						'edited': 1444651212,
						'who_can_view': 0,
						'who_can_edit': 0,
						'views': 1,
						'creator_id': 32190123,
						'creator_name': 'Максим Инютин',
						'editor_id': 32190123,
						'editor_name': 'Максим Инютин'
						}
					]
				  }";

			var db = GetMockedPagesCategory(url, json);

			var titles = db.GetTitles(103292418);

			Assert.That(titles, Is.Not.Null);
		}

		[Test]
		public void GetVersion_NormalCase()
		{
			const string url = "https://api.vk.com/method/pages.getVersion";

			const string json =
					@"{
					'response': {
						'id': 184657135,
						'page_id': 50050492,
						'group_id': 103292418,
						'title': 'Свежие новости',
						'source': 'test',
						'current_user_can_edit': 1,
						'who_can_view': 0,
						'who_can_edit': 0,
						'version_created': 1444644359,
						'creator_id': 32190123,
						'html': '<!--4-->test '
					}
				  }";

			var db = GetMockedPagesCategory(url, json);

			var version = db.GetVersion(184657135, 103292418);

			Assert.That(version.Id, Is.EqualTo(50050492));
			Assert.That(version.GroupId, Is.EqualTo(103292418));
			Assert.That(version.Title, Is.EqualTo("Свежие новости"));
			Assert.That(version.Source, Is.EqualTo("test"));
			Assert.That(version.CurrentUserCanEdit, Is.EqualTo(true));
			Assert.That(version.WhoCanView, Is.EqualTo(PageAccessKind.OnlyAdministrators));
			Assert.That(version.WhoCanEdit, Is.EqualTo(PageAccessKind.OnlyAdministrators));
			Assert.That(version.VersionCreated, Is.EqualTo("1444644359"));
			Assert.That(version.CreatorId, Is.EqualTo(32190123));
			Assert.That(version.Html, Is.EqualTo("<!--4-->test "));
		}

		[Test]
		public void Save1_NormalCase()
		{
			const string url = "https://api.vk.com/method/pages.save";

			const string json =
					@"{
					'response': 50050492
				  }";

			var db = GetMockedPagesCategory(url, json);

			var page = db.Save("123", 123, 32190123, "Свежие новости", 103292418);

			Assert.That(page, Is.EqualTo(50050492));
		}

		[Test]
		public void Save2_NormalCase()
		{
			const string url = "https://api.vk.com/method/pages.save";

			const string json =
					@"{
					'response': 50050492
				  }";

			var db = GetMockedPagesCategory(url, json);

			var page = db.Save("123", 50050492, 32190123, "", 103292418);

			Assert.That(page, Is.EqualTo(50050492));
		}

		[Test]
		public void SaveAccess_NormalCase()
		{
			const string url = "https://api.vk.com/method/pages.saveAccess";

			const string json =
					@"{
					'response': 50050492
				  }";

			var db = GetMockedPagesCategory(url, json);

			var page = db.SaveAccess(50050492, 103292418);

			Assert.That(page, Is.EqualTo(50050492));
		}
	}
}