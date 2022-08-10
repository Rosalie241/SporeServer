/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */
using Microsoft.AspNetCore.Identity;
using SporeServer.Areas.Identity.Data;
using SporeServer.Services;
using System;
using System.Threading.Tasks;

namespace SporeServer.Controllers
{
    public static class ControllerHelper
    {
        /// <summary>
        ///     Simple helper function for atom/(un)subscribe which tries to get SporeServerUser from query, returns null when not found
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async Task<SporeServerUser> GetUserFromQuery(UserManager<SporeServerUser> userManager, string query)
        {
            // ensure uriQuery isnt null
            if (String.IsNullOrEmpty(query))
            {
                return null;
            }

            // make sure the uri request starts with
            // the correct tag
            if (!query.StartsWith("tag:spore.com,2006:user/"))
            {
                return null;
            }

            string uriUser = query.Remove(0, 24);

            // make sure we can parse the user id
            if (!Int64.TryParse(uriUser, out Int64 userId))
            {
                return null;
            }

            return await userManager.FindByIdAsync($"{userId}");
        }

        /// <summary>
        ///     Simple helper function for atom/(un)subscribe which tries to get SporeServerAggregator from a query, returns null when not found
        /// </summary>
        public static async Task<SporeServerAggregator> GetAggregatorFromQuery(IAggregatorManager aggregatorManager, string query)
        {
            // ensure uriQuery isnt null
            if (String.IsNullOrEmpty(query))
            {
                return null;
            }

            // make sure the uri request starts with
            // the correct tag
            if (!query.StartsWith("tag:spore.com,2006:aggregator/"))
            {
                return null;
            }

            string uriAggregator = query.Remove(0, 30);

            // make sure we can parse the aggregator id
            if (!Int64.TryParse(uriAggregator, out Int64 aggregatorId))
            {
                return null;
            }


            return await aggregatorManager.FindByIdAsync(aggregatorId);
        }

    }
}
