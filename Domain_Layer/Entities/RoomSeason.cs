using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Entities
{
    public class RoomSeason
    {
        public int RoomSeasonId { get; set; }
        public DateTime SeasonStart { get; set; }
        public DateTime SeasonEnd { get; set; }
        public decimal RatePerRoom { get; set; }

        public int RoomTypeId { get; set; }

    }
}
