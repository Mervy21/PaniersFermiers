using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Maui.Storage;
using PaniersFermiers.Models;

namespace PaniersFermiers.Services
{
    // AMÉLIORATIONS :
    //  - les mots de passe sont stockés sous forme d'empreinte SHA-256 (plus jamais en clair)
    //  - les comptes créés sont sauvegardés sur le téléphone (ils survivent à la fermeture de l'appli)
    //  - la session est mémorisée : l'utilisateur reste connecté au prochain lancement
    public static class AuthService
    {
        private const string CleComptes = "comptes_v1";
        private const string CleSession = "session_v1";

        // Compte de démo : demo@paniers.cm / 12345678
        private static List<Utilisateur> _comptes = ChargerComptes();

        public static Utilisateur? UtilisateurConnecte { get; private set; }

        public static string Hacher(string motDePasse)
        {
            byte[] octets = SHA256.HashData(Encoding.UTF8.GetBytes(motDePasse));
            return Convert.ToHexString(octets);
        }

        public static bool Connecter(string identifiant, string motDePasse)
        {
            string id = identifiant.Trim().ToLower();
            string tel = Outils.NettoyerTelephone(identifiant);
            string empreinte = Hacher(motDePasse);

            foreach (Utilisateur u in _comptes)
            {
                bool memeIdentifiant = (u.Email.ToLower() == id) || (u.Telephone == tel);
                if (memeIdentifiant && u.MotDePasse == empreinte)
                {
                    OuvrirSession(u);
                    return true;
                }
            }
            return false;
        }

        public static bool EmailExiste(string email)
        {
            return _comptes.Any(u => u.Email.ToLower() == email.Trim().ToLower());
        }

        public static bool TelephoneExiste(string telephone)
        {
            string tel = Outils.NettoyerTelephone(telephone);
            return _comptes.Any(u => u.Telephone == tel);
        }

        // Le RegisterViewModel envoie le mot de passe en clair : on le hache ici.
        public static void Inscrire(Utilisateur utilisateur)
        {
            utilisateur.MotDePasse = Hacher(utilisateur.MotDePasse);
            utilisateur.Telephone = Outils.NettoyerTelephone(utilisateur.Telephone);
            _comptes.Add(utilisateur);
            SauvegarderComptes();
            OuvrirSession(utilisateur);
        }

        public static void Deconnecter()
        {
            UtilisateurConnecte = null;
            Preferences.Default.Remove(CleSession);
        }

        // Appelé au démarrage : renvoie true si un utilisateur était déjà connecté
        public static bool RestaurerSession()
        {
            string email = Preferences.Default.Get(CleSession, "");
            if (email == "") return false;
            Utilisateur? u = _comptes.FirstOrDefault(c => c.Email.ToLower() == email);
            if (u == null) return false;
            UtilisateurConnecte = u;
            return true;
        }

        private static void OuvrirSession(Utilisateur u)
        {
            UtilisateurConnecte = u;
            Preferences.Default.Set(CleSession, u.Email.ToLower());
        }

        // ----- Sauvegarde des comptes -----
        private static List<Utilisateur> ChargerComptes()
        {
            List<Utilisateur> liste = new List<Utilisateur>();
            try
            {
                string brut = Preferences.Default.Get(CleComptes, "");
                if (brut != "")
                {
                    liste = JsonSerializer.Deserialize<List<Utilisateur>>(brut) ?? new List<Utilisateur>();
                }
            }
            catch (Exception)
            {
                liste = new List<Utilisateur>();
            }

            if (!liste.Any(u => u.Email == "demo@paniers.cm"))
            {
                liste.Insert(0, new Utilisateur
                {
                    NomComplet = "Client Démo",
                    Email = "demo@paniers.cm",
                    Telephone = "690000000",
                    MotDePasse = Hacher("12345678")
                });
            }
            return liste;
        }

        private static void SauvegarderComptes()
        {
            Preferences.Default.Set(CleComptes, JsonSerializer.Serialize(_comptes));
        }
    }
}
