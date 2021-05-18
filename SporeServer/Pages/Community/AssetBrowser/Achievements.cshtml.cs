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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SporeServer.Areas.Identity.Data;
using SporeServer.Services;

namespace SporeServer.Pages.Community.AssetBrowser
{
    [Authorize]
    public class AchievementsModel : PageModel
    {
        private readonly IAchievementManager _achievementManager;
        private readonly UserManager<SporeServerUser> _userManager;

        public AchievementsModel(IAchievementManager achievementManager, UserManager<SporeServerUser> userManager)
        {
            _achievementManager = achievementManager;
            _userManager = userManager;
        }

        public Int64[] UnlockedAchievementIds { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var author = await _userManager.GetUserAsync(User);
            UnlockedAchievementIds = await _achievementManager.FindAllByAuthorAsync(author);

            return Page();
        }
    }
}
