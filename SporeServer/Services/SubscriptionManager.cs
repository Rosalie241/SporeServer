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
    public class SubscriptionManager : ISubscriptionManager
    {
        private readonly SporeServerContext _context;
        private readonly ILogger<SubscriptionManager> _logger;

        public SubscriptionManager(SporeServerContext context, ILogger<SubscriptionManager> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> AddAsync(SporeServerUser author, SporeServerUser user)
        {
            try
            {
                var subscription = new SporeServerSubscription()
                {
                    Author = author,
                    User = user
                };

                // add subscription to database
                await _context.Subscriptions.AddAsync(subscription);
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

        public async Task<bool> RemoveAsync(SporeServerSubscription subscription)
        {
            try
            {
                // remove subscription from database
                _context.Subscriptions.Remove(subscription);
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

        public SporeServerSubscription Find(SporeServerUser author, SporeServerUser user)
        {
            return _context.Subscriptions
                    .Where(s => s.AuthorId == author.Id && 
                                s.UserId == user.Id)
                    .FirstOrDefault();
        }

        public SporeServerSubscription[] FindAllByAuthor(SporeServerUser author)
        {
            return _context.Subscriptions
                    .Include(s => s.User)
                    .Where(s => s.AuthorId == author.Id)
                    .ToArray();
        }
    }
}
