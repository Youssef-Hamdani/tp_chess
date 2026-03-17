using System;
using System.Windows.Forms;

namespace chess
{
    public partial class FormPartie : Form
    {
        public FormPartie()
        {
            InitializeComponent();
            InitialiserGrille();
            btnJouerCoup.Click += BtnJouerCoup_Click;
            dgvPlateau.CellClick += DgvPlateau_CellClick;
        }

        public event EventHandler<CoupEventArgs> CoupSoumis;

        public void AfficherPlateau(string plateauSerialise)
        {
            string[] lignes = plateauSerialise.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            for (int ligne = 0; ligne < 8; ligne++)
            {
                string[] colonnes = lignes[ligne].Split(';');

                for (int colonne = 0; colonne < 8; colonne++)
                {
                    dgvPlateau[colonne, ligne].Value = colonnes[colonne] == "__" ? string.Empty : colonnes[colonne];
                }
            }
        }

        public void AfficherMessage(string message)
        {
            lblMessage.Text = message;
        }

        public Coup DemanderCoup()
        {
            return new Coup(
                new Position((int)nudDepartLigne.Value, (int)nudDepartColonne.Value),
                new Position((int)nudArriveeLigne.Value, (int)nudArriveeColonne.Value));
        }

        public void MettreAJourTour(string joueur)
        {
            lblTour.Text = "Tour courant : " + joueur;
        }

        private void InitialiserGrille()
        {
            dgvPlateau.Columns.Clear();
            dgvPlateau.Rows.Clear();

            for (int colonne = 0; colonne < 8; colonne++)
            {
                DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                col.HeaderText = colonne.ToString();
                col.Width = 48;
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvPlateau.Columns.Add(col);
            }

            dgvPlateau.Rows.Add(8);

            for (int ligne = 0; ligne < 8; ligne++)
            {
                dgvPlateau.Rows[ligne].HeaderCell.Value = ligne.ToString();
                dgvPlateau.Rows[ligne].Height = 40;
            }
        }

        private void BtnJouerCoup_Click(object sender, EventArgs e)
        {
            if (CoupSoumis != null)
            {
                CoupSoumis(this, new CoupEventArgs(DemanderCoup()));
            }
        }

        private void DgvPlateau_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            object valeur = dgvPlateau[e.ColumnIndex, e.RowIndex].Value;
            string piece = valeur != null ? valeur.ToString() : string.Empty;

            if (string.IsNullOrWhiteSpace(piece))
            {
                AfficherMessage("Selectionne une case qui contient une piece pour le depart.");
                return;
            }

            nudDepartLigne.Value = e.RowIndex;
            nudDepartColonne.Value = e.ColumnIndex;
            AfficherMessage("Depart selectionne : " + piece + " en (" + e.RowIndex + ", " + e.ColumnIndex + ").");
        }
    }

    public class CoupEventArgs : EventArgs
    {
        public CoupEventArgs(Coup coup)
        {
            Coup = coup;
        }

        public Coup Coup { get; private set; }
    }
}
