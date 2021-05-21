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
    public enum SporeAssetTraitType : Int64
    {
        // CellGame
        CellGameCarnivore = 0xcfb01b93,
        CallGameOmnivore = 0x5ece4770,
        CellGameHerbivore = 0xa8ec6f99,

        // CreatureGame
        CreatureGameAttack = 0x17e5ef84,
        CreatureGameMixed = 0xb9de15f2,
        CreatureGameSocial = 0xfd159d91,

        // TribeGame
        TribeGameAttack = 0x0cac124d,
        TribeGameMixed = 0xc2ca9495,
        TribeGameSocial = 0x60a78928,

        // CivGame
        CivGameMilitary = 0x0a35d0f5,
        CivGameEconomic = 0x3360727f,
        CivGameReligious = 0x6aeb96a1,

        // SpaceGame
        SpaceGameBard = 0x50fee6bf,
        SpaceGameDiplomat = 0xb436c0bc,
        SpaceGameEcologist = 0xb40d2187,
        SpaceGameKnight = 0xf966d429,
        SpaceGameScientist = 0x9ce4639c,
        SpaceGameShaman = 0xcf6c2ab4,
        SpaceGameTrader = 0xb7cb9f7a,
        SpaceGameWanderer = 0xa5656e84,
        SpaceGameWarrior = 0xd9c806e4,
        SpaceGameZealot = 0x2b35f523
    }
}
