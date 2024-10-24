using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace reservations
{
    internal class RoomListViewModel
    {
        public string RoomNumber { get; set; } = "";
        public string Type { get; set; } = "";
        public string Price { get; set; } = "";
        public string Size { get; set; } = "";
        
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public RoomListViewModel(Room r)
        {
            RoomNumber = Convert.ToString(r.RoomNumber);
            Type = r.Type;
            Price = Convert.ToString(r.Price);
            Size = Convert.ToString(r.Size);

            EditCommand = new RelayCommand(Edit);
            DeleteCommand = new RelayCommand(Delete);
        }

        public void Edit()
        {
            using ApplicationDataContext context = new();

            Room? edited = context.Rooms.FirstOrDefault(x => x.RoomNumber == int.Parse(RoomNumber));

            if (edited != null)
            {
                edited.RoomNumber = int.Parse(RoomNumber);
                edited.Type = Type;
                edited.Price = int.Parse(Price);
                edited.Size = int.Parse(Size);

                context.Rooms.Update(edited);

                context.SaveChanges();
            }
        }
        public void Delete()
        {
            using ApplicationDataContext context = new();

            Room? room = context.Rooms.FirstOrDefault(x => x.RoomNumber == int.Parse(RoomNumber));

            if (room != null)
            {
                context.Rooms.Remove(room);
                context.SaveChanges();

                AdminPanelViewModel.getInstance.RemoveRoom(this);
            }
        }
    }
}
