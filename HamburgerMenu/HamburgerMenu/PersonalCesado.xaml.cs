using HamburgerMenu.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HamburgerMenu
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PersonalCesado : ContentPage
	{
        PersonalCesadoListViewModel vm;
        public PersonalCesado ()
		{
			InitializeComponent ();
            vm = new PersonalCesadoListViewModel(this.Navigation);
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.SearchByNameCommand.Execute(string.Empty);
        }
    }
}