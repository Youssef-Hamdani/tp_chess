using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace chess
{
    /// <summary>
    /// Contient les donnees persistantes de l'application, dont la liste des joueurs et la partie courante.
    /// </summary>
    public class Modele
    {
        private const string SignatureSauvegarde = "CHESS_SAVE_V2";
        private readonly List<Joueur> joueurs = new List<Joueur>();

        public Modele()
        {
            joueurs.Add(new Joueur("Fridman, Daniel", 2628, string.Empty, 0m));
            joueurs.Add(new Joueur("Ehvest, Jaan", 2629, string.Empty, 0m));
            joueurs.Add(new Joueur("Roiz, Michael", 2630, string.Empty, 0m));
            joueurs.Add(new Joueur("Milov, Vadim", 2675, string.Empty, 0m));
        }

        public Partie PartieCourante { get; private set; }

        public IReadOnlyList<Joueur> Joueurs
        {
            get { return joueurs.AsReadOnly(); }
        }

        public void DemarrerPartie(Joueur joueurBlanc, Joueur joueurNoir)
        {
            PartieCourante = new Partie(joueurBlanc, joueurNoir);
        }

        public void DemarrerPartie(string nomJoueurBlanc, string nomJoueurNoir)
        {
            Joueur sourceBlanc = ObtenirJoueur(nomJoueurBlanc);
            Joueur sourceNoir = ObtenirJoueur(nomJoueurNoir);

            if (sourceBlanc == null || sourceNoir == null)
            {
                throw new InvalidOperationException("Les joueurs selectionnes sont introuvables.");
            }

            Joueur joueurBlanc = new Joueur(sourceBlanc.Nom, sourceBlanc.Elo, "Blanc", sourceBlanc.Pointage);
            Joueur joueurNoir = new Joueur(sourceNoir.Nom, sourceNoir.Elo, "Noir", sourceNoir.Pointage);
            DemarrerPartie(joueurBlanc, joueurNoir);
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

            if (lignes.Length < 10 || lignes[0] != SignatureSauvegarde)
            {
                return null;
            }

            Joueur joueurBlanc = LireJoueur(lignes[1]);
            Joueur joueurNoir = LireJoueur(lignes[2]);
            int tour = int.Parse(lignes[3]);
            bool partieEstTerminee = bool.Parse(lignes[4]);
            bool dernierCoupEtaitPion = bool.Parse(lignes[5]);
            bool dernierCoupEtaitDoublePasPion = bool.Parse(lignes[6]);
            bool pointageAttribue = bool.Parse(lignes[7]);
            ResultatPartie resultat = (ResultatPartie)Enum.Parse(typeof(ResultatPartie), lignes[8]);
            Coup dernierCoup = LireCoup(lignes[9]);
            string message = lignes.Length > 10 ? DecoderMessage(lignes[10]) : "Partie chargee.";
            Plateau plateau = new Plateau();

            for (int index = 11; index < lignes.Length; index++)
            {
                if (!string.IsNullOrWhiteSpace(lignes[index]))
                {
                    plateau.AjouterPiece(LirePiece(lignes[index]));
                }
            }

            SynchroniserJoueur(joueurBlanc);
            SynchroniserJoueur(joueurNoir);

            PartieCourante = new Partie(
                joueurBlanc,
                joueurNoir,
                plateau,
                tour,
                dernierCoup,
                dernierCoupEtaitPion,
                dernierCoupEtaitDoublePasPion,
                partieEstTerminee,
                message,
                resultat,
                pointageAttribue);

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
                PartieCourante.PointageAttribue.ToString(),
                PartieCourante.Resultat.ToString(),
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

        public Joueur ObtenirJoueur(string nom)
        {
            return joueurs.FirstOrDefault(joueur => string.Equals(joueur.Nom, nom, StringComparison.OrdinalIgnoreCase));
        }

        public void AjusterPointage(string nom, decimal delta)
        {
            Joueur joueur = ObtenirJoueur(nom);

            if (joueur != null)
            {
                joueur.AjusterPointage(delta);
            }
        }

        public void AppliquerPointagePartieCourante()
        {
            if (PartieCourante == null || PartieCourante.PointageAttribue)
            {
                return;
            }

            Joueur joueurBlanc = ObtenirJoueur(PartieCourante.JoueurBlanc.Nom);
            Joueur joueurNoir = ObtenirJoueur(PartieCourante.JoueurNoir.Nom);

            if (joueurBlanc == null || joueurNoir == null)
            {
                return;
            }

            switch (PartieCourante.Resultat)
            {
                case ResultatPartie.VictoireBlanc:
                    joueurBlanc.AjusterPointage(1m);
                    break;
                case ResultatPartie.VictoireNoir:
                    joueurNoir.AjusterPointage(1m);
                    break;
                case ResultatPartie.Nulle:
                    joueurBlanc.AjusterPointage(0.5m);
                    joueurNoir.AjusterPointage(0.5m);
                    break;
            }

            PartieCourante.MarquerPointageAttribue();
        }

        private void SynchroniserJoueur(Joueur joueurCharge)
        {
            if (joueurCharge == null)
            {
                return;
            }

            Joueur joueurExistant = ObtenirJoueur(joueurCharge.Nom);

            if (joueurExistant == null)
            {
                joueurs.Add(new Joueur(joueurCharge.Nom, joueurCharge.Elo, string.Empty, joueurCharge.Pointage));
                return;
            }

            decimal delta = joueurCharge.Pointage - joueurExistant.Pointage;

            if (delta != 0)
            {
                joueurExistant.AjusterPointage(delta);
            }
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
            return string.Join(
                "|",
                joueur.Nom,
                joueur.Elo,
                joueur.Couleur,
                joueur.Pointage.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        private static Joueur LireJoueur(string ligne)
        {
            string[] morceaux = ligne.Split('|');
            return new Joueur(
                morceaux[0],
                int.Parse(morceaux[1]),
                morceaux[2],
                decimal.Parse(morceaux[3], System.Globalization.CultureInfo.InvariantCulture));
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
