using System;

namespace chess
{
    public class Fou : Piece
    {
        public Fou(string couleur, Position position)
            : base(couleur, position)
        {
        }

        public override string Symbole
        {
            get { return Couleur == "Blanc" ? "FB" : "FN"; }
        }

        public override bool MouvementValide(Coup coup, Plateau plateau)
        {
            int deltaLigne = Math.Abs(coup.PositionArrivee.Ligne - coup.PositionDepart.Ligne);
            int deltaColonne = Math.Abs(coup.PositionArrivee.Colonne - coup.PositionDepart.Colonne);
            return deltaLigne == deltaColonne && deltaLigne > 0;
        }
    }
}
