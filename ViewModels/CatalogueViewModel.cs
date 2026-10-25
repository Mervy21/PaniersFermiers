using System.Collections.ObjectModel;
using System.Windows.Input;
using PaniersFermiers.Models;
using PaniersFermiers.Services;
using PaniersFermiers.Views;

namespace PaniersFermiers.ViewModels
{
    public class CatalogueViewModel : BaseViewModel
    {
        private List<ProduitItem> _tousLesProduits = new List<ProduitItem>();
        private string _filtreActif = "Tous";
        private string _texteTous = "Tous";
        private string _recherche = "";
        private bool _estChargement = false;
        private string _erreurChargement = "";

        // Liste affichée à l'écran (change selon le filtre et la recherche)
        public ObservableCollection<ProduitItem> Produits { get; } = new ObservableCollection<ProduitItem>();

        // "Bonjour, Merveilles"
        public string Salutation
        {
            get
            {
                Utilisateur? u = AuthService.UtilisateurConnecte;
                if (u == null || u.NomComplet.Trim() == "") return "Bonjour";
                string[] mots = u.NomComplet.Trim().Split(' ');
                return "Bonjour, " + mots[0];
            }
        }

        public string FiltreActif
        {
            get { return _filtreActif; }
            set { SetProperty(ref _filtreActif, value); }
        }

        public string TexteTous
        {
            get { return _texteTous; }
            set { SetProperty(ref _texteTous, value); }
        }

        public string Recherche
        {
            get { return _recherche; }
            set
            {
                SetProperty(ref _recherche, value ?? "");
                AppliquerFiltre();
            }
        }

        public bool EstChargement
        {
            get { return _estChargement; }
            set
            {
                SetProperty(ref _estChargement, value);
                OnPropertyChanged(nameof(ListeVisible));
            }
        }

        public string ErreurChargement
        {
            get { return _erreurChargement; }
            set
            {
                SetProperty(ref _erreurChargement, value);
                OnPropertyChanged(nameof(AErreur));
                OnPropertyChanged(nameof(ListeVisible));
            }
        }

        public bool AErreur
        {
            get { return _erreurChargement != ""; }
        }

        public bool ListeVisible
        {
            get { return !_estChargement && _erreurChargement == ""; }
        }

        // ----- Résumé du panier (barre du bas) -----
        public bool PanierNonVide
        {
            get { return CommandeService.NombreArticles > 0; }
        }

        public string ResumePanierTexte
        {
            get
            {
                int n = CommandeService.NombreArticles;
                string mot = n > 1 ? " produits" : " produit";
                return n + mot + " • " + Outils.Kg(CommandeService.PoidsTotal);
            }
        }

        public string TotalPanierTexte
        {
            get { return Outils.Fcfa(CommandeService.Total); }
        }

        public ICommand FiltrerCommand { get; }
        public ICommand OuvrirProduitCommand { get; }
        public ICommand AugmenterCommand { get; }
        public ICommand DiminuerCommand { get; }
        public ICommand AjouterCommand { get; }
        public ICommand OuvrirPanierCommand { get; }
        public ICommand MesCommandesCommand { get; }
        public ICommand ProfilCommand { get; }
        public ICommand ReessayerCommand { get; }
        public ICommand TrierCommand { get; }
        private string _tri = "Nom";
        public string Tri
        {
            get { return _tri; }
            set { SetProperty(ref _tri, value); }
        }


        public CatalogueViewModel()
        {
            FiltrerCommand = new Command<string>(Filtrer);
            OuvrirProduitCommand = new Command<ProduitItem>(OuvrirProduit);
            AugmenterCommand = new Command<ProduitItem>(Augmenter);
            DiminuerCommand = new Command<ProduitItem>(Diminuer);
            AjouterCommand = new Command<ProduitItem>(Ajouter);
            OuvrirPanierCommand = new Command(OuvrirPanier);
            MesCommandesCommand = new Command(OuvrirMesCommandes);
            ProfilCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(ProfilPage)));
            ReessayerCommand = new Command(Charger);
            TrierCommand = new Command<string>(t => { Tri = t; AppliquerFiltre(); });
            Charger();

        }

        private async void Charger()
        {
            EstChargement = true;
            ErreurChargement = "";
            try
            {
                await Task.Delay(700);   // simulation du chargement
                _tousLesProduits.Clear();
                foreach (Produit p in CatalogueService.ObtenirTous())
                {
                    _tousLesProduits.Add(new ProduitItem(p));
                }
                TexteTous = "Tous (" + _tousLesProduits.Count + ")";
                CommandeService.ChargerPanier();
                AppliquerFiltre();

                Actualiser();
            }
            catch (Exception)
            {
                ErreurChargement = "Impossible de charger les produits. Réessayez.";
            }
            EstChargement = false;
        }

        // Met à jour la barre du panier et les rappels "dans votre panier"
        public void Actualiser()
        {
            foreach (ProduitItem p in _tousLesProduits)
            {
                p.Rafraichir();
            }
            OnPropertyChanged(nameof(PanierNonVide));
            OnPropertyChanged(nameof(ResumePanierTexte));
            OnPropertyChanged(nameof(TotalPanierTexte));
            OnPropertyChanged(nameof(Salutation));
        }

        private void Filtrer(string categorie)
        {
            FiltreActif = categorie;
            AppliquerFiltre();
        }

        private void AppliquerFiltre()
        {
            Produits.Clear();
            string recherche = _recherche.Trim().ToLower();

            IEnumerable<ProduitItem> resultat = _tousLesProduits.Where(p =>
                (FiltreActif == "Tous" || p.Produit.Categorie == FiltreActif) &&
                (recherche == "" || p.Produit.Nom.ToLower().Contains(recherche)
                                 || p.Produit.Origine.ToLower().Contains(recherche)));

            IOrderedEnumerable<ProduitItem> trie = resultat.OrderBy(p => p.Produit.Disponible ? 0 : 1);
            if (Tri == "PrixCroissant") trie = trie.ThenBy(p => p.Produit.PrixKg);
            else if (Tri == "PrixDecroissant") trie = trie.ThenByDescending(p => p.Produit.PrixKg);
            else trie = trie.ThenBy(p => p.Produit.Nom);

            foreach (ProduitItem p in trie) Produits.Add(p);
        }


        private void Augmenter(ProduitItem item)
        {
            if (item.PeutAugmenter) item.Quantite = item.Quantite + CommandeService.Pas;
        }

        private void Diminuer(ProduitItem item)
        {
            if (item.PeutDiminuer) item.Quantite = item.Quantite - CommandeService.Pas;
        }

        private void Ajouter(ProduitItem item)
        {
            if (!item.Disponible) return;
            CommandeService.AjouterProduit(item.Produit, item.Quantite);
            item.Quantite = CommandeService.QuantiteMin;   // le sélecteur revient à 0.5 kg
            Actualiser();
        }

        private async void OuvrirProduit(ProduitItem item)
        {
            CommandeService.ProduitConsulte = item.Produit;
            await Shell.Current.GoToAsync(nameof(ProduitPage));
        }

        private async void OuvrirPanier()
        {
            await Shell.Current.GoToAsync(nameof(ReservationPage));
        }

        private async void OuvrirMesCommandes()
        {
            await Shell.Current.GoToAsync(nameof(SuiviPage));
        }
    }
}
