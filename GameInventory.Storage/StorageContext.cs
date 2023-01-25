using GameInventory.Storage.Entities;
using GameInventory.Storage.Entities.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameInventory.Storage
{
    public class StorageContext : IdentityDbContext<IdentityUser>
    {
        public StorageContext(DbContextOptions<StorageContext> options) : base(options) { }

        public DbSet<Item> Items { get; set; }
        public DbSet<UserItem> UserItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ItemConfiguration());
            builder.ApplyConfiguration(new UserItemConfiguration());
        }
    }
}
