using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HamburgerMenu.ViewModels;

namespace HamburgerMenu
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MarcaciónZebra : ContentPage
	{
		public MarcaciónZebra ()
		{
			InitializeComponent ();
            BindingContext = new MarcacionZebraViewModel(this.Navigation, CodMarcacion);
        }
        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    CodMarcacion.Focus();
        //}
        protected async override void OnAppearing()
        {
            await Task.Run(() =>
            {
                Task.Delay(100);
                Device.BeginInvokeOnMainThread(() =>
                {
                    CodMarcacion.Focus();
                });
            });
        }

    }
}