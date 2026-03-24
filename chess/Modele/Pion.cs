using System;

namespace chess
{
    public class Pion : Piece
    {
        public Pion(string couleur, Position position)
            : base(couleur, position)
        {
        }

        public override string Symbole
        {
            get { return Couleur == "Blanc" ? "♙" : "♟"; }
        }

        public override bool MouvementValide(Coup coup, Plateau plateau)
        {
            int direction = Couleur == "Blanc" ? -1 : 1;
            int ligneDepartInitiale = Couleur == "Blanc" ? 6 : 1;
            int deltaLigne = coup.PositionArrivee.Ligne - coup.PositionDepart.Ligne;
            int deltaColonne = Math.Abs(coup.PositionArrivee.Colonne - coup.PositionDepart.Colonne);
            Piece pieceDestination = plateau.ObtenirPiece(coup.PositionArrivee);

            if (deltaColonne == 0)
            {
                if (pieceDestination != null)
                {
                    return false;
                }

                if (deltaLigne == direction)
                {
                    return true;
                }

                return coup.PositionDepart.Ligne == ligneDepartInitiale && deltaLigne == 2 * direction;
            }

            return deltaColonne == 1
                && deltaLigne == direction
                && pieceDestination != null
                && pieceDestination.Couleur != Couleur;
        }
    }
}
