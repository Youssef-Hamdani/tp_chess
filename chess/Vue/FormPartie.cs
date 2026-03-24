using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace chess
{
    public partial class FormPartie : Form
    {
        private bool departSelectionne;
        private readonly string[,] codesCases = new string[8, 8];
        private readonly Dictionary<string, Image> imagesPieces = new Dictionary<string, Image>();

        public FormPartie()
        {
            InitializeComponent();
            InitialiserImagesPieces();
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
                    string code = colonnes[colonne] == "__" ? string.Empty : colonnes[colonne];
                    codesCases[ligne, colonne] = code;
                    dgvPlateau[colonne, ligne].Value = string.IsNullOrWhiteSpace(code) ? null : imagesPieces[code];
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

        public void MettreAJourEtat(string etat)
        {
            lblTour.Text = "Etat : " + etat;
        }

        private void InitialiserGrille()
        {
            dgvPlateau.Columns.Clear();
            dgvPlateau.Rows.Clear();

            for (int colonne = 0; colonne < 8; colonne++)
            {
                DataGridViewImageColumn col = new DataGridViewImageColumn();
                col.HeaderText = colonne.ToString();
                col.Width = 56;
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.ImageLayout = DataGridViewImageCellLayout.Zoom;
                dgvPlateau.Columns.Add(col);
            }

            dgvPlateau.Rows.Add(8);

            for (int ligne = 0; ligne < 8; ligne++)
            {
                dgvPlateau.Rows[ligne].HeaderCell.Value = ligne.ToString();
                dgvPlateau.Rows[ligne].Height = 56;
            }
        }

        private void BtnJouerCoup_Click(object sender, EventArgs e)
        {
            if (CoupSoumis != null)
            {
                CoupSoumis(this, new CoupEventArgs(DemanderCoup()));
            }

            departSelectionne = false;
        }

        private void DgvPlateau_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            string piece = codesCases[e.RowIndex, e.ColumnIndex];

            if (string.IsNullOrWhiteSpace(piece))
            {
                nudArriveeLigne.Value = e.RowIndex;
                nudArriveeColonne.Value = e.ColumnIndex;
                AfficherMessage("Arrivee selectionnee en (" + e.RowIndex + ", " + e.ColumnIndex + ").");
                return;
            }

            if (departSelectionne && EstPieceAdverse(piece))
            {
                nudArriveeLigne.Value = e.RowIndex;
                nudArriveeColonne.Value = e.ColumnIndex;
                AfficherMessage("Arrivee selectionnee : " + NomPiece(piece) + " en (" + e.RowIndex + ", " + e.ColumnIndex + ").");
                return;
            }

            nudDepartLigne.Value = e.RowIndex;
            nudDepartColonne.Value = e.ColumnIndex;
            departSelectionne = true;
            AfficherMessage("Depart selectionne : " + NomPiece(piece) + " en (" + e.RowIndex + ", " + e.ColumnIndex + ").");
        }

        private bool EstPieceAdverse(string pieceCliquee)
        {
            string pieceDepart = codesCases[(int)nudDepartLigne.Value, (int)nudDepartColonne.Value];

            if (string.IsNullOrWhiteSpace(pieceDepart))
            {
                return false;
            }

            return ObtenirCouleur(pieceDepart) != ObtenirCouleur(pieceCliquee);
        }

        private string ObtenirCouleur(string piece)
        {
            return piece.EndsWith("B") ? "Blanc" : piece.EndsWith("N") ? "Noir" : string.Empty;
        }

        private string NomPiece(string code)
        {
            switch (code)
            {
                case "PB":
                case "PN":
                    return "pion";
                case "TB":
                case "TN":
                    return "tour";
                case "CB":
                case "CN":
                    return "cavalier";
                case "FB":
                case "FN":
                    return "fou";
                case "DB":
                case "DN":
                    return "reine";
                case "RB":
                case "RN":
                    return "roi";
                default:
                    return "piece";
            }
        }

        private void InitialiserImagesPieces()
        {
            string[] codes = { "PB", "PN", "TB", "TN", "CB", "CN", "FB", "FN", "DB", "DN", "RB", "RN" };

            foreach (string code in codes)
            {
                imagesPieces[code] = CreerImagePiece(code);
            }
        }

        private Image CreerImagePiece(string code)
        {
            Bitmap image = new Bitmap(48, 48);

            using (Graphics graphics = Graphics.FromImage(image))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.Clear(Color.Transparent);

                Color fond = code.EndsWith("B") ? Color.FromArgb(244, 235, 218) : Color.FromArgb(77, 77, 77);
                Color contour = code.EndsWith("B") ? Color.FromArgb(110, 90, 70) : Color.FromArgb(220, 220, 220);
                Color texte = code.EndsWith("B") ? Color.FromArgb(60, 45, 35) : Color.WhiteSmoke;
                Rectangle cercle = new Rectangle(4, 4, 40, 40);

                using (Brush pinceauFond = new SolidBrush(fond))
                using (Pen pinceauContour = new Pen(contour, 2F))
                using (Brush pinceauTexte = new SolidBrush(texte))
                using (Font police = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Pixel))
                {
                    graphics.FillEllipse(pinceauFond, cercle);
                    graphics.DrawEllipse(pinceauContour, cercle);

                    StringFormat alignement = new StringFormat();
                    alignement.Alignment = StringAlignment.Center;
                    alignement.LineAlignment = StringAlignment.Center;

                    graphics.DrawString(code.Substring(0, 1), police, pinceauTexte, cercle, alignement);
                }
            }

            return image;
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
