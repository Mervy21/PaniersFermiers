namespace PaniersFermiers.Models
{
    public class Utilisateur
    {
        public string NomComplet { get; set; } = "";
        public string Email { get; set; } = "";
        public string Telephone { get; set; } = "";
      
        public string MotDePasse { get; set; } = "";

        public string Initiales
        {
            get
            {
                string[] mots = NomComplet.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (mots.Length == 0) return "?";
                if (mots.Length == 1) return mots[0].Substring(0, 1).ToUpper();
                return (mots[0].Substring(0, 1) + mots[mots.Length - 1].Substring(0, 1)).ToUpper();
            }
        }
    }
}
