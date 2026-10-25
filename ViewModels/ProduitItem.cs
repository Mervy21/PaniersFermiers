using PaniersFermiers.Models;
using PaniersFermiers.Services;

namespace PaniersFermiers.ViewModels
{
    public class ProduitItem : BaseViewModel
    {
        private double _quantite = CommandeService.QuantiteMin;

        public Produit Produit { get; }

        public string Nom { get { return Produit.Nom; } }
        public string Emoji { get { return Produit.Emoji; } }
        public string Categorie { get { return Produit.Categorie; } }
        public string Description { get { return Produit.Description; } }
        public string PrixKgTexte { get { return Produit.PrixKgTexte; } }
        public string Origine { get { return Produit.Origine; } }
        public bool Disponible { get { return Produit.Disponible; } }
        public bool Epuise { get { return !Produit.Disponible; } }

        public double Quantite
        {
            get { return _quantite; }
            set
            {
                SetProperty(ref _quantite, value);
                OnPropertyChanged(nameof(QuantiteTexte));
                OnPropertyChanged(nameof(PrixSelectionTexte));
                OnPropertyChanged(nameof(TexteAjout));
                OnPropertyChanged(nameof(PeutDiminuer));
                OnPropertyChanged(nameof(PeutAugmenter));
            }
        }

        public string QuantiteTexte
        {
            get { return Outils.Kg(_quantite); }
        }

        public string PrixSelectionTexte
        {
            get { return Outils.Fcfa((int)Math.Round(Produit.PrixKg * _quantite)); }
        }

        public string TexteAjout
        {
            get
            {
                if (!Produit.Disponible) return "Indisponible";
                return "Ajouter • " + PrixSelectionTexte;
            }
        }

        public bool PeutDiminuer
        {
            get { return Produit.Disponible && _quantite > CommandeService.QuantiteMin; }
        }

        public bool PeutAugmenter
        {
            get { return Produit.Disponible && _quantite < CommandeService.QuantiteMax; }
        }

        // Rappel affiché sur la carte quand le produit est déjà dans le panier
        public string DansPanierTexte
        {
            get
            {
                foreach (LignePanier ligne in CommandeService.ObtenirLignes())
                {
                    if (ligne.Produit.Id == Produit.Id)
                    {
                        return ligne.QuantiteTexte + " dans votre panier";
                    }
                }
                return "";
            }
        }

        public bool EstDansPanier
        {
            get { return DansPanierTexte != ""; }
        }

        public void Rafraichir()
        {
            OnPropertyChanged(nameof(DansPanierTexte));
            OnPropertyChanged(nameof(EstDansPanier));
        }

        public ProduitItem(Produit produit)
        {
            Produit = produit;
        }
    }
}
