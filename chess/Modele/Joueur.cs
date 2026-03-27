using System.Collections.Generic;

namespace chess
{
    /// <summary>
    /// Represente un joueur connu par l'application avec son nom, son elo et son pointage.
    /// </summary>
    public class Joueur
    {
        private readonly List<Coup> coupsJoues = new List<Coup>();

        public Joueur(string nom, string couleur)
            : this(nom, 0, couleur, 0m)
        {
        }

        public Joueur(string nom, int elo, string couleur)
            : this(nom, elo, couleur, 0m)
        {
        }

        public Joueur(string nom, int elo, string couleur, decimal pointage)
        {
            Nom = nom;
            Elo = elo;
            Couleur = couleur;
            Pointage = pointage;
        }

        public Joueur(Joueur autre)
            : this(
                autre != null ? autre.Nom : string.Empty,
                autre != null ? autre.Elo : 0,
                autre != null ? autre.Couleur : string.Empty,
                autre != null ? autre.Pointage : 0m)
        {
        }

        public string Nom { get; private set; }

        public int Elo { get; private set; }

        public string Couleur { get; private set; }

        public decimal Pointage { get; private set; }

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

        public void DefinirCouleur(string couleur)
        {
            Couleur = couleur;
        }

        public void AjusterPointage(decimal delta)
        {
            Pointage += delta;
        }

        public override string ToString()
        {
            return Nom + " (" + Couleur + ")";
        }
    }
}
