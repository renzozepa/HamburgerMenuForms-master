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
	public partial class Tareo : ContentPage
	{
		public Tareo ()
		{
			InitializeComponent ();
		}
        protected override void OnAppearing()
        {
            base.OnAppearing();
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                //conn.CreateTable<TareoPersonal>();                
                var VarTareo = conn.Table<TareoPersonal>().ToList().Where(x => x.ID_TAREADOR == App.Tareador && x.SINCRONIZADO == 1);
                //PersonalListView.ItemsSource = contacts;
                TareoCollectionView.ItemsSource = VarTareo;
            }
        }
    }
}