namespace chess
{
    /// <summary>
    /// Represente un mouvement entre deux positions du plateau.
    /// </summary>
    public class Coup
    {
        public Coup()
            : this(new Position(), new Position())
        {
        }

        public Coup(Position positionDepart, Position positionArrivee)
        {
            PositionDepart = positionDepart;
            PositionArrivee = positionArrivee;
        }

        public Coup(Coup autre)
            : this(
                autre != null ? new Position(autre.PositionDepart) : new Position(),
                autre != null ? new Position(autre.PositionArrivee) : new Position())
        {
        }

        public Position PositionDepart { get; private set; }

        public Position PositionArrivee { get; private set; }

        public bool EstValide()
        {
            return PositionDepart != null
                && PositionArrivee != null
                && PositionDepart.EstValide()
                && PositionArrivee.EstValide()
                && !PositionDepart.Equals(PositionArrivee);
        }
    }
}
