using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HamburgerMenu.Tablas;
using SQLite;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HamburgerMenu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Configuracion : ContentPage
    {
        public Configuracion()
        {
            InitializeComponent();
        }

        private void AlmLocal_Toggled(object sender, ToggledEventArgs e)
        {
            if (AlmLocal.IsToggled)
            {
                AlmServer.IsToggled = false;
                AlmLocalServer.IsToggled = false;
            }
        }

        private void AlmServer_Toggled(object sender, ToggledEventArgs e)
        {
            if (AlmServer.IsToggled)
            {
                AlmLocal.IsToggled = false;
                AlmLocalServer.IsToggled = false;
            }
        }

        private void AlmLocalServer_Toggled(object sender, ToggledEventArgs e)
        {
            if (AlmLocalServer.IsToggled)
            {
                AlmLocal.IsToggled = false;
                AlmServer.IsToggled = false;
            }
        }

        private void Btn_Actualizar(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<Configuracion>();
                var objconfiguracion = conn.Table<ConfiguracionLocal>().FirstOrDefault();

                if (objconfiguracion == null)
                {
                    objconfiguracion.LOCAL = false;
                    objconfiguracion.SERVER = false;
                    objconfiguracion.LOCALSERVER = false;
                    conn.Insert(objconfiguracion);
                }

                objconfiguracion.LOCAL = AlmLocal.IsToggled;
                objconfiguracion.SERVER = AlmServer.IsToggled;
                objconfiguracion.LOCALSERVER = AlmLocalServer.IsToggled;

                conn.Update(objconfiguracion);
            }
        }
    }
}