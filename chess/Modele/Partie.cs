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
            return ValiderEtJouerCoup(coup) == RaisonCoupInvalide.Aucune;
        }

        public RaisonCoupInvalide ValiderEtJouerCoup(Coup coup)
        {
            if (coup == null)
            {
                return RaisonCoupInvalide.CoupNull;
            }

            if (!coup.EstValide())
            {
                return RaisonCoupInvalide.PositionsInvalides;
            }

            Piece piece = Plateau.ObtenirPiece(coup.PositionDepart);

            if (piece == null)
            {
                return RaisonCoupInvalide.AucunePieceAuDepart;
            }

            if (piece.Couleur != GetJoueurCourant().Couleur)
            {
                return RaisonCoupInvalide.MauvaisJoueur;
            }

            Piece pieceDestination = Plateau.ObtenirPiece(coup.PositionArrivee);

            if (pieceDestination != null && pieceDestination.Couleur == piece.Couleur)
            {
                return RaisonCoupInvalide.CaseArriveeOccupeeParAllie;
            }

            if (!piece.MouvementValide(coup, Plateau))
            {
                return RaisonCoupInvalide.MouvementInvalidePourLaPiece;
            }

            if (Plateau.VerifierCollision(coup))
            {
                return RaisonCoupInvalide.CollisionDetectee;
            }

            Plateau.DeplacerPiece(coup);
            GetJoueurCourant().JouerCoup(coup);
            ChangerTour();
            return RaisonCoupInvalide.Aucune;
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
