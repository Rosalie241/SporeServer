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
using SporeServer.Models.Xml;
using SporeServer.SporeTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SporeServer.Services
{
    public class EventManager : IEventManager
    {
        private readonly IAchievementManager _achievementManager;
        private readonly ILeaderboardManager _leaderboardManager;
        private readonly IAssetManager _assetManager;
        private readonly ILogger<EventManager> _logger;

        public EventManager(IAchievementManager achievementManager, ILeaderboardManager leaderboardManager, IAssetManager assetManager, ILogger<EventManager> logger)
        {
            _achievementManager = achievementManager;
            _leaderboardManager = leaderboardManager;
            _assetManager = assetManager;
            _logger = logger;
        }

        public async Task<bool> AddAsync(Stream eventStream, SporeServerUser author)
        {
            try
            {
                var events = await EventsModel.SerializeAsync(eventStream);

                foreach (var eventsEvent in events.Events)
                {
                    switch (eventsEvent.Verb)
                    {
                        // Unlocked Achievement
                        case (Int64)SporeEventType.AchievementUnlocked:
                            await _achievementManager.UnlockAsync(eventsEvent.Args[0], author);
                            break;

                        // Befriended Creature
                        case (Int64)SporeEventType.CreatureBefriended:
                            break;

                        // Extincted Creature
                        case (Int64)SporeEventType.CreatureExtinction:
                            break;

                        // Adventure Won
                        case (Int64)SporeEventType.AdventureWon:
                            {
                                var adventureAssetId = eventsEvent.AssetId;
                                var percentageCompleted = (Int32)eventsEvent.Args[0];
                                var timeInMs = (Int32)eventsEvent.Args[1];
                                // Args[2] contains the amount of captain points earned
                                // maybe a thing to track in the future?
                                // seems to be unrequired for anything ingame though
                                var captainAssetId = eventsEvent.Args[3];

                                var adventureAsset = await _assetManager.FindByIdAsync(adventureAssetId);
                                var captainAsset = await _assetManager.FindByIdAsync(captainAssetId);
                                // make sure we can find the needed assets
                                if (adventureAsset == null || captainAsset == null)
                                {
                                    break;
                                }

                                await _leaderboardManager.AddAsync(adventureAsset, captainAsset, percentageCompleted, timeInMs, author);
                            }
                            break;

                        // Adventure Lost
                        case (Int64)SporeEventType.AdventureLost:
                            break;

                        // Adventure Captain Stats
                        case (Int64)SporeEventType.AdventureCaptainStats:
                            break;

                        // Adventure Captain Name
                        case (Int64)SporeEventType.AdventureCaptainName:
                            break;

                        // Adventure Captain Unlocked Parts
                        case (Int64)SporeEventType.AdventureCaptainUnlockedParts:
                            break;

                        // Unsupported
                        default:
                            _logger.LogWarning($"Invalid Event Verb 0x{eventsEvent.Verb:x}");
                            break;
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed To Add Event For {author.Id}: {e}");
                return false;
            }
        }
    }
}
