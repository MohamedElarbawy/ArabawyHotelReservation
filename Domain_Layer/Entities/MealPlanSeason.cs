using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Entities
{
    public class MealPlanSeason
    {
       
        public int MealPlanSeasonId { get; set; }
        public DateTime SeasonStart { get; set; }
        public DateTime SeasonEnd { get; set; }

        public decimal RatePerAdult { get; set;}
        public decimal RatePerChild { get; set;}

        public int MealPlanId { get; set; }
    }
}
