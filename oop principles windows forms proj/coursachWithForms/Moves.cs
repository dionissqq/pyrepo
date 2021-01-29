using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coursachWithForms
{

    public enum MoveState
    {
        ChoosingPiece,
        ChoosingSquareForPiece,
    }
    class Move
    {
        MoveState _state = MoveState.ChoosingPiece;
        public Players player;
        public Square startingSquare;
        public CertainPiece piece;
        public List<Square> possibleSquares;

        public Move()
        {
            player = Players.Player1;
        }
        public MoveState getState()
        {
            return _state;
        }
        public void nextState()
        {
            if (_state == MoveState.ChoosingPiece)
                _state = MoveState.ChoosingSquareForPiece;
            else
                _state = MoveState.ChoosingPiece;
        }
        public void nextMove()
        {
            if (this.player == Players.Player1)
                this.player = Players.Player2;
            else
                this.player = Players.Player1;
            nextState();
        }
    }
    
}
