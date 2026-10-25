using System.Windows.Input;
using PaniersFermiers.Models;
using PaniersFermiers.Services;

namespace PaniersFermiers.ViewModels
{
    public class ProduitViewModel : BaseViewModel
    {
        private Produit _produit = new Produit();
        private double _quantite = CommandeService.QuantiteMin;

        public string Nom { get; }
        public string Emoji { get; }
        public string Categorie { get; }
        public string PrixKgTexte { get; }
        public string OrigineTexte { get; }
        public string Description { get; }
        public bool Disponible { get; }
        public bool Epuise { get { return !Disponible; } }

        public double Quantite
        {
            get { return _quantite; }
            set
            {
                SetProperty(ref _quantite, value);
                OnPropertyChanged(nameof(QuantiteTexte));
                OnPropertyChanged(nameof(TexteAjout));
                OnPropertyChanged(nameof(PeutDiminuer));
                OnPropertyChanged(nameof(PeutAugmenter));
            }
        }

        public string QuantiteTexte { get { return Outils.Kg(_quantite); } }
        public bool PeutDiminuer { get { return Disponible && _quantite > CommandeService.QuantiteMin; } }
        public bool PeutAugmenter { get { return Disponible && _quantite < CommandeService.QuantiteMax; } }

        public string TexteAjout
        {
            get
            {
                if (!Disponible) return "Indisponible";
                int total = (int)Math.Round(_produit.PrixKg * _quantite);
                return "Ajouter au panier • " + Outils.Fcfa(total);
            }
        }

        public ICommand AugmenterCommand { get; }
        public ICommand DiminuerCommand { get; }
        public ICommand AjouterCommand { get; }

        public ProduitViewModel()
        {
            Produit? consulte = CommandeService.ProduitConsulte;
            if (consulte != null)
            {
                _produit = consulte;
            }

            Nom = _produit.Nom;
            Emoji = _produit.Emoji;
            Categorie = _produit.Categorie;
            PrixKgTexte = _produit.PrixKgTexte;
            OrigineTexte = _produit.Origine;
            Description = _produit.Description;
            Disponible = _produit.Disponible;

            AugmenterCommand = new Command(Augmenter);
            DiminuerCommand = new Command(Diminuer);
            AjouterCommand = new Command(Ajouter);
        }

        private void Augmenter()
        {
            if (PeutAugmenter) Quantite = _quantite + CommandeService.Pas;
        }

        private void Diminuer()
        {
            if (PeutDiminuer) Quantite = _quantite - CommandeService.Pas;
        }

        private async void Ajouter()
        {
            if (!Disponible) return;
            CommandeService.AjouterProduit(_produit, _quantite);
            await Shell.Current.GoToAsync("..");   // retour au catalogue
        }
    }
}
