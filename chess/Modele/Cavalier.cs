using System;

namespace chess
{
    public class Cavalier : Piece
    {
        public Cavalier(string couleur, Position position, bool aDejaBouge = false)
            : base(couleur, position, aDejaBouge)
        {
        }

        public override string Symbole
        {
            get { return Couleur == "Blanc" ? "CB" : "CN"; }
        }

        public override bool MouvementValide(Coup coup, Plateau plateau)
        {
            int deltaLigne = Math.Abs(coup.PositionArrivee.Ligne - coup.PositionDepart.Ligne);
            int deltaColonne = Math.Abs(coup.PositionArrivee.Colonne - coup.PositionDepart.Colonne);
            return (deltaLigne == 2 && deltaColonne == 1) || (deltaLigne == 1 && deltaColonne == 2);
        }

        public override Piece Copier()
        {
            return new Cavalier(Couleur, new Position(Position), ADejaBouge);
        }
    }
}
