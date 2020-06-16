using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HamburgerMenu
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TipoMarcacion : ContentPage
	{
		public TipoMarcacion ()
		{
			InitializeComponent ();
		}

        private void Btn_Ingreso(object sender, EventArgs e)
        {
            App.TipoMarcacion = 1;
            Navigation.PushAsync(new Marcacion());
        }

        private void Btn_SalidaAlmorzar(object sender, EventArgs e)
        {
            App.TipoMarcacion = 2;
            Navigation.PushAsync(new Marcacion());
        }

        private void Btn_Ingresoalmorzar(object sender, EventArgs e)
        {
            App.TipoMarcacion = 3;
            Navigation.PushAsync(new Marcacion());
        }
        private void Btn_Salida(object sender, EventArgs e)
        {
            App.TipoMarcacion = 4;
            Navigation.PushAsync(new Marcacion());
        }

        
    }
}