using HamburgerMenu.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HamburgerMenu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Personal : ContentPage
    {
        PersonalListViewModel vm;
        public Personal()
        {
            InitializeComponent();
            vm = new PersonalListViewModel(this.Navigation);
            BindingContext = vm;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.SearchByNameCommand.Execute(string.Empty);
        }
    }
}