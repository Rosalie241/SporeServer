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
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SporeServer.Areas.Identity.Data;

namespace SporeServer.Data
{
    public class SporeServerContext : IdentityDbContext<SporeServerUser, SporeServerRole, Int64>
    {
        public SporeServerContext(DbContextOptions<SporeServerContext> options)
            : base(options)
        {
        }

        /// <summary>
        ///     Assets
        /// </summary>
        public DbSet<SporeServerAsset> Assets { get; set; }
        /// <summary>
        ///     Asset Comments
        /// </summary>
        public DbSet<SporeServerAssetComment> AssetComments { get; set; }
        /// <summary>
        ///     Aggregators/Sporecasts
        /// </summary>
        public DbSet<SporeServerAggregator> Aggregators { get; set; }
        /// <summary>
        ///     UserSubscriptions/Buddies
        /// </summary>
        public DbSet<SporeServerUserSubscription> UserSubscriptions { get; set; }
        /// <summary>
        ///     AggregatorSubscriptions/Sporecast subscription
        /// </summary>
        public DbSet<SporeServerAggregatorSubscription> AggregatorSubscriptions { get; set; }
        /// <summary>
        ///     Unlocked Achievements
        /// </summary>
        public DbSet<SporeServerUnlockedAchievement> UnlockedAchievements { get; set; }
        /// <summary>
        ///     Leaderboard Entries
        /// </summary>
        public DbSet<SporeServerLeaderboardEntry> LeaderboardEntries { get; set; }
        /// <summary>
        ///     Asset Ratings
        /// </summary>
        public DbSet<SporeServerRating> AssetRatings { get; set; }
        /// <summary>
        ///     Blocked Users
        /// </summary>
        public DbSet<SporeServerBlockedUser> BlockedUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // specify property type
            builder.Entity<SporeServerAsset>()
                .Property(a => a.AssetId)
                .ValueGeneratedOnAdd();

            // create dummy user for dummy asset
            builder.Entity<SporeServerUser>()
                .HasData(new SporeServerUser()
                {
                    Id = 1
                });

            // create dummy asset with starting id
            builder.Entity<SporeServerAsset>()
                .HasData(new SporeServerAsset()
                {
                    AssetId = 600000000000,
                    Used = false,
                    AuthorId = 1
                });

            // configure many to many relationship
            // for aggregators
            builder.Entity<SporeServerAggregator>()
                .HasMany(a => a.Assets)
                .WithMany(a => a.Aggregators);

            // configure one to many relationship
            // for asset tags
            builder.Entity<SporeServerAsset>()
                .HasMany(a => a.Tags)
                .WithOne(t => t.Asset);

            // configure one to many relationship
            // for asset traits
            builder.Entity<SporeServerAsset>()
                .HasMany(a => a.Traits)
                .WithOne(t => t.Asset);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
