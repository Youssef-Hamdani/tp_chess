using System;

namespace chess
{
    public class Reine : Piece
    {
        public Reine(string couleur, Position position)
            : base(couleur, position)
        {
        }

        public override string Symbole
        {
            get { return Couleur == "Blanc" ? "DB" : "DN"; }
        }

        public override bool MouvementValide(Coup coup, Plateau plateau)
        {
            int deltaLigne = Math.Abs(coup.PositionArrivee.Ligne - coup.PositionDepart.Ligne);
            int deltaColonne = Math.Abs(coup.PositionArrivee.Colonne - coup.PositionDepart.Colonne);

            return (deltaLigne == deltaColonne && deltaLigne > 0)
                || (deltaLigne == 0 && deltaColonne > 0)
                || (deltaColonne == 0 && deltaLigne > 0);
        }
    }
}
