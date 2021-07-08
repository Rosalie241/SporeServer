/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SporeServer.Areas.Identity.Data;
using SporeServer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SporeServer.Services
{
    public class AssetCommentManager : IAssetCommentManager
    {
        private readonly SporeServerContext _context;
        private readonly ILogger<AssetCommentManager> _logger;
        public AssetCommentManager(SporeServerContext context, ILogger<AssetCommentManager> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> AddAsync(SporeServerAsset asset, SporeServerUser author, string comment)
        {
            try
            {
                var assetComment = new SporeServerAssetComment()
                {
                    Author = author,
                    Asset = asset,
                    // comments on your own creations are automatically approved
                    Approved = (author.Id == asset.AuthorId),
                    Timestamp = DateTime.Now,
                    Comment = comment
                };

                await _context.AssetComments.AddAsync(assetComment);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"AddAsync: Added Comment {assetComment.CommentId}");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"AddAsync: Failed To Add Comment For {asset.AssetId}: {e}");
                return false;
            }
        }

        public async Task<SporeServerAssetComment[]> FindAllApprovedByAsset(SporeServerAsset asset)
        {
            try
            {
                return await _context.AssetComments
                                .Include(c => c.Author)
                                .Where(c => 
                                    c.AssetId == asset.AssetId && 
                                    c.Approved)
                                .OrderByDescending(c => c.Timestamp)
                                .Take(10)
                                .ToArrayAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"FindAllApprovedByAsset: Failed To Find Approved Comments For {asset.AssetId}: {e}");
                return null;
            }
        }

        public async Task<SporeServerAssetComment[]> FindAllByAssetAuthor(SporeServerUser author)
        {
            try
            {
                return await _context.AssetComments
                                .Include(c => c.Asset)
                                .Where(c =>
                                    c.Asset.AuthorId == author.Id
                                )
                                .OrderByDescending(c => c.Timestamp)
                                .Take(10)
                                .ToArrayAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"FindAllUnApprovedByAuthor: Failed To Find Unapproved Comments For {author.Id}: {e}");
                return null;
            }
        }
    }
}
