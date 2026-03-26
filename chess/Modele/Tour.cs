using System;

namespace chess
{
    public class Tour : Piece
    {
        public Tour(string couleur, Position position, bool aDejaBouge = false)
            : base(couleur, position, aDejaBouge)
        {
        }

        public override string Symbole
        {
            get { return Couleur == "Blanc" ? "TB" : "TN"; }
        }

        public override bool MouvementValide(Coup coup, Plateau plateau)
        {
            int deltaLigne = Math.Abs(coup.PositionArrivee.Ligne - coup.PositionDepart.Ligne);
            int deltaColonne = Math.Abs(coup.PositionArrivee.Colonne - coup.PositionDepart.Colonne);
            return (deltaLigne == 0 && deltaColonne > 0) || (deltaColonne == 0 && deltaLigne > 0);
        }

        public override Piece Copier()
        {
            return new Tour(Couleur, new Position(Position), ADejaBouge);
        }
    }
}
