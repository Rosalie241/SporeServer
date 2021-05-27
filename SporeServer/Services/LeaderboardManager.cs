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
using SporeServer.Models.Xml;
using SporeServer.SporeTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SporeServer.Services
{
    public class LeaderboardManager : ILeaderboardManager
    {
        private readonly SporeServerContext _context;
        private readonly IAchievementManager _achievementManager;
        private readonly ILogger<LeaderboardManager> _logger;

        public LeaderboardManager(SporeServerContext context, IAchievementManager achievementManager, ILogger<LeaderboardManager> logger)
        {
            _context = context;
            _achievementManager = achievementManager;
            _logger = logger;
        }

        public async Task<bool> AddAsync(SporeServerAsset adventureAsset, SporeServerAsset captainAsset,
                                    Int32 percentageCompleted, Int32 timeInMilliseconds,
                                    SporeServerUser author)
        {
            try
            {
                var entry = new SporeServerLeaderboardEntry()
                {
                    Asset = adventureAsset,
                    Author = author,
                    Timestamp = DateTime.Now,
                    PercentageCompleted = percentageCompleted,
                    TimeInMilliseconds = timeInMilliseconds,
                    Captain = captainAsset
                };

                await _context.LeaderboardEntries.AddAsync(entry);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"AddAsync: Added Leaderboard Entry {entry.EntryId} For {entry.AuthorId}");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"AddAsync: Failed To Add Leaderboard Entry: {e}");
                return false;
            }
        }

        public DbSet<SporeServerLeaderboardEntry> GetAllEntries()
        {
            return _context.LeaderboardEntries;
        }
    }
}
