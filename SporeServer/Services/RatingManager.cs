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
    public class RatingManager : IRatingManager
    {
        private readonly SporeServerContext _context;
        private readonly IAssetManager _assetManager;
        private readonly ILogger<RatingManager> _logger;

        public RatingManager(SporeServerContext context, IAssetManager assetManager, ILogger<RatingManager> logger)
        {
            _context = context;
            _assetManager = assetManager;
            _logger = logger;
        }

        /// <summary>
        ///     Updates asset rating
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        private async Task<bool> UpdateAssetRatingAsync(SporeServerAsset asset)
        {
            try
            {
                var assetRatings = await FindAllByAssetAsync(asset);

                // only update asset rating,
                // when we have more than 1 rating
                if (assetRatings != null &&
                    assetRatings.Length > 1)
                {
                    // so we calculate the rating like this:
                    // get the percentage of the positive votes,
                    // then make sure the maximum value is 10.0
                    // and the minimum is -10.0
                    int positiveRatingCount = assetRatings.Where(r => r.Rating).Count();
                    float positivePercentage = positiveRatingCount / (float)assetRatings.Length * 100f;
                    asset.Rating = (positivePercentage / 5) - 10;

                    // update asset
                    await _assetManager.UpdateAsync(asset);
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"UpdateAssetRatingAsync: Failed To Update Asset Rating For {asset.AssetId}: {e}");
                return false;
            }
        }

        public async Task<bool> AddAsync(SporeServerUser author, SporeServerAsset asset, bool rating)
        {
            try
            {
                var assetRating = new SporeServerRating()
                {
                    Author = author,
                    Asset = asset,
                    Rating = rating
                };

                await _context.AssetRatings.AddAsync(assetRating);
                await _context.SaveChangesAsync();

                // update asset rating
                await UpdateAssetRatingAsync(asset);

                _logger.LogInformation($"AddAsync: Added Rating {assetRating.RatingId}");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"AddAsync: Failed To Add Rating For {asset.AssetId}: {e}");
                return false;
            }
        }

        public async Task<SporeServerRating> FindAsync(SporeServerUser author, SporeServerAsset asset)
        {
            try
            {
                return await _context.AssetRatings
                                        .Include(r => r.Asset)
                                        .Where(r => 
                                            r.AuthorId == author.Id &&
                                            r.AssetId == asset.AssetId)
                                        .FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"FindAsync: Failed To Find Rating For {asset.AssetId}: {e}");
                return null;
            }
        }

        public async Task<SporeServerRating[]> FindAllByAssetAsync(SporeServerAsset asset)
        {
            try
            {
                return await _context.AssetRatings
                                        .Where(r => r.AssetId == asset.AssetId)
                                        .ToArrayAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"FindAllByAssetAsync: Failed To Find Ratings For {asset.AssetId}: {e}");
                return null;
            }
        }

        public async Task<bool> UpdateAsync(SporeServerRating rating)
        {
            try
            {
                // update rating in database
                _context.AssetRatings.Update(rating);
                await _context.SaveChangesAsync();

                // update asset rating
                await UpdateAssetRatingAsync(rating.Asset);

                _logger.LogInformation($"UpdateAsync: Updated Rating {rating.RatingId}");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"UpdateAsync: Failed To Updated Rating {rating.RatingId}: {e}");
                return false;
            }
        }
    }
}
