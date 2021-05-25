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
    public class AchievementManager : IAchievementManager
    {
        private readonly SporeServerContext _context;
        private readonly ILogger<AchievementManager> _logger;

        public AchievementManager(SporeServerContext context, ILogger<AchievementManager> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> UnlockAsync(Int64 achievementId, SporeServerUser author)
        {
            try
            {
                // make sure the achievement id is valid
                if (!SporeAchievements.Achievements.Select(a => a.Id).Contains(achievementId))
                {
                    throw new Exception($"Achivement Id {achievementId} is invalid!");
                }

                // make sure achievement isn't already unlocked
                var achievements = await FindAllByAuthorAsync(author);
                if (achievements.Contains(achievementId))
                {
                    return true;
                }

                var achievement = new SporeServerUnlockedAchievement()
                {
                    Author = author,
                    Timestamp = DateTime.Now,
                    AchievementId = achievementId
                };

                // add to the database
                await _context.UnlockedAchievements.AddAsync(achievement);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Unlocked Achievement {achievementId} For User {author.Id}");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed To Unlock Achivement For {author.Id}: {e}");
                return false;
            }
        }

        public async Task<long[]> FindAllByAuthorAsync(SporeServerUser author)
        {
            try
            {
                return await _context.UnlockedAchievements
                                        .Where(u => u.AuthorId == author.Id)
                                        .Select(u => u.AchievementId)
                                        .ToArrayAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed To Find All Unlocked Achievements For {author.Id}: {e}");
                return null;
            }
        }
    }
}
