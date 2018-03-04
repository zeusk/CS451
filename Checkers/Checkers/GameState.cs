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

        public bool getResult()
        {
            throw new NotImplementedException();
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
            int[,] board = new int[8, 8];
            return board;
        }

        public GameState applyMove(List<int> movePair)
        {
            GameState gs = new GameState();
            return gs;
        }

    }
}
