using Domain_Layer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Interfaces
{
    public interface IMealPlanRepo
    {
        Task<MealPlan?> GetMealPlanById(int mealPlanId);
        Task<List<MealPlan>> GetAll();
    }
}
