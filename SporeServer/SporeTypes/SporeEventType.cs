/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */
using System;

namespace SporeServer.SporeTypes
{
    public enum SporeEventType : Int64
    {
        AchievementUnlocked = 0x1edc82b0,

        // Creature Stage
        CreatureBefriended = 0xcdb3f5d8,
        CreatureExtinction = 0xaba34d01,

        // Adventures
        AdventureWon = 0x2d59837b,
        AdventureLost = 0x706ccb27,
        AdventureCaptainStats = 0xd9d7edf2,
        AdventureCaptainName = 0xa31b4d50,
        AdventureCaptainUnlockedParts = 0x0fdb0e00,
    };
}
