using Application_Layer.DtoS;
using Domain_Layer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.Services
{
    public class MealPlanService
    {
        private readonly IMealPlanRepo _mealPlanRepo;
        public MealPlanService(IMealPlanRepo mealPlanRepo)
        {
            _mealPlanRepo = mealPlanRepo;
        }
        public async Task<List<MealPlanDto>> GetMealPlans()
        {
            return (await _mealPlanRepo.GetAll())
                .Select(x => new MealPlanDto(x.MealPlanId, x.MealPlanName))
                .ToList();
        }
    }
}
