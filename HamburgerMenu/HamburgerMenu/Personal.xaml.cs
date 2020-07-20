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
    public partial class Personal : ContentPage
    {
        public Personal()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            PersonalListView.ItemsSource = SearchEmployers();
            //PersonalCollectionView.ItemsSource = SearchEmployers();
        }

        private void SBBusqueda_TextChanged(object sender, TextChangedEventArgs e)
        {
            string variable = string.Empty;
            variable = SBBusqueda.Text;

            try
            {
                var personaltareo = SearchEmployers().Where(f => f.NOMBRE.Contains(variable.ToUpper())).OrderBy(o => o.NOMBRE).ToList();                
                PersonalListView.ItemsSource = SearchEmployers();
                //PersonalCollectionView.ItemsSource = personaltareo;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<PersonalTareo> SearchEmployers()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                //conn.CreateTable<PersonalTareo>();
                return conn.Table<PersonalTareo>().Where(x => x.ID_TAREADOR == App.Tareador).OrderBy(c => c.NOMBRE).ToList();
            }

        }

        private void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                Navigation.PushAsync(new PersonalDetalle
                {
                    BindingContext = e.SelectedItem as PersonalTareo
                });
            }
        }
    }
}