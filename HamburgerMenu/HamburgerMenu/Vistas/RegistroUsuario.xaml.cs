using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HamburgerMenu.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistroUsuario : ContentPage
    {
        public RegistroUsuario()
        {
            InitializeComponent();
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<LoginLocal>();
            }
        }
        protected void Btn_agregar(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {

                IEnumerable<LoginLocal> resultado = ValidarUsuario(conn, Usuario.Text);
                if (resultado.Count() > 0)
                {
                    DisplayAlert("Tareo HAUG", "Usuario ya existente.", "Ok");
                }
                else
                {
                    if (string.Equals(Conf_Contrasenia.Text.ToString().Trim(), Contrasenia.Text.ToString().Trim()))
                    {
                        conn.CreateTable<LoginLocal>();
                        var DatosRegistro = new LoginLocal { NOMBRE = Nombre.Text, USUARIO = Usuario.Text, CONTRASENIA = Contrasenia.Text, TAREADOR = Tareador.Text, CELULAR = Celular.Text };
                        conn.Insert(DatosRegistro);

                        IEnumerable<LoginLocal> resultado_reg = ValidarUsuario(conn, Usuario.Text);
                        conn.CreateTable<Tablas.ConfiguracionLocal>();

                        List<LoginLocal> listll = (List<LoginLocal>)resultado_reg;
                        foreach (LoginLocal itemLoginLocal in listll)
                        {
                            var varConfigLocal = new Tablas.ConfiguracionLocal
                            {
                                ID_USUARIO = itemLoginLocal.ID,
                                LOCAL = false,
                                SERVER = false,
                                LOCALSERVER = true,
                                DISPOSITIVOZEBRA = false,
                                MINUTOSENTREMARCACION = 1
                            };
                            conn.Insert(varConfigLocal);
                        }
                        LimpiarFormulario();
                        App.Current.MainPage.Navigation.PushAsync(new Vistas.Login());
                    }
                    else
                    {
                        DisplayAlert("Tareo HAUG", "Ingresar la misma contraseña.", "Ok");
                    }
                }
            }
        }
        public static IEnumerable<LoginLocal> ValidarUsuario(SQLiteConnection db, string usuario)
        {
            return db.Query<LoginLocal>("Select * From LoginLocal where USUARIO = ? ", usuario);
        }
        void LimpiarFormulario()
        {
            Nombre.Text = string.Empty;
            Tareador.Text = string.Empty;
            Usuario.Text = string.Empty;
            Contrasenia.Text = string.Empty;
            Celular.Text = string.Empty;
            DisplayAlert("Tareo HAUG", "Se agregó correctamente", "Ok");
        }
    }
}