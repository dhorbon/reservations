using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace reservations.ViewModel
{
    internal class ReservationListViewModel
    {
        public string Id { get; set; }
        public string RoomNumber { get; set; }
        public DateTime ReservationStart { get; set; }
        public DateTime ReservationEnd { get; set; }
        public string UserId { get; set; }
        public string GuestFullName { get; set; }
        public string GuestEmail { get; set; }
        public string GuestPhone { get; set; }
        public string Status { get; set; }


        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public ReservationListViewModel(Reservation r)
        {
            Id = Convert.ToString(r.Id);
            RoomNumber = Convert.ToString(r.RoomNumber);
            ReservationStart = r.ReservationStart;
            ReservationEnd = r.ReservationEnd;
            UserId = Convert.ToString(r.UserId);
            GuestFullName = r.GuestFullName;
            GuestEmail = r.GuestEmail;
            GuestPhone = r.GuestPhone;
            Status = r.Status;

            EditCommand = new RelayCommand(Edit);
            DeleteCommand = new RelayCommand(Delete);
        }

        public void Edit()
        {
            using ApplicationDataContext context = new();

            Reservation? edited = context.Reservations.FirstOrDefault(x => x.Id == int.Parse(Id));

            if (edited != null)
            {
                context.Reservations.Update(edited);

                context.SaveChanges();
            }
        }
        public void Delete()
        {
            using ApplicationDataContext context = new();

            Reservation? reservation = context.Reservations.FirstOrDefault(x => x.Id == int.Parse(Id));

            if (reservation != null)
            {
                context.Reservations.Remove(reservation);
                context.SaveChanges();

                AdminPanelViewModel.getInstance.RemoveReservation(this);
            }
        }
    }
}
