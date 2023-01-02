/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */
using SporeServer.Tests.Helpers;
using System.Threading.Tasks;
using Xunit;

namespace SporeServer.Tests
{
    public class RegistrationLoginTests : IClassFixture<CustomWebApplicationFactory<SporeServer.Startup>>
    {
        private readonly CustomWebApplicationFactory<SporeServer.Startup> _factory;

        public RegistrationLoginTests(CustomWebApplicationFactory<SporeServer.Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("test1@tests.com", "<7L9BTj%maSf", "TestUser1", true)]
        [InlineData("test3@tests", "wY~`j##wor9(", "TestUser2", false)]                // can't register invalid email
        [InlineData("test3@tests.com", "TestUser1", "TestUser3", false)]               // can't register invalid password
        [InlineData("test3@tests.com", "|//[FIsBD9z", "ThisUserNameIsTooLong", false)] // can't register invalid username
        public async Task Test_RegisterUser(string email, string password, string displayName, bool expectSuccess = true)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync($"/community/auth/registerNew?Email={email}&Password={password}&DisplayName={displayName}");

            response.EnsureSuccessStatusCode();

            var responseContentString = await response.Content.ReadAsStringAsync();

            if (expectSuccess)
            {
                Assert.Contains("Successfully registered.", responseContentString);
            }
            else
            {
                Assert.DoesNotContain("Successfully registered.", responseContentString);
            }
        }

        [Fact]
        public async Task Test_RegisterTestUser()
        {
            var client = _factory.CreateClient();

            Assert.True(await UserHelpers.RegisterUser(client, UserHelpers.Users.TestUser1));
        }

        [Fact]
        public async Task Test_Login()
        {
            var client = _factory.CreateClient();

            await UserHelpers.RegisterUser(client, UserHelpers.Users.TestUser1);
            UserHelpers.LoginAsUser(client, UserHelpers.Users.TestUser1);

            // first endpoint the game calls to retrieve user data
            // like the username, user id, creations & aggregators of the user
            var response = await client.GetAsync("/pollinator/handshake");

            response.EnsureSuccessStatusCode();
        }
    }
}