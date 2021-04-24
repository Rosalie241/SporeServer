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

            // make sure we don't start at 1 for asset ids
            builder.Entity<SporeServerAsset>()
                .Property(a => a.AssetId)
                .UseIdentityColumn(seed: 600000000);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public async void ReserveNewAsset(SporeServerUser user)
        {
            
        }
    }
}
