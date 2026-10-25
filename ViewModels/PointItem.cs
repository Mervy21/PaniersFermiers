using PaniersFermiers.Models;

namespace PaniersFermiers.ViewModels
{
    public class PointItem : BaseViewModel
    {
        private bool _estSelectionne = false;

        public PointRetrait Point { get; }

        public string Nom { get { return Point.Nom; } }
        public string Detail { get { return Point.Detail; } }
        public string Icone { get { return Point.Icone; } }
        public string JourHoraires { get { return Point.JourHoraires; } }

        public bool EstSelectionne
        {
            get { return _estSelectionne; }
            set { SetProperty(ref _estSelectionne, value); }
        }

        public PointItem(PointRetrait point)
        {
            Point = point;
        }
    }
}