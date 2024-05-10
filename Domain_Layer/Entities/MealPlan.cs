using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Entities
{
    public class MealPlan
    {
        public int MealPlanId { get; set; }
        public string MealPlanName { get; set; }

        public List<MealPlanSeason> MealplanSeasonList { get; set; } = new();

    }
}
