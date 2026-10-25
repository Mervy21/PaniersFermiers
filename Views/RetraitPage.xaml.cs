using PaniersFermiers.ViewModels;

namespace PaniersFermiers.Views
{
    public partial class RetraitPage : ContentPage
    {
        public RetraitPage()
        {
            InitializeComponent();
            BindingContext = new RetraitViewModel();
        }
    }
}
