using System.Collections.ObjectModel;
using System.Windows.Input;
using PaniersFermiers.Models;
using PaniersFermiers.Services;
using PaniersFermiers.Views;

namespace PaniersFermiers.ViewModels
{
    public class RetraitViewModel : BaseViewModel
    {
        private PointItem? _pointSelectionne = null;
        private CreneauItem? _creneauSelectionne = null;
        private bool _chargementPoints = false;
        private bool _chargementCreneaux = false;
        private string _erreurPoints = "";
        private string _messageCreneau = "";
        private string _periodeTexte = "";

        public ObservableCollection<PointItem> Points { get; } = new ObservableCollection<PointItem>();
        public ObservableCollection<CreneauItem> Creneaux { get; } = new ObservableCollection<CreneauItem>();

        // ----- Récapitulatif du panier (bas de l'écran) -----
        public string NomPanierTexte { get; }
        public string DetailPanierTexte { get; }
        public string TotalTexte { get; }

        // ----- Chargement / erreur / vide des points -----
        public bool ChargementPoints
        {
            get { return _chargementPoints; }
            set
            {
                SetProperty(ref _chargementPoints, value);
                OnPropertyChanged(nameof(ContenuVisible));
                OnPropertyChanged(nameof(AucunPoint));
            }
        }

        public string ErreurPoints
        {
            get { return _erreurPoints; }
            set
            {
                SetProperty(ref _erreurPoints, value);
                OnPropertyChanged(nameof(AErreur));
                OnPropertyChanged(nameof(ContenuVisible));
                OnPropertyChanged(nameof(AucunPoint));
            }
        }

        public bool AErreur { get { return _erreurPoints != ""; } }

        public bool ContenuVisible
        {
            get { return !_chargementPoints && _erreurPoints == ""; }
        }

        public bool AucunPoint
        {
            get { return !_chargementPoints && _erreurPoints == "" && Points.Count == 0; }
        }

        // ----- Créneaux -----
        public bool ChargementCreneaux
        {
            get { return _chargementCreneaux; }
            set
            {
                SetProperty(ref _chargementCreneaux, value);
                OnPropertyChanged(nameof(AucunCreneau));
            }
        }

        public bool AucunCreneau
        {
            get { return !_chargementCreneaux && _pointSelectionne != null && Creneaux.Count == 0; }
        }

        public string PeriodeTexte
        {
            get { return _periodeTexte; }
            set
            {
                SetProperty(ref _periodeTexte, value);
                OnPropertyChanged(nameof(APeriode));
            }
        }

        public bool APeriode { get { return _periodeTexte != ""; } }

        public string MessageCreneau
        {
            get { return _messageCreneau; }
            set
            {
                SetProperty(ref _messageCreneau, value);
                OnPropertyChanged(nameof(AMessageCreneau));
            }
        }

        public bool AMessageCreneau { get { return _messageCreneau != ""; } }

        // Bouton "Continuer" : actif seulement si un point ET un créneau sont choisis
        public bool PeutContinuer
        {
            get { return _pointSelectionne != null && _creneauSelectionne != null; }
        }

        public ICommand SelectionnerPointCommand { get; }
        public ICommand SelectionnerCreneauCommand { get; }
        public ICommand ContinuerCommand { get; }
        public ICommand ReessayerCommand { get; }

        public RetraitViewModel()
        {
            int n = CommandeService.NombreArticles;
            NomPanierTexte = "Mon panier";
            DetailPanierTexte = n + (n > 1 ? " produits" : " produit") + " • " + Outils.Kg(CommandeService.PoidsTotal);
            TotalTexte = Outils.Fcfa(CommandeService.Total);


            SelectionnerPointCommand = new Command<PointItem>(SelectionnerPoint);
            SelectionnerCreneauCommand = new Command<CreneauItem>(SelectionnerCreneau);
            ContinuerCommand = new Command(Continuer);
            ReessayerCommand = new Command(Charger);

            Charger();
        }

        private async void Charger()
        {
            ChargementPoints = true;
            ErreurPoints = "";
            Points.Clear();
            Creneaux.Clear();
            try
            {
                await Task.Delay(600);   // simulation du chargement
                foreach (PointRetrait p in PointRetraitService.ObtenirTous())
                {
                    Points.Add(new PointItem(p));
                }
            }
            catch (Exception)
            {
                ErreurPoints = "Impossible de charger les points de retrait. Réessayez.";
            }
            ChargementPoints = false;

            // Le premier point est sélectionné par défaut
            if (Points.Count > 0)
            {
                SelectionnerPoint(Points[0]);
            }
        }

        private async void SelectionnerPoint(PointItem item)
        {
            if (_chargementCreneaux) return;   // on attend la fin du chargement en cours

            foreach (PointItem p in Points)
            {
                p.EstSelectionne = false;
            }
            item.EstSelectionne = true;
            _pointSelectionne = item;
            _creneauSelectionne = null;
            MessageCreneau = "";
            OnPropertyChanged(nameof(PeutContinuer));

            // Chargement des créneaux de ce point
            ChargementCreneaux = true;
            Creneaux.Clear();
            await Task.Delay(400);
            foreach (Creneau c in item.Point.Creneaux)
            {
                Creneaux.Add(new CreneauItem(c));
            }
            PeriodeTexte = item.Point.Periode;
            ChargementCreneaux = false;
        }

        private void SelectionnerCreneau(CreneauItem item)
        {
            if (item.Complet)
            {
                MessageCreneau = "Ce créneau n'est plus disponible. Choisissez-en un autre.";
                return;
            }

            MessageCreneau = "";
            foreach (CreneauItem c in Creneaux)
            {
                c.EstSelectionne = false;
            }
            item.EstSelectionne = true;
            _creneauSelectionne = item;
            OnPropertyChanged(nameof(PeutContinuer));
        }

        private async void Continuer()
        {
            if (_pointSelectionne == null || _creneauSelectionne == null) return;

            CommandeService.PointChoisi = _pointSelectionne.Point;
            CommandeService.CreneauChoisi = _creneauSelectionne.Modele;
            await Shell.Current.GoToAsync(nameof(PaiementPage));
        }
    }
}
