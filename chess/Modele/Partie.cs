using System;
using System.Collections.Generic;

namespace chess
{
    public class Partie
    {
        private bool dernierCoupEtaitPion;

        public Partie(Joueur joueurBlanc, Joueur joueurNoir)
        {
            JoueurBlanc = joueurBlanc;
            JoueurNoir = joueurNoir;
            Plateau = new Plateau();
            Plateau.Initialiser();
            Tour = 0;
            MessageDernierEvenement = "Partie initialisee.";
        }

        public int Tour { get; private set; }

        public Joueur JoueurBlanc { get; private set; }

        public Joueur JoueurNoir { get; private set; }

        public Plateau Plateau { get; private set; }

        public Coup DernierCoup { get; private set; }

        public bool DernierCoupEtaitDoublePasPion { get; private set; }

        public bool PartieEstTerminee { get; private set; }

        public string MessageDernierEvenement { get; private set; }

        public bool JouerCoup(Coup coup)
        {
            return ValiderEtJouerCoup(coup) == RaisonCoupInvalide.Aucune;
        }

        public RaisonCoupInvalide ValiderEtJouerCoup(Coup coup)
        {
            RaisonCoupInvalide validation = ValiderCoup(coup);

            if (validation != RaisonCoupInvalide.Aucune)
            {
                return validation;
            }

            ExecuterCoup(coup);
            return RaisonCoupInvalide.Aucune;
        }

        public IList<Position> ObtenirCoupsLegaux(Position depart)
        {
            List<Position> coups = new List<Position>();

            if (PartieEstTerminee || depart == null || !depart.EstValide())
            {
                return coups;
            }

            Piece piece = Plateau.ObtenirPiece(depart);

            if (piece == null || piece.Couleur != GetJoueurCourant().Couleur)
            {
                return coups;
            }

            for (int ligne = 0; ligne < 8; ligne++)
            {
                for (int colonne = 0; colonne < 8; colonne++)
                {
                    Position arrivee = new Position(ligne, colonne);

                    if (depart.Equals(arrivee))
                    {
                        continue;
                    }

                    Coup coup = new Coup(new Position(depart), arrivee);

                    if (ValiderCoup(coup) == RaisonCoupInvalide.Aucune)
                    {
                        coups.Add(arrivee);
                    }
                }
            }

            return coups;
        }

        public bool VerifierEchec()
        {
            return EstEnEchec(GetJoueurCourant().Couleur);
        }

        public bool EstEnEchec(string couleur)
        {
            return EstEnEchecSurPlateau(couleur, Plateau);
        }

        public bool EstEchecEtMat(string couleur)
        {
            return EstEnEchec(couleur) && !PossedeCoupLegalPourCouleur(couleur);
        }

        public bool EstPat(string couleur)
        {
            return !EstEnEchec(couleur) && !PossedeCoupLegalPourCouleur(couleur);
        }

        public void ChangerTour()
        {
            Tour = Tour == 0 ? 1 : 0;
        }

        public Joueur GetJoueurCourant()
        {
            return Tour == 0 ? JoueurBlanc : JoueurNoir;
        }

        public Joueur GetJoueurAdverse()
        {
            return Tour == 0 ? JoueurNoir : JoueurBlanc;
        }

        private RaisonCoupInvalide ValiderCoup(Coup coup)
        {
            if (PartieEstTerminee)
            {
                return RaisonCoupInvalide.PartieTerminee;
            }

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

            bool estRoque = EstRoque(piece, coup);
            bool estEnPassant = EstEnPassant(piece, coup);

            if (estRoque)
            {
                if (!RoqueValide(piece as Roi, coup))
                {
                    return RaisonCoupInvalide.MouvementInvalidePourLaPiece;
                }
            }
            else
            {
                if (!estEnPassant && !piece.MouvementValide(coup, Plateau))
                {
                    return RaisonCoupInvalide.MouvementInvalidePourLaPiece;
                }

                if (!estEnPassant && Plateau.VerifierCollision(coup))
                {
                    return RaisonCoupInvalide.CollisionDetectee;
                }
            }

            if (LaisseRoiEnEchec(coup, estRoque, estEnPassant, piece.Couleur))
            {
                return RaisonCoupInvalide.RoiEnEchecApresCoup;
            }

            return RaisonCoupInvalide.Aucune;
        }

