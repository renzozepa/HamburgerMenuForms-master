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
	public partial class Marcacion : ContentPage
	{
		public Marcacion ()
		{
			InitializeComponent ();
            BindingContext = new CustomOverlayViewModel(this.Navigation);
        }

        
	}
}