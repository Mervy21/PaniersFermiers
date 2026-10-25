using PaniersFermiers.ViewModels;
namespace PaniersFermiers.Views
{
    public partial class RegisterPage: ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
            BindingContext = new RegisterViewModel();
        }
    } 
}