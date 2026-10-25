using PaniersFermiers.ViewModels;

namespace PaniersFermiers.Views
{
    public partial class ProduitPage : ContentPage
    {
        public ProduitPage()
        {
            InitializeComponent();
            BindingContext = new ProduitViewModel();
        }
    }
}
