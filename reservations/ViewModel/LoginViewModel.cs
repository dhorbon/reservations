using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic.ApplicationServices;
using reservations;
using reservations.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace reservations
{
    internal class LoginViewModel : INotifyPropertyChanged
    {
        private static LoginViewModel? instance = null;

        public static LoginViewModel getInstance
        { 
            get
            { 
                if(instance == null)
                {
                    instance = new();
                }
                return instance;
            } 
        }

        public string login { get; set; } = "";
        public string password { get; set; } = "";
        public string error { get; set; } = "";

        public ICommand LoginCommand { get; set; }

        private LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
        }

        public event PropertyChangedEventHandler? PropertyChanged = (s,e) => { };

        public void Login()
        {
            using ApplicationDataContext context = new();

            //initialize data
            if (!context.Users.Any())
            {
                context.Users.Add(new User("admin", GetStringSha256Hash("admin"), true ));
                context.Users.Add(new User("user1", GetStringSha256Hash("user1"), false));
                context.Users.Add(new User("user2", GetStringSha256Hash("user2"), false));

                context.Rooms.Add(new Room(100, "type1", 10000, 2));
                context.Rooms.Add(new Room(101, "type1", 10000, 2));
                context.Rooms.Add(new Room(102, "type2", 19000, 4));
                context.Rooms.Add(new Room(103, "type2", 19000, 4));

                context.Reservations.Add(new Reservation(100, new DateTime(2024, 12, 1), new DateTime(2024, 12, 8), 2, "Name Surname", "mail@mail.domain", "111222333", "confirmed"));

                context.Database.OpenConnection();
                try
                {
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Rooms ON");
                    context.SaveChanges();
                }
                finally
                {
                    context.Database.CloseConnection();
                }
            }

            var Data = context.Users.Where(u => u.Login == login && u.PasswordHash == GetStringSha256Hash(password));
            if (Data.Any())
            {
                if (Data.First().IsAdmin)
                {
                    MainPageViewModel.getInstance.ChangePage(new AdminPanel());
                }
                else
                {
                    MainPageViewModel.getInstance.ChangePage(new UserPanel());
                }
            }
            else
            {
                error = "wrong information";
                PropertyChanged(this, new(nameof(error)));
            }
        }

        internal static string GetStringSha256Hash(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = Encoding.UTF8.GetBytes(text);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", string.Empty);
            }
        }
    }
}
