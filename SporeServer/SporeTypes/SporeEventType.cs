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
        // Global
        AchievementUnlocked = 0x1edc82b0,

        // Cell Stage (CLG)
        CellKills = 0x3f2955cd,
        CellDeaths = 0xf43a2c4b,
        CellTime = 0x20f60c49,

        // Creature Stage (CRG)
        CreatureBefriended = 0xcdb3f5d8,
        CreatureExtinction = 0xaba34d01,
        CreaturePosse = 0x5e4e0e1e,

        // Tribe Stage (TRG)
        TribeEpicFound = 0x4f84bd4,
        TribeEpicKilled = 0x6e77f36f,
        TribeDomesticated = 0x3110f7cf,
        TribeSuccess = 0x8fd1273c,

        // Civilization Stage (CVG)
        CivEpicCharmed = 0x280fca87,
        CivCaptured = 0xcd416e62,
        CivSuperweapon = 0xf11610bd,
        CivSuccess = 0x3d1afc95,

        // Space Stage (SPG)
        SpaceAllied = 0x5ba499af,
        SpaceWar = 0xc09edaac,
        SpacePosse = 0x27ef0ff0,
        SpaceEpicized = 0xef7163ab,
        SpaceEradicated = 0xbe96cd0,

        // Adventures, Captains
        AdventureWon = 0x2d59837b,
        AdventureLost = 0x706ccb27,
        AdventureCaptainStats = 0xd9d7edf2,
        AdventureCaptainName = 0xa31b4d50,
        AdventureCaptainUnlockedParts = 0xfdb0e00,
    };
}
