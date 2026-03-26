using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace chess
{
    public class Modele
    {
        private const string SignatureSauvegarde = "CHESS_SAVE_V1";

        public Partie PartieCourante { get; private set; }

        public void DemarrerPartie(Joueur joueurBlanc, Joueur joueurNoir)
        {
            PartieCourante = new Partie(joueurBlanc, joueurNoir);
        }

        public bool JouerCoup(Coup coup)
        {
            return PartieCourante != null && PartieCourante.JouerCoup(coup);
        }

        public RaisonCoupInvalide ValiderEtJouerCoup(Coup coup)
        {
            if (PartieCourante == null)
            {
                return RaisonCoupInvalide.CoupNull;
            }

            return PartieCourante.ValiderEtJouerCoup(coup);
        }

        public Partie ChargerPartie()
        {
            return PartieCourante;
        }

        public Partie ChargerPartie(string chemin)
        {
            if (string.IsNullOrWhiteSpace(chemin) || !File.Exists(chemin))
            {
                return null;
            }

            string[] lignes = File.ReadAllLines(chemin);

            if (lignes.Length < 8 || lignes[0] != SignatureSauvegarde)
            {
                return null;
            }

            Joueur joueurBlanc = LireJoueur(lignes[1]);
            Joueur joueurNoir = LireJoueur(lignes[2]);
            int tour = int.Parse(lignes[3]);
            bool partieEstTerminee = bool.Parse(lignes[4]);
            bool dernierCoupEtaitPion = bool.Parse(lignes[5]);
            bool dernierCoupEtaitDoublePasPion = bool.Parse(lignes[6]);
            Coup dernierCoup = LireCoup(lignes[7]);
            string message = lignes.Length > 8 ? DecoderMessage(lignes[8]) : "Partie chargee.";
            Plateau plateau = new Plateau();

            for (int index = 9; index < lignes.Length; index++)
            {
                if (!string.IsNullOrWhiteSpace(lignes[index]))
                {
                    plateau.AjouterPiece(LirePiece(lignes[index]));
                }
            }

            PartieCourante = new Partie(
                joueurBlanc,
                joueurNoir,
                plateau,
                tour,
                dernierCoup,
                dernierCoupEtaitPion,
                dernierCoupEtaitDoublePasPion,
                partieEstTerminee,
                message);

            return PartieCourante;
        }

        public bool SauvegarderPartie()
        {
            return PartieCourante != null;
        }

        public bool SauvegarderPartie(string chemin)
        {
            if (PartieCourante == null || string.IsNullOrWhiteSpace(chemin))
            {
                return false;
            }

            List<string> lignes = new List<string>
            {
                SignatureSauvegarde,
                EcrireJoueur(PartieCourante.JoueurBlanc),
                EcrireJoueur(PartieCourante.JoueurNoir),
                PartieCourante.Tour.ToString(),
                PartieCourante.PartieEstTerminee.ToString(),
                ObtenirDernierCoupEtaitPion().ToString(),
                PartieCourante.DernierCoupEtaitDoublePasPion.ToString(),
                EcrireCoup(PartieCourante.DernierCoup),
                EncoderMessage(PartieCourante.MessageDernierEvenement)
            };

            foreach (Piece piece in PartieCourante.Plateau.Pieces)
            {
                lignes.Add(EcrirePiece(piece));
            }

            File.WriteAllLines(chemin, lignes, Encoding.UTF8);
            return true;
        }

        public Partie GetPartieCourante()
        {
            return PartieCourante;
        }

        private bool ObtenirDernierCoupEtaitPion()
        {
            if (PartieCourante == null || PartieCourante.DernierCoup == null)
            {
                return false;
            }

            Position arrivee = PartieCourante.DernierCoup.PositionArrivee;
            Piece piece = PartieCourante.Plateau.ObtenirPiece(arrivee);
            return piece is Pion;
        }

        private static string EcrireJoueur(Joueur joueur)
        {
            return string.Join("|", joueur.Nom, joueur.Elo, joueur.Couleur);
        }

        private static Joueur LireJoueur(string ligne)
        {
            string[] morceaux = ligne.Split('|');
            return new Joueur(morceaux[0], int.Parse(morceaux[1]), morceaux[2]);
        }

        private static string EcrireCoup(Coup coup)
        {
            if (coup == null)
            {
                return "-";
            }

            return string.Join(
                "|",
                coup.PositionDepart.Ligne,
                coup.PositionDepart.Colonne,
                coup.PositionArrivee.Ligne,
                coup.PositionArrivee.Colonne);
        }

        private static Coup LireCoup(string ligne)
        {
            if (ligne == "-")
            {
                return null;
            }

            string[] morceaux = ligne.Split('|');
            return new Coup(
                new Position(int.Parse(morceaux[0]), int.Parse(morceaux[1])),
                new Position(int.Parse(morceaux[2]), int.Parse(morceaux[3])));
        }

        private static string EcrirePiece(Piece piece)
        {
            return string.Join(
                "|",
                piece.Symbole,
                piece.Position.Ligne,
                piece.Position.Colonne,
                piece.ADejaBouge);
        }

        private static Piece LirePiece(string ligne)
        {
            string[] morceaux = ligne.Split('|');
            string symbole = morceaux[0];
            Position position = new Position(int.Parse(morceaux[1]), int.Parse(morceaux[2]));
            bool aDejaBouge = bool.Parse(morceaux[3]);
            string couleur = symbole.EndsWith("B") ? "Blanc" : "Noir";

            switch (symbole[0])
            {
                case 'P':
                    return new Pion(couleur, position, aDejaBouge);
                case 'T':
                    return new Tour(couleur, position, aDejaBouge);
                case 'C':
                    return new Cavalier(couleur, position, aDejaBouge);
                case 'F':
                    return new Fou(couleur, position, aDejaBouge);
                case 'D':
                    return new Reine(couleur, position, aDejaBouge);
                case 'R':
                    return new Roi(couleur, position, aDejaBouge);
                default:
                    throw new InvalidOperationException("Piece inconnue dans la sauvegarde.");
            }
        }

        private static string EncoderMessage(string message)
        {
            string valeur = message ?? string.Empty;
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(valeur));
        }

        private static string DecoderMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return string.Empty;
            }

            return Encoding.UTF8.GetString(Convert.FromBase64String(message));
        }
    }
}
