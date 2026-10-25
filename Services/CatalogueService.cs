using PaniersFermiers.Models;

namespace PaniersFermiers.Services
{
    public static class CatalogueService
    {
        private static List<Produit> _produits = CreerProduits();

        public static List<Produit> ObtenirTous()
        {
            return _produits;
        }

        // Prix en FCFA par kilo : valeurs indicatives, à adapter librement
        private static List<Produit> CreerProduits()
        {
            List<Produit> liste = new List<Produit>();

            // ----- Légumes -----
            liste.Add(Creer(1, "Tomates", "Légumes", 1000, "🍅", "Ouest", "Rouges et charnues, parfaites pour les sauces, le poisson braisé et les salades."));
            liste.Add(Creer(2, "Oignons", "Légumes", 800, "🧅", "Nord", "Piquants et savoureux, la base de toutes vos sauces."));
            liste.Add(Creer(3, "Carottes", "Légumes", 700, "🥕", "Ouest", "Croquantes et sucrées, à croquer crues ou à cuire dans vos plats."));
            liste.Add(Creer(4, "Poivrons", "Légumes", 1200, "🫑", "Ouest", "Fermes et parfumés, ils relèvent tous vos plats."));
            liste.Add(Creer(5, "Choux", "Légumes", 500, "🥬", "Ouest", "Frais et croquants, pour les salades et les légumes sautés."));
            liste.Add(Creer(6, "Piments", "Légumes", 2500, "🌶️", "Littoral", "Forts et parfumés. À utiliser avec modération !"));
            liste.Add(Creer(7, "Aubergines", "Légumes", 900, "🍆", "Centre", "Fermes et brillantes, délicieuses sautées ou en sauce."));
            liste.Add(Creer(8, "Gombo", "Légumes", 1500, "🌿", "Centre", "Tendre et frais, idéal pour vos sauces gombo."));

            // ----- Fruits -----
            liste.Add(Creer(9, "Bananes douces", "Fruits", 600, "🍌", "Littoral", "Sucrées et fondantes, mûries naturellement."));
            liste.Add(Creer(10, "Ananas", "Fruits", 700, "🍍", "Littoral", "Juteux et sucrés, cueillis à maturité."));
            liste.Add(Creer(11, "Mangues", "Fruits", 800, "🥭", "Ouest", "Charnues et parfumées, un vrai bonheur de saison."));
            liste.Add(Creer(12, "Avocats", "Fruits", 1000, "🥑", "Ouest", "Crémeux et beurrés, parfaits en salade ou tartinés."));
            liste.Add(Creer(13, "Oranges", "Fruits", 500, "🍊", "Centre", "Juteuses et rafraîchissantes, riches en vitamine C."));
            liste.Add(Creer(14, "Pastèques", "Fruits", 400, "🍉", "Nord", "Gorgées d'eau, idéales par temps chaud."));

            // Produit hors saison : sert à montrer l'état "indisponible"
            Produit safou = Creer(15, "Safou", "Fruits", 1800, "🫐", "Centre", "Fruit local à déguster avec du maïs ou du plantain. Hors saison pour le moment.");
            safou.Disponible = false;
            liste.Add(safou);

            // ----- Tubercules et plantains -----
            liste.Add(Creer(16, "Plantains", "Tubercules", 600, "🍌", "Littoral", "À frire, à bouillir ou à griller : le grand classique."));
            liste.Add(Creer(17, "Manioc", "Tubercules", 400, "🍠", "Centre", "Pour le bâton de manioc et les plats traditionnels."));
            liste.Add(Creer(18, "Macabo", "Tubercules", 700, "🥔", "Centre", "Pour le macabo pilé, un classique camerounais."));
            liste.Add(Creer(19, "Igname", "Tubercules", 800, "🍠", "Nord-Ouest", "Farineuse et savoureuse, à bouillir ou à frire."));
            liste.Add(Creer(20, "Pommes de terre", "Tubercules", 700, "🥔", "Nord-Ouest", "Fermes et polyvalentes : frites, bouillies ou en purée."));

            return liste;
        }

        private static Produit Creer(int id, string nom, string categorie, int prixKg, string emoji, string origine, string description)
        {
            Produit p = new Produit();
            p.Id = id;
            p.Nom = nom;
            p.Categorie = categorie;
            p.PrixKg = prixKg;
            p.Emoji = emoji;
            p.Origine = origine;
            p.Description = description;
            return p;
        }
    }
}
