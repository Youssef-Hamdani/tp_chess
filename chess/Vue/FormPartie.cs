using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace chess
{
    /// <summary>
    /// Fenetre principale d'une partie en cours.
    /// </summary>
    public partial class FormPartie : Form
    {
        private bool departSelectionne;
        private readonly string[,] codesCases = new string[8, 8];
        private readonly Dictionary<string, Image> imagesPieces = new Dictionary<string, Image>();
        private readonly Dictionary<string, Image> imagesPiecesCapturables = new Dictionary<string, Image>();
        private readonly Image imageCaseVide = CreerImageVide();
        private readonly Image imageCoupPossible = CreerImageCoupPossible();
        private readonly Color caseClaire = Color.FromArgb(247, 240, 221);
        private readonly Color caseFoncee = Color.FromArgb(212, 120, 32);
        private readonly Color caseSelectionnee = Color.FromArgb(188, 214, 141);
        private readonly Color caseRoiEnEchec = Color.FromArgb(221, 91, 91);
        private string couleurJoueurCourant = "Blanc";
        private string infoJoueurBlanc = "Blancs : -";
        private string infoJoueurNoir = "Noirs : -";
        private bool plateauInverse;
        private Position positionSelectionnee;
        private Position positionRoiEnEchec;
        private readonly List<Position> coupsPossibles = new List<Position>();

        public FormPartie()
        {
            InitializeComponent();
            InitialiserImagesPieces();
            InitialiserGrille();
            btnJouerCoup.Click += BtnJouerCoup_Click;
            btnSauvegarderPartie.Click += BtnSauvegarderPartie_Click;
            btnAbandonner.Click += BtnAbandonner_Click;
            btnDemanderNulle.Click += BtnDemanderNulle_Click;
            btnQuitter.Click += BtnQuitter_Click;
            chkRetournerSelonTour.CheckedChanged += ChkRetournerSelonTour_CheckedChanged;
            dgvPlateau.CellMouseClick += DgvPlateau_CellMouseClick;
        }

        public event EventHandler<CoupEventArgs> CoupSoumis;
        public event EventHandler SauvegardeDemandee;
        public event EventHandler AbandonDemande;
        public event EventHandler NulleDemandee;
        public event EventHandler QuitterDemande;

        public Func<Position, IList<Position>> CoupsPossiblesDemandes { get; set; }

        /// <summary>
        /// Met a jour le contenu du plateau a partir d'une representation serialisee simple.
        /// </summary>
        public void AfficherPlateau(string plateauSerialise)
        {
            ReinitialiserSelection();
            string[] lignes = plateauSerialise.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            for (int ligne = 0; ligne < 8; ligne++)
            {
                string[] colonnes = lignes[ligne].Split(';');

                for (int colonne = 0; colonne < 8; colonne++)
                {
                    string code = colonnes[colonne] == "__" ? string.Empty : colonnes[colonne];
                    codesCases[ligne, colonne] = code;
                }
            }

            RafraichirPlateau();
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

        /// <summary>
        /// Met a jour le texte d'etat et l'orientation dependant du joueur courant.
        /// </summary>
        public void MettreAJourEtat(string etat)
        {
            lblTour.Text = "Etat : " + etat;
            couleurJoueurCourant = etat.IndexOf("Noir", StringComparison.OrdinalIgnoreCase) >= 0 ? "Noir" : "Blanc";
            AppliquerOrientationSelonTour();
            RafraichirInformationsJoueurs();
            RafraichirPlateau();
        }

        public void MettreAJourJoueurs(string joueurBlanc, string joueurNoir)
        {
            infoJoueurBlanc = string.IsNullOrWhiteSpace(joueurBlanc) ? "Blancs : -" : joueurBlanc;
            infoJoueurNoir = string.IsNullOrWhiteSpace(joueurNoir) ? "Noirs : -" : joueurNoir;
            RafraichirInformationsJoueurs();
        }

        public void DefinirInteractionActive(bool active)
        {
            dgvPlateau.Enabled = active;
            btnJouerCoup.Enabled = active;
            btnAbandonner.Enabled = active;
            btnDemanderNulle.Enabled = active;
        }

        public void MettreEnEvidenceRoiEnEchec(Position position)
        {
            positionRoiEnEchec = position != null ? new Position(position) : null;
            RafraichirPlateau();
        }

        private void InitialiserGrille()
        {
            dgvPlateau.Columns.Clear();
            dgvPlateau.Rows.Clear();

            for (int colonne = 0; colonne < 8; colonne++)
            {
                DataGridViewImageColumn col = new DataGridViewImageColumn();
                col.HeaderText = string.Empty;
                col.Width = 44;
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.ImageLayout = DataGridViewImageCellLayout.Zoom;
                dgvPlateau.Columns.Add(col);
            }

            dgvPlateau.Rows.Add(8);

            for (int ligne = 0; ligne < 8; ligne++)
            {
                dgvPlateau.Rows[ligne].HeaderCell.Value = string.Empty;
                dgvPlateau.Rows[ligne].Height = 44;
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
        }

        private void BtnSauvegarderPartie_Click(object sender, EventArgs e)
        {
            if (SauvegardeDemandee != null)
            {
                SauvegardeDemandee(this, EventArgs.Empty);
            }
        }

        private void BtnAbandonner_Click(object sender, EventArgs e)
        {
            if (AbandonDemande != null)
            {
                AbandonDemande(this, EventArgs.Empty);
            }
        }

        private void BtnDemanderNulle_Click(object sender, EventArgs e)
        {
            if (NulleDemandee != null)
            {
                NulleDemandee(this, EventArgs.Empty);
            }
        }

        private void BtnQuitter_Click(object sender, EventArgs e)
        {
            if (QuitterDemande != null)
            {
                QuitterDemande(this, EventArgs.Empty);
            }
        }

        private void ChkRetournerSelonTour_CheckedChanged(object sender, EventArgs e)
        {
            AppliquerOrientationSelonTour();
            RafraichirInformationsJoueurs();
            RafraichirPlateau();
        }

        private void DgvPlateau_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            if (e.Button == MouseButtons.Right)
            {
                ReinitialiserSelection();
                RafraichirPlateau();
                AfficherMessage("Selection effacee.");
                return;
            }

            Position positionCliquee = ConvertirVersPositionPlateau(e.RowIndex, e.ColumnIndex);
            string piece = codesCases[positionCliquee.Ligne, positionCliquee.Colonne];

            if (departSelectionne && EstCoupPossible(positionCliquee))
            {
                nudArriveeLigne.Value = positionCliquee.Ligne;
                nudArriveeColonne.Value = positionCliquee.Colonne;
                SoumettreCoupDepuisSelection();
                return;
            }

            if (string.IsNullOrWhiteSpace(piece))
            {
                nudArriveeLigne.Value = positionCliquee.Ligne;
                nudArriveeColonne.Value = positionCliquee.Colonne;
                AfficherMessage("Arrivee selectionnee en (" + positionCliquee.Ligne + ", " + positionCliquee.Colonne + ").");
                RafraichirPlateau();
                return;
            }

            if (departSelectionne && EstPieceAdverse(piece))
            {
                nudArriveeLigne.Value = positionCliquee.Ligne;
                nudArriveeColonne.Value = positionCliquee.Colonne;
                AfficherMessage("Arrivee selectionnee : " + NomPiece(piece) + " en (" + positionCliquee.Ligne + ", " + positionCliquee.Colonne + ").");
                RafraichirPlateau();
                return;
            }

            if (ObtenirCouleur(piece) != couleurJoueurCourant)
            {
                AfficherMessage("Cette piece n'appartient pas au joueur courant.");
                RafraichirPlateau();
                return;
            }

            nudDepartLigne.Value = positionCliquee.Ligne;
            nudDepartColonne.Value = positionCliquee.Colonne;
            departSelectionne = true;
            positionSelectionnee = new Position(positionCliquee);
            MettreAJourCoupsPossibles();
            AfficherMessage("Depart selectionne : " + NomPiece(piece) + " en (" + positionCliquee.Ligne + ", " + positionCliquee.Colonne + ").");
            RafraichirPlateau();
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
                imagesPieces[code] = ChargerImagePiece(code);
                imagesPiecesCapturables[code] = CreerImageCapturable(imagesPieces[code]);
            }
        }

        private Image ChargerImagePiece(string code)
        {
            string chemin = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Assets",
                "Pieces",
                "png",
                code + ".png");

            if (!File.Exists(chemin))
            {
                return imageCaseVide;
            }

            using (Image source = Image.FromFile(chemin))
            {
                return new Bitmap(source);
            }
        }

        private static Image CreerImageVide()
        {
            Bitmap image = new Bitmap(8, 8);
            image.MakeTransparent();
            return image;
        }

        private static Image CreerImageCoupPossible()
        {
            Bitmap image = new Bitmap(92, 92);

            using (Graphics graphics = Graphics.FromImage(image))
            using (SolidBrush ombre = new SolidBrush(Color.FromArgb(45, 0, 0, 0)))
            using (SolidBrush cercle = new SolidBrush(Color.FromArgb(135, 60, 60, 60)))
            {
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                graphics.FillEllipse(ombre, 31, 33, 30, 30);
                graphics.FillEllipse(cercle, 33, 31, 26, 26);
            }

            return image;
        }

        private static Image CreerImageCapturable(Image imageSource)
        {
            Bitmap image = new Bitmap(imageSource.Width, imageSource.Height);

            using (Graphics graphics = Graphics.FromImage(image))
            using (SolidBrush halo = new SolidBrush(Color.FromArgb(110, 206, 54, 54)))
            using (Pen contour = new Pen(Color.FromArgb(175, 166, 22, 22), 4f))
            {
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                graphics.FillEllipse(halo, 6, 6, image.Width - 12, image.Height - 12);
                graphics.DrawEllipse(contour, 10, 10, image.Width - 20, image.Height - 20);
                graphics.DrawImage(imageSource, 0, 0, image.Width, image.Height);
            }

            return image;
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

        private void RafraichirPlateau()
        {
            AppliquerCouleursEchiquier();

            for (int ligneAffichage = 0; ligneAffichage < 8; ligneAffichage++)
            {
                for (int colonneAffichage = 0; colonneAffichage < 8; colonneAffichage++)
                {
                    Position position = ConvertirVersPositionPlateau(ligneAffichage, colonneAffichage);
                    string code = codesCases[position.Ligne, position.Colonne];
                    DataGridViewCell cellule = dgvPlateau[colonneAffichage, ligneAffichage];
                    bool estSelectionnee = positionSelectionnee != null && positionSelectionnee.Equals(position);
                    bool roiEnEchec = positionRoiEnEchec != null && positionRoiEnEchec.Equals(position);
                    bool estCoupPossible = EstCoupPossible(position);
                    bool estCapture = estCoupPossible && !string.IsNullOrWhiteSpace(code);
                    Color couleurCase = (position.Ligne + position.Colonne) % 2 == 0 ? caseClaire : caseFoncee;

                    // L'information d'echec doit rester visible, mais la case
                    // explicitement selectionnee par l'usager garde la priorite visuelle.
                    if (roiEnEchec)
                    {
                        couleurCase = caseRoiEnEchec;
                    }

                    if (estSelectionnee)
                    {
                        couleurCase = caseSelectionnee;
                    }

                    cellule.Style.BackColor = couleurCase;
                    cellule.Style.SelectionBackColor = couleurCase;

                    if (string.IsNullOrWhiteSpace(code))
                    {
                        cellule.Value = estCoupPossible ? imageCoupPossible : imageCaseVide;
                    }
                    else
                    {
                        cellule.Value = estCapture ? imagesPiecesCapturables[code] : imagesPieces[code];
                    }
                }
            }
        }

        private void ReinitialiserSelection()
        {
            departSelectionne = false;
            positionSelectionnee = null;
            coupsPossibles.Clear();
        }

        private void MettreAJourCoupsPossibles()
        {
            coupsPossibles.Clear();

            if (positionSelectionnee == null)
            {
                return;
            }

            string piece = codesCases[positionSelectionnee.Ligne, positionSelectionnee.Colonne];

            if (string.IsNullOrWhiteSpace(piece))
            {
                return;
            }

            if (CoupsPossiblesDemandes != null)
            {
                foreach (Position coup in CoupsPossiblesDemandes(new Position(positionSelectionnee)))
                {
                    coupsPossibles.Add(coup);
                }

                return;
            }

            for (int ligne = 0; ligne < 8; ligne++)
            {
                for (int colonne = 0; colonne < 8; colonne++)
                {
                    Position arrivee = new Position(ligne, colonne);

                    if (positionSelectionnee.Equals(arrivee))
                    {
                        continue;
                    }

                    if (PeutAtteindreCase(positionSelectionnee, arrivee, piece))
                    {
                        coupsPossibles.Add(arrivee);
                    }
                }
            }
        }

        private bool EstCoupPossible(Position position)
        {
            foreach (Position coupPossible in coupsPossibles)
            {
                if (coupPossible.Equals(position))
                {
                    return true;
                }
            }

            return false;
        }

        private void SoumettreCoupDepuisSelection()
        {
            AfficherMessage("Coup en cours...");

            if (CoupSoumis != null)
            {
                CoupSoumis(this, new CoupEventArgs(DemanderCoup()));
            }
        }

        private void AppliquerOrientationSelonTour()
        {
            plateauInverse = chkRetournerSelonTour.Checked
                && string.Equals(couleurJoueurCourant, "Noir", StringComparison.OrdinalIgnoreCase);
        }

        private void RafraichirInformationsJoueurs()
        {
            bool joueurBlancEnHaut = plateauInverse;

            lblJoueurHaut.Text = joueurBlancEnHaut ? infoJoueurBlanc : infoJoueurNoir;
            lblJoueurBas.Text = joueurBlancEnHaut ? infoJoueurNoir : infoJoueurBlanc;

            bool joueurBlancCourant = string.Equals(couleurJoueurCourant, "Blanc", StringComparison.OrdinalIgnoreCase);
            AppliquerStylePolice(lblJoueurHaut, (joueurBlancEnHaut == joueurBlancCourant) ? FontStyle.Bold : FontStyle.Regular);
            AppliquerStylePolice(lblJoueurBas, (joueurBlancEnHaut != joueurBlancCourant) ? FontStyle.Bold : FontStyle.Regular);
        }

        private static void AppliquerStylePolice(Label label, FontStyle style)
        {
            if (label.Font.Style == style)
            {
                return;
            }

            Font anciennePolice = label.Font;
            label.Font = new Font(anciennePolice, style);
            anciennePolice.Dispose();
        }

        private Position ConvertirVersPositionPlateau(int ligneAffichage, int colonneAffichage)
        {
            if (!plateauInverse)
            {
                return new Position(ligneAffichage, colonneAffichage);
            }

            return new Position(7 - ligneAffichage, 7 - colonneAffichage);
        }

        private bool PeutAtteindreCase(Position depart, Position arrivee, string piece)
        {
            string pieceDestination = codesCases[arrivee.Ligne, arrivee.Colonne];

            if (!string.IsNullOrWhiteSpace(pieceDestination) && ObtenirCouleur(pieceDestination) == ObtenirCouleur(piece))
            {
                return false;
            }

            switch (piece[0])
            {
                case 'P':
                    return MouvementPionValide(depart, arrivee, piece, pieceDestination);
                case 'T':
                    return MouvementTourValide(depart, arrivee) && !TrajetBloque(depart, arrivee);
                case 'C':
                    return MouvementCavalierValide(depart, arrivee);
                case 'F':
                    return MouvementFouValide(depart, arrivee) && !TrajetBloque(depart, arrivee);
                case 'D':
                    return MouvementReineValide(depart, arrivee) && !TrajetBloque(depart, arrivee);
                case 'R':
                    return MouvementRoiValide(depart, arrivee);
                default:
                    return false;
            }
        }

        private bool MouvementPionValide(Position depart, Position arrivee, string piece, string pieceDestination)
        {
            int direction = piece.EndsWith("B") ? -1 : 1;
            int ligneDepartInitiale = piece.EndsWith("B") ? 6 : 1;
            int deltaLigne = arrivee.Ligne - depart.Ligne;
            int deltaColonne = Math.Abs(arrivee.Colonne - depart.Colonne);

            if (deltaColonne == 0)
            {
                if (!string.IsNullOrWhiteSpace(pieceDestination))
                {
                    return false;
                }

                if (deltaLigne == direction)
                {
                    return true;
                }

                if (depart.Ligne == ligneDepartInitiale && deltaLigne == 2 * direction)
                {
                    return !TrajetBloque(depart, arrivee);
                }

                return false;
            }

            return deltaColonne == 1
                && deltaLigne == direction
                && !string.IsNullOrWhiteSpace(pieceDestination)
                && ObtenirCouleur(pieceDestination) != ObtenirCouleur(piece);
        }

        private static bool MouvementTourValide(Position depart, Position arrivee)
        {
            int deltaLigne = Math.Abs(arrivee.Ligne - depart.Ligne);
            int deltaColonne = Math.Abs(arrivee.Colonne - depart.Colonne);
            return (deltaLigne == 0 && deltaColonne > 0) || (deltaColonne == 0 && deltaLigne > 0);
        }

        private static bool MouvementCavalierValide(Position depart, Position arrivee)
        {
            int deltaLigne = Math.Abs(arrivee.Ligne - depart.Ligne);
            int deltaColonne = Math.Abs(arrivee.Colonne - depart.Colonne);
            return (deltaLigne == 2 && deltaColonne == 1) || (deltaLigne == 1 && deltaColonne == 2);
        }

        private static bool MouvementFouValide(Position depart, Position arrivee)
        {
            int deltaLigne = Math.Abs(arrivee.Ligne - depart.Ligne);
            int deltaColonne = Math.Abs(arrivee.Colonne - depart.Colonne);
            return deltaLigne == deltaColonne && deltaLigne > 0;
        }

        private static bool MouvementReineValide(Position depart, Position arrivee)
        {
            return MouvementTourValide(depart, arrivee) || MouvementFouValide(depart, arrivee);
        }

        private static bool MouvementRoiValide(Position depart, Position arrivee)
        {
            int deltaLigne = Math.Abs(arrivee.Ligne - depart.Ligne);
            int deltaColonne = Math.Abs(arrivee.Colonne - depart.Colonne);
            return deltaLigne <= 1 && deltaColonne <= 1 && (deltaLigne + deltaColonne > 0);
        }

        private bool TrajetBloque(Position depart, Position arrivee)
        {
            int pasLigne = Math.Sign(arrivee.Ligne - depart.Ligne);
            int pasColonne = Math.Sign(arrivee.Colonne - depart.Colonne);
            int ligneCourante = depart.Ligne + pasLigne;
            int colonneCourante = depart.Colonne + pasColonne;

            while (ligneCourante != arrivee.Ligne || colonneCourante != arrivee.Colonne)
            {
                if (!string.IsNullOrWhiteSpace(codesCases[ligneCourante, colonneCourante]))
                {
                    return true;
                }

                ligneCourante += pasLigne;
                colonneCourante += pasColonne;
            }

            return false;
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
