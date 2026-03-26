using System;

namespace chess
{
    public class Fou : Piece
    {
        public Fou(string couleur, Position position, bool aDejaBouge = false)
            : base(couleur, position, aDejaBouge)
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

        public override Piece Copier()
        {
            return new Fou(Couleur, new Position(Position), ADejaBouge);
        }
    }
}
