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
    public class RoomTypeConfig : IEntityTypeConfiguration<RoomType>
    {
        public void Configure(EntityTypeBuilder<RoomType> builder)
        {
            builder.Property(p => p.RoomTypeId).IsRequired().UseIdentityColumn();
            builder.Property(p=>p.RoomTypeName).IsRequired().HasMaxLength(200);

            builder.HasMany(p => p.RoomSeasonList).WithOne().HasForeignKey(x => x.RoomTypeId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