        private void ExecuterCoup(Coup coup)
        {
            Piece piece = Plateau.ObtenirPiece(coup.PositionDepart);
            bool estRoque = EstRoque(piece, coup);
            bool estEnPassant = EstEnPassant(piece, coup);
            bool grandRoque = estRoque && coup.PositionArrivee.Colonne < coup.PositionDepart.Colonne;
            bool promotion = false;
            List<string> evenements = new List<string>();

            Plateau.DeplacerPiece(coup, estEnPassant);

            if (estRoque)
            {
                Plateau.DeplacerTourPourRoque(piece.Couleur, grandRoque);
                evenements.Add(grandRoque ? "Grand roque." : "Petit roque.");
            }

            if (estEnPassant)
            {
                evenements.Add("Prise en passant.");
            }

            if (piece is Pion && (coup.PositionArrivee.Ligne == 0 || coup.PositionArrivee.Ligne == 7))
            {
                Plateau.RemplacerPiece(
                    coup.PositionArrivee,
                    new Reine(piece.Couleur, new Position(coup.PositionArrivee), true));
                promotion = true;
                evenements.Add("Pion promu en reine.");
            }

            DernierCoup = new Coup(coup);
            dernierCoupEtaitPion = piece is Pion;
            DernierCoupEtaitDoublePasPion = piece is Pion
                && Math.Abs(coup.PositionArrivee.Ligne - coup.PositionDepart.Ligne) == 2;

            GetJoueurCourant().JouerCoup(coup);
            ChangerTour();

            if (EstEnEchec(GetJoueurCourant().Couleur))
            {
                if (!JoueurCourantPossedeCoupLegal())
                {
                    PartieEstTerminee = true;
                    evenements.Add("Echec et mat. " + GetJoueurAdverse().Nom + " gagne.");
                }
                else
                {
                    evenements.Add("Echec au roi " + GetJoueurCourant().Couleur.ToLower() + ".");
                }
            }
            else if (!JoueurCourantPossedeCoupLegal())
            {
                PartieEstTerminee = true;
                evenements.Add("Pat. Partie nulle.");
            }

            if (!promotion && evenements.Count == 0)
            {
                evenements.Add("Coup joue.");
            }

            MessageDernierEvenement = string.Join(" ", evenements);
        }

        private bool JoueurCourantPossedeCoupLegal()
        {
            return PossedeCoupLegalPourCouleur(GetJoueurCourant().Couleur);
        }

        private bool PossedeCoupLegalPourCouleur(string couleur)
        {
            int tourSauvegarde = Tour;
            Tour = couleur == "Blanc" ? 0 : 1;

            foreach (Piece piece in Plateau.ObtenirPieces(GetJoueurCourant().Couleur))
            {
                if (ObtenirCoupsLegaux(piece.Position).Count > 0)
                {
                    Tour = tourSauvegarde;
                    return true;
                }
            }

            Tour = tourSauvegarde;
            return false;
        }

        private bool EstRoque(Piece piece, Coup coup)
        {
            return piece is Roi
                && coup.PositionDepart.Ligne == coup.PositionArrivee.Ligne
                && Math.Abs(coup.PositionArrivee.Colonne - coup.PositionDepart.Colonne) == 2;
        }

        private bool RoqueValide(Roi roi, Coup coup)
        {
            if (roi == null || roi.ADejaBouge)
            {
                return false;
            }

            bool grandRoque = coup.PositionArrivee.Colonne < coup.PositionDepart.Colonne;
            int ligne = coup.PositionDepart.Ligne;
            Position positionTour = new Position(ligne, grandRoque ? 0 : 7);
            Tour tour = Plateau.ObtenirPiece(positionTour) as Tour;

            if (tour == null || tour.ADejaBouge || tour.Couleur != roi.Couleur)
            {
                return false;
            }

            int[] colonnesVides = grandRoque ? new[] { 1, 2, 3 } : new[] { 5, 6 };

            foreach (int colonne in colonnesVides)
            {
                if (Plateau.ObtenirPiece(new Position(ligne, colonne)) != null)
                {
                    return false;
                }
            }

            string couleurAdverse = ObtenirCouleurAdverse(roi.Couleur);
            int[] colonnesControlees = grandRoque ? new[] { 4, 3, 2 } : new[] { 4, 5, 6 };

            foreach (int colonne in colonnesControlees)
            {
                if (EstCaseAttaquee(new Position(ligne, colonne), couleurAdverse, Plateau))
                {
                    return false;
                }
            }

            return true;
        }

