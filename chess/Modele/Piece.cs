namespace chess
{
    public abstract class Piece
    {
        protected Piece(string couleur, Position position)
        {
            Couleur = couleur;
            Position = position;
        }

        public string Couleur { get; private set; }

        public Position Position { get; private set; }

        public abstract string Symbole { get; }

        public void Bouger(Position pos)
        {
            Position = pos;
        }

        public abstract bool MouvementValide(Coup coup, Plateau plateau);
    }
}
