using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.DtoS
{
    public class ReservationCreateDto
    {
        [Required]
        public int RoomTypeId { get; set; }
        [Required]
        public int MealPlanId { get; set; }
        [Required]
        public int NoOfAdults { get; set; }
        [Required]
        public int NoOfChildren { get; set; }
        [Required]
        public DateTime CheckIn { get; set; }
        [Required]
        public DateTime CheckOut { get; set; }
        [Required]
        public int NoOfRooms { get; set; }
        public string? GuestName { get; set; }
        public string? PhoneNumber { get; set; }

    }
}
