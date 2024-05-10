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


        public async Task<Result<ReservationDto>> CreateReservation(ReservationCreateDto reservationDto)
        {

            DateTime checkIn = reservationDto.CheckIn;
            DateTime checkOut = reservationDto.CheckOut;
            Maybe<RoomType> maybeRoomType = await _roomRepo.GetRoomTypeById(reservationDto.RoomTypeId);
            Maybe<MealPlan> maybeMealPlan = await _mealPlanRepo.GetMealPlanById(reservationDto.MealPlanId); 
            
            var validationResult = ValidateReservation(checkIn, checkOut, maybeRoomType,maybeMealPlan,reservationDto.NoOfAdults,reservationDto.NoOfRooms);

            if (validationResult.IsFailure)
                return Result.Failure<ReservationDto>(validationResult.Error);

            var roomType = maybeRoomType.Value;
            var mealPlan = maybeMealPlan.Value;

            List<int> allRoomsIdListByType = await _roomRepo.GetRoomsIdListByType(reservationDto.RoomTypeId);

            List<int> reservedRoomsIdList = await _reservationRepo.GetReservedRoomsIdList(checkIn, checkOut, allRoomsIdListByType);

            List<int> availableRoomsIdList = allRoomsIdListByType.Except(reservedRoomsIdList).ToList();

            if (availableRoomsIdList.Count == 0)
                return Result.Failure<ReservationDto>("There is no available rooms, You can try choose another room type or some other time");
            if (availableRoomsIdList.Count < reservationDto.NoOfRooms)
                return Result.Failure<ReservationDto>($"There is no enough rooms, The available rooms now id {availableRoomsIdList.Count}");


            var availableRooms = await _roomRepo.GetRoomsByIdList(idList: availableRoomsIdList, take: reservationDto.NoOfRooms);

            var seasonsIntersectWithReservatoinTime = roomType.RoomSeasonList.Where(x =>
                (x.SeasonStart <= checkIn && checkIn <= x.SeasonEnd) ||
                (x.SeasonStart <= checkOut && checkOut <= x.SeasonEnd) ||
                (checkIn <= x.SeasonStart && x.SeasonEnd <= checkOut)
                );
            if(!seasonsIntersectWithReservatoinTime.Any())
                return Result.Failure<ReservationDto>($"Sorry, Something wrong happend,Please try in another time");

            decimal roomsCost = seasonsIntersectWithReservatoinTime.Sum(x =>
                    (GetMinDate(x.SeasonEnd, checkOut).Date - GetMaxDate(x.SeasonStart,checkIn).Date).Days 
                    * x.RatePerRoom
                    ) * reservationDto.NoOfRooms;



            var plansSeasonsIntersectWithReservatoinTime = mealPlan.MealplanSeasonList.Where(x =>
                (x.SeasonStart <= checkIn && checkIn <= x.SeasonEnd) ||
                (x.SeasonStart <= checkOut && checkOut <= x.SeasonEnd) ||
                (checkIn <= x.SeasonStart && x.SeasonEnd <= checkOut)
                );
            if(!plansSeasonsIntersectWithReservatoinTime.Any())
                return Result.Failure<ReservationDto>($"Sorry, Something wrong happend,Please try in another time");

            decimal planCost = plansSeasonsIntersectWithReservatoinTime.Sum(x =>
                    (GetMinDate(x.SeasonEnd, checkOut).Date - GetMaxDate(x.SeasonStart,checkIn).Date).Days 
                    * (x.RatePerAdult * reservationDto.NoOfAdults + x.RatePerChild * reservationDto.NoOfChildren)
                    );


            decimal totalCost = roomsCost + planCost;
            long maxReservationNo = await _reservationRepo.GetMaxReservationNO();

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
                ReservationNo = maxReservationNo + 1,
                GuestName = reservationDto.GuestName,
                PhoneNumber = reservationDto.PhoneNumber,

            };

            var saveReservationResult = await _reservationRepo.AddReservation(reservation);
            if(saveReservationResult.IsFailure)
                return Result.Failure<ReservationDto>(saveReservationResult.Error);



            return Result.Success(ReservationDto.MapFrom(reservation));
        }


        private Result ValidateReservation(DateTime checkIn, DateTime checkOut,Maybe<RoomType> maybeRoomType, Maybe<MealPlan> maybeMealPlan , int noOfAdults,int noOfRooms)
        {
            if (checkIn >= checkOut)
                return Result.Failure("Check Out Can't Be Before Check In");
            if (!(noOfAdults > 0))
                return Result.Failure("Enter Number Of Adults");
            if (!(noOfRooms > 0))
                return Result.Failure("Enter Number Of Rooms");
            if (maybeRoomType.HasNoValue)
                return Result.Failure("Room Type Not Found");
            if (maybeMealPlan.HasNoValue)
                return Result.Failure("Meal Plan Not Found");


            return Result.Success();
        }


        private DateTime GetMaxDate(DateTime date1, DateTime date2) => date1 >= date2 ? date1 : date2;
        private DateTime GetMinDate(DateTime date1, DateTime date2) => date1 <= date2 ? date1 : date2;
    }
}
