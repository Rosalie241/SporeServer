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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SporeServer.Areas.Identity.Data;
using SporeServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace SporeServer.Pages.Community.AssetBrowser
{
    [Authorize]
    public class LeaderBoardDlgModel : PageModel
    {
        private readonly ILeaderboardManager _leaderboardManager;
        private readonly UserManager<SporeServerUser> _userManager;

        public LeaderBoardDlgModel(ILeaderboardManager leaderboardManager, UserManager<SporeServerUser> userManager)
        {
            _leaderboardManager = leaderboardManager;
            _userManager = userManager;
        }

        /// <summary>
        ///     Leaderboard Entries
        /// </summary>
        public SporeServerLeaderboardEntry[] Entries { get; set; }

        public async Task<IActionResult> OnGet(Int64 id, Int32 score, Int64 time)
        {
            Entries = await _leaderboardManager.GetAllEntries()
                        .Include(e => e.Author)
                        .Where(
                            e => e.AssetId == id
                        ).OrderBy(
                            e => e.TimeInMilliseconds
                        ).OrderBy(
                            e => e.PercentageCompleted
                        ).Take(10).ToArrayAsync();

            return Page();
        }
    }
}
