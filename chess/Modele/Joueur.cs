using System.Collections.Generic;

namespace chess
{
    public class Joueur
    {
        private readonly List<Coup> coupsJoues = new List<Coup>();

        public Joueur(string nom, string couleur)
            : this(nom, 0, couleur)
        {
        }

        public Joueur(string nom, int elo, string couleur)
        {
            Nom = nom;
            Elo = elo;
            Couleur = couleur;
        }

        public Joueur(Joueur autre)
            : this(
                autre != null ? autre.Nom : string.Empty,
                autre != null ? autre.Elo : 0,
                autre != null ? autre.Couleur : string.Empty)
        {
        }

        public string Nom { get; private set; }

        public int Elo { get; private set; }

        public string Couleur { get; private set; }

        public IReadOnlyList<Coup> CoupsJoues
        {
            get { return coupsJoues.AsReadOnly(); }
        }

        public void JouerCoup(Coup coup)
        {
            if (coup != null)
            {
                coupsJoues.Add(coup);
            }
        }

        public override string ToString()
        {
            return Nom + " (" + Couleur + ")";
        }
    }
}
