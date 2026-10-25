using PaniersFermiers.Services;

namespace PaniersFermiers.Models
{
    // Une ligne du panier : un produit + une quantité en kilos
    public class LignePanier
    {
        public Produit Produit { get; set; }
        public double QuantiteKg { get; set; }

        public LignePanier(Produit produit, double quantiteKg)
        {
            Produit = produit;
            QuantiteKg = quantiteKg;
        }

        public string Nom { get { return Produit.Nom; } }
        public string Emoji { get { return Produit.Emoji; } }
        public string PrixKgTexte { get { return Produit.PrixKgTexte; } }
        public string QuantiteTexte { get { return Outils.Kg(QuantiteKg); } }

        public int Total
        {
            get { return (int)Math.Round(Produit.PrixKg * QuantiteKg); }
        }

        public string TotalTexte { get { return Outils.Fcfa(Total); } }

        public bool PeutDiminuer { get { return QuantiteKg > CommandeService.QuantiteMin; } }
        public bool PeutAugmenter { get { return QuantiteKg < CommandeService.QuantiteMax; } }
    }
}

