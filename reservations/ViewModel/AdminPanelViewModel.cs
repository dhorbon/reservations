using reservations.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace reservations
{
    internal class AdminPanelViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<UserListViewModel> users { get; set; } = new();
        public ObservableCollection<RoomListViewModel> rooms { get; set; } = new();
        public ObservableCollection<ReservationListViewModel> reservations { get; set; } = new();

        private static AdminPanelViewModel? Instance = null;

        public string NewLogin { get; set; } = "";
        public string NewPassword { get; set; } = "";
        public bool NewIsAdmin { get; set; } = false;
        public string UserError { get; set; } = "";

        public ICommand AddUserCommand { get; set; }

        public string NewRoomNumber { get; set; } = "";
        public string NewType { get; set; } = "";
        public string NewPrice { get; set; } = "";
        public string NewSize { get; set; } = "";
        public string RoomError { get; set; } = "";

        public ICommand AddRoomCommand { get; set; }

        public string NewResRoomNumber { get; set; }
        public DateTime NewReservationStart { get; set; }
        public DateTime NewReservationEnd { get; set; }
        public string NewResUserId { get; set; }
        public string NewGuestFullName { get; set; }
        public string NewGuestEmail { get; set; }
        public string NewGuestPhone { get; set; }
        public string NewStatus { get; set; }
        public string ReservationError { get; set; }

        public ICommand AddReservationCommand { get; set; }

        public static AdminPanelViewModel getInstance
        {
            get
            {
                if(Instance == null)
                {
                    Instance = new();
                }
                return Instance;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged = (s,e) => { };

        private AdminPanelViewModel()
        {
            using ApplicationDataContext context = new();

            AddUserCommand = new RelayCommand(AddUser);
            AddRoomCommand = new RelayCommand(AddRoom);
            AddReservationCommand = new RelayCommand(AddReservation);

            foreach (var user in context.Users)
            {
                users.Add(new(user));
            }
            foreach (var room in context.Rooms)
            {
                rooms.Add(new(room));
            }
            foreach (var reservation in context.Reservations)
            {
                reservations.Add(new(reservation));
            }
        }

        public void AddUser()
        {
            if(NewLogin == string.Empty || NewPassword == string.Empty)
            {
                UserError = "Make sure all data is filled in";
                PropertyChanged(this, new(nameof(UserError)));
            }
            else
            {
                User user = new(NewLogin, LoginViewModel.GetStringSha256Hash(NewPassword), NewIsAdmin);

                using ApplicationDataContext context = new();
                context.Users.Add(user);
                context.SaveChanges();

                users.Add(new(user));

                NewLogin = string.Empty;
                NewPassword = string.Empty;
                NewIsAdmin = false;
                UserError = string.Empty;

                PropertyChanged(this, new(nameof(NewLogin)));
                PropertyChanged(this, new(nameof(NewPassword)));
                PropertyChanged(this, new(nameof(NewIsAdmin)));
                PropertyChanged(this, new(nameof(UserError)));
            }
        }
        public void RemoveUser(UserListViewModel user)
        {
            users.Remove(user);
        }

        public void AddRoom()
        {
            if (NewRoomNumber == string.Empty || NewType == string.Empty || NewPrice == string.Empty || NewSize == string.Empty)
            {
                RoomError = "Make sure all data is filled in";
                PropertyChanged(this, new(nameof(RoomError)));
            }
            else
            {
                Room room = new(int.Parse(NewRoomNumber), NewType, int.Parse(NewPrice), int.Parse(NewSize));

                using ApplicationDataContext context = new();
                context.Rooms.Add(room);
                context.SaveChanges();

                rooms.Add(new(room));

                RoomError = string.Empty;

                PropertyChanged(this, new(nameof(RoomError)));
            }
        }
        public void RemoveRoom(RoomListViewModel room)
        {
            rooms.Remove(room);
        }

        public void AddReservation()
        {
            if (
                NewResRoomNumber == string.Empty ||
                NewResUserId == string.Empty ||
                NewGuestFullName == string.Empty ||
                NewGuestEmail == string.Empty ||
                NewGuestPhone == string.Empty ||
                NewStatus == string.Empty
            )
            {
                ReservationError = "Make sure all data is filled in";
                PropertyChanged(this, new(nameof(ReservationError)));
            }
            else
            {
                Reservation reservation = new(int.Parse(NewResRoomNumber),NewReservationStart,NewReservationEnd,int.Parse(NewResUserId),NewGuestFullName,NewGuestEmail,NewGuestPhone,NewStatus);

                using ApplicationDataContext context = new();
                context.Reservations.Add(reservation);
                context.SaveChanges();

                reservations.Add(new(reservation));

                ReservationError = string.Empty;

                PropertyChanged(this, new(nameof(ReservationError)));
            }
        }
        public void RemoveReservation(ReservationListViewModel reservation)
        {
            reservations.Remove(reservation);
        }
    }
}
