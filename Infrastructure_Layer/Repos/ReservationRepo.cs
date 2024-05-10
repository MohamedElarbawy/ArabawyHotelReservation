using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using Domain_Layer.Entities;
using Domain_Layer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure_Layer.Repos
{
    public class ReservationRepo : IReservationRepo
    {
        private readonly ApplicationContext _context;
        public ReservationRepo(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<List<int>> GetReservedRoomsIdList(DateTime checkIn, DateTime checkOut, List<int> roomsIdList)
        {
            return await _context.Reservations
                   .Include(x => x.RoomList)
                   .Where(x =>
                  x.RoomList.Any(r => roomsIdList.Contains(r.RoomId)) &&
                  ((x.CheckIn <= checkIn && checkIn <= x.CheckOut) ||
                   (x.CheckIn <= checkOut && checkOut <= x.CheckOut))
                   )
                   .SelectMany(x => x.RoomList)
                   .Select(x => x.RoomId)
                   .ToListAsync();
        }



        public async Task<Result> AddReservation(Reservation reservation)
        {
            try
            {
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            catch (DbUpdateException dbExce)
            {
                return Result.Failure(dbExce.Message);
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }

        }
    }
}
