using Application_Layer.DtoS;
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
        private readonly IMealPlanRepo _mealPlanRepo;
        public ReservationService(IReservationRepo reservationRepo, IRoomRepo roomRepo, IMealPlanRepo mealPlanRepo)
        {
            _reservationRepo = reservationRepo;
            _roomRepo = roomRepo;
            _mealPlanRepo = mealPlanRepo;
        }


        public async Task<Result<Reservation>> CreateReservation(ReservationCreateDto reservationDto)
        {
            Maybe<RoomType> maybeRoomType = await _roomRepo.GetRoomTypeById(reservationDto.RoomTypeId);
            if (maybeRoomType.HasNoValue)
                return Result.Failure<Reservation>("Room Type Not Found");
            var roomType = maybeRoomType.Value;

            Maybe<MealPlan> maybeMealPlan = await _mealPlanRepo.GetMealPlanById(reservationDto.MealPlanId); 
            if (maybeMealPlan.HasNoValue)
                return Result.Failure<Reservation>("Meal Plan Not Found");
            var mealPlan = maybeMealPlan.Value;

            DateTime checkIn = reservationDto.CheckIn;
            DateTime checkOut = reservationDto.CheckOut;

            List<int> allRoomsIdListByType = await _roomRepo.GetRoomsIdListByType(reservationDto.RoomTypeId);

            List<int> reservedRoomsIdList = await _reservationRepo.GetReservedRoomsIdList(checkIn, checkOut, allRoomsIdListByType);

            List<int> availableRoomsIdList = allRoomsIdListByType.Except(reservedRoomsIdList).ToList();

            if (availableRoomsIdList.Count == 0)
                return Result.Failure<Reservation>("There is no available rooms, You can try choose another room type or some other time");
            if (availableRoomsIdList.Count < reservationDto.NoOfRooms)
                return Result.Failure<Reservation>($"There is no enough rooms, The available rooms now id {availableRoomsIdList.Count}");


            var availableRooms = await _roomRepo.GetRoomsByIdList(idList: availableRoomsIdList, take: reservationDto.NoOfRooms);

            var seasonsIntersectWithReservatoinTime = roomType.RoomSeasonList.Where(x =>
                (x.SeasonStart <= checkIn && checkIn <= x.SeasonEnd) ||
                (x.SeasonStart <= checkOut && checkOut <= x.SeasonEnd) ||
                (checkIn <= x.SeasonStart && x.SeasonEnd <= checkOut)
                );

            decimal roomsCost = seasonsIntersectWithReservatoinTime.Sum(x =>
                    (GetMinDate(x.SeasonEnd, checkOut).Date - GetMaxDate(x.SeasonStart,checkIn).Date).Days 
                    * x.RatePerRoom
                    ) * reservationDto.NoOfRooms;



            var plsnSeasonsIntersectWithReservatoinTime = mealPlan.MealplanSeasonList.Where(x =>
                (x.SeasonStart <= checkIn && checkIn <= x.SeasonEnd) ||
                (x.SeasonStart <= checkOut && checkOut <= x.SeasonEnd) ||
                (checkIn <= x.SeasonStart && x.SeasonEnd <= checkOut)
                );

            decimal planCost = plsnSeasonsIntersectWithReservatoinTime.Sum(x =>
                    (GetMinDate(x.SeasonEnd, checkOut).Date - GetMaxDate(x.SeasonStart,checkIn).Date).Days 
                    * (x.RatePerAdult * reservationDto.NoOfAdults + x.RatePerChild * reservationDto.NoOfChildren)
                    );


            decimal totalCost = roomsCost + planCost;

            var reservation = new Reservation
            {
                CheckIn = checkIn,
                CheckOut = checkOut,
                TotalCost = totalCost,
                MealPlanCost = planCost,
                RoomCost = roomsCost,
                NoOfAdults = reservationDto.NoOfAdults,
                NoOfChildren = reservationDto.NoOfChildren,
                NoOfRooms = reservationDto.NoOfRooms,
                MealPlanId = reservationDto.MealPlanId,
                MealPlan = mealPlan,
                RoomList = availableRooms,
                ReservationNo = 1,
                GuestName = reservationDto.GuestName,
                PhoneNumber = reservationDto.PhoneNumber,

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
