using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace coursachWithForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            tableLayoutPanel1.GetControlFromPosition(2, 2).BackgroundImage = Properties.Resources.King;
            Globals.mainBoard = new MainBoard();
            Globals.move = new Move();
            Globals.factory = new PossibleMovesFactory();
            Globals.mainBoard.initDefault();
            Globals.pl1SideBoard = new sideBoard();
            Globals.pl1SideBoard.owner = Players.Player1;
            Globals.pl2SideBoard = new sideBoard();
            Globals.pl2SideBoard.owner = Players.Player2;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    Button btn = (Button)tableLayoutPanel1.GetControlFromPosition(j, i);
                    btn.MouseDown += (s, args) => {
                        TableLayoutPanelCellPosition pos = tableLayoutPanel1.GetCellPosition(btn);
                        proceedClick(8 - pos.Row, pos.Column - 1,args);
                    };
                }
            }

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Button btn = (Button)tableLayoutPanel2.GetControlFromPosition(j, i);
                    btn.Click += (s, e) => {
                        TableLayoutPanelCellPosition pos = tableLayoutPanel2.GetCellPosition(btn);
                        proceedSideBoardClick(pos.Column, pos.Row,Globals.pl2SideBoard);
                    };
                    btn = (Button)tableLayoutPanel5.GetControlFromPosition(j, i);
                    btn.Click += (s, e) => {
                        TableLayoutPanelCellPosition pos = tableLayoutPanel5.GetCellPosition(btn);
                        proceedSideBoardClick(pos.Column, pos.Row, Globals.pl1SideBoard);
                    };
                }
            }
            proceesMainBoard();
            proceedSideBoards();

        }
  
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void proceedSideBoardClick(int j,int i,sideBoard board)
        {
            if (board.owner == Globals.move.player)
            {
                if (2 * i + j < board.items.Count)
                {
                    cleanPossibleMoves();
                    Globals.move.possibleSquares = board.getPossibleMoves(Globals.mainBoard, board.items[2 * i + j], Globals.move.player);
                    Globals.move.startingSquare = null;
                    Globals.move.piece = board.items[2 * i + j];
                    if (Globals.move.getState() == MoveState.ChoosingPiece)
                        Globals.move.nextState();
                    showPossibleMoves();
                }
            }
        }
        private void proceesMainBoard()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Bitmap res = Properties.Resources.none;
                    Square sq = Globals.mainBoard.getAt(8-i, j);
                    if (sq.getPieceState())
                    {
                        CertainPiece piece = sq.getPiece();
                        switch (piece.getPieceType())
                        {
                            case (ShogiPieces.King):
                                res = Properties.Resources.King;
                                break;
                            case (ShogiPieces.Gold):
                                res= Properties.Resources.Gold;
                                break;
                            case (ShogiPieces.Silver):
                                if (piece.getPromotion() == Promoted.Unpromoted)
                                    res = Properties.Resources.Silver;
                                else
                                    res = Properties.Resources.Silver_p;
                                break;
                            case (ShogiPieces.Knight):
                                if (piece.getPromotion() == Promoted.Unpromoted)
                                    res = Properties.Resources.Knight;
                                else
                                    res = Properties.Resources.Gold_p; 
                                break;
                            case (ShogiPieces.Lance):
                                if (piece.getPromotion() == Promoted.Unpromoted)
                                    res = Properties.Resources.Lance;
                                else
                                    res = Properties.Resources.Lance_p;
                                break;
                            case (ShogiPieces.Rook):
                                if (piece.getPromotion() == Promoted.Unpromoted)
                                    res = Properties.Resources.Rook;
                                else
                                    res = Properties.Resources.Rook_p;
                                break;
                            case (ShogiPieces.Bishop):
                                if (piece.getPromotion() == Promoted.Unpromoted)
                                    res = Properties.Resources.Bishop;
                                else
                                    res = Properties.Resources.Bishop_p;
                                break;
                            case (ShogiPieces.Pawn):
                                if (piece.getPromotion() == Promoted.Unpromoted)
                                    res = Properties.Resources.Pawn;
                                else
                                    res = Properties.Resources.Pawn_p;
                                break;

                        }
                        if(sq.getOwner()==Players.Player2)
                            res.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    }
                    tableLayoutPanel1.GetControlFromPosition(j+1, i).BackgroundImage = res;

                }
            }

        }
        private void proceedSquare(Square sq)
        {
            Bitmap res = Properties.Resources.none;
            if (sq.getPieceState())
            {
                CertainPiece piece = sq.getPiece();
                switch (piece.getPieceType())
                {
                    case (ShogiPieces.King):
                        res = Properties.Resources.King;
                        break;
                    case (ShogiPieces.Gold):
                        res = Properties.Resources.Gold;
                        break;
                    case (ShogiPieces.Silver):
                        if (piece.getPromotion() == Promoted.Unpromoted)
                            res = Properties.Resources.Silver;
                        else
                            res = Properties.Resources.Silver_p;
                        break;
                    case (ShogiPieces.Knight):
                        if (piece.getPromotion() == Promoted.Unpromoted)
                            res = Properties.Resources.Knight;
                        else
                            res = Properties.Resources.Gold_p;
                        break;
                    case (ShogiPieces.Lance):
                        if (piece.getPromotion() == Promoted.Unpromoted)
                            res = Properties.Resources.Lance;
                        else
                            res = Properties.Resources.Lance_p;
                        break;
                    case (ShogiPieces.Rook):
                        if (piece.getPromotion() == Promoted.Unpromoted)
                            res = Properties.Resources.Rook;
                        else
                            res = Properties.Resources.Rook_p;
                        break;
                    case (ShogiPieces.Bishop):
                        if (piece.getPromotion() == Promoted.Unpromoted)
                            res = Properties.Resources.Bishop;
                        else
                            res = Properties.Resources.Bishop_p;
                        break;
                    case (ShogiPieces.Pawn):
                        if (piece.getPromotion() == Promoted.Unpromoted)
                            res = Properties.Resources.Pawn;
                        else
                            res = Properties.Resources.Pawn_p;
                        break;
                }
                if (sq.getOwner() == Players.Player2)
                    res.RotateFlip(RotateFlipType.Rotate180FlipNone);
            }
            
            tableLayoutPanel1.GetControlFromPosition(sq.x+1, 8-sq.y).BackgroundImage = res;
        }
        private void proceedClick(int x, int y, EventArgs args)
        {
            Square sq = Globals.mainBoard.getAt(x, y);
            MouseEventArgs me = (MouseEventArgs)args;
            if (me.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (Globals.move.getState() == MoveState.ChoosingPiece)
                {
                    if (sq.getPieceState() && sq.getOwner() == Globals.move.player)
                    {
                        proceedPossibleMoves(sq);
                        Globals.move.nextState();
                    }
                }
                else
                {
                    if (sq.getPieceState() && sq.getOwner() == Globals.move.player)
                    {
                        proceedPossibleMoves(sq);
                    }
                    else
                    {
                        if (Globals.move.possibleSquares.Contains(sq))
                        {
                            if (sq.getPieceState())
                            {
                                CertainPiece pc = sq.getPiece();
                                pc.depromote();
                                if (Globals.move.player == Players.Player1)
                                    Globals.pl1SideBoard.items.Add(pc);
                                else
                                    Globals.pl2SideBoard.items.Add(pc);
                                proceedSideBoards();
                            }
                            if (Globals.move.startingSquare != null)
                            {
                                sq.setPiece(Globals.move.startingSquare.getPiece());
                                Globals.move.startingSquare.removePiece();
                                proceedSquare(Globals.move.startingSquare);
                            }
                            else
                            {
                                sideBoard board;
                                if (Globals.move.player == Players.Player1)
                                    board = Globals.pl1SideBoard;
                                else
                                    board = Globals.pl2SideBoard;
                                board.items.Remove(Globals.move.piece);
                                Globals.move.piece.setPlayer(Globals.move.player);
                                sq.setPiece(Globals.move.piece);
                                proceedSideBoards();
                                Globals.move.piece = null;
                            }
                            cleanPossibleMoves();
                            proceedSquare(sq);
                            Globals.move.nextMove();
                        }
                    }
                }
            }
            else
            {
                if (me.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    if ( ((x<3)||(x>=6)) && Globals.move.getState() == MoveState.ChoosingSquareForPiece && Globals.move.possibleSquares.Contains(sq) && (Globals.move.startingSquare!=null)) 
                    {
                        if (sq.getPieceState())
                        {
                            CertainPiece pc = sq.getPiece();
                            pc.depromote();
                            if (Globals.move.player == Players.Player1)
                                Globals.pl1SideBoard.items.Add(pc);
                            else
                                Globals.pl2SideBoard.items.Add(pc);
                            proceedSideBoards();
                        }
                        CertainPiece pPiece = Globals.move.startingSquare.getPiece();
                        pPiece.promote();
                        sq.setPiece(pPiece);
                        Globals.move.startingSquare.removePiece();
                        proceedSquare(Globals.move.startingSquare);
                        cleanPossibleMoves();
                        proceedSquare(sq);
                        Globals.move.nextMove();
                    }
                }
            }
        }
        private void proceedSideBoards()
        {
            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 2; j++)
                {
                    Bitmap res = Properties.Resources.none;
                    if (Globals.pl1SideBoard.items.Count() > 2 * i + j)
                    {
                        switch (Globals.pl1SideBoard.items[2 * i + j].getPieceType())
                        {
                            case (ShogiPieces.King):
                                res = Properties.Resources.King;
                                break;
                            case (ShogiPieces.Gold):
                                res = Properties.Resources.Gold;
                                break;
                            case (ShogiPieces.Silver):
                                res = Properties.Resources.Silver;
                                break;
                            case (ShogiPieces.Knight):
                                res = Properties.Resources.Knight;
                                break;
                            case (ShogiPieces.Lance):
                                res = Properties.Resources.Lance;
                                break;
                            case (ShogiPieces.Rook):
                                res = Properties.Resources.Rook;
                                break;
                            case (ShogiPieces.Bishop):
                                res = Properties.Resources.Bishop;
                                break;
                            case (ShogiPieces.Pawn):
                                res = Properties.Resources.Pawn;
                                break;
                        }
                    }
                    Bitmap res1 = Properties.Resources.none;
                    if (Globals.pl2SideBoard.items.Count() > 2 * i + j)
                    {
                        switch (Globals.pl2SideBoard.items[2 * i + j].getPieceType())
                        {
                            case (ShogiPieces.King):
                                res1 = Properties.Resources.King;
                                break;
                            case (ShogiPieces.Gold):
                                res1 = Properties.Resources.Gold;
                                break;
                            case (ShogiPieces.Silver):
                                res1 = Properties.Resources.Silver;
                                break;
                            case (ShogiPieces.Knight):
                                res1 = Properties.Resources.Knight;
                                break;
                            case (ShogiPieces.Lance):
                                res1 = Properties.Resources.Lance;
                                break;
                            case (ShogiPieces.Rook):
                                res1 = Properties.Resources.Rook;
                                break;
                            case (ShogiPieces.Bishop):
                                res1 = Properties.Resources.Bishop;
                                break;
                            case (ShogiPieces.Pawn):
                                res1 = Properties.Resources.Pawn;
                                break;
                        }
                    }
                    tableLayoutPanel5.GetControlFromPosition(j, i).BackgroundImage = res;
                    res1.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    tableLayoutPanel2.GetControlFromPosition(j, i).BackgroundImage = res1;
                }
            }
        }
        private void proceedPossibleMoves(Square sq)
        {
            cleanPossibleMoves();
            Globals.move.startingSquare = sq;
            Globals.move.possibleSquares = sq.getPossibleMoves(Globals.mainBoard);
            showPossibleMoves();
            
        }
        private void showPossibleMoves()
        {
            foreach (Square sqr in Globals.move.possibleSquares)
            {
                Button btn = (Button)tableLayoutPanel1.GetControlFromPosition(sqr.x + 1, 8 - sqr.y);
                btn.Image = Properties.Resources.dot;
            }
        }
        private void cleanPossibleMoves()
        {
            if (Globals.move.possibleSquares != null)
                foreach (Square sqr in Globals.move.possibleSquares)
                {
                    Button btn = (Button)tableLayoutPanel1.GetControlFromPosition(sqr.x + 1, 8 - sqr.y);
                    btn.Image = null;
                }
        }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button90_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {

        }

        private void button92_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Globals.mainBoard = new MainBoard();
            Globals.move = new Move();
            Globals.factory = new PossibleMovesFactory();
            Globals.mainBoard.initDefault();
            Globals.pl1SideBoard = new sideBoard();
            Globals.pl1SideBoard.owner = Players.Player1;
            Globals.pl2SideBoard = new sideBoard();
            Globals.pl2SideBoard.owner = Players.Player2;
            proceesMainBoard();
            proceedSideBoards();
        }
    }
    
}
