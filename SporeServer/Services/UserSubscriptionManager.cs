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
    public class UserSubscriptionManager : IUserSubscriptionManager
    {
        private readonly SporeServerContext _context;
        private readonly ILogger<UserSubscriptionManager> _logger;

        public UserSubscriptionManager(SporeServerContext context, ILogger<UserSubscriptionManager> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> AddAsync(SporeServerUser author, SporeServerUser user)
        {
            try
            {
                var subscription = new SporeServerUserSubscription()
                {
                    Author = author,
                    User = user
                };

                // add subscription to database
                await _context.UserSubscriptions.AddAsync(subscription);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"AddAsync: Added Subscription {subscription.SubscriptionId} For User {author.Id}");
                return true;
            }
            catch(Exception e)
            {
                _logger.LogError($"AddAsync: Failed To Add Subscription: {e}");
                return false;
            }
        }

        public async Task<bool> RemoveAsync(SporeServerUserSubscription subscription)
        {
            try
            {
                // remove subscription from database
                _context.UserSubscriptions.Remove(subscription);
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

        public SporeServerUserSubscription Find(SporeServerUser author, SporeServerUser user)
        {
            return _context.UserSubscriptions
                    .Where(s => s.AuthorId == author.Id && 
                                s.UserId == user.Id)
                    .FirstOrDefault();
        }

        public SporeServerUserSubscription[] FindAllByAuthor(SporeServerUser author)
        {
            return _context.UserSubscriptions
                    .Include(s => s.User)
                    .Where(s => s.AuthorId == author.Id)
                    .ToArray();
        }
    }
}
