using System;

namespace chess
{
    public class Roi : Piece
    {
        public Roi(string couleur, Position position, bool aDejaBouge = false)
            : base(couleur, position, aDejaBouge)
        {
        }

        public override string Symbole
        {
            get { return Couleur == "Blanc" ? "RB" : "RN"; }
        }

        public override bool MouvementValide(Coup coup, Plateau plateau)
        {
            int deltaLigne = Math.Abs(coup.PositionArrivee.Ligne - coup.PositionDepart.Ligne);
            int deltaColonne = Math.Abs(coup.PositionArrivee.Colonne - coup.PositionDepart.Colonne);
            return deltaLigne <= 1 && deltaColonne <= 1 && (deltaLigne + deltaColonne > 0);
        }

        public override Piece Copier()
        {
            return new Roi(Couleur, new Position(Position), ADejaBouge);
        }
    }
}
