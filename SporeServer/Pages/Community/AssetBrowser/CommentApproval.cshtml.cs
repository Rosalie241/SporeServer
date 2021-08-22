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
    public class CommentApprovalModel : PageModel
    {
        private readonly UserManager<SporeServerUser> _userManager;
        private readonly IAssetCommentManager _assetCommentManager;

        public CommentApprovalModel(UserManager<SporeServerUser> userManager, IAssetCommentManager assetCommentManager)
        {
            _userManager = userManager;
            _assetCommentManager = assetCommentManager;
        }

        /// <summary>
        ///     Whether there are any unapproved comments
        /// </summary>
        public bool HasUnapprovedComments { get; set; }
        /// <summary>
        ///     Unapproved comments
        /// </summary>
        public SporeServerAssetComment[] UnapprovedComments { get; set; }
        /// <summary>
        ///     Whether there are any approved comments
        /// </summary>
        public bool HasApprovedComments { get; set; }
        /// <summary>
        ///     Approved comments
        /// </summary>
        public SporeServerAssetComment[] ApprovedComments { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Console.WriteLine($"{Request.Path}{Request.QueryString}");

            var author = await _userManager.GetUserAsync(User);

            var approveCommentString = Request.Query["approveComment"].ToString();
            if (Int64.TryParse(approveCommentString, out Int64 commentId))
            {
                var comment = await _assetCommentManager.FindByIdAsync(commentId);
                // only approve when found and the asset author is the current user
                if (comment != null && comment.Asset.AuthorId == author.Id)
                {
                    if (!await _assetCommentManager.ApproveAsync(comment))
                    {
                        return StatusCode(500);
                    }
                }
            }

            var rejectCommentString = Request.Query["rejectComment"].ToString();
            if (Int64.TryParse(rejectCommentString, out commentId))
            {
                var comment = await _assetCommentManager.FindByIdAsync(commentId);
                // only remove when found and the asset author is the current user
                if (comment != null && comment.Asset.AuthorId == author.Id)
                {
                    if (!await _assetCommentManager.RemoveAsync(comment))
                    {
                        return StatusCode(500);
                    }
                }
            }

            UnapprovedComments = await _assetCommentManager.FindAllUnApprovedByAssetAuthorAsync(author);
            HasUnapprovedComments = UnapprovedComments != null && UnapprovedComments.Length > 0;

            ApprovedComments = await _assetCommentManager.FindAllApprovedByAssetAuthorAsync(author);
            HasApprovedComments = ApprovedComments != null && ApprovedComments.Length > 0;

            return Page();
        }
    }
}
