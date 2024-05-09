using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Entities
{
    public class Room
    {

        public int RoomId { get; set; }
        public int RoomNo { get; set; }
        public int AdultsCapcity { get; set; }
        public int ChildrenCapcity { get; set; }


        public int RoomTypeId { get; set; }
        public RoomType RoomType { get; set; }




    }
}
