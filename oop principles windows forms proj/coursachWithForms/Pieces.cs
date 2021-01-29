using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coursachWithForms
{
    public enum ShogiPieces
    {
        None,
        King,
        Rook,
        Bishop,
        Gold,
        Silver,
        Knight,
        Lance,
        Pawn
    }
    public enum Promoted
    {
        Promoted,
        Unpromoted
    }
    [Serializable]
    public abstract class Piece
    {
        protected ShogiPieces pieceClass;
    }
    [Serializable]
    class JustPiece : Piece
    {
        public JustPiece(ShogiPieces pieceClass)
        {
            this.pieceClass = pieceClass;
        }
        public ShogiPieces getPieceType()
        {
            return pieceClass;
        }

    }
    [Serializable]
    class CertainPiece : Piece
    {
        JustPiece _piece;
        private Players player;
        protected Promoted promotion = Promoted.Unpromoted;
        public CertainPiece(JustPiece piece, Players player)
        {
            this._piece = piece;
            this.player = player;
        }
        public ShogiPieces getPieceType()
        {
            return _piece.getPieceType();
        }
        public Promoted getPromotion()
        {
            return promotion;
        }
        public void promote()
        {
            promotion = Promoted.Promoted;
        }
        public void depromote()
        {
            promotion = Promoted.Unpromoted;
        }
        public void setPlayer(Players player)
        {
            this.player = player;
        }
        public Players getPlayer()
        {
            return player;
        }
        public void switchPlayer()
        {
            if (this.player == Players.Player1)
                this.player = Players.Player2;
            else
                this.player = Players.Player1;
        }
    }

    class JustPieceFactory
    {
        private Hashtable pieces = new Hashtable();
        public JustPiece getPiece(ShogiPieces pieceClass)
        {
            JustPiece piece = pieces[pieceClass] as JustPiece;
            if (piece == null)
            {
                piece = new JustPiece(pieceClass);
                pieces.Add(pieceClass, piece);
            }
            return piece;
        }
    }
}
