using HamburgerMenu.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HamburgerMenu
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PersonalS10Cesado : ContentPage
	{
        PersonalS10CesadoViewModel vm;

        public PersonalS10Cesado ()
		{
            InitializeComponent();
            vm = new PersonalS10CesadoViewModel(this.Navigation);
            BindingContext = vm;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.SearchByNameCommand.Execute(string.Empty);
        }
    }
}