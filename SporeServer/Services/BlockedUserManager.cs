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
using SporeServer.SporeTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SporeServer.Services
{
    public class BlockedUserManager : IBlockedUserManager
    {
        private readonly SporeServerContext _context;
        private readonly ILogger<AchievementManager> _logger;

        public BlockedUserManager(SporeServerContext context, ILogger<AchievementManager> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> AddAsync(SporeServerUser author, SporeServerUser user)
        {
            try
            {
                var blockedUser = new SporeServerBlockedUser()
                {
                    Author = author,
                    User = user,
                };

                await _context.BlockedUsers.AddAsync(blockedUser);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"AddAsync: Added Block {blockedUser.BlockedUserId}");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"AddAsync: Failed To Add Block For {user.Id} from {author.Id}: {e}");
                return false;
            }
        }

        public async Task<bool> RemoveAsync(SporeServerBlockedUser blockedUser)
        {
            try
            {
                _context.BlockedUsers.Remove(blockedUser);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"RemoveAsync: Removed Block {blockedUser.BlockedUserId}");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"RemoveAsync: Failed To Remove Block {blockedUser.BlockedUserId}: {e}");
                return false;
            }
        }

        public async Task<SporeServerBlockedUser> FindAsync(SporeServerUser author, SporeServerUser user)
        {
            try
            {
                return await _context.BlockedUsers
                                            .Include(c => c.Author)
                                            .Include(c => c.User)
                                            .Where(c => c.Author == author && c.User == user)
                                            .FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"FindAsync: Failed To Find Block For {user.Id} From {author.Id}: {e}");
                return null;
            }
        }

        public async Task<SporeServerBlockedUser[]> FindAllByAuthorAsync(SporeServerUser author)
        {
            try
            {
                return await _context.BlockedUsers
                                            .Include(c => c.Author)
                                            .Include(c => c.User)
                                            .Where(c => c.Author == author)
                                            .ToArrayAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"FindAsync: Failed To Find Blocks From {author.Id}: {e}");
                return null;
            }
        }
    }
}
