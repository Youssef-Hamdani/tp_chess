using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace chess
{
    public partial class FormPartie : Form
    {
        private bool departSelectionne;
        private readonly string[,] codesCases = new string[8, 8];
        private readonly Dictionary<string, Image> imagesPieces = new Dictionary<string, Image>();
        private readonly Color caseClaire = Color.FromArgb(247, 240, 221);
        private readonly Color caseFoncee = Color.FromArgb(212, 120, 32);

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
                col.HeaderText = string.Empty;
                col.Width = 56;
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.ImageLayout = DataGridViewImageCellLayout.Zoom;
                dgvPlateau.Columns.Add(col);
            }

            dgvPlateau.Rows.Add(8);

            for (int ligne = 0; ligne < 8; ligne++)
            {
                dgvPlateau.Rows[ligne].HeaderCell.Value = string.Empty;
                dgvPlateau.Rows[ligne].Height = 56;
            }

            dgvPlateau.BorderStyle = BorderStyle.None;
            dgvPlateau.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvPlateau.BackgroundColor = caseClaire;
            dgvPlateau.ColumnHeadersVisible = false;
            dgvPlateau.RowHeadersVisible = false;
            dgvPlateau.ScrollBars = ScrollBars.None;
            dgvPlateau.DefaultCellStyle.NullValue = null;
            dgvPlateau.DefaultCellStyle.SelectionBackColor = Color.FromArgb(134, 176, 92);
            dgvPlateau.DefaultCellStyle.SelectionForeColor = Color.Black;

            AppliquerCouleursEchiquier();
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
            Bitmap image = new Bitmap(52, 52);

            using (Graphics graphics = Graphics.FromImage(image))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                graphics.Clear(Color.Transparent);

                string glyphe = ObtenirGlyphePiece(code);
                GraphicsPath chemin = new GraphicsPath();
                StringFormat alignement = new StringFormat();
                alignement.Alignment = StringAlignment.Center;
                alignement.LineAlignment = StringAlignment.Center;

                chemin.AddString(
                    glyphe,
                    new FontFamily("Segoe UI Symbol"),
                    (int)FontStyle.Regular,
                    36F,
                    new Rectangle(1, 2, 50, 48),
                    alignement);

                if (code.EndsWith("B"))
                {
                    using (Pen contour = new Pen(Color.FromArgb(45, 45, 45), 2.2F))
                    using (Brush remplissage = new SolidBrush(Color.WhiteSmoke))
                    {
                        graphics.DrawPath(contour, chemin);
                        graphics.FillPath(remplissage, chemin);
                    }
                }
                else
                {
                    using (Pen contour = new Pen(Color.FromArgb(230, 230, 230), 1.5F))
                    using (Brush remplissage = new SolidBrush(Color.FromArgb(30, 30, 30)))
                    {
                        graphics.DrawPath(contour, chemin);
                        graphics.FillPath(remplissage, chemin);
                    }
                }
            }

            return image;
        }

        private string ObtenirGlyphePiece(string code)
        {
            switch (code)
            {
                case "PB":
                    return "\u2659";
                case "PN":
                    return "\u265F";
                case "TB":
                    return "\u2656";
                case "TN":
                    return "\u265C";
                case "CB":
                    return "\u2658";
                case "CN":
                    return "\u265E";
                case "FB":
                    return "\u2657";
                case "FN":
                    return "\u265D";
                case "DB":
                    return "\u2655";
                case "DN":
                    return "\u265B";
                case "RB":
                    return "\u2654";
                case "RN":
                    return "\u265A";
                default:
                    return string.Empty;
            }
        }

        private void AppliquerCouleursEchiquier()
        {
            for (int ligne = 0; ligne < 8; ligne++)
            {
                for (int colonne = 0; colonne < 8; colonne++)
                {
                    Color couleur = (ligne + colonne) % 2 == 0 ? caseClaire : caseFoncee;
                    dgvPlateau[colonne, ligne].Style.BackColor = couleur;
                    dgvPlateau[colonne, ligne].Style.SelectionBackColor = couleur;
                }
            }
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