        private bool EstEnPassant(Piece piece, Coup coup)
        {
            if (!(piece is Pion) || !dernierCoupEtaitPion || !DernierCoupEtaitDoublePasPion || DernierCoup == null)
            {
                return false;
            }

            int direction = piece.Couleur == "Blanc" ? -1 : 1;
            int deltaLigne = coup.PositionArrivee.Ligne - coup.PositionDepart.Ligne;
            int deltaColonne = Math.Abs(coup.PositionArrivee.Colonne - coup.PositionDepart.Colonne);

            if (deltaLigne != direction || deltaColonne != 1)
            {
                return false;
            }

            if (Plateau.ObtenirPiece(coup.PositionArrivee) != null)
            {
                return false;
            }

            Position positionPionAdverse = new Position(coup.PositionDepart.Ligne, coup.PositionArrivee.Colonne);
            Piece pionAdverse = Plateau.ObtenirPiece(positionPionAdverse);

            if (!(pionAdverse is Pion) || pionAdverse.Couleur == piece.Couleur)
            {
                return false;
            }

            return DernierCoup.PositionArrivee.Equals(positionPionAdverse);
        }

        private bool LaisseRoiEnEchec(Coup coup, bool estRoque, bool estEnPassant, string couleur)
        {
            Plateau simulation = new Plateau(Plateau);
            SimulerCoup(simulation, coup, estRoque, estEnPassant);
            return EstEnEchecSurPlateau(couleur, simulation);
        }

        private void SimulerCoup(Plateau plateau, Coup coup, bool estRoque, bool estEnPassant)
        {
            Piece piece = plateau.ObtenirPiece(coup.PositionDepart);
            bool grandRoque = estRoque && coup.PositionArrivee.Colonne < coup.PositionDepart.Colonne;

            plateau.DeplacerPiece(coup, estEnPassant);

            if (estRoque && piece != null)
            {
                plateau.DeplacerTourPourRoque(piece.Couleur, grandRoque);
            }

            if (piece is Pion && (coup.PositionArrivee.Ligne == 0 || coup.PositionArrivee.Ligne == 7))
            {
                plateau.RemplacerPiece(
                    coup.PositionArrivee,
                    new Reine(piece.Couleur, new Position(coup.PositionArrivee), true));
            }
        }

        private bool EstEnEchecSurPlateau(string couleur, Plateau plateau)
        {
            Piece roi = plateau.ObtenirRoi(couleur);

            if (roi == null)
            {
                return false;
            }

            return EstCaseAttaquee(roi.Position, ObtenirCouleurAdverse(couleur), plateau);
        }

        private bool EstCaseAttaquee(Position position, string couleurAttaquante, Plateau plateau)
        {
            foreach (Piece piece in plateau.ObtenirPieces(couleurAttaquante))
            {
                if (piece is Pion)
                {
                    int direction = piece.Couleur == "Blanc" ? -1 : 1;
                    int ligneAttaque = piece.Position.Ligne + direction;

                    if (ligneAttaque == position.Ligne
                        && Math.Abs(piece.Position.Colonne - position.Colonne) == 1)
                    {
                        return true;
                    }

                    continue;
                }

                Coup coup = new Coup(new Position(piece.Position), new Position(position));

                if (piece.MouvementValide(coup, plateau) && !plateau.VerifierCollision(coup))
                {
                    return true;
                }
            }

            return false;
        }

        private static string ObtenirCouleurAdverse(string couleur)
        {
            return couleur == "Blanc" ? "Noir" : "Blanc";
        }
    }
}
