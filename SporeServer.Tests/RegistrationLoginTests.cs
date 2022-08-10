/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */
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

        [Fact]
        public async Task Test_Registration()
        {
            var client = _factory.CreateClient();

            Assert.True(await TestsHelper.RegisterTestUser(client));
        }

        [Fact]
        public async Task Test_Login()
        {
            var client = _factory.CreateClient();

            await TestsHelper.RegisterTestUser(client);
            TestsHelper.LoginAsTestUser(client);

            // first endpoint the game calls to retrieve user data
            // like the username, user id, creations & aggregators of the user
            var response = await client.GetAsync("/pollinator/handshake");

            response.EnsureSuccessStatusCode();
        }
    }
}