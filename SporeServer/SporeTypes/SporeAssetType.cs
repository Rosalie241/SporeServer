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
    public enum SporeAssetType : Int64
    {
        Creature = 0x2b978c46,
        Building = 0x2399be55,
        Vehicle = 0x24682294,
        UFO = 0x476a98c7,
        Adventure = 0x366a930d
    };
}
