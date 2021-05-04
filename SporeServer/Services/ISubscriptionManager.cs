using SporeServer.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SporeServer.Services
{
    public interface ISubscriptionManager
    {
        /// <summary>
        ///     Adds a new subscription for author, who follows the given user
        /// </summary>
        /// <param name="author"></param>
        /// <param name="follow"></param>
        /// <returns></returns>
        Task<bool> AddAsync(SporeServerUser author, SporeServerUser user);

        /// <summary>
        ///     Removes the given subscription
        /// </summary>
        /// <param name="author"></param>
        /// <param name="follow"></param>
        /// <returns></returns>
        Task<bool> RemoveAsync(SporeServerSubscription subscription);

        /// <summary>
        ///     Finds subscription with user for author, returns null when not found
        /// </summary>
        /// <param name="author"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        SporeServerSubscription Find(SporeServerUser author, SporeServerUser user);

        /// <summary>
        ///     Finds all subscriptions for author
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        SporeServerSubscription[] FindAllByAuthor(SporeServerUser author);
    }
}
