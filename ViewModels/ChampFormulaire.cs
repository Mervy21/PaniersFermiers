namespace PaniersFermiers.ViewModels
{
    // Un champ de formulaire = sa valeur + son message d'erreur
    public class ChampFormulaire : BaseViewModel
    {
        private string _valeur = "";
        private string _erreur = "";

        public string Valeur
        {
            get { return _valeur; }
            set
            {
                SetProperty(ref _valeur, value ?? "");
                // Dès que l'utilisateur retape, on efface l'erreur
                if (_erreur != "")
                {
                    Erreur = "";
                }
            }
        }

        public string Erreur
        {
            get { return _erreur; }
            set
            {
                SetProperty(ref _erreur, value);
                OnPropertyChanged(nameof(AErreur));
            }
        }

        public bool AErreur
        {
            get { return _erreur != ""; }
        }

        public bool EstVide()
        {
            return string.IsNullOrWhiteSpace(_valeur);
        }
    }
}