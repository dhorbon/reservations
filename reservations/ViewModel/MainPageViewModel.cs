using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace reservations.ViewModel
{
    internal class MainPageViewModel : INotifyPropertyChanged
    {
        private static MainPageViewModel? instance = null;

        public static MainPageViewModel getInstance
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

        private MainPageViewModel()
        {
            CurrentPage = new LoginPage();
        }

        public Page CurrentPage { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged = (s,e) => { };

        public void ChangePage(Page page)
        {
            CurrentPage = page;
            PropertyChanged(this, new(nameof(CurrentPage)));
        }
    }
}
