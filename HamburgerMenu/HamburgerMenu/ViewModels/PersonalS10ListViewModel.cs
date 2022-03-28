using HamburgerMenu.Tablas;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HamburgerMenu.ViewModels
{
    public class PersonalS10ListViewModel : BaseViewModel
    {
        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }
        public ICommand RefreshCommand { get; set; }
        public ICommand SearchByNameCommand { get; set; }
        public ICommand GoToDetailsCommand { get; set; }
        public INavigation Navigation { get; set; }

        private ObservableCollection<PersonalS10ViewModel> _personalS10ViewModel;
        public ObservableCollection<PersonalS10ViewModel> LPersonalsS10ViewModel
        {
            get { return _personalS10ViewModel; }
            set
            {
                _personalS10ViewModel = value;
                OnPropertyChanged();
            }
        }

        private PersonalS10ViewModel _itemSeleccionado;
        public PersonalS10ViewModel ItemSeleccionado
        {
            get { return _itemSeleccionado; }
            set { _itemSeleccionado = value; OnPropertyChanged(); }
        }

        public PersonalS10ListViewModel(INavigation navigation)
        {
            Navigation = navigation;

            RefreshCommand = new Command(async () =>
            {
                IsRefreshing = true;
                await LoadData();
                IsRefreshing = false;
            });

            SearchByNameCommand = new Command<string>(async (name) => await LoadData());
            //GoToDetailsCommand = new Command<Type>(async (pageType) => await GoToDetails(pageType));
        }
        async Task LoadData()
        {
            LPersonalsS10ViewModel = new ObservableCollection<PersonalS10ViewModel>();
            var personalTareos = await App.Context.FilterItemsAsync<Tablas.Personal>("Personal", $"CodIdentificador = " + App.Tareador);

            foreach (var item in personalTareos)
                LPersonalsS10ViewModel.Add(new PersonalS10ViewModel(item));
        }

        //async Task GoToDetails(Type pageType)
        //{
        //    //if (ItemSeleccionado != null)
        //    //{
        //    //    var page = (Page)Activator.CreateInstance(pageType);
        //    //    page.BindingContext = new PersonalDetalleViewModel(ItemSeleccionado);
        //    //    await Navigation.PushAsync(page);

        //    //    ItemSeleccionado = null;
        //    //}
        //}

    }
}
