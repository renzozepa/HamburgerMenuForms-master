using SQLite;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HamburgerMenu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HamburgerMenu : MasterDetailPage
    {
        public HamburgerMenu()
        {
            InitializeComponent();
            MyMenu();            
        }
        public void MyMenu()
        {
            Detail = new NavigationPage(new Feed());
            List<Menu> menu = new List<Menu>
            {
                new Menu{ Page= new MiPerfil(),MenuTitle="Mi perfil",  MenuDetail="Mi perfil",icon="user.png",Id=1},
                new Menu{ Page= new Sincronizar(),MenuTitle="Sincronizar",  MenuDetail="Sincronizar",icon="actualizar.png",Id=2},
                //new Menu{ Page= new Proyectos(),MenuTitle="Proyectos",  MenuDetail="Proyectos disponibles",icon="settings.png",Id=3},
                //new Menu{ Page= new Horarios(),MenuTitle="Horarios",  MenuDetail="Horarios disponibles",icon="settings.png",Id=4},
                new Menu{ Page= new PersonalDisponible(),MenuTitle="Personal disponible",  MenuDetail="Personal disponible",icon="user.png",Id=5},
                new Menu{ Page= new Tareos(),MenuTitle="Tareo",  MenuDetail="Tareo",icon="ic_fingerprint.png",Id=6},
                new Menu{ Page= new TipoMarcacion(),MenuTitle="Marcación",  MenuDetail="Marcación",icon="message.png",Id=7},
                new Menu{ Page= new Configuracion(),MenuTitle="Configuración",  MenuDetail="Configuración",icon="ic_build.png",Id=8},
                new Menu{ Page= new Vistas.Login(),MenuTitle="Cerrar sesión",  MenuDetail="Cerrar sesión",icon="salir.png",Id=9}                
            };
            ListMenu.ItemsSource = menu;
            
        }

        private void ListMenu_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            
            var menu = e.SelectedItem as Menu;
            if (menu != null)
            {
                if (menu.Id == 9)
                {
                    App.Token = null;
                }
                if (App.Token != null)
                {
                    IsPresented = false;
                    Detail = new NavigationPage(menu.Page);
                }
                else {
                    if (menu.Id == 1 || menu.Id == 2 || menu.Id == 9)
                    {
                        IsPresented = false;
                        Detail = new NavigationPage(menu.Page);
                    }
                }                                   
            }
        }

        public class Menu
        {
            public int Id
            {
                get;
                set;
            }
            public string MenuTitle
            {
                get;
                set;
            }
            public string MenuDetail
            {
                get;
                set;
            }

            public ImageSource icon
            {
                get;
                set;
            }

            public Page Page
            {
                get;
                set;
            }
        }
    }
}