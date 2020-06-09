﻿using SQLite;
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
                IEnumerable<LoginLocal> resultado = SELECT_WHERE(db, usuario.Text, contra.Text);
                if (resultado.Count() > 0)
                {
                    List<LoginLocal> listll = (List<LoginLocal>)resultado;
                    foreach (LoginLocal itemLoginLocal in listll)
                    {
                        App.Tareador = itemLoginLocal.TAREADOR.ToString();
                    }
                    
                    Navigation.PushAsync(new HamburgerMenu());
                }
                else
                {
                    DisplayAlert("Tareo HAUG", "Verifique su usuario/contraseñ", "Ok");
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
        public static IEnumerable<LoginLocal> SELECT_WHERE(SQLiteConnection db, string usuario, string contra)
        {
            return db.Query<LoginLocal>("SELECT * FROM LoginLocal where USUARIO = ? and CONTRASENIA = ?", usuario, contra);
        }

        private void Btn_Listar(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ConsultaRegistro());
        }
    }
}