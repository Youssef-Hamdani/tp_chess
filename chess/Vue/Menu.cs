using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace chess
{
    /// <summary>
    /// Menu principal qui affiche la liste des joueurs, les pointages et les actions de haut niveau.
    /// </summary>
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
            btnNouvellePartie.Click += (_, __) => ChoisirNouvellePartie();
            btnChargerPartie.Click += (_, __) => ChoisirChargerPartie();
            btnAjouterPoint.Click += (_, __) => AjusterPointage(1m);
            btnRetirerPoint.Click += (_, __) => AjusterPointage(-1m);
            btnQuitter.Click += (_, __) => QuitterProgramme();
        }

        public event EventHandler<NouvellePartieDemandeeEventArgs> NouvellePartieDemandee;

        public event EventHandler ChargerPartieDemandee;

        public event EventHandler<AjustementPointageEventArgs> AjustementPointageDemande;

        public event EventHandler QuitterDemandee;

        public void Afficher()
        {
            Show();
        }

        public void AfficherJoueurs(IReadOnlyList<Joueur> joueurs)
        {
            string joueurListeSelectionne = lstJoueurs.SelectedItem as string;
            string joueurBlancSelectionne = cmbJoueurBlanc.SelectedItem as string;
            string joueurNoirSelectionne = cmbJoueurNoir.SelectedItem as string;
            List<string> lignes = joueurs
                .Select(joueur => joueur.Nom + " - ELO " + joueur.Elo + " - Pointage " + joueur.Pointage.ToString("0.0"))
                .ToList();
            List<string> noms = joueurs.Select(joueur => joueur.Nom).ToList();

            lstJoueurs.BeginUpdate();
            lstJoueurs.Items.Clear();
            lstJoueurs.Items.AddRange(lignes.ToArray());
            lstJoueurs.EndUpdate();

            cmbJoueurBlanc.BeginUpdate();
            cmbJoueurNoir.BeginUpdate();
            cmbJoueurBlanc.Items.Clear();
            cmbJoueurNoir.Items.Clear();
            cmbJoueurBlanc.Items.AddRange(noms.ToArray());
            cmbJoueurNoir.Items.AddRange(noms.ToArray());
            cmbJoueurBlanc.EndUpdate();
            cmbJoueurNoir.EndUpdate();

            RestaurerSelectionListe(joueurListeSelectionne);
            RestaurerSelectionCombo(cmbJoueurBlanc, joueurBlancSelectionne, 0);
            RestaurerSelectionCombo(cmbJoueurNoir, joueurNoirSelectionne, noms.Count > 1 ? 1 : 0);
        }

        public void ChoisirNouvellePartie()
        {
            string joueurBlanc = cmbJoueurBlanc.SelectedItem as string;
            string joueurNoir = cmbJoueurNoir.SelectedItem as string;

            if (string.IsNullOrWhiteSpace(joueurBlanc) || string.IsNullOrWhiteSpace(joueurNoir))
            {
                MessageBox.Show("Selectionnez deux joueurs pour demarrer une partie.", "Nouvelle partie", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.Equals(joueurBlanc, joueurNoir, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Les joueurs blanc et noir doivent etre differents.", "Nouvelle partie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (NouvellePartieDemandee != null)
            {
                NouvellePartieDemandee(this, new NouvellePartieDemandeeEventArgs(joueurBlanc, joueurNoir));
            }
        }

        public void ChoisirChargerPartie()
        {
            if (ChargerPartieDemandee != null)
            {
                ChargerPartieDemandee(this, EventArgs.Empty);
            }
        }

        public void AjusterPointage(decimal delta)
        {
            string nom = ObtenirNomJoueurSelectionne();

            if (string.IsNullOrWhiteSpace(nom))
            {
                MessageBox.Show("Selectionnez un joueur dans la liste.", "Pointage", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (AjustementPointageDemande != null)
            {
                AjustementPointageDemande(this, new AjustementPointageEventArgs(nom, delta));
            }
        }

        public void QuitterProgramme()
        {
            if (QuitterDemandee != null)
            {
                QuitterDemandee(this, EventArgs.Empty);
            }
        }

        private string ObtenirNomJoueurSelectionne()
        {
            string ligne = lstJoueurs.SelectedItem as string;

            if (string.IsNullOrWhiteSpace(ligne))
            {
                return string.Empty;
            }

            int index = ligne.IndexOf(" - ", StringComparison.Ordinal);
            return index > 0 ? ligne.Substring(0, index) : ligne;
        }

        private void RestaurerSelectionListe(string ligneSelectionnee)
        {
            if (!string.IsNullOrWhiteSpace(ligneSelectionnee))
            {
                int index = lstJoueurs.Items.IndexOf(ligneSelectionnee);

                if (index >= 0)
                {
                    lstJoueurs.SelectedIndex = index;
                    return;
                }
            }

            if (lstJoueurs.Items.Count > 0)
            {
                lstJoueurs.SelectedIndex = 0;
            }
        }

        private static void RestaurerSelectionCombo(ComboBox comboBox, string valeur, int indexParDefaut)
        {
            if (!string.IsNullOrWhiteSpace(valeur) && comboBox.Items.Contains(valeur))
            {
                comboBox.SelectedItem = valeur;
                return;
            }

            if (comboBox.Items.Count > indexParDefaut)
            {
                comboBox.SelectedIndex = indexParDefaut;
            }
        }
    }

    public class NouvellePartieDemandeeEventArgs : EventArgs
    {
        public NouvellePartieDemandeeEventArgs(string joueurBlanc, string joueurNoir)
        {
            JoueurBlanc = joueurBlanc;
            JoueurNoir = joueurNoir;
        }

        public string JoueurBlanc { get; private set; }

        public string JoueurNoir { get; private set; }
    }

    public class AjustementPointageEventArgs : EventArgs
    {
        public AjustementPointageEventArgs(string nomJoueur, decimal delta)
        {
            NomJoueur = nomJoueur;
            Delta = delta;
        }

        public string NomJoueur { get; private set; }

        public decimal Delta { get; private set; }
    }
}
