﻿using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using VkNet.Model.RequestParams;

namespace VkNet.Tests.Categories.Friends
{
	[TestFixture]
	[ExcludeFromCodeCoverage]
	public class FriendsGetRequests : BaseTest
	{
		[Test]
		public void DefaultParams()
		{
			Url = "https://api.vk.com/method/friends.getRequests";

			Json = @"{
                'response': {
                    'count': 1,
                    'items': [435460566]
                }
            }";

			var result = Api.Friends.GetRequests(new FriendsGetRequestsParams());
			Assert.NotNull(result);
			Assert.AreEqual(1, result.Count);
		}

		[Test]
		public void Extended()
		{
			Url = "https://api.vk.com/method/friends.getRequests";

			Json = @"{
                'response': {
                    'count': 1,
                    'items': [{
                        'user_id': 435460566
                    }]
                }
            }";

			var result = Api.Friends.GetRequestsExtended(new FriendsGetRequestsParams
			{
					Extended = true
			});

			Assert.NotNull(result);
			Assert.AreEqual(1, result.Count);
		}
	}
}