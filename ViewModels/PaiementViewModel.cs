using System.Windows.Input;
using PaniersFermiers.Models;
using PaniersFermiers.Services;

namespace PaniersFermiers.ViewModels
{
    public class PaiementViewModel : BaseViewModel
    {
        private bool _especes = true;
        private bool _estChargement = false;
        private string _messageErreur = "";

        public string LieuTexte { get; }
        public string CreneauTexte { get; }
        public string TotalTexte { get; }
        public string NombrePaniersTexte { get; }

        public bool Especes
        {
            get { return _especes; }
            set
            {
                SetProperty(ref _especes, value);
                OnPropertyChanged(nameof(MobileMoney));
            }
        }

        public bool MobileMoney { get { return !_especes; } }

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

        public bool FormulaireActif { get { return !_estChargement; } }

        public string TexteBouton
        {
            get { return _estChargement ? "Enregistrement..." : "Confirmer ma commande"; }
        }

        public string MessageErreur
        {
            get { return _messageErreur; }
            set
            {
                SetProperty(ref _messageErreur, value);
                OnPropertyChanged(nameof(AErreur));
            }
        }

        public bool AErreur { get { return _messageErreur != ""; } }

        public ICommand ChoisirEspecesCommand { get; }
        public ICommand ChoisirMobileMoneyCommand { get; }
        public ICommand ConfirmerCommand { get; }

        public PaiementViewModel()
        {
            PointRetrait? point = CommandeService.PointChoisi;
            Creneau? creneau = CommandeService.CreneauChoisi;

            LieuTexte = "";
            CreneauTexte = "";
            if (point != null)
            {
                LieuTexte = point.Nom;
            }
            if (point != null && creneau != null)
            {
                CreneauTexte = point.Jour + " • " + creneau.libelle;
            }

            TotalTexte = Outils.Fcfa(CommandeService.Total);
            int n = CommandeService.NombreArticles;
            NombrePaniersTexte = n + (n > 1 ? " produits" : " produit") + " • " + Outils.Kg(CommandeService.PoidsTotal);


            ChoisirEspecesCommand = new Command(ChoisirEspeces);
            ChoisirMobileMoneyCommand = new Command(ChoisirMobileMoney);
            ConfirmerCommand = new Command(Confirmer);
        }

        private void ChoisirEspeces()
        {
            Especes = true;
        }

        private void ChoisirMobileMoney()
        {
            Especes = false;
        }

        private async void Confirmer()
        {
            MessageErreur = "";
            EstChargement = true;
            try
            {
                await Task.Delay(1000);   // simulation de l'enregistrement
                CommandeService.ModePaiement = _especes ? "Espèces / Monnaie exacte" : "Orange Money / Mobile Money";
                Commande commande = CommandeService.ConfirmerCommande();
                EstChargement = false;

                await Shell.Current.DisplayAlert("Commande confirmée",
                    "Référence : " + commande.Reference + "\nVous réglerez au moment du retrait.",
                    "Voir le suivi");

                // On repart du catalogue puis on ouvre le suivi (retour = catalogue)
                await Shell.Current.GoToAsync("//CataloguePage");
                await Shell.Current.GoToAsync(nameof(Views.SuiviPage));
            }
            catch (Exception)
            {
                EstChargement = false;
                MessageErreur = "La commande n'a pas pu être enregistrée. Votre panier est conservé.";
            }
        }
    }
}
