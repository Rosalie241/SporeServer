/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */
using Microsoft.Extensions.Logging;
using SporeServer.Areas.Identity.Data;
using SporeServer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SporeServer.Services
{
    public class AssetCommentReportManager : IAssetCommentReportManager
    {
        private readonly ILogger<AssetCommentReportManager> _logger;
        private readonly SporeServerContext _context;

        public AssetCommentReportManager(SporeServerContext context, ILogger<AssetCommentReportManager> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> ReportAsync(SporeServerAssetComment comment, SporeServerUser author)
        {
            try
            {
                var assetCommentReport = new SporeServerAssetCommentReport()
                {
                    Author = author,
                    Timestamp = DateTime.Now,
                    AssetComment = comment
                };

                //await _context.AssetCommentReports.AddAsync(assetCommentReport);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"ReportAsync: Added Report {assetCommentReport.AssetCommentId} For {comment.CommentId} From {author.Id}");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"ReportAsync: Failed To Add Report For {comment.CommentId}: {e}");
                return false;
            }
        }
    }
}
