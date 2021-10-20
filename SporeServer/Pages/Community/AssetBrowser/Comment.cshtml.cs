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
        /// <summary>
        ///     Comments Count
        /// </summary>
        public Int32 CommentsCount { get; set; }
        /// <summary>
        ///     Whether User Has Sent A Comment
        /// </summary>
        public bool HasSentComment { get; set; }
        /// <summary>
        ///     Sent Comment
        /// </summary>
        public string SentComment { get; set; }

        public async Task<IActionResult> OnGet(Int64 id)
        {
            Console.WriteLine($"/community/assetBrowser/comment/{id}");

            Asset = await _assetManager.FindByIdAsync(id, true);
            AssetExists = (Asset != null);

            if (!AssetExists)
            {
                return Page();
            }

            // if the comment query exists,
            // try to add the comment
            SentComment = Request.Query["comment"];
            HasSentComment = !String.IsNullOrEmpty(SentComment) && SentComment.Length <= 150;
            if (HasSentComment)
            {
                var author = await _userManager.GetUserAsync(User);

                if (!await _assetCommentManager.AddAsync(Asset, author, SentComment))
                {
                    return StatusCode(500);
                }

                // no need to tell the user we've sent it
                // to the asset author for approval, when
                // the asset author comments on their own
                // creation
                HasSentComment = Asset.AuthorId != author.Id;
            }

            Comments = await _assetCommentManager.FindAllApprovedByAssetAsync(Asset);
            CommentsCount = Comments == null ? 0 : Comments.Length;
            return Page();
        }
    }
}
