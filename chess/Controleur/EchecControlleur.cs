using System;
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
                formPartie.FormClosed += (_, __) => formPartie = null;
            }

            formPartie.AfficherPlateau(modele.GetPartieCourante().Plateau.SerialiserPourVue());
            formPartie.MettreAJourEtat("Joueur courant : " + modele.GetPartieCourante().GetJoueurCourant());
            formPartie.AfficherMessage("Nouvelle partie initialisee.");
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
                formPartie.AfficherPlateau(modele.GetPartieCourante().Plateau.SerialiserPourVue());
                formPartie.MettreAJourEtat("Joueur courant : " + modele.GetPartieCourante().GetJoueurCourant());
                formPartie.AfficherMessage("Coup joue.");
            }
            else
            {
                formPartie.MettreAJourEtat("Joueur courant : " + modele.GetPartieCourante().GetJoueurCourant());
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
                case RaisonCoupInvalide.CaseArriveeOccupee:
                    return "La case d'arrivee est deja occupee.";
                case RaisonCoupInvalide.MouvementInvalidePourLaPiece:
                    return "Le mouvement ne correspond pas a cette piece.";
                case RaisonCoupInvalide.CollisionDetectee:
                    return "Une piece bloque le trajet.";
                default:
                    return "Coup invalide.";
            }
        }
    }
}
