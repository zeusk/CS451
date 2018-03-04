using System;
using System.Collections.Generic;
using System.Linq;
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
 

        //Not useful for now
        /*
        private int _currentPlayer;
        public int currentPlayer
        {
            get
            {
                return _currentPlayer;
            }
            set
            {
                _currentPlayer = value;
            }
        }*/

        public int[,] getBoard()
        {
            return this._checkerBoard.getBoard();
        }

        public GameState applyMove(List<int> movePair, int playerID)
        {
            this._checkerBoard.applyMove(movePair, playerID);
            return this;
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
