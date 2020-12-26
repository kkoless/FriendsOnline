﻿using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using VkNet.Model;

namespace VkNet.Tests.Models
{
	[TestFixture]
	[ExcludeFromCodeCoverage]
	public class GroupModel : BaseTest
	{
		[Test]
		public void ShouldHaveField_Trending()
		{
			var group = new Group();
			Assert.That(group, Has.Property("Trending"));
		}

		[Test]
		public void Trending_ShouldBeFalse()
		{
			Json = @"{
						'id': 1153959,
						'trending': 0
					  }";

			var response = GetResponse();
			var group = Group.FromJson(response);
			Assert.That(group.Trending, Is.False);
		}

		[Test]
		public void Trending_ShouldBeFalse2()
		{
			Json = @"{
						'id': 1153959
					  }";

			var response = GetResponse();
			var group = Group.FromJson(response);
			Assert.That(group.Trending, Is.False);
		}

		[Test]
		public void Trending_ShouldBeTrue()
		{
			Json = @"{
						'id': 1153959,
						'trending': 1
					  }";

			var response = GetResponse();
			var group = Group.FromJson(response);
			Assert.That(group.Trending, Is.True);
		}
	}
}