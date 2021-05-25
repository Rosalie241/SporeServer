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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SporeServer.Areas.Identity.Data
{
    public class SporeServerUserSubscription
    {
        /// <summary>
        ///     User Subcription Id
        /// </summary>
        [Key]
        public Int64 SubscriptionId { get; set; }

        /// <summary>
        ///     Author Id of user subscription
        /// </summary>
        public Int64 AuthorId { get; set; }
        /// <summary>
        ///     Author of user subscription
        /// </summary>
        public SporeServerUser Author { get; set; }

        /// <summary>
        ///     User Id
        /// </summary>
        public Int64 UserId { get; set; }

        /// <summary>
        ///     User who Author is subscribed to
        /// </summary>
        public SporeServerUser User { get; set; }
    }
}
