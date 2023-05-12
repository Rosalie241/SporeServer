/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */
namespace SporeServer.SporeTypes
{
    public struct SporeArcheTypeDefinition
    {
        /// <summary>
        ///     Archetype for definition
        /// </summary>
        public SporeArcheType ArcheType { get; set; }

        /// <summary>
        ///     Allowed model types
        /// </summary>
        public SporeModelType[] ModelTypes { get; set; }
        /// <summary>
        ///     Allowed mouth types
        /// </summary>
        public SporeMouthType[] MouthTypes;

        /// <summary>
        ///     Minimum cost (-1 = any)
        /// </summary>
        public int MinCost { get; set; }
        /// <summary>
        ///     Maximum cost (-1 = any)
        /// </summary>
        public int MaxCost { get; set; }

        /// <summary>
        ///     Minimum amount of graspers (-1 = any)
        /// </summary>
        public int MinGraspers { get; set; }
        /// <summary>
        ///     Maximum amount of graspers (-1 = any)
        /// </summary>
        public int MaxGraspers { get; set; }

        /// <summary>
        ///     Minimum amount of feet (-1 = any)
        /// </summary>
        public int MinFeet { get; set; }
        /// <summary>
        ///     Maximum amount of feet (-1 = any)
        /// </summary>
        public int MaxFeet { get; set; }

        /// <summary>
        ///     Minimum sing level (-1 = any)
        /// </summary>
        public int MinSingLevel { get; set; }
        /// <summary>
        ///     Maximum sing level
        /// </summary>
        public int MaxSingLevel { get; set; }

        /// <summary>
        ///     Minimum dance level (-1 = any)
        /// </summary>
        public int MinDanceLevel { get; set; }
        /// <summary>
        ///     Maximum dance level (-1 = any)
        /// </summary>
        public int MaxDanceLevel { get; set; }

        /// <summary>
        ///     Minimum charm level (-1 = any)
        /// </summary>
        public int MinCharmLevel { get; set; }
        /// <summary>
        ///     Maximum charm level (-1 = any)
        /// </summary>
        public int MaxCharmLevel { get; set; }

        /// <summary>
        ///     Minimum pose level (-1 = any)
        /// </summary>
        public int MinPoseLevel { get; set; }
        /// <summary>
        ///     Maximum pose level (-1 = any)
        /// </summary>
        public int MaxPoseLevel { get; set; }

        /// <summary>
        ///     Minimum total social level (-1 = any)
        /// </summary>
        public int MinTotalSocialLevel { get; set; }
        /// <summary>
        ///     Maximum total social level (-1 = any)
        /// </summary>
        public int MaxTotalSocialLevel { get; set; }

        /// <summary>
        ///     Minimum bite level (-1 = any)
        /// </summary>
        public int MinBiteLevel { get; set; }
        /// <summary>
        ///     Maximum bite level (-1 = any)
        /// </summary>
        public int MaxBiteLevel { get; set; }

        /// <summary>
        ///     Minimum strike level (-1 = any)
        /// </summary>
        public int MinStrikeLevel { get; set; }
        /// <summary>
        ///     Maximum strike level (-1 = any)
        /// </summary>
        public int MaxStrikeLevel { get; set; }

        /// <summary>
        ///     Minimum charge level (-1 = any)
        /// </summary>
        public int MinChargeLevel { get; set; }
        /// <summary>
        ///     Maximum charge level (-1 = any)
        /// </summary>
        public int MaxChargeLevel { get; set; }

        /// <summary>
        ///     Minimum spit level (-1 = any)
        /// </summary>
        public int MinSpitLevel { get; set; }
        /// <summary>
        ///     Maximum spit level (-1 = any)
        /// </summary>
        public int MaxSpitLevel { get; set; }

        /// <summary>
        ///     Minimum total attack level (-1 = any)
        /// </summary>
        public int MinTotalAttackLevel { get; set; }
        /// <summary>
        ///     Maximum total attack level (-1 = any)
        /// </summary>
        public int MaxTotalAttackLevel { get; set; }

        /// <summary>
        ///     Minimum health level (-1 = any)
        /// </summary>
        public int MinHealthLevel { get; set; }
        /// <summary>
        ///     Maximum health level (-1 = any)
        /// </summary>
        public int MaxHealthLevel { get; set; }
    }
}
