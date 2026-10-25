using System.Windows.Input;
using PaniersFermiers.Models;
using PaniersFermiers.Services;

namespace PaniersFermiers.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private bool _cguAcceptees = false;
        private bool _estChargement = false;

        public ChampFormulaire Nom { get; } = new ChampFormulaire();
        public ChampFormulaire Email { get; } = new ChampFormulaire();
        public ChampFormulaire Telephone { get; } = new ChampFormulaire();
        public ChampFormulaire MotDePasse { get; } = new ChampFormulaire();
        public ChampFormulaire Confirmation { get; } = new ChampFormulaire();

        public bool CguAcceptees
        {
            get { return _cguAcceptees; }
            set
            {
                SetProperty(ref _cguAcceptees, value);
                OnPropertyChanged(nameof(PeutCreer));
            }
        }

        public bool EstChargement
        {
            get { return _estChargement; }
            set
            {
                SetProperty(ref _estChargement, value);
                OnPropertyChanged(nameof(FormulaireActif));
                OnPropertyChanged(nameof(PeutCreer));
                OnPropertyChanged(nameof(TexteBouton));
            }
        }

        public bool FormulaireActif
        {
            get { return !_estChargement; }
        }

        // Bouton grisé tant que les conditions ne sont pas cochées
        public bool PeutCreer
        {
            get { return _cguAcceptees && !_estChargement; }
        }

        public string TexteBouton
        {
            get { return _estChargement ? "Création en cours..." : "Créer mon compte →"; }
        }

        public ICommand CreerCompteCommand { get; }

        public RegisterViewModel()
        {
            CreerCompteCommand = new Command(CreerCompte);
        }

        private bool Valider()
        {
            bool ok = true;

            if (Nom.EstVide())
            {
                Nom.Erreur = "Ce champ est obligatoire.";
                ok = false;
            }

            if (Email.EstVide())
            {
                Email.Erreur = "Ce champ est obligatoire.";
                ok = false;
            }
            else if (!Outils.EmailValide(Email.Valeur.Trim()))
            {
                Email.Erreur = "Vérifiez le format de votre e-mail.";
                ok = false;
            }
            else if (AuthService.EmailExiste(Email.Valeur))
            {
                Email.Erreur = "Cette adresse e-mail est déjà utilisée.";
                ok = false;
            }

            if (Telephone.EstVide())
            {
                Telephone.Erreur = "Ce champ est obligatoire.";
                ok = false;
            }
            else if (!Outils.TelephoneValide(Telephone.Valeur))
            {
                Telephone.Erreur = "Numéro invalide (ex : 6 90 12 34 56).";
                ok = false;
            }
            else if (AuthService.TelephoneExiste(Telephone.Valeur))
            {
                Telephone.Erreur = "Ce numéro est déjà utilisé.";
                ok = false;
            }

            if (MotDePasse.EstVide())
            {
                MotDePasse.Erreur = "Ce champ est obligatoire.";
                ok = false;
            }
            else if (MotDePasse.Valeur.Length < 8)
            {
                MotDePasse.Erreur = "Au moins 8 caractères.";
                ok = false;
            }

            if (Confirmation.EstVide())
            {
                Confirmation.Erreur = "Ce champ est obligatoire.";
                ok = false;
            }
            else if (Confirmation.Valeur != MotDePasse.Valeur)
            {
                Confirmation.Erreur = "Les mots de passe ne correspondent pas.";
                ok = false;
            }

            return ok;
        }

        private async void CreerCompte()
        {
            if (!Valider()) return;

            EstChargement = true;
            await Task.Delay(1000);   // simulation d'un appel serveur

            Utilisateur u = new Utilisateur();
            u.NomComplet = Nom.Valeur.Trim();
            u.Email = Email.Valeur.Trim();
            u.Telephone = Outils.NettoyerTelephone(Telephone.Valeur);
            u.MotDePasse = MotDePasse.Valeur;
            AuthService.Inscrire(u);

            EstChargement = false;

            await Shell.Current.DisplayAlert("Compte créé", "Bienvenue " + u.NomComplet + " !", "Continuer");
            await Shell.Current.GoToAsync("//CataloguePage");
        }
    }
}