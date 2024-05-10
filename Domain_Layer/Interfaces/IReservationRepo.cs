using CSharpFunctionalExtensions;
using Domain_Layer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Interfaces
{
    public interface IReservationRepo
    {
        Task<List<int>> GetReservedRoomsIdList(DateTime checkIn, DateTime checkOut, List<int> allRoomsIdListByType);
        Task<Result> AddReservation(Reservation reservation);
        Task<long> GetMaxReservationNO();
    }
}
