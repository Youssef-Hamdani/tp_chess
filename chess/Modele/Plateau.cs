using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chess
{
    public class Plateau
    {
        private readonly List<Piece> pieces = new List<Piece>();

        public IReadOnlyList<Piece> Pieces
        {
            get { return pieces.AsReadOnly(); }
        }

        public void Initialiser()
        {
            pieces.Clear();

            AjouterPiecesPrincipales("Noir", 0);
            AjouterPions("Noir", 1);
            AjouterPions("Blanc", 6);
            AjouterPiecesPrincipales("Blanc", 7);
        }

        public Piece ObtenirPiece(Position pos)
        {
            return pieces.FirstOrDefault(piece =>
                piece.Position.Ligne == pos.Ligne && piece.Position.Colonne == pos.Colonne);
        }

        public void DeplacerPiece(Coup coup)
        {
            Piece piece = ObtenirPiece(coup.PositionDepart);

            if (piece != null)
            {
                piece.Bouger(new Position(coup.PositionArrivee.Ligne, coup.PositionArrivee.Colonne));
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            for (int ligne = 0; ligne < 8; ligne++)
            {
                for (int colonne = 0; colonne < 8; colonne++)
                {
                    Piece piece = ObtenirPiece(new Position(ligne, colonne));
                    builder.Append((piece != null ? piece.Symbole : "..").PadRight(3));
                }

                builder.AppendLine();
            }

            return builder.ToString();
        }

        public string SerialiserPourVue()
        {
            StringBuilder builder = new StringBuilder();

            for (int ligne = 0; ligne < 8; ligne++)
            {
                for (int colonne = 0; colonne < 8; colonne++)
                {
                    Piece piece = ObtenirPiece(new Position(ligne, colonne));
                    builder.Append(piece != null ? piece.Symbole : "__");

                    if (colonne < 7)
                    {
                        builder.Append(";");
                    }
                }

                if (ligne < 7)
                {
                    builder.AppendLine();
                }
            }

            return builder.ToString();
        }

        private void AjouterPiecesPrincipales(string couleur, int ligne)
        {
            pieces.Add(new Tour(couleur, new Position(ligne, 0)));
            pieces.Add(new Cavalier(couleur, new Position(ligne, 1)));
            pieces.Add(new Fou(couleur, new Position(ligne, 2)));
            pieces.Add(new Reine(couleur, new Position(ligne, 3)));
            pieces.Add(new Roi(couleur, new Position(ligne, 4)));
            pieces.Add(new Fou(couleur, new Position(ligne, 5)));
            pieces.Add(new Cavalier(couleur, new Position(ligne, 6)));
            pieces.Add(new Tour(couleur, new Position(ligne, 7)));
        }

        private void AjouterPions(string couleur, int ligne)
        {
            for (int colonne = 0; colonne < 8; colonne++)
            {
                pieces.Add(new Pion(couleur, new Position(ligne, colonne)));
            }
        }
    }
}
