using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coursachWithForms
{
    abstract class Board
    {
        protected List<List<Square>> matrix = new List<List<Square>>();
        abstract public void init(string filename);
        abstract public Square getAt(int x, int y);

    }
    class MainBoard : Board
    {
        public string type = "main";

        public override Square getAt(int x, int y)
        {
        return matrix[x][y];
        }
        public override void init(string filename)
        {

        }
        public void initDefault()
        {
            ShogiPieces[,] pieces = new ShogiPieces[9, 9] {
                {ShogiPieces.Lance , ShogiPieces.Knight,ShogiPieces.Silver,ShogiPieces.Gold,ShogiPieces.King,ShogiPieces.Gold,ShogiPieces.Silver,ShogiPieces.Knight,ShogiPieces.Lance},
                {ShogiPieces.None,ShogiPieces.Bishop,ShogiPieces.None,ShogiPieces.None,ShogiPieces.None,ShogiPieces.None,ShogiPieces.None,ShogiPieces.Rook ,ShogiPieces.None },
                {ShogiPieces.Pawn,ShogiPieces.Pawn,ShogiPieces.Pawn,ShogiPieces.Pawn,ShogiPieces.Pawn,ShogiPieces.Pawn,ShogiPieces.Pawn,ShogiPieces.Pawn,ShogiPieces.Pawn },
                {ShogiPieces.None,ShogiPieces.None,ShogiPieces.None,ShogiPieces.None,ShogiPieces.None,ShogiPieces.None,ShogiPieces.None,ShogiPieces.None,ShogiPieces.None },
                {ShogiPieces.None,ShogiPieces.None,ShogiPieces.None,ShogiPieces.None,ShogiPieces.None,ShogiPieces.None,ShogiPieces.None,ShogiPieces.None,ShogiPieces.None },
                {ShogiPieces.None,ShogiPieces.None,ShogiPieces.None,ShogiPieces.None,ShogiPieces.None,ShogiPieces.None,ShogiPieces.None,ShogiPieces.None,ShogiPieces.None },
                {ShogiPieces.Pawn,ShogiPieces.Pawn,ShogiPieces.Pawn,ShogiPieces.Pawn,ShogiPieces.Pawn,ShogiPieces.Pawn,ShogiPieces.Pawn,ShogiPieces.Pawn,ShogiPieces.Pawn },
                {ShogiPieces.None,ShogiPieces.Rook,ShogiPieces.None,ShogiPieces.None,ShogiPieces.None,ShogiPieces.None,ShogiPieces.None,ShogiPieces.Bishop ,ShogiPieces.None },
                {ShogiPieces.Lance , ShogiPieces.Knight,ShogiPieces.Silver,ShogiPieces.Gold,ShogiPieces.King,ShogiPieces.Gold,ShogiPieces.Silver,ShogiPieces.Knight,ShogiPieces.Lance}
            };

            JustPieceFactory factory = new JustPieceFactory();
            SquareWithotCoordinates sqWC = new SquareWithotCoordinates();

            for (int i = 0; i < 9; i++)
            {
                List<Square> list = new List<Square>();
                for (int j = 0; j < 9; j++)
                {
                    Players prayer;
                    if (i > 4)
                        prayer = Players.Player2;
                    else
                        prayer = Players.Player1;
                    SquareWithotCoordinates sqWt = sqWC.copy();
                    if (pieces[i, j] != ShogiPieces.None)
                        sqWt.setPiece(new CertainPiece(factory.getPiece(pieces[i, j]), prayer));
                    Square sqre = new Square(sqWt);
                    sqre.x = j;
                    sqre.y = i;
                    list.Add(sqre);
                }
                matrix.Add(list);
            }
        }
    }
    class sideBoard
    {
        public List<CertainPiece> items = new List<CertainPiece>();
        public Players owner;
        public List<Square> getPossibleMoves(MainBoard board, CertainPiece piece,Players player)
        {
            List<Square> list = new List<Square>();
            int y ;
            if (piece.getPieceType() == ShogiPieces.Knight)
                y = 7;
            else
            {
                if (piece.getPieceType() == ShogiPieces.Pawn)
                    y = 8;
                else
                    y = 9;
            }
            if (player == Players.Player1)
                for (int i = 0; i < 9; i++)
                {
                    bool cannot=false;
                    if (piece.getPieceType() == ShogiPieces.Pawn)
                        for (int j = 0; j < 9; j++)
                            if (board.getAt(j, i).getPieceState() && board.getAt(j, i).getPiece().getPieceType() == ShogiPieces.Pawn && board.getAt(j, i).getPiece().getPlayer()!=piece.getPlayer())
                            {
                                cannot = true;
                                break;
                            }
                                
                    if (cannot)
                        continue;
                    for (int j = 0; j < y; j++)
                    {
                        if (!board.getAt(j, i).getPieceState())
                            list.Add(board.getAt(j, i));
                    }
                }
            else
                for (int i = 0; i < 9; i++)
                {
                    bool cannot = false;
                    if (piece.getPieceType() == ShogiPieces.Pawn)
                        for (int j = 0; j < 9; j++)
                            if (board.getAt(j, i).getPieceState() && board.getAt(j, i).getPiece().getPieceType() == ShogiPieces.Pawn && board.getAt(j, i).getPiece().getPlayer()!=piece.getPlayer())
                            {
                                cannot = true;
                                break;
                            }

                    if (cannot)
                        continue;
                    for (int j = 8; j > 8-y; j--)
                    {
                        if (!board.getAt(j, i).getPieceState())
                            list.Add(board.getAt(j, i));
                    }
                }
            return list;
        }
    }
}
