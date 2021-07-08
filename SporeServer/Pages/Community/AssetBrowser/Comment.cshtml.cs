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
    public class CommentModel : PageModel
    {
        private readonly UserManager<SporeServerUser> _userManager;
        private readonly IAssetManager _assetManager;
        private readonly IAssetCommentManager _assetCommentManager;
        public CommentModel(UserManager<SporeServerUser> userManager, IAssetManager assetManager, IAssetCommentManager assetCommentManager)
        {
            _userManager = userManager;
            _assetManager = assetManager;
            _assetCommentManager = assetCommentManager;
        }

        /// <summary>
        ///     Whether the given asset for this page exists
        /// </summary>
        public bool AssetExists { get; set; }
        /// <summary>
        ///     Asset For Page
        /// </summary>
        public SporeServerAsset Asset { get; set; }
        /// <summary>
        ///     Comments
        /// </summary>
        public SporeServerAssetComment[] Comments { get; set; }
        public Int32 CommentsCount { get; set; }
        public async Task<IActionResult> OnGet(Int64 id)
        {
            Console.WriteLine($"/community/assetBrowser/comment/{id}");

            Asset = await _assetManager.FindByIdAsync(id);
            AssetExists = (Asset != null);

            if (!AssetExists)
            {
                return Page();
            }

            // if the comment query exists,
            // try to add the comment
            string comment = Request.Query["comment"];
            if (!String.IsNullOrEmpty(comment) && 
                comment.Length <= 150)
            {
                var author = await _userManager.GetUserAsync(User);

                if (!await _assetCommentManager.AddAsync(Asset, author, comment))
                {
                    return StatusCode(500);
                }
            }

            Comments = await _assetCommentManager.FindAllApprovedByAsset(Asset);
            CommentsCount = Comments == null ? 0 : Comments.Length;
            return Page();
        }
    }
}
