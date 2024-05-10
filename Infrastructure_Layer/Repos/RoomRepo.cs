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
    public class RoomRepo : IRoomRepo
    {
        private readonly ApplicationContext _context;
        public RoomRepo(ApplicationContext context)
        {
            _context = context;
        }


        public async Task<List<Room>> GetRoomsByIdList(List<int> idList, int take)
        {
            return await _context.Rooms
                .Where(x => idList.Contains(x.RoomId))
                .OrderBy(x => x.RoomNo)
                .Take(take)
                .ToListAsync();
        }

        public async Task<List<int>> GetRoomsIdListByType(int roomTypeId)
        {
            return await _context.Rooms.Where(x => x.RoomTypeId == roomTypeId)
                .Select(x => x.RoomId).ToListAsync();
        }

        public async Task<RoomType?> GetRoomTypeById(int roomTypeId)
        {
            return await _context.RoomTypes
                 .Include(x => x.RoomSeasonList)
                 .FirstOrDefaultAsync(x => x.RoomTypeId == roomTypeId);
        }

    }
}
