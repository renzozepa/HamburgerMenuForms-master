using HamburgerMenu.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HamburgerMenu
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PersonalS10 : ContentPage
	{
        PersonalS10ListViewModel vm;
        public PersonalS10 ()
		{
			InitializeComponent ();
            vm = new PersonalS10ListViewModel(this.Navigation);
            BindingContext = vm;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.SearchByNameCommand.Execute(string.Empty);
        }
    }
}