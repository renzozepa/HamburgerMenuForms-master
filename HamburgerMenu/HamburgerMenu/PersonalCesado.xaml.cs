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
	public partial class PersonalCesado : ContentPage
	{
		public PersonalCesado ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            PersonalListView.ItemsSource = SearchEmployers();
        }

        private void SBBusqueda_TextChanged(object sender, TextChangedEventArgs e)
        {
            string variable = string.Empty;
            variable = SBBusqueda.Text;

            try
            {
                List<Tablas.PersonalTareo> ListaPersonalTareo = SearchEmployers();

                for (int i = ListaPersonalTareo.Count - 1; i >= 0; i--)
                {
                    var item = ListaPersonalTareo[i];
                    if (!ListaPersonalTareo.Contains(item))
                    {
                        ListaPersonalTareo.Remove(item);
                    }
                }

                var personaltareo = SearchEmployers().Where(f => f.NOMBRE.Contains(variable.ToUpper())).OrderBy(o => o.NOMBRE).ToList();
                PersonalListView.ItemsSource = personaltareo;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Tablas.PersonalTareo> SearchEmployers()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                //conn.CreateTable<PersonalTareo>();
                return conn.Table<Tablas.PersonalTareo>().Where(x => x.ID_TAREADOR == App.Tareador && x.ID_SITUACION > 10).OrderBy(c => c.NOMBRE).ToList();
            }

        }

        private void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                Navigation.PushAsync(new PersonalDetalle
                {
                    BindingContext = e.SelectedItem as Tablas.PersonalTareo
                });
            }
        }
    }
}