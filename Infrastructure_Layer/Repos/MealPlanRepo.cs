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
    public class MealPlanRepo : IMealPlanRepo
    {
        private readonly ApplicationContext _context;
        public MealPlanRepo(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<MealPlan?> GetMealPlanById(int mealPlanId)
        {
            return await _context.MealPlans
                 .Include(x => x.MealplanSeasonList)
                 .FirstOrDefaultAsync(x => x.MealPlanId == mealPlanId);
        }

        public async Task<List<MealPlan>> GetAll()
        {
          return await _context.MealPlans.ToListAsync();
        }

    }
}
