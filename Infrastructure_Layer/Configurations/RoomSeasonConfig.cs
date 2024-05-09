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
    public class RoomSeasonConfig : IEntityTypeConfiguration<RoomSeason>
    {
        public void Configure(EntityTypeBuilder<RoomSeason> builder)
        {
            builder.Property(p => p.RoomSeasonId).IsRequired().UseIdentityColumn();
            builder.Property(p=>p.SeasonStart).HasColumnType("datetime");
            builder.Property(p=>p.SeasonEnd).HasColumnType("datetime");
            builder.Property(p=>p.RatePerRoom).IsRequired().HasColumnType("MONEY");
            
        }
    }
}
