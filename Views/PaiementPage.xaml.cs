using PaniersFermiers.ViewModels;

namespace PaniersFermiers.Views
{
    public partial class PaiementPage : ContentPage
    {
        public PaiementPage()
        {
            InitializeComponent();
            BindingContext = new PaiementViewModel();
        }
    }
}
