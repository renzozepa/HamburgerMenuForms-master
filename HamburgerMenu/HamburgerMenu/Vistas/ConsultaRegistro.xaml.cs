using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HamburgerMenu.Vistas
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConsultaRegistro : ContentPage
	{
		public ConsultaRegistro ()
		{
			InitializeComponent ();
		}

        void NewContactToolbarItem_Clicked(object sender, System.EventArgs e) => Navigation.PushAsync(new RegistroUsuario());
        protected override void OnAppearing()
        {
            base.OnAppearing();
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<LoginLocal>();
                var contacts = conn.Table<LoginLocal>().ToList();
                contactsListView.ItemsSource = contacts;
            }
        }
    }
}