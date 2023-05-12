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
    public struct SporeBlock
    {
        public SporeBlock() { }

        /// <summary>
        ///     Id of given block
        /// </summary>
        public Int64 BlockId = 0;
        /// <summary>
        ///     Directory of the block
        /// </summary>
        public SporeBlockType BlockType = SporeBlockType.None;

        /// <summary>
        ///     Price
        /// </summary>
        public int Price = 0;
        /// <summary>
        ///     Complexity score
        /// </summary>
        public int ComplexityScore = 0;

        /// <summary>
        ///     Whether block has a mouth (>0 = true)
        /// </summary>
        public int CapabilityMouth = 0;
        /// <summary>
        ///     Whether block is carnivorous (>0 = true)
        /// </summary>
        public int CapabilityCarnivorous = 0;
        /// <summary>
        ///     Whether block is herbivorous (>0 = true)
        /// </summary>
        public int CapabilityHerbivorous = 0;
        /// <summary>
        ///     Whether block is a grasper (>0 = true)
        /// </summary>
        public int CapabilityGrasper = 0;
        /// <summary>
        ///     Whether block is a foot (>0 = true)
        /// </summary>
        public int CapabilityFoot = 0;
        /// <summary>
        ///     Whether block is a spine (>0 = true)
        /// </summary>
        public int CapabilitySpine = 0;

        /// <summary>
        ///     Health level
        /// </summary>
        public int CapabilityHealth = 0;
        /// <summary>
        ///     Speed level
        /// </summary>
        public int CapabilityCreatureSpeed = 0;
        /// <summary>
        ///     Jump level
        /// </summary>
        public int CapabilityJump = 0;

        /// <summary>
        ///     Sing level
        /// </summary>
        public int CapabilityVocalize = 0;
        /// <summary>
        ///     Charm level
        /// </summary>
        public int CapabilityFlaunt = 0;
        /// <summary>
        ///     Pose level
        /// </summary>
        public int CapabilityPosture = 0;
        /// <summary>
        ///     Dance level
        /// </summary>
        public int CapabilityDance = 0;
        /// <summary>
        ///     Bite level
        /// </summary>
        public int CapabilityBite = 0;

        /// <summary>
        ///     Charge level
        /// </summary>
        public int CapabilityCharge = 0;
        /// <summary>
        ///     Strike level
        /// </summary>
        public int CapabilityStrike = 0;
        /// <summary>
        ///     Spit level
        /// </summary>
        public int CapabilitySpit = 0;
    }
}
