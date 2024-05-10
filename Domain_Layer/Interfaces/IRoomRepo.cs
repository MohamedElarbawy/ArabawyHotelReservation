using Domain_Layer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Interfaces
{
    public interface IRoomRepo
    {

        Task<List<int>> GetRoomsIdListByType(int roomTypeId);

        Task<List<Room>> GetRoomsByIdList(List<int> idList,int take);

        Task<RoomType?> GetRoomTypeById(int roomTypeId);
        Task<MealPlan?> GetMealPlanById(int mealPlanId);
    }
}
