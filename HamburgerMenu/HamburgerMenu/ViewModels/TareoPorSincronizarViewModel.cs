using HamburgerMenu.Tablas;
using SQLite;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HamburgerMenu.ViewModels
{
    public class TareoPorSincronizarViewModel : BaseViewModel
    {
        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }
        public ICommand RefreshCommand { get; set; }
        public INavigation Navigation { get; set; }
        public ObservableCollection<TareoPersonalS10> _tareoXSincronizar;
        public ObservableCollection<TareoPersonalS10> TareoXSincronizar
        {
            get { return _tareoXSincronizar; }
            set { _tareoXSincronizar = value; OnPropertyChanged(); }
        }
        public TareoPorSincronizarViewModel(INavigation navigation)
        {
            
            TareoPersonalXSincronizar();

            Navigation = navigation;

            RefreshCommand = new Command(async () =>
            {
                IsRefreshing = true;
                await Task.Delay(3000);
                TareoPersonalXSincronizar();
                IsRefreshing = false;
            });
        }

        public void TareoPersonalXSincronizar()
        {
            TareoXSincronizar = new ObservableCollection<TareoPersonalS10>();

            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<TareoPersonal>();
                var LTareoPorSincronizar = conn.Table<TareoPersonalS10>().ToList().Where(x => x.ID_TAREADOR == App.Tareador && x.SINCRONIZADO == 0);
                foreach (var item in LTareoPorSincronizar)
                    TareoXSincronizar.Add(item);
            }
        }
    }
}
