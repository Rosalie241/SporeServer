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
    public class AggregatorManager : IAggregatorManager
    {
        private readonly SporeServerContext _context;
        private readonly ILogger<AggregatorManager> _logger;

        public AggregatorManager(SporeServerContext context, ILogger<AggregatorManager> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<SporeServerAggregator> AddAsync(SporeServerUser author, string name, string description, SporeServerAsset[] assets)
        {
            try
            {
                var aggregator = new SporeServerAggregator()
                {
                    Name = name,
                    Description = description,
                    Timestamp = DateTime.Now,
                    Author = author,
                    Assets = assets
                };

                // add aggregator to database
                await _context.Aggregators.AddAsync(aggregator);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"AddAsync: Added Aggregator {aggregator.AggregatorId} For User {aggregator.Author.Id}");
                return aggregator;
            }
            catch(Exception e)
            {
                _logger.LogError($"AddAsync: Failed To Add Aggregator: {e}");
                return null;
            }
        }

        public async Task<bool> RemoveAsync(SporeServerAggregator aggregator)
        {
            try
            {
                // remove aggregator from database
                _context.Aggregators.Remove(aggregator);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"RemoveAsync: Removed Aggregator {aggregator.AggregatorId}");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"RemoveAsync: Failed To Remove Aggregator: {e}");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(SporeServerAggregator aggregator)
        {
            try
            {
                // update aggregator from database
                _context.Aggregators.Update(aggregator);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"UpdateAsync: Updated Aggregator {aggregator.AggregatorId}");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"UpdateAsync: Failed To Update Aggregator {aggregator.AggregatorId}: {e}");
                return false;
            }
        }

        public async Task<SporeServerAggregator> FindByIdAsync(Int64 id)
        {
            try
            {
                return await _context.Aggregators
                                        .Include(a => a.Assets)
                                        .ThenInclude(a => a.Author)
                                        .Include(a => a.Author)
                                        .Where(a => a.AggregatorId == id)
                                        .FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"FindByIdAsync: Failed To Find Aggregator {id}: {e}");
                return null;
            }
        }

        public async Task<SporeServerAggregator[]> FindByAuthorAsync(SporeServerUser author)
        {
            try
            {
                return await _context.Aggregators
                                        .Include(a => a.Assets)
                                        .Include(a => a.Author)
                                        .Where(a => a.AuthorId == author.Id)
                                        .ToArrayAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"FindByAuthorAsync: Failed To Find Aggregators For Author {author.Id}: {e}");
                return null;
            }
        }

        public async Task<Int32> GetCountByAuthorAsync(SporeServerUser author)
        {
            try
            {
                return await _context.Aggregators
                                        .Where(a => a.AuthorId == author.Id)
                                        .CountAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"GetCountByAuthorAsync: Failed To Get Count By Author {author.Id}: {e}");
                return 0;
            }
        }

        public DbSet<SporeServerAggregator> GetAllAggregators()
        {
            return _context.Aggregators;
        }
    }
}
