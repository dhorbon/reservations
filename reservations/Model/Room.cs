using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reservations
{
    internal class Room
    {
        [Key]
        public int RoomNumber { get; set; }
        public string Type { get; set; }
        public int Price { get; set; }
        public int Size { get; set; }

        public Room(int roomNumber, string type, int price, int size)
        {
            RoomNumber = roomNumber;
            Type = type;
            Price = price;
            Size = size;
        }
    }
}
