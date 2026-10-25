using System.Collections.ObjectModel;
using System.Windows.Input;
using PaniersFermiers.Models;
using PaniersFermiers.Services;

namespace PaniersFermiers.ViewModels
{
    public class SuiviViewModel : BaseViewModel
    {
        private Commande? _commandeAffichee = null;
        private bool _estChargement = false;

        public ObservableCollection<Commande> Commandes { get; } = new ObservableCollection<Commande>();

        public bool EstChargement
        {
            get { return _estChargement; }
            set
            {
                SetProperty(ref _estChargement, value);
                OnPropertyChanged(nameof(AfficherVide));
                OnPropertyChanged(nameof(AfficherDetail));
                OnPropertyChanged(nameof(AfficherListe));
            }
        }

        public bool AfficherVide { get { return !_estChargement && _commandeAffichee == null; } }
        public bool AfficherDetail { get { return !_estChargement && _commandeAffichee != null; } }
        public bool AfficherListe { get { return !_estChargement && Commandes.Count > 1; } }

        // ----- Infos de la commande affichée -----
        private int Etape
        {
            get
            {
                if (_commandeAffichee == null) return 0;
                return _commandeAffichee.Etape;
            }
        }

        public string ReferenceAffichee
        {
            get { return _commandeAffichee == null ? "" : _commandeAffichee.ReferenceAffichee; }
        }

        public string StatutTexte
        {
            get { return _commandeAffichee == null ? "" : _commandeAffichee.StatutTexte; }
        }

        public string ResumeTexte
        {
            get
            {
                if (_commandeAffichee == null) return "";
                return _commandeAffichee.ResumeTexte + " • " + _commandeAffichee.TotalTexte;
            }
        }

        public string RetraitTexte
        {
            get
            {
                if (_commandeAffichee == null) return "";
                return _commandeAffichee.LieuRetrait + "\n" + _commandeAffichee.JourRetrait + " • " + _commandeAffichee.CreneauRetrait;
            }
        }
        public string ArticlesTexte
        {
            get { return _commandeAffichee == null ? "" : _commandeAffichee.ArticlesTexte; }
        }


        // ----- Chronologie -----
        public bool Etape1Atteinte { get { return Etape >= 1; } }
        public bool Etape2Atteinte { get { return Etape >= 2; } }
        public bool Etape3Atteinte { get { return Etape >= 3; } }
        public string Sous1 { get { return SousTexte(1); } }
        public string Sous2 { get { return SousTexte(2); } }
        public string Sous3 { get { return SousTexte(3); } }
        public bool PeutAvancer { get { return _commandeAffichee != null && Etape < 3; } }
        public bool PeutAnnuler { get { return _commandeAffichee != null && _commandeAffichee.PeutEtreAnnulee; } }
        public string DateTexte
        {
            get { return _commandeAffichee == null ? "" : _commandeAffichee.JourRetrait; }
        }

        private string SousTexte(int numero)
        {
            if (Etape > numero) return "Terminé";
            if (Etape == numero) return "Actuel";
            return "À venir";
        }

        public ICommand SelectionnerCommand { get; }
        public ICommand AvancerCommand { get; }
        public ICommand CalendrierCommand { get; }
        public ICommand ContacterCommand { get; }
        public ICommand VoirPaniersCommand { get; }
        public ICommand AnnulerCommand { get; }

        public SuiviViewModel()
        {
            SelectionnerCommand = new Command<Commande>(AfficherCommande);
            AvancerCommand = new Command(Avancer);
            CalendrierCommand = new Command(AjouterAuCalendrier);
            ContacterCommand = new Command(Contacter);
            VoirPaniersCommand = new Command(VoirPaniers);
            AnnulerCommand = new Command(Annuler);
            Charger();
        }

        private async void Charger()
        {
            EstChargement = true;
            await Task.Delay(500);   // simulation du chargement
            RechargerListe();
            if (Commandes.Count > 0)
            {
                AfficherCommande(Commandes[0]);
            }
            else
            {
                AfficherCommande(null);
            }
            EstChargement = false;
        }

        private void RechargerListe()
        {
            Commandes.Clear();
            foreach (Commande c in CommandeService.ObtenirCommandes())
            {
                Commandes.Add(c);
            }
        }

        private void AfficherCommande(Commande? commande)
        {
            _commandeAffichee = commande;
            MettreAJourAffichage();
        }

        private void MettreAJourAffichage()
        {
            OnPropertyChanged(nameof(AfficherVide));
            OnPropertyChanged(nameof(AfficherDetail));
            OnPropertyChanged(nameof(AfficherListe));
            OnPropertyChanged(nameof(ReferenceAffichee));
            OnPropertyChanged(nameof(StatutTexte));
            OnPropertyChanged(nameof(ResumeTexte));
            OnPropertyChanged(nameof(RetraitTexte));
            OnPropertyChanged(nameof(ArticlesTexte));
            OnPropertyChanged(nameof(Etape1Atteinte));
            OnPropertyChanged(nameof(Etape2Atteinte));
            OnPropertyChanged(nameof(Etape3Atteinte));
            OnPropertyChanged(nameof(Sous1));
            OnPropertyChanged(nameof(Sous2));
            OnPropertyChanged(nameof(Sous3));
            OnPropertyChanged(nameof(PeutAvancer));
            OnPropertyChanged(nameof(PeutAnnuler));
            OnPropertyChanged(nameof(DateTexte));
        }

        // DÉMO : simule l'avancement de la commande (le producteur récolte, puis dépose au relais)
        private void Avancer()
        {
            if (_commandeAffichee == null) return;
            if (_commandeAffichee.Etape >= 3) return;

            _commandeAffichee.Etape = _commandeAffichee.Etape + 1;
            CommandeService.MettreAJourCommande(_commandeAffichee);
            RechargerListe();
            MettreAJourAffichage();
        }

        // NOUVEAU : annuler la commande affichée (avec confirmation)
        private async void Annuler()
        {
            if (_commandeAffichee == null) return;
            bool confirme = await Shell.Current.DisplayAlert("Annuler la commande",
                "Voulez-vous vraiment annuler la " + _commandeAffichee.ReferenceAffichee + " ?",
                "Oui, annuler", "Non");
            if (!confirme) return;

            if (CommandeService.AnnulerCommande(_commandeAffichee))
            {
                RechargerListe();
                AfficherCommande(Commandes.Count > 0 ? Commandes[0] : null);
                await Shell.Current.DisplayAlert("Commande annulée", "Votre commande a bien été annulée.", "OK");
            }
        }

        private async void AjouterAuCalendrier()
        {
            if (_commandeAffichee == null) return;
            await Shell.Current.DisplayAlert("Rappel de retrait",
                _commandeAffichee.JourRetrait + " • " + _commandeAffichee.CreneauRetrait + "\n" + _commandeAffichee.LieuRetrait,
                "OK");
        }

        private async void Contacter()
        {
            await Shell.Current.DisplayAlert("Contacter le point relais", "Téléphone : 6 90 00 00 00", "OK");
        }

        private async void VoirPaniers()
        {
            await Shell.Current.GoToAsync("//CataloguePage");
        }
    }
}

