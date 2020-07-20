using HamburgerMenu.Tablas;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		}
        protected override void OnAppearing()
        {
            base.OnAppearing();
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                //conn.CreateTable<TareoPersonal>();                
                var contacts = conn.Table<TareoPersonal>().ToList().Where(x => x.ID_TAREADOR == App.Tareador && x.SINCRONIZADO == 0);
                //PersonalListView.ItemsSource = contacts;
                TareoCollectionView.ItemsSource = contacts;
            }
        }
    }
}