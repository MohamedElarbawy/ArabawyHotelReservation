using Domain_Layer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure_Layer.Configurations
{
    public class RoomConfig : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.Property(p => p.RoomId).IsRequired().UseIdentityColumn();
            builder.Property(p => p.RoomNo).IsRequired();
            builder.Property(p => p.AdultsCapcity).IsRequired();
            builder.Property(p => p.ChildrenCapcity).IsRequired();

            builder.HasOne(p => p.RoomType).WithMany().HasForeignKey(p => p.RoomTypeId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
