using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameInventory.Storage.Entities.Configuration
{
    public class UserItemConfiguration : IEntityTypeConfiguration<UserItem>
    {
        public void Configure(EntityTypeBuilder<UserItem> builder)
        {
            builder.HasKey(x => x.Id);            
        }
    }
}
