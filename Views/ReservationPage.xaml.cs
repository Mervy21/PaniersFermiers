using PaniersFermiers.ViewModels;

namespace PaniersFermiers.Views
{
    public partial class ReservationPage : ContentPage
    {
        public ReservationPage()
        {
            InitializeComponent();
            BindingContext = new ReservationViewModel();
        }
    }
}
