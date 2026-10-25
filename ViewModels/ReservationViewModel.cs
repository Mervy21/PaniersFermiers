using System.Collections.ObjectModel;
using System.Windows.Input;
using PaniersFermiers.Models;
using PaniersFermiers.Services;
using PaniersFermiers.Views;

namespace PaniersFermiers.ViewModels
{
    public class ReservationViewModel : BaseViewModel
    {
        public ObservableCollection<LignePanier> Lignes { get; } = new ObservableCollection<LignePanier>();

        public bool PanierVide { get { return Lignes.Count == 0; } }
        public bool ContenuVisible { get { return Lignes.Count > 0; } }

        public string TotalTexte
        {
            get { return Outils.Fcfa(CommandeService.Total); }
        }

        public string NombreArticlesTexte
        {
            get
            {
                int n = Lignes.Count;
                return "Sous-total (" + n + (n > 1 ? " produits)" : " produit)");
            }
        }

        public string PoidsTexte
        {
            get { return "Poids total : " + Outils.Kg(CommandeService.PoidsTotal); }
        }

        // Règle du minimum de commande
        public bool MinimumAtteint
        {
            get { return CommandeService.Total >= CommandeService.MontantMinimum; }
        }

        public bool MinimumNonAtteint
        {
            get { return !MinimumAtteint; }
        }

        public string MessageMinimum
        {
            get
            {
                int reste = CommandeService.MontantMinimum - CommandeService.Total;
                return "Ajoutez encore " + Outils.Fcfa(reste) + " pour atteindre le minimum de commande ("
                       + Outils.Fcfa(CommandeService.MontantMinimum) + ").";
            }
        }

        public ICommand AugmenterCommand { get; }
        public ICommand DiminuerCommand { get; }
        public ICommand SupprimerCommand { get; }
        public ICommand ViderCommand { get; }
        public ICommand ChoisirPointCommand { get; }

        public ReservationViewModel()
        {
            AugmenterCommand = new Command<LignePanier>(Augmenter);
            DiminuerCommand = new Command<LignePanier>(Diminuer);
            SupprimerCommand = new Command<LignePanier>(Supprimer);
            ViderCommand = new Command(Vider);
            ChoisirPointCommand = new Command(ChoisirPoint);
            Recharger();
        }

        // Reconstruit la liste à partir du service puis prévient l'écran
        private void Recharger()
        {
            Lignes.Clear();
            foreach (LignePanier ligne in CommandeService.ObtenirLignes())
            {
                Lignes.Add(ligne);
            }

            OnPropertyChanged(nameof(PanierVide));
            OnPropertyChanged(nameof(ContenuVisible));
            OnPropertyChanged(nameof(TotalTexte));
            OnPropertyChanged(nameof(NombreArticlesTexte));
            OnPropertyChanged(nameof(PoidsTexte));
            OnPropertyChanged(nameof(MinimumAtteint));
            OnPropertyChanged(nameof(MinimumNonAtteint));
            OnPropertyChanged(nameof(MessageMinimum));
        }

        private void Augmenter(LignePanier ligne)
        {
            CommandeService.ModifierQuantite(ligne, ligne.QuantiteKg + CommandeService.Pas);
            Recharger();
        }

        private void Diminuer(LignePanier ligne)
        {
            CommandeService.ModifierQuantite(ligne, ligne.QuantiteKg - CommandeService.Pas);
            Recharger();
        }

        private void Supprimer(LignePanier ligne)
        {
            CommandeService.RetirerLigne(ligne);
            Recharger();
        }

        private async void Vider()
        {
            bool confirme = await Shell.Current.DisplayAlert("Vider le panier",
                "Voulez-vous retirer tous les produits de votre panier ?", "Oui", "Non");
            if (confirme)
            {
                CommandeService.ViderPanier();
                Recharger();
            }
        }

        private async void ChoisirPoint()
        {
            if (!MinimumAtteint) return;
            await Shell.Current.GoToAsync(nameof(RetraitPage));
        }
    }
}
