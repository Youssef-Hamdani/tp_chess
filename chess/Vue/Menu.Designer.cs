namespace chess
{
    partial class Menu
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.Button btnNouvellePartie;
        private System.Windows.Forms.Button btnChargerPartie;

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
            this.SuspendLayout();
            // 
            // lblTitre
            // 
            this.lblTitre.AutoSize = true;
            this.lblTitre.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitre.Location = new System.Drawing.Point(86, 32);
            this.lblTitre.Name = "lblTitre";
            this.lblTitre.Size = new System.Drawing.Size(214, 32);
            this.lblTitre.TabIndex = 0;
            this.lblTitre.Text = "Jeu d'echecs MVC";
            // 
            // btnNouvellePartie
            // 
            this.btnNouvellePartie.Location = new System.Drawing.Point(92, 100);
            this.btnNouvellePartie.Name = "btnNouvellePartie";
            this.btnNouvellePartie.Size = new System.Drawing.Size(208, 40);
            this.btnNouvellePartie.TabIndex = 1;
            this.btnNouvellePartie.Text = "Nouvelle partie";
            this.btnNouvellePartie.UseVisualStyleBackColor = true;
            // 
            // btnChargerPartie
            // 
            this.btnChargerPartie.Location = new System.Drawing.Point(92, 156);
            this.btnChargerPartie.Name = "btnChargerPartie";
            this.btnChargerPartie.Size = new System.Drawing.Size(208, 40);
            this.btnChargerPartie.TabIndex = 2;
            this.btnChargerPartie.Text = "Charger partie";
            this.btnChargerPartie.UseVisualStyleBackColor = true;
            // 
            // Menu
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 247);
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
