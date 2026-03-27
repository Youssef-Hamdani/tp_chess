namespace chess
{
    partial class Menu
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.Button btnNouvellePartie;
        private System.Windows.Forms.Button btnChargerPartie;
        private System.Windows.Forms.Button btnAjouterPoint;
        private System.Windows.Forms.Button btnRetirerPoint;
        private System.Windows.Forms.Button btnQuitter;
        private System.Windows.Forms.ListBox lstJoueurs;
        private System.Windows.Forms.ComboBox cmbJoueurBlanc;
        private System.Windows.Forms.ComboBox cmbJoueurNoir;
        private System.Windows.Forms.Label lblJoueurs;
        private System.Windows.Forms.Label lblJoueurBlanc;
        private System.Windows.Forms.Label lblJoueurNoir;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitre = new System.Windows.Forms.Label();
            this.btnNouvellePartie = new System.Windows.Forms.Button();
            this.btnChargerPartie = new System.Windows.Forms.Button();
            this.btnAjouterPoint = new System.Windows.Forms.Button();
            this.btnRetirerPoint = new System.Windows.Forms.Button();
            this.btnQuitter = new System.Windows.Forms.Button();
            this.lstJoueurs = new System.Windows.Forms.ListBox();
            this.cmbJoueurBlanc = new System.Windows.Forms.ComboBox();
            this.cmbJoueurNoir = new System.Windows.Forms.ComboBox();
            this.lblJoueurs = new System.Windows.Forms.Label();
            this.lblJoueurBlanc = new System.Windows.Forms.Label();
            this.lblJoueurNoir = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTitre
            // 
            this.lblTitre.AutoSize = true;
            this.lblTitre.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitre.Location = new System.Drawing.Point(22, 16);
            this.lblTitre.Name = "lblTitre";
            this.lblTitre.Size = new System.Drawing.Size(212, 30);
            this.lblTitre.TabIndex = 0;
            this.lblTitre.Text = "Jeu d'echecs MVC";
            // 
            // btnNouvellePartie
            // 
            this.btnNouvellePartie.Location = new System.Drawing.Point(27, 303);
            this.btnNouvellePartie.Name = "btnNouvellePartie";
            this.btnNouvellePartie.Size = new System.Drawing.Size(183, 36);
            this.btnNouvellePartie.TabIndex = 1;
            this.btnNouvellePartie.Text = "Nouvelle partie";
            this.btnNouvellePartie.UseVisualStyleBackColor = true;
            // 
            // btnChargerPartie
            // 
            this.btnChargerPartie.Location = new System.Drawing.Point(216, 303);
            this.btnChargerPartie.Name = "btnChargerPartie";
            this.btnChargerPartie.Size = new System.Drawing.Size(183, 36);
            this.btnChargerPartie.TabIndex = 2;
            this.btnChargerPartie.Text = "Charger partie";
            this.btnChargerPartie.UseVisualStyleBackColor = true;
            // 
            // btnAjouterPoint
            // 
            this.btnAjouterPoint.Location = new System.Drawing.Point(27, 345);
            this.btnAjouterPoint.Name = "btnAjouterPoint";
            this.btnAjouterPoint.Size = new System.Drawing.Size(183, 30);
            this.btnAjouterPoint.TabIndex = 3;
            this.btnAjouterPoint.Text = "Ajouter 1 point";
            this.btnAjouterPoint.UseVisualStyleBackColor = true;
            // 
            // btnRetirerPoint
            // 
            this.btnRetirerPoint.Location = new System.Drawing.Point(216, 345);
            this.btnRetirerPoint.Name = "btnRetirerPoint";
            this.btnRetirerPoint.Size = new System.Drawing.Size(183, 30);
            this.btnRetirerPoint.TabIndex = 4;
            this.btnRetirerPoint.Text = "Retirer 1 point";
            this.btnRetirerPoint.UseVisualStyleBackColor = true;
            // 
            // btnQuitter
            // 
            this.btnQuitter.Location = new System.Drawing.Point(27, 381);
            this.btnQuitter.Name = "btnQuitter";
            this.btnQuitter.Size = new System.Drawing.Size(372, 32);
            this.btnQuitter.TabIndex = 5;
            this.btnQuitter.Text = "Quitter";
            this.btnQuitter.UseVisualStyleBackColor = true;
            // 
            // lstJoueurs
            // 
            this.lstJoueurs.FormattingEnabled = true;
            this.lstJoueurs.Location = new System.Drawing.Point(27, 79);
            this.lstJoueurs.Name = "lstJoueurs";
            this.lstJoueurs.Size = new System.Drawing.Size(372, 147);
            this.lstJoueurs.TabIndex = 6;
            // 
            // cmbJoueurBlanc
            // 
            this.cmbJoueurBlanc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbJoueurBlanc.FormattingEnabled = true;
            this.cmbJoueurBlanc.Location = new System.Drawing.Point(27, 261);
            this.cmbJoueurBlanc.Name = "cmbJoueurBlanc";
            this.cmbJoueurBlanc.Size = new System.Drawing.Size(177, 21);
            this.cmbJoueurBlanc.TabIndex = 7;
            // 
            // cmbJoueurNoir
            // 
            this.cmbJoueurNoir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbJoueurNoir.FormattingEnabled = true;
            this.cmbJoueurNoir.Location = new System.Drawing.Point(222, 261);
            this.cmbJoueurNoir.Name = "cmbJoueurNoir";
            this.cmbJoueurNoir.Size = new System.Drawing.Size(177, 21);
            this.cmbJoueurNoir.TabIndex = 8;
            // 
            // lblJoueurs
            // 
            this.lblJoueurs.AutoSize = true;
            this.lblJoueurs.Location = new System.Drawing.Point(24, 60);
            this.lblJoueurs.Name = "lblJoueurs";
            this.lblJoueurs.Size = new System.Drawing.Size(110, 13);
            this.lblJoueurs.TabIndex = 9;
            this.lblJoueurs.Text = "Liste et pointage";
            // 
            // lblJoueurBlanc
            // 
            this.lblJoueurBlanc.AutoSize = true;
            this.lblJoueurBlanc.Location = new System.Drawing.Point(24, 245);
            this.lblJoueurBlanc.Name = "lblJoueurBlanc";
            this.lblJoueurBlanc.Size = new System.Drawing.Size(70, 13);
            this.lblJoueurBlanc.TabIndex = 10;
            this.lblJoueurBlanc.Text = "Joueur blanc";
            // 
            // lblJoueurNoir
            // 
            this.lblJoueurNoir.AutoSize = true;
            this.lblJoueurNoir.Location = new System.Drawing.Point(219, 245);
            this.lblJoueurNoir.Name = "lblJoueurNoir";
            this.lblJoueurNoir.Size = new System.Drawing.Size(63, 13);
            this.lblJoueurNoir.TabIndex = 11;
            this.lblJoueurNoir.Text = "Joueur noir";
            // 
            // Menu
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 430);
            this.Controls.Add(this.lblJoueurNoir);
            this.Controls.Add(this.lblJoueurBlanc);
            this.Controls.Add(this.lblJoueurs);
            this.Controls.Add(this.cmbJoueurNoir);
            this.Controls.Add(this.cmbJoueurBlanc);
            this.Controls.Add(this.lstJoueurs);
            this.Controls.Add(this.btnQuitter);
            this.Controls.Add(this.btnRetirerPoint);
            this.Controls.Add(this.btnAjouterPoint);
            this.Controls.Add(this.btnChargerPartie);
            this.Controls.Add(this.btnNouvellePartie);
            this.Controls.Add(this.lblTitre);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Menu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Menu";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
