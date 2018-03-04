using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Checkers
{
    public class GameState
    {
        public GameState (string playerName)
        {
            this.player1Name = playerName;
            this._checkerBoard = new CheckerBoard();
        }

        private CheckerBoard _checkerBoard;
        public CheckerBoard cb
        {
            get
            {
                return _checkerBoard;
            }
            set
            {
                _checkerBoard = value;
            }
        }

        private string _player1Name;
        public string player1Name
        {
            get
            {
                return _player1Name;
            }
            set
            {
                _player1Name = value;
            }
        }

        private string _player2Name;

        public int getResult()
        {
            return this._checkerBoard.getResult();
        }

        public string player2Name
        {
            get
            {
                return _player2Name;
            }
            set
            {
                _player2Name = value;
            }
        }
 
        public int[,] getBoard()
        {
            return this._checkerBoard.getBoard();
        }

        public static String toString(GameState gs)
        {
            return "";
        }

        public static GameState fromString(String gState)
        {
            GameState ret = new GameState("");

            // ret.state = parse(gState);

            return ret;
        }

        public GameState applyMove(List<int> movePair, int playerID)
        {
            Debug.WriteLine("before:");
            this._checkerBoard.printBoard();
            if (this._checkerBoard.applyMove(movePair, playerID))
            {
                Debug.WriteLine("valid--after:");
                this._checkerBoard.printBoard();
                return this;
            }
            else
            {
                Debug.WriteLine("invalid move:");
                return null;
            }
        }

        public bool checkAvailableJump(int x, int y, int playerID)
        {
            int[] pos = new int[2];
            pos[0] = x;
            pos[1] = y;
            return this._checkerBoard.checkAnyJumpPossiblePiece(pos,playerID);
        }
    }
}
