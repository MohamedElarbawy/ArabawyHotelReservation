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
    public class ReservationConfig : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.Property(p => p.ReservationId).IsRequired().UseIdentityColumn();
            builder.Property(p=>p.GuestName).HasMaxLength(200);
            builder.Property(p=>p.ReservationGuid).HasMaxLength(100);
            builder.Property(p=>p.PhoneNumber).HasMaxLength(100);
            builder.Property(p=>p.CheckIn).HasColumnType("datetime");
            builder.Property(p=>p.CheckOut).HasColumnType("datetime");
            
            builder.HasMany(p=>p.RoomList).WithMany();
            builder.HasOne(p=>p.MealPlan).WithMany().HasForeignKey(p=>p.MealPlanId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
