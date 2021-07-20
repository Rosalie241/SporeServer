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
    public class AggregatorSubscriptionManager : IAggregatorSubscriptionManager
    {
        private readonly SporeServerContext _context;
        private readonly ILogger<AggregatorSubscriptionManager> _logger;

        public AggregatorSubscriptionManager(SporeServerContext context, ILogger<AggregatorSubscriptionManager> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> AddAsync(SporeServerUser author, SporeServerAggregator aggregator)
        {
            try
            {
                var subscription = new SporeServerAggregatorSubscription()
                {
                    Author = author,
                    Aggregator = aggregator
                };

                // add subscription to database
                await _context.AggregatorSubscriptions.AddAsync(subscription);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"AddAsync: Added Subscription {subscription.SubscriptionId} For User {author.Id}");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"AddAsync: Failed To Add Subscription: {e}");
                return false;
            }
        }

        public async Task<bool> RemoveAsync(SporeServerAggregatorSubscription subscription)
        {
            try
            {
                // remove subscription from database
                _context.AggregatorSubscriptions.Remove(subscription);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"RemoveAsync: Removed Subscription {subscription.SubscriptionId}");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"RemoveAsync: Failed To Remove Subscription: {e}");
                return false;
            }
        }

        public async Task<SporeServerAggregatorSubscription> FindAsync(SporeServerUser author, SporeServerAggregator aggregator)
        {
            try
            {
                return await _context.AggregatorSubscriptions
                                        .Where(s => s.AuthorId == author.Id &&
                                                    s.AggregatorId == aggregator.AggregatorId)
                                        .FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"FindAsync: Failed To Find Subscription: {e}");
                return null;
            }
        }

        public async Task<SporeServerAggregatorSubscription[]> FindAllByAuthorAsync(SporeServerUser author)
        {
            try
            {
                return await _context.AggregatorSubscriptions
                                        .Include(s => s.Aggregator)
                                        .Include(s => s.Aggregator.Author)
                                        .Where(s => s.AuthorId == author.Id)
                                        .ToArrayAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"FindAllByAuthorAsync: Failed To Find By Author {author.Id}: {e}");
                return null;
            }
        }

        public async Task<Int32> GetSubscriberCountAsync(SporeServerAggregator aggregator)
        {
            try
            {
                return await _context.AggregatorSubscriptions
                                        .Where(a => a.AggregatorId == aggregator.AggregatorId)
                                        .CountAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"GetSubscriberCount: Failed To Get Subscriber Count For {aggregator.AggregatorId}: {e}");
                return 0;
            }
        }
    }
}
