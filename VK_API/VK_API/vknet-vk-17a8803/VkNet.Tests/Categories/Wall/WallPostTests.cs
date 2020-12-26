﻿using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using VkNet.Exception;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace VkNet.Tests.Categories.Wall
{
	[ExcludeFromCodeCoverage]
	public class WallPostTests : BaseTest
	{
		[Test]
		public void Post_ReturnValidateNeeded()
		{
			Url = "https://api.vk.com/method/wall.post";

			Json =
					@"
                {
                    'error' : {
                    'error_code' : 17,
                    'error_msg' : 'Validation required: please open redirect_uri in browser',
                    'redirect_uri' : 'https://m.vk.com/activation?act=validate&api_hash=****&hash=***',
                    'request_params' : [
                        {
                        'key' : 'oauth',
                        'value' : '1'
                        },
                        {
                        'key' : 'method',
                        'value' : 'wall.post'
                        },
                        {
                        'key' : 'owner_id',
                        'value' : '-153877099'
                        },
                        {
                        'key' : 'from_group',
                        'value' : '1'
                        },
                        {
                        'key' : 'message',
                        'value' : 'Test'
                        },
                        {
                        'key' : 'v',
                        'value' : '5.64'
                        }
                    ]
                    }
                 }
                 ";

			Assert.That(() => VkErrors.IfErrorThrowException(Json), Throws.TypeOf<NeedValidationException>());
		}

		[Test]
		public void Post_AccessToAddingPostDenied()
		{
			Url = "https://api.vk.com/method/wall.post";

			Json =
					@"{
                   'error': {
                     'error_code': 214,
                     'error_msg': 'Access to adding post denied: access to the wall is closed',
                     'request_params': [
                       {
                         'key': 'oauth',
                         'value': '1'
                       },
                       {
                         'key': 'method',
                         'value': 'wall.post'
                       },
                       {
                         'key': 'owner_id',
                         'value': '411773664'
                       },
                       {
                         'key': 'message',
                         'value': 'Privet'
                       },
                       {
                         'key': 'v',
                         'value': '5.74'
                       }
                     ]
                   }
                 }";

			Assert.Throws<PostLimitException>(() => Api.Wall.Post(new WallPostParams()));
		}
	}
}