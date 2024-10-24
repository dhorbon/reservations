using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reservations
{
    internal class Reservation
    {
        [Key]
        public int Id { get; set; }
        public int RoomNumber { get; set; }
        public DateTime ReservationStart { get; set; }
        public DateTime ReservationEnd { get; set; }
        public int UserId { get; set; }
        public string GuestFullName { get; set; }
        public string GuestEmail { get; set; }
        public string GuestPhone { get; set;}
        public string Status { get; set; }

        public Reservation(int roomNumber, DateTime reservationStart, DateTime reservationEnd, int userId, string guestFullName, string guestEmail, string guestPhone, string status)
        {
            RoomNumber = roomNumber;
            ReservationStart = reservationStart;
            ReservationEnd = reservationEnd;
            UserId = userId;
            GuestFullName = guestFullName;
            GuestEmail = guestEmail;
            GuestPhone = guestPhone;
            Status = status;
        }
    }
}
