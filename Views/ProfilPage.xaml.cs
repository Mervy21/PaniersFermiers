using PaniersFermiers.ViewModels;

namespace PaniersFermiers.Views
{
    public partial class ProfilPage : ContentPage
    {
        public ProfilPage()
        {
            InitializeComponent();
            BindingContext = new ProfilViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((ProfilViewModel)BindingContext).Actualiser();
        }
    }
}