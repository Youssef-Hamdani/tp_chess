using System;
using System.Windows.Forms;

namespace chess
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            EchecControlleur controleur = new EchecControlleur();
            Application.Run(controleur.MenuPrincipal);
        }
    }
}
