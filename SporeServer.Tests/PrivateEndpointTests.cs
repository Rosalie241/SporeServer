/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace SporeServer.Tests
{
    public class PrivateEndpointTests : IClassFixture<CustomWebApplicationFactory<SporeServer.Startup>>
    {
        private readonly CustomWebApplicationFactory<SporeServer.Startup> _factory;

        public PrivateEndpointTests(CustomWebApplicationFactory<SporeServer.Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        // API endpoints
        [InlineData("/pollinator/handshake")]
        [InlineData("/pollinator/atom/randomAsset")]
        [InlineData("/pollinator/atom/downloadQueue")]
        [InlineData("/pollinator/atom/asset/")]
        [InlineData("/pollinator/atom/subscribe")]
        [InlineData("/pollinator/atom/unsubscribe")]
        [InlineData("/pollinator/atom/user/1")]
        [InlineData("/pollinator/atom/aggregator/100", 404)]
        [InlineData("/pollinator/atom/delete")]
        [InlineData("/pollinator/event/upload", 200, true)]
        [InlineData("/pollinator/public-interface/AssetUploadServlet", 200, true)]
        [InlineData("/pollinator/upload/status/1")]
        [InlineData("/community/assetBrowser/deleteAsset/1")]
        [InlineData("/community/assetBrowser/editSporecast", 404)]
        [InlineData("/community/assetBrowser/rate")]
        [InlineData("/community/public-interface/SnapshotUploadServlet", 200, true)]
        // XHTML pages
        [InlineData("/community/assetBrowser/achievements")]
        [InlineData("/community/assetBrowser/comment/100")]
        [InlineData("/community/assetBrowser/commentApproval")]
        [InlineData("/community/assetBrowser/createSporecast")]
        [InlineData("/community/assetBrowser/findBuddy")]
        [InlineData("/community/assetBrowser/findSporecast")]
        [InlineData("/community/assetBrowser/home")]
        [InlineData("/community/assetBrowser/leaderBoardDlg/100/100/100")]
        [InlineData("/community/assetBrowser/profile/1")]
        [InlineData("/community/assetBrowser/requiredPacks")]
        [InlineData("/community/assetBrowser/singleDownload")]
        [InlineData("/community/assetBrowser/store")]
        public async Task Test_IsPrivateEndpointAccessable(string url, int expectedStatuscode = 200, bool usePost = false)
        {
            var client = _factory.CreateClient();

            await TestsHelper.RegisterTestUser(client);
            TestsHelper.LoginAsTestUser(client);

            HttpResponseMessage response;
            
            if (usePost)
            {
                response = await client.PostAsync(url, null);
            }
            else
            {
                response = await client.GetAsync(url);
            }

            Assert.Equal(expectedStatuscode, (int)response.StatusCode);
        }
    }
}
