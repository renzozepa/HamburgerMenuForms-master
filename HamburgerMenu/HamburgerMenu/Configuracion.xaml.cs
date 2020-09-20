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
            CargaInicial();                        
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
        public void CargaInicial()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<ConfiguracionLocal>();
                IEnumerable<ConfiguracionLocal> VarConfiguracionLocal = ValidarExistenciaConfiguracion(conn);
                if (VarConfiguracionLocal.Count() > 0)
                {
                    var objconfiguracion = conn.Table<ConfiguracionLocal>().FirstOrDefault(u => u.ID_USUARIO == App.Usuario);
                    if (objconfiguracion == null)
                    {

                    }

                    AlmLocal.IsToggled = objconfiguracion.LOCAL;
                    AlmServer.IsToggled = objconfiguracion.SERVER;
                    AlmLocalServer.IsToggled = objconfiguracion.LOCALSERVER;
                    DispositivoZebra.IsToggled = objconfiguracion.DISPOSITIVOZEBRA;
                    
                }                
            }
        }
        private void Btn_Actualizar(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                IEnumerable<ConfiguracionLocal> VarConfiguracionLocal = ValidarExistenciaConfiguracion(conn);
                if (VarConfiguracionLocal.Count() > 0)
                {
                    var objconfiguracion = conn.Table<ConfiguracionLocal>().FirstOrDefault(u => u.ID_USUARIO == App.Usuario);
                    if (objconfiguracion == null)
                    {

                    }

                    objconfiguracion.LOCAL = AlmLocal.IsToggled;
                    objconfiguracion.SERVER = AlmServer.IsToggled;
                    objconfiguracion.LOCALSERVER = AlmLocalServer.IsToggled;
                    objconfiguracion.DISPOSITIVOZEBRA = DispositivoZebra.IsToggled;

                    conn.Update(objconfiguracion);
                    DisplayAlert("Haug Tareo","Se actualizo correctamente los datos.","Ok");
                }
                else
                {                    
                    var DatosRegistro = new ConfiguracionLocal { ID_USUARIO = App.Usuario, LOCAL = false, SERVER = false, LOCALSERVER = false };
                    conn.Insert(DatosRegistro);
                }
            }
        }
        public static IEnumerable<ConfiguracionLocal> ValidarExistenciaConfiguracion(SQLiteConnection db)
        {
            return db.Query<ConfiguracionLocal>("Select * From ConfiguracionLocal WHERE ID_USUARIO = ?", App.Usuario);
        }
    }
}