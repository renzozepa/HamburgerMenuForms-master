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
                return conn.Table<Tablas.Horario>().OrderBy(c => c.DESCRIPCION).ToList();
            }
        }
        private void SBBusqueda_TextChanged(object sender, TextChangedEventArgs e)
        {
            string variable = string.Empty;
            variable = SBBusqueda.Text;

            try
            {
                List<Tablas.Horario> ListaHorario = SearchHorario();

                for (int i = ListaHorario.Count - 1; i >= 0; i--)
                {
                    var item = ListaHorario[i];
                    if (!ListaHorario.Contains(item))
                    {
                        ListaHorario.Remove(item);
                    }
                }

                var Horariotareo = SearchHorario().Where(f => f.DESCRIPCION.Contains(variable.ToUpper())).OrderBy(o => o.DESCRIPCION).ToList();
                HorarioListView.ItemsSource = Horariotareo;
            }
            catch (Exception)
            {
                throw;
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