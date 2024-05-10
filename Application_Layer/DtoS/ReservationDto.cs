using Domain_Layer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.DtoS
{
    public class ReservationDto
    {
        public int ReservationId { get; set; }
        public string ReservationNo { get; set; }
        public string ReservationGuid { get; set; }
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
        public string NoOfAdults { get; set; }
        public string NoOfChildren { get; set; }
        public string NoOfRooms { get; set; }
        public string RoomCost { get; set; }
        public string MealPlanCost { get; set; }
        public string TotalCost { get; set; }

        public string RoomNumbers { get; set; }
        public string MealPlan {  get; set; }

        public static ReservationDto MapFrom(Reservation reservation)
        {
            var reservationDto = new ReservationDto
            {
                ReservationId = reservation.ReservationId,
                ReservationNo = reservation.ReservationNo.ToString(),
                ReservationGuid = reservation.ReservationGuid,
                CheckIn = reservation.CheckIn.ToString("dd/MM/yyyy"),
                CheckOut = reservation.CheckOut.ToString("dd/MM/yyyy"),
                NoOfAdults = reservation.NoOfAdults.ToString(),
                NoOfChildren = reservation.NoOfChildren.ToString(),
                NoOfRooms = reservation.NoOfRooms.ToString(),
                MealPlanCost = reservation.MealPlanCost.ToString(),
                TotalCost = reservation.TotalCost.ToString(),
                RoomCost = reservation.RoomCost.ToString(),
            };

            reservationDto.RoomNumbers = string.Join(',', reservation.RoomList.Select(x => x.RoomNo.ToString()));
            reservationDto.MealPlan = reservation.MealPlan.MealPlanName;

            return reservationDto;
        }


    }


}
