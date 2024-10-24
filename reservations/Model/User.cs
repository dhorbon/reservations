using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reservations
{
    internal class User
    {
        [Key]
        public int Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public bool IsAdmin { get; set; }

        public User(string login, string passwordHash, bool isAdmin)
        {
            Login = login;
            PasswordHash = passwordHash;
            IsAdmin = isAdmin;
        }
    }
}
