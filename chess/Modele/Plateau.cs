using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chess
{
    public class Plateau
    {
        private readonly List<Piece> pieces = new List<Piece>();

        public Plateau()
        {
        }

        public Plateau(Plateau autre)
        {
            if (autre == null)
            {
                return;
            }

            foreach (Piece piece in autre.pieces)
            {
                pieces.Add(piece.Copier());
            }
        }

        public IReadOnlyList<Piece> Pieces
        {
            get { return pieces.AsReadOnly(); }
        }

        public void Initialiser()
        {
            pieces.Clear();

            AjouterPiecesPrincipales("Noir", 0);
            AjouterPions("Noir", 1);
            AjouterPions("Blanc", 6);
            AjouterPiecesPrincipales("Blanc", 7);
        }

        public Piece ObtenirPiece(Position pos)
        {
            return pieces.FirstOrDefault(piece =>
                piece.Position.Ligne == pos.Ligne && piece.Position.Colonne == pos.Colonne);
        }

        public void AjouterPiece(Piece piece)
        {
            if (piece != null)
            {
                pieces.Add(piece);
            }
        }

        public IEnumerable<Piece> ObtenirPieces(string couleur)
        {
            return pieces.Where(piece => piece.Couleur == couleur);
        }

        public Piece ObtenirRoi(string couleur)
        {
            return pieces.FirstOrDefault(piece => piece.Couleur == couleur && piece is Roi);
        }

        public void DeplacerPiece(Coup coup, bool priseEnPassant = false)
        {
            Piece piece = ObtenirPiece(coup.PositionDepart);

            if (piece == null)
            {
                return;
            }

            if (priseEnPassant)
            {
                int lignePieceCapturee = coup.PositionDepart.Ligne;
                RetirerPiece(new Position(lignePieceCapturee, coup.PositionArrivee.Colonne));
            }
            else
            {
                RetirerPiece(coup.PositionArrivee);
            }

            piece.Bouger(new Position(coup.PositionArrivee.Ligne, coup.PositionArrivee.Colonne));
        }

        public void DeplacerTourPourRoque(string couleur, bool grandRoque)
        {
            int ligne = couleur == "Blanc" ? 7 : 0;
            Position departTour = new Position(ligne, grandRoque ? 0 : 7);
            Position arriveeTour = new Position(ligne, grandRoque ? 3 : 5);
            Piece tour = ObtenirPiece(departTour);

            if (tour != null)
            {
                tour.Bouger(arriveeTour);
            }
        }

        public void RemplacerPiece(Position position, Piece nouvellePiece)
        {
            RetirerPiece(position);

            if (nouvellePiece != null)
            {
                pieces.Add(nouvellePiece);
            }
        }

        public void RetirerPiece(Position position)
        {
            Piece piece = ObtenirPiece(position);

            if (piece != null)
            {
                pieces.Remove(piece);
            }
        }

        public bool VerifierCollision(Coup coup)
        {
            Piece piece = ObtenirPiece(coup.PositionDepart);

            if (piece == null || piece is Cavalier || piece is Roi)
            {
                return false;
            }

            int differenceLigne = coup.PositionArrivee.Ligne - coup.PositionDepart.Ligne;
            int differenceColonne = coup.PositionArrivee.Colonne - coup.PositionDepart.Colonne;
            int pasLigne = Math.Sign(differenceLigne);
            int pasColonne = Math.Sign(differenceColonne);

            if (piece is Pion)
            {
                if (differenceColonne != 0)
                {
                    return false;
                }

                if (Math.Abs(differenceLigne) == 2)
                {
                    int ligneIntermediaire = coup.PositionDepart.Ligne + pasLigne;
                    return ObtenirPiece(new Position(ligneIntermediaire, coup.PositionDepart.Colonne)) != null;
                }

                return false;
            }

            int ligneCourante = coup.PositionDepart.Ligne + pasLigne;
            int colonneCourante = coup.PositionDepart.Colonne + pasColonne;

            while (ligneCourante != coup.PositionArrivee.Ligne || colonneCourante != coup.PositionArrivee.Colonne)
            {
                if (ObtenirPiece(new Position(ligneCourante, colonneCourante)) != null)
                {
                    return true;
                }

                ligneCourante += pasLigne;
                colonneCourante += pasColonne;
            }

            return false;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            for (int ligne = 0; ligne < 8; ligne++)
            {
                for (int colonne = 0; colonne < 8; colonne++)
                {
                    Piece piece = ObtenirPiece(new Position(ligne, colonne));
                    builder.Append((piece != null ? piece.Symbole : "..").PadRight(3));
                }

                builder.AppendLine();
            }

            return builder.ToString();
        }

        public string SerialiserPourVue()
        {
            StringBuilder builder = new StringBuilder();

            for (int ligne = 0; ligne < 8; ligne++)
            {
                for (int colonne = 0; colonne < 8; colonne++)
                {
                    Piece piece = ObtenirPiece(new Position(ligne, colonne));
                    builder.Append(piece != null ? piece.Symbole : "__");

                    if (colonne < 7)
                    {
                        builder.Append(";");
                    }
                }

                if (ligne < 7)
                {
                    builder.AppendLine();
                }
            }

            return builder.ToString();
        }

        private void AjouterPiecesPrincipales(string couleur, int ligne)
        {
            pieces.Add(new Tour(couleur, new Position(ligne, 0)));
            pieces.Add(new Cavalier(couleur, new Position(ligne, 1)));
            pieces.Add(new Fou(couleur, new Position(ligne, 2)));
            pieces.Add(new Reine(couleur, new Position(ligne, 3)));
            pieces.Add(new Roi(couleur, new Position(ligne, 4)));
            pieces.Add(new Fou(couleur, new Position(ligne, 5)));
            pieces.Add(new Cavalier(couleur, new Position(ligne, 6)));
            pieces.Add(new Tour(couleur, new Position(ligne, 7)));
        }

        private void AjouterPions(string couleur, int ligne)
        {
            for (int colonne = 0; colonne < 8; colonne++)
            {
                pieces.Add(new Pion(couleur, new Position(ligne, colonne)));
            }
        }
    }
}
