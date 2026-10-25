using System.Globalization;
namespace PaniersFermiers.Services
{
    public static class Outils
    {
        private static readonly CultureInfo Fr = new CultureInfo("fr-FR");

        public static string Fcfa(int montant)
        {
            return montant.ToString("N0", Fr).Replace('\u202F', ' ').Replace('\u00A0', ' ') + " FCFA";
        }
        public static string Kg(double poids)
        {
            if (poids < 1) return (poids * 1000).ToString("0", Fr) + " g";
            return poids.ToString("0.#", Fr) + " kg";
        }

        public static bool EmailValide(string email)
        {
            if (email.Contains(" ")) return false;
            int arobase = email.IndexOf('@');
            if (arobase < 1) return false;
            int point = email.LastIndexOf('.');
            return point > arobase + 1 && point < email.Length - 1;
        }
        public static string NettoyerTelephone(string telephone)
        {
            string tel = telephone.Replace(" ", "").Replace("-", "");
            if (tel.StartsWith("+237"))
            {
                tel = tel.Substring(4);
            }
            return tel;
        }
        public static bool TelephoneValide(string telephone)
        {
            string tel = NettoyerTelephone(telephone);
            if (tel.Length != 9) return false;
            if (tel[0] != '6') return false;
            foreach (char c in tel)
            {
                if (!char.IsDigit(c)) return false;
            }
            return true;
        }
        public static string Operateur(string telephone)
        {
            string tel = NettoyerTelephone(telephone);
            if (!TelephoneValide(tel)) return "";
            int debut = int.Parse(tel.Substring(0, 3));
            if ((debut >= 650 && debut <= 654) || (debut >= 670 && debut <= 689)) return "MTN Mobile Money";
            if ((debut >= 655 && debut <= 659) || (debut >= 690 && debut <= 699)) return "Orange Money";
            return "";
        }

    }
}
