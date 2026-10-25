using PaniersFermiers.Services;

namespace PaniersFermiers.Models
{
    public class Produit
    {
        public int Id { get; set; }
        public string Nom { get; set; } = "";
        public string Categorie { get; set; } = "";   
        public int PrixKg { get; set; }               
        public string Emoji { get; set; } = "";
        public string Origine { get; set; } = "";     
        public string Description { get; set; } = "";
        public bool Disponible { get; set; } = true;  

        public string PrixKgTexte
        {
            get { return Outils.Fcfa(PrixKg) + " / kg"; }
        }
    }
}
