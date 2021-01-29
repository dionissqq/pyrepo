using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace coursachWithForms
{
    [Serializable]
    abstract class PieceState
    {
        public abstract bool hasPiece();
    }
    [Serializable]
    class WPieceState : PieceState
    {
        public override bool hasPiece()
        {
            return true;
        }
    }
    [Serializable]
    class WithoutPieceState : PieceState
    {
        public override bool hasPiece()
        {
            return false;
        }
    }
    [Serializable]
    abstract class AbstractSquare
    {
        abstract public void setPiece(CertainPiece piece);
        abstract public void removePiece();
        abstract public CertainPiece getPiece();
        abstract public bool getPieceState();
    }

    [Serializable]
    class SquareWithotCoordinates : AbstractSquare
    {
        private PieceState _pieceState = new WithoutPieceState();
        private CertainPiece _piece;
        public override void setPiece(CertainPiece piece)
        {
            _piece = piece;
            _pieceState = new WPieceState();
        }
        public override CertainPiece getPiece()
        {
            return this._piece;
        }
        public override void removePiece()
        {
            _piece = null;
            _pieceState = new WithoutPieceState();
        }
        public override bool getPieceState()
        {
            return _pieceState.hasPiece();
        }
        public Players getPiecePlayer()
        {
            return _piece.getPlayer();
        }
        public SquareWithotCoordinates copy()
        {
            SquareWithotCoordinates sq;
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream("data.bin", FileMode.Create))
            {
                formatter.Serialize(stream, this);
            }

            using (Stream stream = new FileStream("data.bin", FileMode.Open))
            {
                sq = (SquareWithotCoordinates)formatter.Deserialize(stream);
            }
            return sq;
        }
    }
    class Square : AbstractSquare
    {
        
        private PossibleMovesStrategy strategy;
        public int x;
        public int y;
        SquareWithotCoordinates _square;
        public Square(SquareWithotCoordinates sq)
        {
            _square = sq;
        }
        public List<Square> getPossibleMoves(MainBoard board)
        {
            List<Square> squares = new List<Square>();
            if (this.getPieceState())
            {
                strategy = Globals.factory.getPossibleMovesStrategy(getPiece());
                squares = strategy.possibleMoves(board, this);
            }
            return squares;
        }
        public override CertainPiece getPiece()
        {
            return this._square.getPiece();
        }
        public override void setPiece(CertainPiece piece)
        {
            if (_square != null)
                _square.setPiece(piece);
        }
        public override void removePiece()
        {
            if (_square != null)
                _square.removePiece();
        }
        public override bool getPieceState()
        {
            if (_square != null)
                return _square.getPieceState();
            else
                return new WithoutPieceState().hasPiece();
        }
        public Players getOwner()
        {
            return _square.getPiecePlayer();
        }
    }
}
