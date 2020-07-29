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
            DpFechaTransac.Date = DateTime.Now.Date;
        }

        private void Btn_Ingreso(object sender, EventArgs e)
        {
            App.TipoMarcacion = 1;
            MarcarFecha();
            Navigation.PushAsync(new Marcacion());
        }

        private void Btn_SalidaAlmorzar(object sender, EventArgs e)
        {
            App.TipoMarcacion = 2;
            MarcarFecha();
            Navigation.PushAsync(new Marcacion());
        }

        private void Btn_Ingresoalmorzar(object sender, EventArgs e)
        {
            App.TipoMarcacion = 3;
            MarcarFecha();
            Navigation.PushAsync(new Marcacion());
        }
        private void Btn_Salida(object sender, EventArgs e)
        {
            App.TipoMarcacion = 4;
            MarcarFecha();
            Navigation.PushAsync(new Marcacion());
        }

        private void CbCambioFecha_StateChanged(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {
            if (CbCambioFecha.IsChecked == true)
            {
                DpFechaTransac.IsEnabled = true;
            }
            else {
                DpFechaTransac.IsEnabled = false;
            }
        }

        private void MarcarFecha()
        {
            if (CbCambioFecha.IsChecked == true)
            {
                App.FMarcacion = DpFechaTransac.Date;
            }
            else
            {
                App.FMarcacion = DateTime.Now.Date;
            }
        }
    }
}