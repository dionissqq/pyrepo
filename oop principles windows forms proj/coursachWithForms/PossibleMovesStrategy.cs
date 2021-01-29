using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coursachWithForms
{
    abstract class PossibleMovesStrategy
    {
        abstract public List<Square> possibleMoves(Board board, Square current);
    }
    class KingPossibleMovesStrategy : PossibleMovesStrategy
    {
        public override List<Square> possibleMoves(Board board, Square current)
        {
            List<Square> possibleMoves = new List<Square>();
            int x = current.x;
            int y = current.y;

            for (int i = x - 1; i <= x + 1; i++)
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (i < 9 && j < 9 && i >= 0 && j >= 0 && !(i == x && j == y) )
                    {
                        Square sq = board.getAt(j, i);
                        if (!sq.getPieceState() || (sq.getPieceState() && (sq.getOwner() != current.getOwner())))
                            possibleMoves.Add(sq);
                    }
                }
            return possibleMoves;
        }
    }
    class GoldPossibleMovesStrategy : PossibleMovesStrategy
    {
        public override List<Square> possibleMoves(Board board, Square current)
        {
            List<Square> possibleMoves = new List<Square>();
            int x = current.x;
            int y = current.y;
            if (Globals.move.player == Players.Player1)
            {
                for (int i = x - 1; i <= x + 1; i++)
                    for (int j = y - 1; j <= y + 1; j++)
                    {
                        if (!(i == x && j == y) && i >= 0 && j >= 0 && i < 9 && j < 9 && !(i == (x + 1) && j == (y - 1)) && !(i == (x - 1) && j == (y - 1)))
                        {
                            Square sq = board.getAt(j, i);
                            if (!sq.getPieceState() || (sq.getPieceState() && (sq.getOwner() != current.getOwner())))
                                possibleMoves.Add(sq);
                        }
                    }
            }
            else
            {
                for (int i = x - 1; i <= x + 1; i++)
                    for (int j = y - 1; j <= y + 1; j++)
                    {
                        if (!(i == x && j == y) && i >= 0 && j >= 0 && i < 9 && j < 9 && !(i == (x + 1) && j == (y + 1)) && !(i == (x - 1) && j == (y + 1)))
                        {
                            Square sq = board.getAt(j, i);
                            if (!sq.getPieceState() || (sq.getPieceState() && (sq.getOwner() != current.getOwner())))
                                possibleMoves.Add(sq);
                        }
                    }
            }
            return possibleMoves;
        }
    }
    class SilverPossibleMovesStrategy : PossibleMovesStrategy
    {
        public override List<Square> possibleMoves(Board board, Square current)
        {
            List<Square> possibleMoves = new List<Square>();
            int x = current.x;
            int y = current.y;
            if (Globals.move.player == Players.Player1) { 
                for (int i = x - 1; i <= x + 1; i++)
                    for (int j = y - 1; j <= y + 1; j++)
                    {
                        
                        if (!(i == x && j == y) && i >= 0 && j >= 0 && i < 9 && j < 9 && !(i == x && j == (y - 1)) && !(i == (x + 1) && j == y) && !(i == (x - 1) && j == y))
                        {
                            Square sq = board.getAt(j, i);
                            if( !sq.getPieceState() || (sq.getPieceState() && (sq.getOwner() != current.getOwner())))
                                possibleMoves.Add(sq);
                        }
                    }
            }
            else
            {
                for (int i = x - 1; i <= x + 1; i++)
                    for (int j = y - 1; j <= y + 1; j++)
                    {

                        if (!(i == x && j == y) && i >=0 && j>=0 && i < 9 && j < 9 && !(i == x && j == (y + 1)) && !(i == (x + 1) && j == y) && !(i == (x - 1) && j == y) )
                        {
                            Square sq = board.getAt(j, i);
                            if (!sq.getPieceState() || (sq.getPieceState() && (sq.getOwner() != current.getOwner())))
                                possibleMoves.Add(sq);
                        }
                    }
            }
            return possibleMoves;
        }
    }
    class PawnPossibleMovesStrategy : PossibleMovesStrategy
    {
        public override List<Square> possibleMoves(Board board, Square current)
        {
            
            List<Square> possibleMoves = new List<Square>();
            int x = current.x;
            int y = current.y;
            if (Globals.move.player == Players.Player1)
            {
                if (y  < 8)
                {
                    Square sq = board.getAt(y+1, x);
                    if (!sq.getPieceState() || (sq.getPieceState() && (sq.getOwner() != current.getOwner()))) {
                        possibleMoves.Add(sq);
                    }
                }
            }
            else
            {
                if (y - 1 >=0)
                {
                    Square sq = board.getAt(y-1, x);
                    if ((!sq.getPieceState() || (sq.getPieceState() && (sq.getOwner() != current.getOwner()))))
                        possibleMoves.Add(sq);
                }
            }
            return possibleMoves;
        }
    }
    class KnigthPossibleMovesStrategy : PossibleMovesStrategy
    {
        public override List<Square> possibleMoves(Board board, Square current)
        {
            List<Square> possibleMoves = new List<Square>();
            int x = current.x;
            int y = current.y;
            if(Globals.move.player == Players.Player1) { 
                if (y < 7 && x > 0)
                {
                    Square sq = board.getAt(y+2, x-1);
                    if (!sq.getPieceState() || (sq.getPieceState() && (sq.getOwner() != current.getOwner())))
                        possibleMoves.Add(sq);
                }
                if (y < 7 && x < 8)
                {
                    Square sq = board.getAt(y+2, x+1);
                    if ((!sq.getPieceState() || (sq.getPieceState() && (sq.getOwner() != current.getOwner()))))
                        possibleMoves.Add(sq);
                }
            }
            else
            {
                if (y >1 && x > 0)
                {
                    Square sq = board.getAt(y - 2, x - 1);
                    if ((!sq.getPieceState() || (sq.getPieceState() && (sq.getOwner() != current.getOwner()))))
                        possibleMoves.Add(sq);
                }
                if (y >1 && x < 8)
                {
                    Square sq = board.getAt(y - 2, x + 1);
                    if ((!sq.getPieceState() || (sq.getPieceState() && (sq.getOwner() != current.getOwner()))))
                        possibleMoves.Add(sq);
                }
            }
            return possibleMoves;
        }
    }
    class LancePossibleMovesStrategy : PossibleMovesStrategy
    {
        public override List<Square> possibleMoves(Board board, Square current)
        {
            List<Square> possibleMoves = new List<Square>();
            int x = current.x;
            int y = current.y;
            if (Globals.move.player == Players.Player1)
            {
                for (int i = y + 1; i < 9; i++)
                {
                    Square sq = board.getAt(i, x);
                    if (!sq.getPieceState())
                        possibleMoves.Add(sq);
                    if (sq.getPieceState())
                    {
                        if (sq.getOwner() != current.getOwner())
                            possibleMoves.Add(sq);
                        break;
                    }
                }
            }
            else
            {
                for (int i = y -1; i >= 0; i--)
                {
                    Square sq = board.getAt(i, x);
                    if (!sq.getPieceState())
                        possibleMoves.Add(sq);
                    if (sq.getPieceState())
                    {
                        if (sq.getOwner() != current.getOwner())
                            possibleMoves.Add(sq);
                        break;
                    }
                }
            }
            return possibleMoves;
        }
    }
    class RookPossibleMovesStrategy : PossibleMovesStrategy
    {
        public override List<Square> possibleMoves(Board board, Square current)
        {
            List<Square> possibleMoves = new List<Square>();
            int x = current.x;
            int y = current.y;

            for (int i = y + 1; i < 9; i++)
            {
                Square sq = board.getAt(i, x);
                if (!sq.getPieceState())
                    possibleMoves.Add(sq);
                if (sq.getPieceState())
                {
                    if (sq.getOwner() != current.getOwner())
                        possibleMoves.Add(sq);
                    break;
                }
            }
            for (int i = y - 1; i >= 0; i--)
            {
                Square sq = board.getAt(i, x);
                if (!sq.getPieceState())
                    possibleMoves.Add(sq);
                if (sq.getPieceState())
                {
                    if (sq.getOwner() != current.getOwner())
                        possibleMoves.Add(sq);
                    break;
                }
            }
            for (int i = x + 1; i < 9; i++)
            {
                Square sq = board.getAt(y, i);
                if (!sq.getPieceState())
                    possibleMoves.Add(sq);
                if (sq.getPieceState())
                {
                    if (sq.getOwner() != current.getOwner())
                        possibleMoves.Add(sq);
                    break;
                }
            }
            for (int i = x - 1; i >= 0; i--)
            {
                Square sq = board.getAt(y, i);
                if (!sq.getPieceState())
                    possibleMoves.Add(sq);
                if (sq.getPieceState())
                {
                    if (sq.getOwner() != current.getOwner())
                        possibleMoves.Add(sq);
                    break;
                }
            }

            return possibleMoves;
        }
    }
    class PromotedRookPossibleMovesStrategy : PossibleMovesStrategy
    {
        public override List<Square> possibleMoves(Board board, Square current)
        {
            List<Square> possibleMoves = new List<Square>();
            int x = current.x;
            int y = current.y;
            for (int i = y + 1; i < 9; i++)
            {
                Square sq = board.getAt(i, x);
                if (!sq.getPieceState())
                    possibleMoves.Add(sq);
                if (sq.getPieceState())
                {
                    if (sq.getOwner() != current.getOwner())
                        possibleMoves.Add(sq);
                    break;
                }
            }
            for (int i = y - 1; i >= 0; i--)
            {
                Square sq = board.getAt(i, x);
                if (!sq.getPieceState())
                    possibleMoves.Add(sq);
                if (sq.getPieceState())
                {
                    if (sq.getOwner() != current.getOwner())
                        possibleMoves.Add(sq);
                    break;
                }
            }
            for (int i = x + 1; i < 9; i++)
            {
                Square sq = board.getAt(y, i);
                if (!sq.getPieceState())
                    possibleMoves.Add(sq);
                if (sq.getPieceState())
                {
                    if (sq.getOwner() != current.getOwner())
                        possibleMoves.Add(sq);
                    break;
                }
            }
            for (int i = x - 1; i >= 0; i--)
            {
                Square sq = board.getAt(y, i);
                if (!sq.getPieceState())
                    possibleMoves.Add(sq);
                if (sq.getPieceState())
                {
                    if (sq.getOwner() != current.getOwner())
                        possibleMoves.Add(sq);
                    break;
                }
            }
            if (x < 8 && y < 8)
            {
                Square sq = board.getAt(y + 1, x + 1);
                if (!sq.getPieceState() || (sq.getPieceState() && (sq.getOwner() != current.getOwner())))
                    possibleMoves.Add(sq);
            }
            if (x < 8 && y > 0)
            {
                Square sq = board.getAt(y -1, x+1);
                if (!sq.getPieceState() || (sq.getPieceState() && (sq.getOwner() != current.getOwner())))
                    possibleMoves.Add(sq);
            }
            if (x > 0 && y < 8)
            {
                Square sq = board.getAt(y+1, x-1);
                if (!sq.getPieceState() || (sq.getPieceState() && (sq.getOwner() != current.getOwner())))
                    possibleMoves.Add(sq);
            }
            if (x > 0 && y > 0)
            {
                Square sq = board.getAt(y - 1, x - 1);
                if (!sq.getPieceState() || (sq.getPieceState() && (sq.getOwner() != current.getOwner())))
                    possibleMoves.Add(sq);
            }
            return possibleMoves;
        }
    }
    class BishopPossibleMovesStrategy : PossibleMovesStrategy
    {
        public override List<Square> possibleMoves(Board board, Square current)
        {
            List<Square> possibleMoves = new List<Square>();
            int x = current.x;
            int y = current.y;
            for (int i = 1; x + i < 9 && y + i < 9; i++)
            {
                Square sq = board.getAt(y+i, x + i);
                if (!sq.getPieceState())
                    possibleMoves.Add(sq);
                if (sq.getPieceState())
                {
                    if (sq.getOwner() != current.getOwner())
                        possibleMoves.Add(sq);
                    break;
                }
            }
            for (int i = 1; x - i >= 0 && y - i >= 0; i++)
            {
                Square sq = board.getAt(y - i, x - i);
                if (!sq.getPieceState())
                    possibleMoves.Add(sq);
                if (sq.getPieceState())
                {
                    if (sq.getOwner() != current.getOwner())
                        possibleMoves.Add(sq);
                    break;
                }
            }
            for (int i = 1; x + i < 9 && y - i >= 0; i++)
            {
                Square sq = board.getAt(y - i, x + i);
                if (!sq.getPieceState())
                    possibleMoves.Add(sq);
                if (sq.getPieceState())
                {
                    if (sq.getOwner() != current.getOwner())
                        possibleMoves.Add(sq);
                    break;
                }
            }
            for (int i = 1; x - i > 0 && y + i < 9; i++)
            {
                Square sq = board.getAt(y+ i, x-i);
                if (!sq.getPieceState())
                    possibleMoves.Add(sq);
                if (sq.getPieceState())
                {
                    if (sq.getOwner() != current.getOwner())
                        possibleMoves.Add(sq);
                    break;
                }
            }
            return possibleMoves;
        }
    }
    class PromotedBishopPossibleMovesStrategy : PossibleMovesStrategy
    {
        public override List<Square> possibleMoves(Board board, Square current)
        {
            List<Square> possibleMoves = new List<Square>();
            int x = current.x;
            int y = current.y;
            for (int i = 1; x + i < 9 && y + i < 9; i++)
            {
                Square sq = board.getAt(y + i, x + i);
                if (!sq.getPieceState())
                    possibleMoves.Add(sq);
                if (sq.getPieceState())
                {
                    if (sq.getOwner() != current.getOwner())
                        possibleMoves.Add(sq);
                    break;
                }
            }
            for (int i = 1; x - i >= 0 && y - i >= 0; i++)
            {
                Square sq = board.getAt(y - i, x - i);
                if (!sq.getPieceState())
                    possibleMoves.Add(sq);
                if (sq.getPieceState())
                {
                    if (sq.getOwner() != current.getOwner())
                        possibleMoves.Add(sq);
                    break;
                }
            }
            for (int i = 1; x + i < 9 && y - i >= 0; i++)
            {
                Square sq = board.getAt(y - i, x + i);
                if (!sq.getPieceState())
                    possibleMoves.Add(sq);
                if (sq.getPieceState())
                {
                    if (sq.getOwner() != current.getOwner())
                        possibleMoves.Add(sq);
                    break;
                }
            }
            for (int i = 1; x - i > 0 && y + i < 9; i++)
            {
                Square sq = board.getAt(y + i, x - i);
                if (!sq.getPieceState())
                    possibleMoves.Add(sq);
                if (sq.getPieceState())
                {
                    if (sq.getOwner() != current.getOwner())
                        possibleMoves.Add(sq);
                    break;
                }
            }
            if (x < 9 && y < 8)
            {
                Square sq = board.getAt(y+1,x);
                if (!sq.getPieceState() || (sq.getPieceState() && (sq.getOwner() != current.getOwner())))
                    possibleMoves.Add(sq);
            }
            if (x < 8 && y > 9)
            {
                Square sq = board.getAt(y, x+1);
                if (!sq.getPieceState() || (sq.getPieceState() && (sq.getOwner() != current.getOwner())))
                    possibleMoves.Add(sq);
            }
            if (x > 0 && y < 9)
            {
                Square sq = board.getAt(y, x-1);
                if (!sq.getPieceState() || (sq.getPieceState() && (sq.getOwner() != current.getOwner())))
                    possibleMoves.Add(sq);
            }
            if (x < 9 && y > 0)
            {
                Square sq = board.getAt(y-1,x);
                if (!sq.getPieceState() || (sq.getPieceState() && (sq.getOwner() != current.getOwner())))
                    possibleMoves.Add(sq);
            }
            return possibleMoves;
        }
    }
    class PossibleMovesFactory
    {
        private Hashtable strategies = new Hashtable();
        public PossibleMovesStrategy getPossibleMovesStrategy(CertainPiece piece)
        {
            string str = "";
            if (piece.getPromotion() == Promoted.Promoted)
                str = "Promoted";
            str += piece.getPieceType().ToString();
            PossibleMovesStrategy strategy = strategies[str] as PossibleMovesStrategy;
            if (strategy == null)
            {
                switch (str)
                {
                    case "King":
                        strategy = new KingPossibleMovesStrategy();
                        break;
                    case "Gold":
                        strategy = new GoldPossibleMovesStrategy();
                        break;
                    case "Silver":
                        strategy = new SilverPossibleMovesStrategy();
                        break;
                    case "Knight":
                        strategy = new KnigthPossibleMovesStrategy();
                        break;
                    case "Lance":
                        strategy = new LancePossibleMovesStrategy();
                        break;
                    case "Pawn":
                        strategy = new PawnPossibleMovesStrategy();
                        break;
                    case "Rook":
                        strategy = new RookPossibleMovesStrategy();
                        break;
                    case "Bishop":
                        strategy = new BishopPossibleMovesStrategy();
                        break;
                    case "PromotedGold":
                        strategy = new GoldPossibleMovesStrategy();
                        break;
                    case "PromotedSilver":
                        strategy = new GoldPossibleMovesStrategy();
                        break;
                    case "PromotedKnight":
                        strategy = new GoldPossibleMovesStrategy();
                        break;
                    case "PromotedLance":
                        strategy = new GoldPossibleMovesStrategy();
                        break;
                    case "PromotedPawn":
                        strategy = new GoldPossibleMovesStrategy();
                        break;
                    case "PromotedRook":
                        strategy = new PromotedRookPossibleMovesStrategy();
                        break;
                    case "PromotedBishop":
                        strategy = new PromotedBishopPossibleMovesStrategy();
                        break;
                }
                strategies.Add(str, strategy);
            }
            return strategy;
        }
    }
}
