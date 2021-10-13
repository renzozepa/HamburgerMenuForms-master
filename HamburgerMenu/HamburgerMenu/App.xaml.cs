using HamburgerMenu.Data;
using HamburgerMenu.Vistas;
using Syncfusion.Licensing;
using System;
using System.IO;
using Xamarin.Forms;

namespace HamburgerMenu
{
    public partial class App : Application
	{
        public static string FilePath;
        public static string Tareador;
        public static Int32 TipoMarcacion;
        public static DateTime FMarcacion;
        public static Int32 Usuario;
        public static string Token;
        public static string Celular;
        public static string Sucursal;
        public static DateTime FExpiracion;

        private static DatabaseContext context;

        public static DatabaseContext Context
        {
            get
            {
                if (context == null)
                {
                    var dbPath = Path.Combine(
                        Environment.GetFolderPath(
                            System.Environment.SpecialFolder.Personal),
                        "Tareo.db3");

                    context = new DatabaseContext(dbPath);
                }

                return context;
            }
        }

        public App ()
		{            
            SyncfusionLicenseProvider.RegisterLicense("MjkwNjkxQDMxMzgyZTMxMmUzME5FUHcybWR6VmllZjZESzhKMVFGME5LVWp2cU5DblBqNkhPT0FabDdIWE09");
            InitializeComponent();
			MainPage = new NavigationPage(new Login()); 
		}
        public App(string filepath)
        {
            FilePath = filepath;
            SyncfusionLicenseProvider.RegisterLicense("MjkwNjkxQDMxMzgyZTMxMmUzME5FUHcybWR6VmllZjZESzhKMVFGME5LVWp2cU5DblBqNkhPT0FabDdIWE09");
            InitializeComponent();
            MainPage = new NavigationPage(new Login());                        
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
