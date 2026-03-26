namespace chess
{
    partial class FormPartie
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvPlateau;
        private System.Windows.Forms.NumericUpDown nudDepartLigne;
        private System.Windows.Forms.NumericUpDown nudDepartColonne;
        private System.Windows.Forms.NumericUpDown nudArriveeLigne;
        private System.Windows.Forms.NumericUpDown nudArriveeColonne;
        private System.Windows.Forms.Button btnJouerCoup;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblTour;
        private System.Windows.Forms.Label lblDepart;
        private System.Windows.Forms.Label lblArrivee;
        private System.Windows.Forms.Button btnSauvegarderPartie;
        private System.Windows.Forms.CheckBox chkRetournerSelonTour;

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
            this.dgvPlateau = new System.Windows.Forms.DataGridView();
            this.nudDepartLigne = new System.Windows.Forms.NumericUpDown();
            this.nudDepartColonne = new System.Windows.Forms.NumericUpDown();
            this.nudArriveeLigne = new System.Windows.Forms.NumericUpDown();
            this.nudArriveeColonne = new System.Windows.Forms.NumericUpDown();
            this.btnJouerCoup = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblTour = new System.Windows.Forms.Label();
            this.lblDepart = new System.Windows.Forms.Label();
            this.lblArrivee = new System.Windows.Forms.Label();
            this.btnSauvegarderPartie = new System.Windows.Forms.Button();
            this.chkRetournerSelonTour = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlateau)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDepartLigne)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDepartColonne)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudArriveeLigne)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudArriveeColonne)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvPlateau
            // 
            this.dgvPlateau.AllowUserToAddRows = false;
            this.dgvPlateau.AllowUserToDeleteRows = false;
            this.dgvPlateau.AllowUserToResizeColumns = false;
            this.dgvPlateau.AllowUserToResizeRows = false;
            this.dgvPlateau.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPlateau.Location = new System.Drawing.Point(12, 12);
            this.dgvPlateau.MultiSelect = false;
            this.dgvPlateau.Name = "dgvPlateau";
            this.dgvPlateau.ReadOnly = true;
            this.dgvPlateau.RowHeadersWidth = 60;
            this.dgvPlateau.RowTemplate.Height = 25;
            this.dgvPlateau.Size = new System.Drawing.Size(356, 356);
            this.dgvPlateau.TabIndex = 0;
            // 
            // nudDepartLigne
            // 
            this.nudDepartLigne.Location = new System.Drawing.Point(402, 82);
            this.nudDepartLigne.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.nudDepartLigne.Name = "nudDepartLigne";
            this.nudDepartLigne.Size = new System.Drawing.Size(64, 20);
            this.nudDepartLigne.TabIndex = 1;
            // 
            // nudDepartColonne
            // 
            this.nudDepartColonne.Location = new System.Drawing.Point(483, 82);
            this.nudDepartColonne.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.nudDepartColonne.Name = "nudDepartColonne";
            this.nudDepartColonne.Size = new System.Drawing.Size(64, 20);
            this.nudDepartColonne.TabIndex = 2;
            // 
            // nudArriveeLigne
            // 
            this.nudArriveeLigne.Location = new System.Drawing.Point(402, 146);
            this.nudArriveeLigne.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.nudArriveeLigne.Name = "nudArriveeLigne";
            this.nudArriveeLigne.Size = new System.Drawing.Size(64, 20);
            this.nudArriveeLigne.TabIndex = 3;
            // 
            // nudArriveeColonne
            // 
            this.nudArriveeColonne.Location = new System.Drawing.Point(483, 146);
            this.nudArriveeColonne.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.nudArriveeColonne.Name = "nudArriveeColonne";
            this.nudArriveeColonne.Size = new System.Drawing.Size(64, 20);
            this.nudArriveeColonne.TabIndex = 4;
            // 
            // btnJouerCoup
            // 
            this.btnJouerCoup.Location = new System.Drawing.Point(402, 196);
            this.btnJouerCoup.Name = "btnJouerCoup";
            this.btnJouerCoup.Size = new System.Drawing.Size(145, 35);
            this.btnJouerCoup.TabIndex = 5;
            this.btnJouerCoup.Text = "Jouer le coup";
            this.btnJouerCoup.UseVisualStyleBackColor = true;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(399, 258);
            this.lblMessage.MaximumSize = new System.Drawing.Size(180, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(103, 13);
            this.lblMessage.TabIndex = 6;
            this.lblMessage.Text = "Pret a jouer un coup";
            // 
            // lblTour
            // 
            this.lblTour.AutoSize = true;
            this.lblTour.Location = new System.Drawing.Point(399, 24);
            this.lblTour.MaximumSize = new System.Drawing.Size(180, 0);
            this.lblTour.Name = "lblTour";
            this.lblTour.Size = new System.Drawing.Size(68, 13);
            this.lblTour.TabIndex = 7;
            this.lblTour.Text = "Etat";
            // 
            // lblDepart
            // 
            this.lblDepart.AutoSize = true;
            this.lblDepart.Location = new System.Drawing.Point(402, 61);
            this.lblDepart.Name = "lblDepart";
            this.lblDepart.Size = new System.Drawing.Size(114, 13);
            this.lblDepart.TabIndex = 8;
            this.lblDepart.Text = "Depart (ligne, colonne)";
            // 
            // lblArrivee
            // 
            this.lblArrivee.AutoSize = true;
            this.lblArrivee.Location = new System.Drawing.Point(402, 125);
            this.lblArrivee.Name = "lblArrivee";
            this.lblArrivee.Size = new System.Drawing.Size(115, 13);
            this.lblArrivee.TabIndex = 9;
            this.lblArrivee.Text = "Arrivee (ligne, colonne)";
            // 
            // btnSauvegarderPartie
            // 
            this.btnSauvegarderPartie.Location = new System.Drawing.Point(402, 237);
            this.btnSauvegarderPartie.Name = "btnSauvegarderPartie";
            this.btnSauvegarderPartie.Size = new System.Drawing.Size(145, 30);
            this.btnSauvegarderPartie.TabIndex = 10;
            this.btnSauvegarderPartie.Text = "Sauvegarder";
            this.btnSauvegarderPartie.UseVisualStyleBackColor = true;
            // 
            // chkRetournerSelonTour
            // 
            this.chkRetournerSelonTour.AutoSize = true;
            this.chkRetournerSelonTour.Checked = true;
            this.chkRetournerSelonTour.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRetournerSelonTour.Location = new System.Drawing.Point(402, 283);
            this.chkRetournerSelonTour.Name = "chkRetournerSelonTour";
            this.chkRetournerSelonTour.Size = new System.Drawing.Size(134, 17);
            this.chkRetournerSelonTour.TabIndex = 11;
            this.chkRetournerSelonTour.Text = "Retourner selon le tour";
            this.chkRetournerSelonTour.UseVisualStyleBackColor = true;
            // 
            // FormPartie
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 381);
            this.Controls.Add(this.chkRetournerSelonTour);
            this.Controls.Add(this.btnSauvegarderPartie);
            this.Controls.Add(this.lblArrivee);
            this.Controls.Add(this.lblDepart);
            this.Controls.Add(this.lblTour);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnJouerCoup);
            this.Controls.Add(this.nudArriveeColonne);
            this.Controls.Add(this.nudArriveeLigne);
            this.Controls.Add(this.nudDepartColonne);
            this.Controls.Add(this.nudDepartLigne);
            this.Controls.Add(this.dgvPlateau);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FormPartie";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Partie d\'echecs";
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlateau)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDepartLigne)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDepartColonne)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudArriveeLigne)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudArriveeColonne)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
