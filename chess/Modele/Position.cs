namespace chess
{
    public class Position
    {
        public Position()
            : this(0, 0)
        {
        }

        public Position(int ligne, int colonne)
        {
            Ligne = ligne;
            Colonne = colonne;
        }

        public Position(Position autre)
            : this(autre != null ? autre.Ligne : 0, autre != null ? autre.Colonne : 0)
        {
        }

        public int Ligne { get; set; }

        public int Colonne { get; set; }

        public bool EstValide()
        {
            return Ligne >= 0 && Ligne < 8 && Colonne >= 0 && Colonne < 8;
        }

        public override bool Equals(object obj)
        {
            Position autre = obj as Position;
            return autre != null && Ligne == autre.Ligne && Colonne == autre.Colonne;
        }

        public override int GetHashCode()
        {
            return (Ligne * 397) ^ Colonne;
        }

        public override string ToString()
        {
            return "(" + Ligne + ", " + Colonne + ")";
        }
    }
}
