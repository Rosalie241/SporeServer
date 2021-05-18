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
        private readonly ILogger<EventManager> _logger;

        public EventManager(IAchievementManager achievementManager, ILogger<EventManager> logger)
        {
            _achievementManager = achievementManager;
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

                        // Unsupported
                        default:
                            _logger.LogWarning($"Invalid Event Verb {eventsEvent.Verb}");
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
