using CSharpFunctionalExtensions;
using Domain_Layer.Entities;
using Domain_Layer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.Services
{
    public class ReservationService
    {
        private readonly IReservationRepo _reservationRepo;
        private readonly IRoomRepo _roomRepo;
        public ReservationService(IReservationRepo reservationRepo, IRoomRepo roomRepo)
        {
            _reservationRepo = reservationRepo;
            _roomRepo = roomRepo;
        }


        public async Task<Result<Reservation>> CreateReservation(int roomTypeId, int mealPlanId, int noOfAdults, int noOfChildren, DateTime checkIn, DateTime checkOut, int noOfRooms,string? guestName,string? phoneNumber)
        {
            Maybe<RoomType> maybeRoomType = await _roomRepo.GetRoomTypeById(roomTypeId);
            if (maybeRoomType.HasNoValue)
                return Result.Failure<Reservation>("Room Type Not Found");
            var roomType = maybeRoomType.Value;

            Maybe<MealPlan> maybeMealPlan = await _roomRepo.GetMealPlanById(mealPlanId); 
            if (maybeMealPlan.HasNoValue)
                return Result.Failure<Reservation>("Meal Plan Not Found");
            var mealPlan = maybeMealPlan.Value;

            List<int> allRoomsIdListByType = await _roomRepo.GetRoomsIdListByType(roomTypeId);

            List<int> reservedRoomsIdList = await _reservationRepo.GetReservedRoomsIdList(checkIn, checkOut, allRoomsIdListByType);

            List<int> availableRoomsIdList = allRoomsIdListByType.Except(reservedRoomsIdList).ToList();

            if (availableRoomsIdList.Count == 0)
                return Result.Failure<Reservation>("There is no available rooms, You can try choose another room type or some other time");
            if (availableRoomsIdList.Count < noOfRooms)
                return Result.Failure<Reservation>($"There is no enough rooms, The available rooms now id {availableRoomsIdList.Count}");


            var availableRooms = await _roomRepo.GetRoomsByIdList(idList: availableRoomsIdList, take: noOfRooms);

            var seasonsIntersectWithReservatoinTime = roomType.RoomSeasonList.Where(x =>
                (x.SeasonStart <= checkIn && checkIn <= x.SeasonEnd) ||
                (x.SeasonStart <= checkOut && checkOut <= x.SeasonEnd) ||
                (checkIn <= x.SeasonStart && x.SeasonEnd <= checkOut)
                );

            decimal roomsCost = seasonsIntersectWithReservatoinTime.Sum(x =>
                    (GetMinDate(x.SeasonEnd, checkOut).Date - GetMaxDate(x.SeasonStart,checkIn).Date).Days 
                    * x.RatePerRoom
                    ) * noOfRooms;



            var plsnSeasonsIntersectWithReservatoinTime = mealPlan.MealplanSeasonList.Where(x =>
                (x.SeasonStart <= checkIn && checkIn <= x.SeasonEnd) ||
                (x.SeasonStart <= checkOut && checkOut <= x.SeasonEnd) ||
                (checkIn <= x.SeasonStart && x.SeasonEnd <= checkOut)
                );

            decimal planCost = plsnSeasonsIntersectWithReservatoinTime.Sum(x =>
                    (GetMinDate(x.SeasonEnd, checkOut).Date - GetMaxDate(x.SeasonStart,checkIn).Date).Days 
                    * (x.RatePerAdult * noOfAdults + x.RatePerChild * noOfChildren)
                    );


            decimal totalCost = roomsCost + planCost;

            var reservation = new Reservation
            {
                CheckIn = checkIn,
                CheckOut = checkOut,
                TotalCost = totalCost,
                MealPlanCost = planCost,
                RoomCost = roomsCost,
                NoOfAdults = noOfAdults,
                NoOfChildren = noOfChildren,
                NoOfRooms = noOfRooms,
                MealPlanId = mealPlanId,
                MealPlan = mealPlan,
                RoomList = availableRooms,
                ReservationNo = 1,
                GuestName = guestName,
                PhoneNumber = phoneNumber,

            };

            var saveReservationResult = await _reservationRepo.AddReservation(reservation);
            if(saveReservationResult.IsFailure)
                return Result.Failure<Reservation>(saveReservationResult.Error);



            return Result.Success(reservation);
        }


        private DateTime GetMaxDate(DateTime date1, DateTime date2) => date1 >= date2 ? date1 : date2;
        private DateTime GetMinDate(DateTime date1, DateTime date2) => date1 <= date2 ? date1 : date2;
    }
}
