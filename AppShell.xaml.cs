using PaniersFermiers.Views;

namespace PaniersFermiers
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(ReservationPage), typeof(ReservationPage));
            Routing.RegisterRoute(nameof(RetraitPage), typeof(RetraitPage));
            Routing.RegisterRoute(nameof(PaiementPage), typeof(PaiementPage));
            Routing.RegisterRoute(nameof(SuiviPage), typeof(SuiviPage));
            Routing.RegisterRoute(nameof(ProduitPage), typeof(ProduitPage));
            Routing.RegisterRoute(nameof(ProfilPage), typeof(ProfilPage));   // NOUVEAU

            // NOUVEAU : si l'utilisateur était déjà connecté, on ouvre directement le catalogue
            Loaded += async (s, e) =>
            {
                if (Services.AuthService.RestaurerSession())
                {
                    await GoToAsync("//CataloguePage");
                }
            };

        }
    }
}
