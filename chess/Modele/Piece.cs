namespace chess
{
    /// <summary>
    /// Classe abstraite de base pour toutes les pieces d'echecs.
    /// </summary>
    public abstract class Piece
    {
        protected Piece(string couleur, Position position, bool aDejaBouge = false)
        {
            Couleur = couleur;
            Position = position;
            ADejaBouge = aDejaBouge;
        }

        public string Couleur { get; private set; }

        public Position Position { get; private set; }

        public bool ADejaBouge { get; private set; }

        public abstract string Symbole { get; }

        public void Bouger(Position pos)
        {
            Position = pos;
            ADejaBouge = true;
        }

        public abstract bool MouvementValide(Coup coup, Plateau plateau);

        public abstract Piece Copier();
    }
}
