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
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace SporeServer.Tests
{
    public class UploadTests : IClassFixture<CustomWebApplicationFactory<SporeServer.Startup>>
    {
        private readonly CustomWebApplicationFactory<SporeServer.Startup> _factory;

        public UploadTests(CustomWebApplicationFactory<SporeServer.Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("<events><event verb=\"0x1edc82b0\" timestamp=\"2022-08-09T17:57:38Z\"><arg>649090660</arg></event></events>")]
        [InlineData("<invalid-event />")]
        public async Task Test_UploadEvent(string eventXml, int expectedStatuscode = 200)
        {
            var client = _factory.CreateClient();

            await UserHelpers.RegisterUser(client, UserHelpers.Users.TestUser1);
            UserHelpers.LoginAsUser(client, UserHelpers.Users.TestUser1);

            var response = await client.PostAsync("/pollinator/event/upload", new StringContent(eventXml));

            Assert.Equal(expectedStatuscode, (int)response.StatusCode);
        }

        [Theory]
        [InlineData("TestUploadData/600000000000.xml", "TestUploadData/600000000000.png", "TestUploadData/600000000000_lrg.png", true, 0, "0x2b978c46")]
        //[InlineData("<events><event verb=\"0x2d59837b\" timestamp=\"2022-08-09T17:57:38Z\"/></events>")]
        public async Task Test_UploadAsset(string modelDataFileName, string thumbDataFileName, string imageDataFileName, bool expectSuccess = true, int slurp = 0, string typeId = "", string description = "", string tags = "", string traitGuids = "")
        {
            var client = _factory.CreateClient();

            await UserHelpers.RegisterUser(client, UserHelpers.Users.TestUser1);
            UserHelpers.LoginAsUser(client, UserHelpers.Users.TestUser1);
            
            Int64 nextAssetId = await UserHelpers.GetTestUserAssetNextId(client);

            // make sure GetTestUserAssetNextId() succeeded
            Assert.NotEqual(-1, nextAssetId);

            FileStream modelDataFileStream = File.OpenRead(modelDataFileName);
            FileStream thumbDataFileStream = File.OpenRead(thumbDataFileName);
            FileStream imageDataFileStream = File.OpenRead(imageDataFileName);
            StreamContent modelDataContent = new StreamContent(modelDataFileStream);
            StreamContent thumbDataContent = new StreamContent(thumbDataFileStream);
            StreamContent imageDataContent = new StreamContent(imageDataFileStream);

            HttpResponseMessage response;

            // construct asset form
            var formDataContent = new MultipartFormDataContent();
            formDataContent.Add(new StringContent(typeId), "TypeId");
            formDataContent.Add(new StringContent(nextAssetId.ToString()), "AssetId");
            formDataContent.Add(new StringContent(description), "Description");
            formDataContent.Add(new StringContent(tags), "Tags");
            formDataContent.Add(new StringContent(traitGuids), "TraitGuids");
            formDataContent.Add(new ByteArrayContent(await modelDataContent.ReadAsByteArrayAsync()), "ModelData", "ModelData");
            formDataContent.Add(new ByteArrayContent(await thumbDataContent.ReadAsByteArrayAsync()), "ThumbnailData", "ThumbnailData");
            formDataContent.Add(new ByteArrayContent(await imageDataContent.ReadAsByteArrayAsync()), "ImageData", "ImageData");

            // upload asset form
            response = await client.PostAsync($"/pollinator/public-interface/AssetUploadServlet?slurp={slurp}", formDataContent);
            response.EnsureSuccessStatusCode();

            // check if upload was successful or not
            response = await client.GetAsync($"/pollinator/upload/status/{nextAssetId}");
            response.EnsureSuccessStatusCode();

            if (expectSuccess)
            {
                Assert.Contains("<Status>Success</Status>", await response.Content.ReadAsStringAsync());
            }
            else
            {
                Assert.Contains("<Status>Failed</Status>", await response.Content.ReadAsStringAsync());
            }

            modelDataFileStream.Close();
            imageDataFileStream.Close();
        }
    }
}
