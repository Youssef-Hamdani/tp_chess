using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace chess
{
    /// <summary>
    /// Controleur principal de l'application qui fait le lien entre le modele et les vues.
    /// </summary>
    public class EchecControlleur
    {
        private readonly Modele modele;
        private readonly Menu menu;
        private FormPartie formPartie;

        public EchecControlleur()
        {
            modele = new Modele();
            menu = new Menu();

            menu.NouvellePartieDemandee += Menu_NouvellePartieDemandee;
            menu.ChargerPartieDemandee += (_, __) => ChargerPartie();
            menu.AjustementPointageDemande += Menu_AjustementPointageDemande;
            menu.QuitterDemandee += (_, __) => Application.Exit();
            RafraichirMenuJoueurs();
        }

        public Menu MenuPrincipal
        {
            get { return menu; }
        }

        public void DemarrerPartie(Joueur j1, Joueur j2)
        {
            modele.DemarrerPartie(j1, j2);
            AfficherPartieCourante("Nouvelle partie initialisee.");
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
                TraiterFinDePartieSiNecessaire(partie);
                AfficherPartieCourante(partie.MessageDernierEvenement);
            }
            else
            {
                formPartie.MettreAJourEtat(ConstruireEtatPartie());
                formPartie.AfficherMessage(ObtenirMessageErreur(resultat));
            }
        }

        public void ChargerPartie()
        {
            using (OpenFileDialog dialogue = CreerDialogueChargement())
            {
                if (dialogue.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                Partie partie = modele.ChargerPartie(dialogue.FileName);

                if (partie == null)
                {
                    MessageBox.Show(
                        "Impossible de charger cette sauvegarde.",
                        "Chargement",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                RafraichirMenuJoueurs();
                AfficherPartieCourante("Partie chargee.");
            }
        }

        public void SauvegarderPartie()
        {
            if (modele.GetPartieCourante() == null)
            {
                return;
            }

            using (SaveFileDialog dialogue = CreerDialogueSauvegarde())
            {
                if (dialogue.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                if (modele.SauvegarderPartie(dialogue.FileName))
                {
                    formPartie.AfficherMessage("Partie sauvegardee.");
                }
            }
        }

        private void Menu_NouvellePartieDemandee(object sender, NouvellePartieDemandeeEventArgs e)
        {
            modele.DemarrerPartie(e.JoueurBlanc, e.JoueurNoir);
            AfficherPartieCourante("Nouvelle partie initialisee.");
        }

        private void Menu_AjustementPointageDemande(object sender, AjustementPointageEventArgs e)
        {
            modele.AjusterPointage(e.NomJoueur, e.Delta);
            RafraichirMenuJoueurs();
        }

        private void FormPartie_CoupSoumis(object sender, CoupEventArgs e)
        {
            JouerCoup(e.Coup);
        }

        private void FormPartie_SauvegardeDemandee(object sender, EventArgs e)
        {
            SauvegarderPartie();
        }

        private void FormPartie_AbandonDemande(object sender, EventArgs e)
        {
            Partie partie = modele.GetPartieCourante();

            if (partie == null || partie.PartieEstTerminee)
            {
                return;
            }

            partie.AbandonnerPartie();
            TraiterFinDePartieSiNecessaire(partie);
            AfficherPartieCourante(partie.MessageDernierEvenement);
            MessageBox.Show(partie.MessageDernierEvenement, "Abandon", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FormPartie_NulleDemandee(object sender, EventArgs e)
        {
            Partie partie = modele.GetPartieCourante();

            if (partie == null || partie.PartieEstTerminee)
            {
                return;
            }

            DialogResult resultat = MessageBox.Show(
                "Le joueur adverse accepte-t-il la nulle ?",
                "Demande de nulle",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (!partie.DemanderNulle(resultat == DialogResult.Yes))
            {
                formPartie.AfficherMessage("La nulle a ete refusee.");
                return;
            }

            TraiterFinDePartieSiNecessaire(partie);
            AfficherPartieCourante(partie.MessageDernierEvenement);
            MessageBox.Show(partie.MessageDernierEvenement, "Partie nulle", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FormPartie_QuitterDemande(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void AfficherPartieCourante(string message)
        {
            if (modele.GetPartieCourante() == null)
            {
                return;
            }

            AssurerFormPartie();
            Partie partie = modele.GetPartieCourante();
            formPartie.AfficherPlateau(partie.Plateau.SerialiserPourVue());
            formPartie.MettreAJourEtat(ConstruireEtatPartie());
            formPartie.AfficherMessage(message);
            formPartie.DefinirInteractionActive(!partie.PartieEstTerminee);
            formPartie.Show();
            formPartie.BringToFront();
        }

        private void AssurerFormPartie()
        {
            if (formPartie != null && !formPartie.IsDisposed)
            {
                return;
            }

            formPartie = new FormPartie();
            formPartie.CoupSoumis += FormPartie_CoupSoumis;
            formPartie.SauvegardeDemandee += FormPartie_SauvegardeDemandee;
            formPartie.AbandonDemande += FormPartie_AbandonDemande;
            formPartie.NulleDemandee += FormPartie_NulleDemandee;
            formPartie.QuitterDemande += FormPartie_QuitterDemande;
            formPartie.CoupsPossiblesDemandes = ObtenirCoupsPossiblesDepuisModele;
            formPartie.FormClosed += (_, __) => formPartie = null;
        }

        private void RafraichirMenuJoueurs()
        {
            menu.AfficherJoueurs(modele.Joueurs);
        }

        private void TraiterFinDePartieSiNecessaire(Partie partie)
        {
            if (partie == null || !partie.PartieEstTerminee)
            {
                return;
            }

            modele.AppliquerPointagePartieCourante();
            RafraichirMenuJoueurs();

            if (partie.MessageDernierEvenement.IndexOf("Echec et mat", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                MessageBox.Show(
                    partie.MessageDernierEvenement,
                    "Echec et mat",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private static OpenFileDialog CreerDialogueChargement()
        {
            return new OpenFileDialog
            {
                Filter = "Sauvegarde echecs (*.echecs)|*.echecs|Tous les fichiers (*.*)|*.*",
                Title = "Charger une partie"
            };
        }

        private static SaveFileDialog CreerDialogueSauvegarde()
        {
            return new SaveFileDialog
            {
                Filter = "Sauvegarde echecs (*.echecs)|*.echecs|Tous les fichiers (*.*)|*.*",
                Title = "Sauvegarder la partie",
                DefaultExt = "echecs",
                AddExtension = true,
                FileName = "partie.echecs"
            };
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
