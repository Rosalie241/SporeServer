/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SporeServer.Tests.Helpers
{
    internal static class UserHelpers
    {
        public static class Users
        {
            public static UserInfo TestUser1 = new UserInfo() { Email = "tests@tests.com", Password = "TestsUser1_", DisplayName = "TestsUser" };
        }

        public struct UserInfo
        {
            public string Email { get; set; }
            public string Password { get; set;  }
            public string DisplayName { get; set;  }
        }

        /// <summary>
        ///     Registers Test User
        /// </summary>
        public static async Task<bool> RegisterUser(HttpClient client, UserInfo userInfo)
        {
            var response = await client.GetAsync($"/community/auth/registerNew?Email={userInfo.Email}&Password={userInfo.Password}&DisplayName={userInfo.DisplayName}");

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }



            return (await response.Content.ReadAsStringAsync()).Contains("Successfully registered.");
        }


        /// <summary>
        ///     Sets required headers to login as Test User
        /// </summary>
        public static void LoginAsUser(HttpClient client, UserInfo userInfo)
        {
            client.DefaultRequestHeaders.Add("Spore-User", userInfo.Email);
            client.DefaultRequestHeaders.Add("Spore-Password", userInfo.Password);
        }

        /// <summary>
        ///     Retrieves the test user's next asset id
        /// </summary>
        /// <returns></returns>
        public static async Task<Int64> GetTestUserAssetNextId(HttpClient client)
        {
            var response = await client.GetAsync("/pollinator/handshake");

            if (!response.IsSuccessStatusCode)
            {
                return -1;
            }

            string handshakeXml = await response.Content.ReadAsStringAsync();

            XDocument document = XDocument.Parse(handshakeXml);

            XElement? rootElement = document.Root;
            if (rootElement == null)
            {
                return -1;
            }

            XElement? xmlElement = rootElement.Descendants().Where(e => e.Name.LocalName == "next-id").FirstOrDefault();
            if (xmlElement == null)
            {
                return -1;
            }

            if (Int64.TryParse(xmlElement.Value, out Int64 nextId))
            {
                return nextId;
            }

            return -1;
        }
    }
}
