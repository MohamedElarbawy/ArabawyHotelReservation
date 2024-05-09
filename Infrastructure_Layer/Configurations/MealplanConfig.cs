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
    public class MealplanConfig : IEntityTypeConfiguration<MealPlan>
    {
        public void Configure(EntityTypeBuilder<MealPlan> builder)
        {
            builder.Property(p => p.MealPlanId).IsRequired().UseIdentityColumn();
            builder.Property(p => p.MealPlanName).IsRequired().HasMaxLength(200);
            builder.HasMany(p => p.MealplanSeasonList).WithOne().HasForeignKey(x => x.MealPlanId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
