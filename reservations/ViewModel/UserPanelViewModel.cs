using reservations.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace reservations
{
    internal class UserPanelViewModel : INotifyPropertyChanged
    {
        private static UserPanelViewModel? instance = null;

        public event PropertyChangedEventHandler? PropertyChanged = (s,e) => { };

        public static UserPanelViewModel getInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new();
                }
                return instance;
            }
        }

        private UserPanelViewModel()
        {
            NewRoomType = "Select room type";
            PropertyChanged(this, new(nameof(NewRoomType)));

            AddReservationCommand = new RelayCommand(AddReservation);
        }

        public int LoggedUser { get; set; } = -1;

        public ObservableCollection<UserPanelListViewModel> Reservations { get; set; } = new();
        public ObservableCollection<string> RoomTypeList { get; set; } = new();

        public string NewRoomType { get; set; }

        public DateTime NewStartDate { get; set; } = DateTime.Now;
        public DateTime NewEndDate { get; set; } = DateTime.Now.AddDays(1);
        public string NewFullName { get; set; } = "";
        public string NewEmail { get; set; } = "";
        public string NewPhone { get; set; } = "";

        public string error { get; set; } = "";

        public ICommand AddReservationCommand { get; set; }

        public void SetUser(User u)
        {
            LoggedUser = u.Id;

            using ApplicationDataContext context = new();

            UpdateReservations();

            RoomTypeList.Add("Select room type");
            foreach (var roomType in context.Rooms.Select(x => x.Type).Distinct())
            {
                RoomTypeList.Add(roomType);
            }

        }

        public void UpdateReservations()
        {
            while(Reservations.Count != 0)
            {
                Reservations.RemoveAt(0);
            }

            using ApplicationDataContext context = new();

            foreach (var res in (context.Reservations.Where(x => x.Status != "cancelled").Where(x => x.UserId == LoggedUser)))
            {
                if(res.Status == "cancelled")
                {
                    throw new Exception("WTF");
                }
                Reservations.Add(new(res));
            }
        }

        public void AddReservation()
        {
            using ApplicationDataContext context = new();

            List<Room> viableRooms = context.Rooms.AsEnumerable()
                .Where(x => x.Type == NewRoomType)
                .Where(x => Room.IsAvaliableFromTo(x,NewStartDate, NewEndDate))
                .ToList();

            if(viableRooms.Count == 0)
            {
                error = "no avaliable rooms of this type at selected date";
                PropertyChanged(this, new(nameof(error)));
                return;
            }
            int NewRoomNumber = viableRooms[0].RoomNumber;

            if(
                NewFullName == "" ||
                NewEmail == "" ||
                NewPhone == ""
            )
            {
                error = "please fill all data";
                PropertyChanged(this, new(nameof(error)));
                return;
            }

            Reservation NewReservation = new(
                NewRoomNumber,
                NewStartDate,
                NewEndDate,
                LoggedUser,
                NewFullName,
                NewEmail,
                NewPhone,
                "accepted"
                );

            context.Reservations.Add(NewReservation);
            context.SaveChanges();

            Reservations.Add(new(NewReservation));

            error = "reservation succesfull";
            PropertyChanged(this, new(nameof(error)));
        }
    }
}
