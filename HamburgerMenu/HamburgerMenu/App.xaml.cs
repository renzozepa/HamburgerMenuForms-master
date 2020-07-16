using HamburgerMenu.Vistas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace HamburgerMenu
{
	public partial class App : Application
	{
        public static string FilePath;
        public static string Tareador;
        public static Int32 TipoMarcacion;
        public static Int32 Usuario;
        public App ()
		{
            InitializeComponent();
			MainPage = new NavigationPage(new Login()); 
		}
        public App(string filepath)
        {
            InitializeComponent();
            MainPage = new NavigationPage(new Login());            
            FilePath = filepath;
        }

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
