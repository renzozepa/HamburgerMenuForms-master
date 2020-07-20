using SQLite;
using System;
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
        }
        protected void Btn_agregar(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                if (string.Equals(Conf_Contrasenia.Text.ToString().Trim(),Contrasenia.Text.ToString().Trim()))
                {
                    conn.CreateTable<LoginLocal>();
                    var DatosRegistro = new LoginLocal { NOMBRE = Nombre.Text, USUARIO = Usuario.Text, CONTRASENIA = Contrasenia.Text, TAREADOR = Tareador.Text, CELULAR = Celular.Text };
                    conn.Insert(DatosRegistro);
                    LimpiarFormulario();
                }
                else {
                    DisplayAlert("Tareo HAUG", "Ingresar la misma contraseña.", "Ok");
                }
                
            }            
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