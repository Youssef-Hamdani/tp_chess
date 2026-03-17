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
            formPartie.MettreAJourTour("Sprint 1 : deplacement simple");
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

            bool succes = modele.JouerCoup(coup);

            if (succes)
            {
                formPartie.AfficherPlateau(modele.GetPartieCourante().Plateau.SerialiserPourVue());
                formPartie.AfficherMessage("Coup joue.");
            }
            else
            {
                formPartie.AfficherMessage("Coup invalide.");
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
    }
}
