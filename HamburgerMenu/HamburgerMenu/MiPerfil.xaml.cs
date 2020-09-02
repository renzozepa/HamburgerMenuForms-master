using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HamburgerMenu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MiPerfil : ContentPage
    {
        public MiPerfil()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            FindUsuarioId();
        }

        public void FindUsuarioId()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<PersonalTareo>();
                var perfil = conn.Table<LoginLocal>().FirstOrDefault(j => j.ID == App.Usuario);

                if (perfil == null)
                {
                    throw new Exception("Usuario no encontrado en la base de datos!");
                }

                Nombre.Text = perfil.NOMBRE.ToString();
                Usuario.Text = perfil.USUARIO.ToString();
                Contrasenia.Text = perfil.CONTRASENIA.ToString();
                Tareador.Text = perfil.TAREADOR.ToString();
                if (!string.IsNullOrEmpty(perfil.CELULAR))
                {
                    Celular.Text = perfil.CELULAR.ToString();
                }

                if (!string.IsNullOrEmpty(perfil.TOKEN))
                {
                    Token.Text = perfil.TOKEN.ToString();
                }
                
                FToken.Text = perfil.FECHA_VIGENCIA.ToString("dd/MM/yyyy");
                
            }
        }

        private void BtnActualizar_Clicked(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<PersonalTareo>();
                var perfil = conn.Table<LoginLocal>().FirstOrDefault(j => j.ID == App.Usuario);

                if (perfil == null)
                {
                    throw new Exception("Usuario no encontrado en la base de datos!");
                }

                perfil.NOMBRE = Nombre.Text.ToString();
                perfil.CONTRASENIA = Contrasenia.Text.ToString();
                perfil.TAREADOR = Tareador.Text.ToString();
                perfil.CELULAR = Celular.Text.ToString();

                conn.Update(perfil);
            }
        }
    }
}