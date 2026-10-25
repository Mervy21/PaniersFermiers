# PaniersFermiers

Application mobile .NET MAUI (net9.0) de réservation de paniers de fruits et légumes
en direct des producteurs du Cameroun.

## Lancer le projet

1. Ouvrir `PaniersFermiers.sln` dans Visual Studio 2022 (workload **.NET MAUI** installé).
2. Faire **Nettoyer la solution**, puis **Régénérer la solution** (une fois, pour prendre en compte la nouvelle police d'icônes).
3. Choisir la cible (émulateur Android ou *Windows Machine*) et lancer.

Compte de démonstration : `demo@paniers.cm` / `12345678`

## Fonctionnalités

- Connexion / inscription (mots de passe hachés en SHA-256, comptes et session conservés sur l'appareil)
- Catalogue : recherche, filtres par catégorie, tri, fiche produit
- Panier, choix du point relais et du créneau, paiement à la remise
- Suivi des commandes (avec annulation avant récolte), ajout au calendrier
- Page profil : statistiques, aide, déconnexion

## Structure

```
Models/       Données (Produit, Commande, Utilisateur, ...)
Services/     Logique (AuthService, CatalogueService, CommandeService, ...)
ViewModels/   Logique des écrans (MVVM)
Views/        Écrans XAML
Resources/    Styles, couleurs, polices (OpenSans + icônes Font Awesome)
```

## Icônes

Les pictogrammes de l'interface utilisent la police **Font Awesome Free (solid)**,
enregistrée sous l'alias `FaSolid` dans `MauiProgram.cs`.
Chaque icône est déclarée une seule fois dans `App.xaml` (clés `Ic...`) et réutilisée dans les vues :

```xml
<Label Text="{StaticResource IcLoupe}" FontFamily="FaSolid" FontSize="15" />
```

Les emojis des produits (tomate, carotte, ...) sont conservés comme illustrations : ils sont dans `Services/CatalogueService.cs`.
Licence de la police : voir `Licences/FontAwesome-Free-LICENSE.txt` (SIL OFL 1.1).
