using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace reservations
{
    internal class UserListViewModel
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public bool IsAdmin { get; set; }

        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        
        public UserListViewModel(User u)
        {
            Id = u.Id;
            Login = u.Login;
            IsAdmin = u.IsAdmin;

            EditCommand = new RelayCommand(Edit);
            DeleteCommand = new RelayCommand(Delete);
        }

        public void Edit()
        {
            using ApplicationDataContext context = new();
            
            User? edited = context.Users.FirstOrDefault(x => x.Id == Id);

            if(edited != null)
            {
                edited.IsAdmin = IsAdmin;

                context.Users.Update(edited);

                context.SaveChanges();
            }
        }
        public void Delete()
        {
            using ApplicationDataContext context = new();

            User? user = context.Users.FirstOrDefault(x => x.Id == Id);

            if (user != null)
            {
                context.Users.Remove(user);
                context.SaveChanges();

                AdminPanelViewModel.getInstance.RemoveUser(this);
            }
        }
    }
}
