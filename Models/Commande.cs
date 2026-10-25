using PaniersFermiers.Services;

namespace PaniersFermiers.Models
{
    public class Commande
    {
        public string Reference { get; set; } = "";
        public string EmailClient { get; set; } = "";
        public DateTime DateCommande { get; set; } = DateTime.Now;
        public int NombreArticles { get; set; }
        public double PoidsKg { get; set; }
        public string ArticlesTexte { get; set; } = "";
        public int Total { get; set; }
        public string LieuRetrait { get; set; } = "";
        public string JourRetrait { get; set; } = "";
        public string CreneauRetrait { get; set; } = "";
        public string ModePaiement { get; set; } = "";
        public int Etape { get; set; } = 1;

        public string ReferenceAffichee
        {
            get { return "Commande #" + Reference; }
        }

        public string TotalTexte
        {
            get { return Outils.Fcfa(Total); }
        }

        public string ResumeTexte
        {
            get
            {
                string mot = NombreArticles > 1 ? " produits" : " produit";
                return NombreArticles + mot + " • " + Outils.Kg(PoidsKg);
            }
        }

        public string StatutTexte
        {
            get
            {
                if (Etape == 1) return "Validée";
                if (Etape == 2) return "Récoltée";
                return "Prête à être retirée";
            }
        }

        public bool PeutEtreAnnulee
        {
            get { return Etape == 1; }
        }
    }
}

