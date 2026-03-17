using System;
using System.Windows.Forms;

namespace chess
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
            btnNouvellePartie.Click += (_, __) => ChoisirNouvellePartie();
            btnChargerPartie.Click += (_, __) => ChoisirChargerPartie();
        }

        public event EventHandler NouvellePartieDemandee;

        public event EventHandler ChargerPartieDemandee;

        public void Afficher()
        {
            Show();
        }

        public void ChoisirNouvellePartie()
        {
            if (NouvellePartieDemandee != null)
            {
                NouvellePartieDemandee(this, EventArgs.Empty);
            }
        }

        public void ChoisirChargerPartie()
        {
            if (ChargerPartieDemandee != null)
            {
                ChargerPartieDemandee(this, EventArgs.Empty);
            }
        }
    }
}
