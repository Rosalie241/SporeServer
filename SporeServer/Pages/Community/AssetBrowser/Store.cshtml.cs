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

namespace SporeServer.Pages.Community.AssetBrowser
{
    [Authorize]
    public class StoreModel : PageModel
    {
        /// <summary>
        ///     Whether to show the big background
        /// </summary>
        public bool ShowBigBackground { get; set; }


        public IActionResult OnGet()
        {
            // parse AppResolution header
            var resolution = Request.Headers["AppResolution"].FirstOrDefault();
            if (!String.IsNullOrEmpty(resolution))
            {
                // make sure it's in the right format
                var splitResolution = resolution.Split('x');

                if (splitResolution.Length == 2)
                {
                    // the official server
                    // only checks the width,
                    // so let's do the same here
                    int width = Int32.Parse(splitResolution[0]);

                    // show big background
                    // when the resolution width
                    // is large enough
                    ShowBigBackground = width >= 1000;
                }
            }

            return Page();
        }
    }
}
