using PaniersFermiers.ViewModels;

namespace PaniersFermiers.Views
{
    public partial class SuiviPage : ContentPage
    {
        public SuiviPage()
        {
            InitializeComponent();
            BindingContext = new SuiviViewModel();
        }
    }
}
