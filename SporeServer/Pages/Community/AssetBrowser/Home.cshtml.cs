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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SporeServer.Data;
using Microsoft.AspNetCore.Identity;
using SporeServer.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace SporeServer.Pages.Community.AssetBrowser
{
    [Authorize]
    public class HomeModel : PageModel
    {
        private readonly SporeServerContext _context;
        private readonly UserManager<SporeServerUser> _userManager;

        public HomeModel(SporeServerContext context, UserManager<SporeServerUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public int GetSubscriberCountAsync()
        {
           // SporeServerUser user = await _userManager.GetUserAsync(User);

            //if (user == null)
            //{
              //  return -1;
            //}

            return 0; // _context.Users.Where(u => u.Buddies.Contains(user)).Count();
        }

        public void OnGet()
        {
          

        }
    }
}
