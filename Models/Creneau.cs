namespace PaniersFermiers.Models
{
    public class Creneau
    {
        public string libelle { get; set; } = "";
        public bool Complet { get; set; }
        public Creneau(string libelle, bool complet)
        {
            this.libelle = libelle;
            Complet = complet;
        }
    }
}
