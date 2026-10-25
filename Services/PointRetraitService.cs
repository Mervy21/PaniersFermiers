using PaniersFermiers.Models;

namespace PaniersFermiers.Services
{
    public static class PointRetraitService
    {
        public static List<PointRetrait> ObtenirTous()
        {
            List<PointRetrait> liste = new List<PointRetrait>();

            liste.Add(new PointRetrait
            {
                Id = 1,
                Nom = "Poste Centrale - Cathédrale",
                Detail = "À 800 m • Centre-ville",
                Icone = "\uf54e",   // icône « boutique » (police FaSolid)
                Jour = "Jeudi",
                Horaires = "16h30 - 19h00",
                Periode = "Jeudi après-midi",
                Creneaux = new List<Creneau>
                {
                    new Creneau("16h30 - 17h15", false),
                    new Creneau("17h15 - 18h00", true),    // complet : pour montrer l'état "désactivé"
                    new Creneau("18h00 - 19h00", false)
                }
            });

            liste.Add(new PointRetrait
            {
                Id = 2,
                Nom = "Ferme d'Etoudi - Vente directe",
                Detail = "À 4.5 km • Parking gratuit",
                Icone = "\uf722",   // icône « tracteur » (police FaSolid)
                Jour = "Vendredi",
                Horaires = "15h00 - 18h30",
                Periode = "Vendredi après-midi",
                Creneaux = new List<Creneau>
                {
                    new Creneau("15h00 - 16h00", false),
                    new Creneau("16h00 - 17h00", false),
                    new Creneau("17h00 - 18h30", false)
                }
            });

            return liste;
        }
    }
}