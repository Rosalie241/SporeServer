﻿/*
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

namespace SporeServer.Tests
{
    internal static class TestsHelper
    {
        private class TestUserInfo
        {
            public static readonly string Email = "tests@tests.com";
            public static readonly string Password = "TestsUser1_";
            public static readonly string DisplayName = "TestsUser";
        }

        private static bool HasRegisteredTestUser = false;
        private static bool RegisteredReturnValue = false;

        /// <summary>
        ///     Registers Test User
        /// </summary>
        public static async Task<bool> RegisterTestUser(HttpClient client)
        {
            // we should only try to register once
            if (HasRegisteredTestUser)
            {
                return RegisteredReturnValue;
            }

            var response = await client.GetAsync($"/community/auth/registerNew?Email={TestUserInfo.Email}&Password={TestUserInfo.Password}&DisplayName={TestUserInfo.DisplayName}");

            HasRegisteredTestUser = true;
            RegisteredReturnValue = response.IsSuccessStatusCode && (await response.Content.ReadAsStringAsync()).Contains("Successfully registered.");
            return RegisteredReturnValue;
        }


        /// <summary>
        ///     Sets required headers to login as Test User
        /// </summary>
        public static void LoginAsTestUser(HttpClient client)
        {
            client.DefaultRequestHeaders.Add("Spore-User", TestUserInfo.Email);
            client.DefaultRequestHeaders.Add("Spore-Password", TestUserInfo.Password);
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
