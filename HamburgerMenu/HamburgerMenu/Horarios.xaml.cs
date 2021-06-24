using SQLite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HamburgerMenu
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Horarios : ContentPage
	{
		public Horarios ()
		{
			InitializeComponent ();
		}
        protected override void OnAppearing()
        {
            base.OnAppearing();
            HorarioListView.ItemsSource = SearchHorario();
        }
        public List<Tablas.Horario> SearchHorario()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                return conn.Table<Tablas.Horario>().OrderBy(c => c.ID_HORARIO).ToList();
            }
        }        
        private void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //if (e.SelectedItem != null)
            //{
            //    Navigation.PushAsync(new PersonalDetalle
            //    {
            //        BindingContext = e.SelectedItem as Tablas.PersonalTareo
            //    });
            //}
        }
    }
}