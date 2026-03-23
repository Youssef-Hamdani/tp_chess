namespace chess
{
    public class Partie
    {
        public Partie(Joueur joueurBlanc, Joueur joueurNoir)
        {
            JoueurBlanc = joueurBlanc;
            JoueurNoir = joueurNoir;
            Plateau = new Plateau();
            Plateau.Initialiser();
            Tour = 0;
        }

        public int Tour { get; private set; }

        public Joueur JoueurBlanc { get; private set; }

        public Joueur JoueurNoir { get; private set; }

        public Plateau Plateau { get; private set; }

        public bool JouerCoup(Coup coup)
        {
            if (coup == null || !coup.EstValide())
            {
                return false;
            }

            Piece piece = Plateau.ObtenirPiece(coup.PositionDepart);
            Piece pieceDestination = Plateau.ObtenirPiece(coup.PositionArrivee);

            if (piece == null || pieceDestination != null)
            {
                return false;
            }

            Plateau.DeplacerPiece(coup);
            return true;
        }

        public bool VerifierEchec()
        {
            return false;
        }

        public void ChangerTour()
        {
            Tour = Tour == 0 ? 1 : 0;
        }

        public Joueur GetJoueurCourant()
        {
            return Tour == 0 ? JoueurBlanc : JoueurNoir;
        }
    }
}
