using HamburgerMenu.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HamburgerMenu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TareoPorSincronizar : ContentPage
	{
		public TareoPorSincronizar ()
		{
			InitializeComponent ();
            BindingContext = new TareoPorSincronizarViewModel(this.Navigation);
		}
        protected override void OnAppearing()
        {
            //base.OnAppearing();
            //using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            //{
            //    //conn.CreateTable<TareoPersonal>();                
            //    var ListTareoPorSincronizar = conn.Table<TareoPersonal>().ToList().Where(x => x.ID_TAREADOR == App.Tareador && x.SINCRONIZADO == 0);
            //    //PersonalListView.ItemsSource = contacts;
            //    TareoCollectionView.ItemsSource = ListTareoPorSincronizar;
            //}
        }
    }
}