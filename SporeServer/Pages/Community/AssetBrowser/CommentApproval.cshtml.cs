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
        private readonly IBlockedUserManager _blockedUserManager;

        public CommentApprovalModel(UserManager<SporeServerUser> userManager, IAssetCommentManager assetCommentManager, IBlockedUserManager blockedUserManager)
        {
            _userManager = userManager;
            _assetCommentManager = assetCommentManager;
            _blockedUserManager = blockedUserManager;
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
        /// <summary>
        ///     Whether there are any blocked users
        /// </summary>
        public bool HasBlockedUsers { get; set; }
        /// <summary>
        ///     Blocked users
        /// </summary>
        public SporeServerBlockedUser[] BlockedUsers { get; set; }
        /// <summary>
        ///     Blocked user ids
        /// </summary>
        public Int64[] BlockedUserIds { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Console.WriteLine($"{Request.Path}{Request.QueryString}");

            var author = await _userManager.GetUserAsync(User);

            var unblockUserString = Request.Query["unblockUser"].ToString();
            if (Int64.TryParse(unblockUserString, out Int64 userId))
            {
                var user = await _userManager.FindByIdAsync($"{userId}");
                var blockedUser = await _blockedUserManager.FindAsync(author, user);
                // only unblock user when block has been found
                if (blockedUser != null)
                {
                    if (!await _blockedUserManager.RemoveAsync(blockedUser))
                    {
                        return StatusCode(500);
                    }
                }
            }

            var blockUserString = Request.Query["blockUser"].ToString();
            if (Int64.TryParse(blockUserString, out userId))
            {
                var user = await _userManager.FindByIdAsync($"{userId}");
                var blockedUser = await _blockedUserManager.FindAsync(author, user);
                // only block user when user has been found
                // and when no existing block has been found
                if (user != null && blockedUser == null)
                {
                    if (!await _blockedUserManager.AddAsync(author, user))
                    {
                        return StatusCode(500);
                    }
                }
            }

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

            var approveAllCommentsString = Request.Query["approveAllComments"].ToString();
            if (Int64.TryParse(approveAllCommentsString, out Int64 approveAllComments))
            {
                // make sure approveAllComments is 1
                if (approveAllComments == 1)
                {
                    // loop over each unapproved comment and try to approve it
                    var unapprovedComments = await _assetCommentManager.FindAllUnApprovedByAssetAuthorAsync(author);
                    if (unapprovedComments != null)
                    {
                        foreach (var comment in unapprovedComments)
                        {
                            if (!await _assetCommentManager.ApproveAsync(comment))
                            {
                                return StatusCode(500);
                            }
                        }
                    }
                }
            }

            UnapprovedComments = await _assetCommentManager.FindAllUnApprovedByAssetAuthorAsync(author);
            HasUnapprovedComments = UnapprovedComments != null && UnapprovedComments.Length > 0;

            ApprovedComments = await _assetCommentManager.FindAllApprovedByAssetAuthorAsync(author);
            HasApprovedComments = ApprovedComments != null && ApprovedComments.Length > 0;

            BlockedUsers = await _blockedUserManager.FindAllByAuthorAsync(author);
            BlockedUserIds = BlockedUsers.Select(b => b.UserId).ToArray();
            HasBlockedUsers = BlockedUsers != null && BlockedUsers.Length > 0;

            return Page();
        }
    }
}
