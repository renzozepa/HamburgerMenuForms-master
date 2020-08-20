using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HamburgerMenu.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btn_login(object sender, EventArgs e)
        {
            try
            {                
                var db = new SQLiteConnection(App.FilePath);
                IEnumerable<LoginLocal> resultado = ConsultarUsuario(db, usuario.Text, contra.Text);
                if (resultado.Count() > 0)
                {
                    List<LoginLocal> listll = (List<LoginLocal>)resultado;
                    foreach (LoginLocal itemLoginLocal in listll)
                    {
                        App.Tareador = itemLoginLocal.TAREADOR.ToString();
                        App.Usuario = Convert.ToInt32(itemLoginLocal.ID.ToString());
                        if (!string.IsNullOrEmpty(itemLoginLocal.TOKEN))
                        {
                            App.Token = itemLoginLocal.TOKEN.ToString();
                            App.FExpiracion = itemLoginLocal.FECHA_VIGENCIA.Date;
                        }
                        
                        if (!string.IsNullOrEmpty(itemLoginLocal.CELULAR))
                        {
                            App.Celular = itemLoginLocal.CELULAR.ToString();
                        }                        
                    }
                    
                    Navigation.PushAsync(new HamburgerMenu());
                }
                else
                {
                    DisplayAlert("Tareo HAUG", "Verifique su usuario/contraseña", "Ok");
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message.ToString(),"Ok");
            }
            
        }
        private void btn_Registrar(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegistroUsuario());
        }
        public static IEnumerable<LoginLocal> ConsultarUsuario(SQLiteConnection db, string usuario, string contra)
        {
            db.CreateTable<LoginLocal>();
            return db.Query<LoginLocal>("Select * From LoginLocal where USUARIO = ? and CONTRASENIA = ?", usuario, contra);
        }

        private void Btn_Listar(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ConsultaRegistro());
        }
    }
}