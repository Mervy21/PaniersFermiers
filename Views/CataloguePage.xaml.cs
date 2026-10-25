using PaniersFermiers.ViewModels;

namespace PaniersFermiers.Views
{
    public partial class CataloguePage : ContentPage
    {
        public CataloguePage()
        {
            InitializeComponent();
            BindingContext = new CatalogueViewModel();
        }

        // Appelée chaque fois que la page redevient visible (retour du panier, de la fiche produit...)
        protected override void OnAppearing()
        {
            base.OnAppearing();
            CatalogueViewModel vm = (CatalogueViewModel)BindingContext;
            vm.Actualiser();
        }
    }
}
