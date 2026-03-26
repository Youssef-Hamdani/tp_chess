using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace chess
{
    public class EchecControlleur
    {
        private readonly Modele modele;
        private readonly Menu menu;
        private FormPartie formPartie;

        public EchecControlleur()
        {
            modele = new Modele();
            menu = new Menu();

            menu.NouvellePartieDemandee += (_, __) => DemarrerPartie(
                new Joueur("Joueur Blanc", 1, "Blanc"),
                new Joueur("Joueur Noir", 2, "Noir"));
            menu.ChargerPartieDemandee += (_, __) => ChargerPartie();
        }

        public Menu MenuPrincipal
        {
            get { return menu; }
        }

        public void DemarrerPartie(Joueur j1, Joueur j2)
        {
            modele.DemarrerPartie(j1, j2);

            if (formPartie == null || formPartie.IsDisposed)
            {
                formPartie = new FormPartie();
                formPartie.CoupSoumis += FormPartie_CoupSoumis;
                formPartie.CoupsPossiblesDemandes = ObtenirCoupsPossiblesDepuisModele;
                formPartie.FormClosed += (_, __) => formPartie = null;
            }

            formPartie.AfficherPlateau(modele.GetPartieCourante().Plateau.SerialiserPourVue());
            formPartie.MettreAJourEtat(ConstruireEtatPartie());
            formPartie.AfficherMessage("Nouvelle partie initialisee.");
            formPartie.DefinirInteractionActive(true);
            formPartie.Show();
            formPartie.BringToFront();
        }

        public void JouerCoup(Coup coup)
        {
            if (formPartie == null || modele.GetPartieCourante() == null)
            {
                return;
            }

            RaisonCoupInvalide resultat = modele.ValiderEtJouerCoup(coup);

            if (resultat == RaisonCoupInvalide.Aucune)
            {
                Partie partie = modele.GetPartieCourante();
                formPartie.AfficherPlateau(partie.Plateau.SerialiserPourVue());
                formPartie.MettreAJourEtat(ConstruireEtatPartie());
                formPartie.AfficherMessage(partie.MessageDernierEvenement);
                formPartie.DefinirInteractionActive(!partie.PartieEstTerminee);
            }
            else
            {
                formPartie.MettreAJourEtat(ConstruireEtatPartie());
                formPartie.AfficherMessage(ObtenirMessageErreur(resultat));
            }
        }

        public void ChargerPartie()
        {
            MessageBox.Show(
                "Le chargement sera implemente au Sprint 2.",
                "Sprint 1",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public void SauvegarderPartie()
        {
            MessageBox.Show(
                "La sauvegarde sera implementee au Sprint 2.",
                "Sprint 1",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void FormPartie_CoupSoumis(object sender, CoupEventArgs e)
        {
            JouerCoup(e.Coup);
        }

        private IList<Position> ObtenirCoupsPossiblesDepuisModele(Position position)
        {
            Partie partie = modele.GetPartieCourante();

            if (partie == null)
            {
                return new List<Position>();
            }

            return partie.ObtenirCoupsLegaux(position);
        }

        private string ConstruireEtatPartie()
        {
            Partie partie = modele.GetPartieCourante();

            if (partie == null)
            {
                return "Aucune partie.";
            }

            if (partie.PartieEstTerminee)
            {
                return "Partie terminee.";
            }

            string etat = "Joueur courant : " + partie.GetJoueurCourant();

            if (partie.EstEnEchec(partie.GetJoueurCourant().Couleur))
            {
                etat += " - en echec";
            }

            return etat;
        }

        private string ObtenirMessageErreur(RaisonCoupInvalide raison)
        {
            switch (raison)
            {
                case RaisonCoupInvalide.CoupNull:
                    return "Aucun coup n'a ete fourni.";
                case RaisonCoupInvalide.PositionsInvalides:
                    return "Les positions choisies sont invalides.";
                case RaisonCoupInvalide.AucunePieceAuDepart:
                    return "Il n'y a aucune piece sur la case de depart.";
                case RaisonCoupInvalide.MauvaisJoueur:
                    return "Cette piece n'appartient pas au joueur courant.";
                case RaisonCoupInvalide.CaseArriveeOccupeeParAllie:
                    return "La case d'arrivee contient deja une piece alliee.";
                case RaisonCoupInvalide.CaseArriveeOccupee:
                    return "La case d'arrivee est deja occupee.";
                case RaisonCoupInvalide.MouvementInvalidePourLaPiece:
                    return "Le mouvement ne correspond pas a cette piece.";
                case RaisonCoupInvalide.CollisionDetectee:
                    return "Une piece bloque le trajet.";
                case RaisonCoupInvalide.RoiEnEchecApresCoup:
                    return "Ce coup laisserait votre roi en echec.";
                case RaisonCoupInvalide.PartieTerminee:
                    return "La partie est deja terminee.";
                default:
                    return "Coup invalide.";
            }
        }
    }
}
