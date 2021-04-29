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
    public class SporeServerContext : IdentityDbContext<SporeServerUser, IdentityRole<Int64>, Int64>
    {
        public SporeServerContext(DbContextOptions<SporeServerContext> options)
            : base(options)
        {
        }

        public DbSet<SporeServerAsset> Assets { get; set; }

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

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
