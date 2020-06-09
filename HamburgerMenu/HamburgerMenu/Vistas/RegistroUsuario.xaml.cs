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
                conn.CreateTable<LoginLocal>();
                var DatosRegistro = new LoginLocal { NOMBRE = Nombre.Text, USUARIO = Usuario.Text, CONTRASENIA = Contrasenia.Text , TAREADOR = Tareador.Text };
                conn.Insert(DatosRegistro);
                LimpiarFormulario();
            }            
        }
        void LimpiarFormulario()
        {
            Nombre.Text = string.Empty;
            Tareador.Text = string.Empty;
            Usuario.Text = string.Empty;
            Contrasenia.Text = string.Empty;
            DisplayAlert("Tareo HAUG", "Se agregó correctamente", "Ok");
        }
    }
}