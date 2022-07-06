using System;
using Bnz.API.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Bnz.API.Tests.Tests
{
    [TestFixture]
    public class UserTests
    {
        private User user;

        [Test]
        public async Task VerifyGetUsersRequest()
        {
            var actual = await BnzApi.GetUsers();
            actual.StatusCode.ShouldBe(HttpStatusCode.OK);
            var actualUsers = JsonConvert.DeserializeObject<List<User>>(actual.Content.ReadAsStringAsync().Result);
            actualUsers.Count.ShouldBe(10);
        }

        [TestCase(8, "Nicholas Runolfsdottir V")]
        public async Task VerifyGetUserRequestById(int userId, string expectedName)
        {
            var actual = await BnzApi.GetUser(userId);
            actual.StatusCode.ShouldBe(HttpStatusCode.OK);
            var actualUser = JsonConvert.DeserializeObject<User>(actual.Content.ReadAsStringAsync().Result);
            actualUser.Name.ShouldBe(expectedName);
        }

        [Test]
        public async Task VerifyPostUserRequest()
        {
            var actual = await BnzApi.CreateUser(user);
            actual.StatusCode.ShouldBe(HttpStatusCode.Created);
            var actualUsers = JsonConvert.DeserializeObject<User>(actual.Content.ReadAsStringAsync().Result);
            actualUsers.ShouldBeEquivalentTo(user);
        }

        [OneTimeSetUp]
        public void Init()
        {
            BnzApiClient.InitializeBnzApiClient();
            var config = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();
            var section = config.GetSection(nameof(User));
            user = section.Get<User>();
        }
    }
}