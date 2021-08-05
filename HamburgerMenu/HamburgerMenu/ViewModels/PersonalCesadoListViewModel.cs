using HamburgerMenu.Tablas;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HamburgerMenu.ViewModels
{
    public class PersonalCesadoListViewModel : BaseViewModel
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


        private ObservableCollection<PersonalViewModel> _personalViewModel;
        public ObservableCollection<PersonalViewModel> LPersonalsViewModel
        {
            get { return _personalViewModel; }
            set
            {
                _personalViewModel = value;
                OnPropertyChanged();
            }
        }

        private PersonalViewModel _itemSeleccionado;
        public PersonalViewModel ItemSeleccionado
        {
            get { return _itemSeleccionado; }
            set { _itemSeleccionado = value; OnPropertyChanged(); }
        }

        public PersonalCesadoListViewModel(INavigation navigation)
        {
            Navigation = navigation;

            RefreshCommand = new Command(async () =>
            {
                IsRefreshing = true;
                await LoadData();
                IsRefreshing = false;
            });

            SearchByNameCommand = new Command<string>(async (name) => await LoadData());
            GoToDetailsCommand = new Command<Type>(async (pageType) => await GoToDetails(pageType));
        }

        async Task LoadData()
        {
            LPersonalsViewModel = new ObservableCollection<PersonalViewModel>();
            var personalTareos = await App.Context.FilterItemsAsync<PersonalTareo>("PersonalTareo", $"ID_SITUACION > 10");

            foreach (var item in personalTareos)
                LPersonalsViewModel.Add(new PersonalViewModel(item));
        }

        async Task GoToDetails(Type pageType)
        {
            if (ItemSeleccionado != null)
            {
                var page = (Page)Activator.CreateInstance(pageType);
                page.BindingContext = new PersonalDetalleViewModel(ItemSeleccionado);
                await Navigation.PushAsync(page);

                ItemSeleccionado = null;
            }
        }
    }
}
