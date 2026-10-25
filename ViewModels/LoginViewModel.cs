using System.Windows.Input;
using PaniersFermiers.Services;
using PaniersFermiers.Views;

namespace PaniersFermiers.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private bool _estChargement = false;
        private bool _motDePasseCache = true;
        private string _erreurGenerale = "";

        public ChampFormulaire Identifiant { get; } = new ChampFormulaire();
        public ChampFormulaire MotDePasse { get; } = new ChampFormulaire();

        public string ErreurGenerale
        {
            get { return _erreurGenerale; }
            set
            {
                SetProperty(ref _erreurGenerale, value);
                OnPropertyChanged(nameof(AErreurGenerale));
            }
        }

        public bool AErreurGenerale
        {
            get { return _erreurGenerale != ""; }
        }

        public bool EstChargement
        {
            get { return _estChargement; }
            set
            {
                SetProperty(ref _estChargement, value);
                OnPropertyChanged(nameof(FormulaireActif));
                OnPropertyChanged(nameof(TexteBouton));
            }
        }

        // Champs et bouton grisés pendant le chargement
        public bool FormulaireActif
        {
            get { return !_estChargement; }
        }

        public string TexteBouton
        {
            get { return _estChargement ? "Connexion en cours..." : "Se connecter"; }
        }

        public bool MotDePasseCache
        {
            get { return _motDePasseCache; }
            set
            {
                SetProperty(ref _motDePasseCache, value);
                OnPropertyChanged(nameof(TexteAffichage));
            }
        }

        public string TexteAffichage
        {
            get { return _motDePasseCache ? "Afficher" : "Masquer"; }
        }

        public ICommand SeConnecterCommand { get; }
        public ICommand BasculerMotDePasseCommand { get; }
        public ICommand MotDePasseOublieCommand { get; }
        public ICommand InscriptionCommand { get; }

        public LoginViewModel()
        {
            SeConnecterCommand = new Command(SeConnecter);
            BasculerMotDePasseCommand = new Command(BasculerMotDePasse);
            MotDePasseOublieCommand = new Command(MotDePasseOublie);
            InscriptionCommand = new Command(AllerInscription);
        }

        private async void SeConnecter()
        {
            // 1. On efface les anciens messages
            Identifiant.Erreur = "";
            MotDePasse.Erreur = "";
            ErreurGenerale = "";

            // 2. Validation des champs
            bool valide = true;
            if (Identifiant.EstVide())
            {
                Identifiant.Erreur = "Ce champ est obligatoire.";
                valide = false;
            }
            if (MotDePasse.EstVide())
            {
                MotDePasse.Erreur = "Ce champ est obligatoire.";
                valide = false;
            }
            if (!valide) return;

            // 3. Chargement (simulation d'un appel serveur)
            EstChargement = true;
            await Task.Delay(800);
            bool ok = AuthService.Connecter(Identifiant.Valeur, MotDePasse.Valeur);
            EstChargement = false;

            // 4. Résultat
            if (ok)
            {
                MotDePasse.Valeur = "";
                await Shell.Current.GoToAsync("//CataloguePage");
            }
            else
            {
                ErreurGenerale = "Identifiant ou mot de passe incorrect.";
            }
        }

        private void BasculerMotDePasse()
        {
            MotDePasseCache = !MotDePasseCache;
        }

        private async void MotDePasseOublie()
        {
            await Shell.Current.DisplayAlert("Mot de passe oublié",
                "Un lien de réinitialisation sera envoyé à votre adresse e-mail.",
                "OK");
        }

        private async void AllerInscription()
        {
            await Shell.Current.GoToAsync(nameof(RegisterPage));
        }
    }
}