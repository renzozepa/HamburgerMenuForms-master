using System;
using System.Collections.Generic;
using System.Text;

namespace HamburgerMenu.ViewModels
{
    public class PersonalDetalleViewModel : BaseViewModel
    {
        private PersonalViewModel _personalVM;
        public PersonalViewModel PersonalVM
        {
            get { return _personalVM; }
            set { _personalVM = value; OnPropertyChanged(); }
        }
        public PersonalDetalleViewModel(PersonalViewModel Personal)
        {
            PersonalVM = Personal;
        }
    }
}
