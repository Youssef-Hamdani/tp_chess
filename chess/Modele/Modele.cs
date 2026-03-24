namespace chess
{
    public class Modele
    {
        public Partie PartieCourante { get; private set; }

        public void DemarrerPartie(Joueur joueurBlanc, Joueur joueurNoir)
        {
            PartieCourante = new Partie(joueurBlanc, joueurNoir);
        }

        public bool JouerCoup(Coup coup)
        {
            return PartieCourante != null && PartieCourante.JouerCoup(coup);
        }

        public RaisonCoupInvalide ValiderEtJouerCoup(Coup coup)
        {
            if (PartieCourante == null)
            {
                return RaisonCoupInvalide.CoupNull;
            }

            return PartieCourante.ValiderEtJouerCoup(coup);
        }

        public Partie ChargerPartie()
        {
            return PartieCourante;
        }

        public bool SauvegarderPartie()
        {
            return PartieCourante != null;
        }

        public Partie GetPartieCourante()
        {
            return PartieCourante;
        }
    }
}
