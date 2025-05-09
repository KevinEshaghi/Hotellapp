using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelSystem
{
    public class Room
    {
   
        public int roomId { get; set; }

   
        public int RoomNumber { get; set; }

      
        public string RoomType { get; set; }

   
        public int MaxOccupants { get; set; }

     
        public int ExtraBeds { get; set; }

        public Room(int roomNumber, string roomType, int maxOccupants, int extraBeds = 0)
        {
            RoomNumber = roomNumber;
            RoomType = roomType;
            MaxOccupants = maxOccupants;
            ExtraBeds = extraBeds;
        }

       
       // public Room() { }
    }
}
