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
    public enum SporeModelType : Int64
    {
        // Creatures
        Creature = 0x9ea3031a,
        TribeCreature = 0x372e2c04,
        CivCreature = 0xccc35c46,
        SpaceCreature = 0x65672ade,
        AdventureCreature = 0x4178b8e8,

        // Buildings
        BuildingCityHall = 0x99e92f05,
        BuildingHouse = 0x4e3f7777,
        BuildingIndustry = 0x47c10953,
        BuildingEntertainment = 0x72c49181,

        // Military Vehicles
        VehicleMilitaryLand = 0x7d433fad,
        VehicleMilitaryWater = 0x8f963dcb,
        VehicleMilitaryAir = 0x441cd3e6,

        // Economic Vehicles
        VehicleEconomicLand = 0xf670aa43,
        VehicleEconomicWater = 0x2a5147a9,
        VehicleEconomicAir = 0x1a4e0708,

        // Cultural Vehicles
        VehicleCulturalLand = 0x9ad7d4aa,
        VehicleCulturalWater = 0x1f2a25b6,
        VehicleCulturalAir = 0x449c040f,

        // Colony Vehicles
        VehicleColonyLand = 0xbc1041e6,
        VehicleColonyWater = 0xc15695da,
        VehicleColonyAir = 0x2090a11b,

        // Ufo Vehicle
        VehicleUfo = 0x98e03c0d,

        // Adventures
        AdventureUnset = 0x20790816,
        AdventureTemplate = 0x27818fe6,
        AdventureAttack = 0x287adcdc,
        AdventureDefend = 0xc34c5e14,
        AdventureSocialize = 0xfb734cd1,
        AdventureExplore = 0x37fd4e0d,
        AdventureQuest = 0xc422519e,
        AdventureStory = 0xb4707f8f,
        AdventureCollect = 0x25a6ea6e,
        AdventurePuzzle = 0xe27ddad4
    }
}
