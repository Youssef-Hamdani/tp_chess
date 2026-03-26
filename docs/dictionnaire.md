# Dictionnaire de classes

## Controleur

### EchecControlleur
- Role : orchestre les interactions entre les vues et le modele.
- Responsabilites :
  - demarrer, charger et sauvegarder une partie
  - transmettre les coups du formulaire au modele
  - mettre a jour les pointages et les messages de fin de partie

## Vue

### Menu
- Role : ecran principal de l'application.
- Responsabilites :
  - afficher la liste des joueurs et leur pointage
  - choisir les joueurs blanc et noir
  - lancer une nouvelle partie
  - charger une partie
  - ajuster le pointage

### FormPartie
- Role : afficher l'etat courant de la partie.
- Responsabilites :
  - dessiner le plateau et les pieces
  - selectionner un coup a la souris ou via les controles numeriques
  - offrir les actions de sauvegarde, abandon, nulle et quitter
  - retourner le plateau selon le joueur courant

## Modele

### Modele
- Role : facade du domaine.
- Responsabilites :
  - conserver la partie courante
  - conserver la liste des joueurs
  - gerer la persistence des parties
  - appliquer les pointages

### Partie
- Role : coeur des regles d'echecs.
- Responsabilites :
  - valider et jouer les coups
  - detecter echec, mat, pat, repetition
  - gerer roque, en passant, promotion
  - clore une partie par abandon ou nulle

### Plateau
- Role : contenir les pieces et fournir les operations de plateau.
- Responsabilites :
  - initialiser les pieces
  - deplacer, retirer et remplacer des pieces
  - verifier les collisions

### Joueur
- Role : representer un joueur et son pointage.

### Coup
- Role : representer un mouvement entre une position de depart et une position d'arrivee.

### Position
- Role : representer une case du plateau.

### Piece
- Role : abstraction de base pour les pieces.

### Tour, Cavalier, Fou, Reine, Roi, Pion
- Role : specialiser la regle de deplacement de chaque piece.
