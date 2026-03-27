# TP Échecs

Ce dépôt contient une application WinForms en C# permettant à deux joueurs de jouer aux échecs dans une architecture MVC.

Le projet couvre :
- la création et le chargement d'une partie;
- l'affichage du plateau et des pièces;
- les déplacements valides et invalides avec rétroaction;
- la gestion des tours;
- l'échec, l'échec et mat et le pat;
- le roque, la prise en passant et la promotion;
- la sauvegarde et le chargement;
- la gestion des joueurs, du pointage, de l'abandon et de la nulle.

## Documentation

- Diagramme de classe : [docs/diagrammes/diag_de_classe.png](docs/diagrammes/diag_de_classe.png)
- Diagramme de séquence 1 : [docs/diagrammes/daig_de_seq_1.png](docs/diagrammes/daig_de_seq_1.png)
- Diagramme de séquence 2 : [docs/diagrammes/daig_de_seq_2.png](docs/diagrammes/daig_de_seq_2.png)
- Dictionnaire de classes Doxygen : [docs/dictionnaire_de_classes/](docs/dictionnaire_de_classes/) ou [docs/dictionnaire_de_classes/index.html](docs/dictionnaire_de_classes/index.html)

## Architecture MVC

### Vue

Les classes [Menu](chess/Vue/Menu.cs) et [FormPartie](chess/Vue/FormPartie.cs) composent l'interface utilisateur.

### Contrôleur

La classe [EchecControlleur](chess/Controleur/EchecControlleur.cs) coordonne les interactions entre les vues et le modèle.

### Modèle

Les classes du dossier [chess/Modele](chess/Modele) représentent la logique métier :
- [Modele](chess/Modele/Modele.cs)
- [Partie](chess/Modele/Partie.cs)
- [Plateau](chess/Modele/Plateau.cs)
- [Joueur](chess/Modele/Joueur.cs)
- [Piece](chess/Modele/Piece.cs) et ses sous-classes

## Choix de conception

### Centralisation des règles

La classe [Partie](chess/Modele/Partie.cs) agit comme expert principal des règles d'échecs. Elle valide les coups, gère les états de fin de partie et coordonne les règles spéciales.

### Responsabilité du plateau

La classe [Plateau](chess/Modele/Plateau.cs) gère la structure spatiale du jeu : les pièces présentes, les déplacements, les remplacements et les collisions.

### Polymorphisme des pièces

Chaque sous-classe de [Piece](chess/Modele/Piece.cs) implémente son propre `MouvementValide(...)`, ce qui évite de concentrer toutes les règles dans une seule structure conditionnelle.

### Vue légère

[FormPartie](chess/Vue/FormPartie.cs) reste orientée affichage et interaction. La validation réelle d'un coup est effectuée dans le modèle par l'intermédiaire du contrôleur.

## Persistance

La sauvegarde est prise en charge par [Modele](chess/Modele/Modele.cs). Le fichier de sauvegarde conserve :
- les joueurs;
- le tour courant;
- le dernier coup;
- les informations nécessaires aux règles spéciales;
- le résultat de partie;
- l'état du pointage;
- les pièces encore présentes sur le plateau.

## Limites connues

- La communication MVC n'est pas entièrement sérialisée au sens le plus strict de l'énoncé.
- La promotion est automatique en reine.
- La documentation code n'est pas exhaustive sur chaque méthode utilitaire.

## Projet Visual Studio

- Solution : [chess.sln](chess.sln)
- Projet WinForms : [chess/chess.csproj](chess/chess.csproj)

## Lancer le projet

1. Ouvrir [chess.sln](chess.sln) dans Visual Studio.
2. Définir `chess` comme projet de démarrage si nécessaire.
3. Compiler et lancer l'application.
