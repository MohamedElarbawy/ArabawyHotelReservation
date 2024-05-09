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
    public class MealPlanSesonConfig : IEntityTypeConfiguration<MealPlanSeason>
    {
        public void Configure(EntityTypeBuilder<MealPlanSeason> builder)
        {
            builder.Property(p => p.MealPlanSeasonId).IsRequired().UseIdentityColumn();
            builder.Property(p => p.SeasonStart).HasColumnType("datetime");
            builder.Property(p => p.SeasonEnd).HasColumnType("datetime");
            builder.Property(p => p.RatePerAdult).IsRequired().HasColumnType("MONEY");
            builder.Property(p => p.RatePerChild).IsRequired().HasColumnType("MONEY");
        }
    }
}
