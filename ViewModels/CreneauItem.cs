using PaniersFermiers.Models;

namespace PaniersFermiers.ViewModels
{
    public class CreneauItem : BaseViewModel
    {
        private bool _estSelectionne = false;

        public Creneau Modele { get; }

        public string Libelle { get { return Modele.libelle; } }
        public bool Complet { get { return Modele.Complet; } }

        public string Etat
        {
            get
            {
                if (Modele.Complet) return "Complet";
                if (_estSelectionne) return "Choisi";
                return "Disponible";
            }
        }

        public bool EstSelectionne
        {
            get { return _estSelectionne; }
            set
            {
                SetProperty(ref _estSelectionne, value);
                OnPropertyChanged(nameof(Etat));
            }
        }

        public CreneauItem(Creneau modele)
        {
            Modele = modele;
        }
    }
}