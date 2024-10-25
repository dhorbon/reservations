using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace reservations.ViewModel
{
    internal class UserPanelListViewModel
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; } = "";
        public DateTime StartDate { get; set; } = new();
        public DateTime EndDate { get; set; } = new();
        public string Status { get; set; } = "";

        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public UserPanelListViewModel(Reservation r)
        {
            Id = r.Id;
            RoomNumber = Convert.ToString(r.RoomNumber);
            StartDate = r.ReservationStart;
            EndDate = r.ReservationEnd;
            Status = r.Status;

            EditCommand = new RelayCommand(Edit);
            DeleteCommand = new RelayCommand(Delete);
        }

        public void Edit() { throw new NotImplementedException(); }
        public void Delete(){
            using ApplicationDataContext context = new();

            Reservation? edited = context.Reservations.First(x => x.Id == Id);

            edited.Status = "cancelled";

            context.Reservations.Update(edited);
            context.SaveChanges();

            UserPanelViewModel.getInstance.UpdateReservations();
        }
    }
}
