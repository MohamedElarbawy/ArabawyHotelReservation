using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Entities
{
    public class Reservation
    {
        public Reservation()
        {
            this.ReservationGuid = Guid.NewGuid().ToString();
            this.RoomList = new();
        }
        public int ReservationId { get; set; }
        public long ReservationNo { get; set; }
        public string ReservationGuid { get; init; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int NoOfAdults { get; set; }
        public int NoOfChildren { get; set; }
        public int NoOfRooms { get; set; }
        public string? GuestName { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsCanceled { get; set; }


        public int RoomId { get; set; }
        public List<Room> RoomList { get; set; }
        public int MealPlanId { get; set; }
        public MealPlan MealPlan { get; set; }



    }
}
