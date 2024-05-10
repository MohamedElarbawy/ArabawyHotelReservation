using Application_Layer.DtoS;
using Domain_Layer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.Services
{
    public class RoomService
    {
        private readonly IRoomRepo _roomRepo;
        public RoomService(IRoomRepo roomRepo)
        {
            _roomRepo = roomRepo;
        }


        public async Task<List<RoomTypeDto>> GetRoomTypes()
        {
            return (await _roomRepo.GetAllRoomTypes())
                .Select(x => new RoomTypeDto(x.RoomTypeId, x.RoomTypeName))
                .ToList();
        }
    }
}
