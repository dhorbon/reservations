using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents.Serialization;

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

        public static bool IsAvaliableFromTo(Room room, DateTime start, DateTime end)
        {
            using ApplicationDataContext context = new();

            List<Reservation> res = context.Reservations
                .Where(x => x.RoomNumber == room.RoomNumber)
                .Where(x => x.Status != "cancelled")
                .ToList();
            for (int i = 0; i < res.Count; i++)
            {
                if ((
                    res[i].ReservationStart >= start && res[i].ReservationStart <= end) || (
                    res[i].ReservationEnd >= start && res[i].ReservationEnd <= end
                    ))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
