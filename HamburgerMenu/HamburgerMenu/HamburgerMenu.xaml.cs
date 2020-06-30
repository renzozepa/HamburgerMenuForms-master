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
                new Menu{ Page= new MiPerfil(),MenuTitle="Mi perfil",  MenuDetail="Mi perfil",icon="user.png"},
                new Menu{ Page= new Sincronizar(),MenuTitle="Sincronizar",  MenuDetail="Sincronizar",icon="actualizar.png"},
                new Menu{ Page= new Proyectos(),MenuTitle="Proyectos",  MenuDetail="Proyectos disponibles",icon="settings.png"},
                new Menu{ Page= new Personal(),MenuTitle="Personal disponible",  MenuDetail="Personal disponible",icon="user.png"},
                new Menu{ Page= new Tareo(),MenuTitle="Tareo",  MenuDetail="Tareo",icon="ic_fingerprint.png"},
                new Menu{ Page= new TipoMarcacion(),MenuTitle="Marcación",  MenuDetail="Marcación",icon="message.png"},
                new Menu{ Page= new Vistas.Login(),MenuTitle="Cerrar sesión",  MenuDetail="Cerrar sesión",icon="salir.png"}
            };
            ListMenu.ItemsSource = menu;
        }

        private void ListMenu_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var menu = e.SelectedItem as Menu;
            if (menu != null)
            {
                IsPresented = false;
                Detail = new NavigationPage(menu.Page);
            }
        }

        public class Menu
        {
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