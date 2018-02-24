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

        private static string _player1Name;
        public static string player1Name
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

        private static string _player2Name;

        public bool getResult()
        {
            throw new NotImplementedException();
        }

        public static string player2Name
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

        private static int _currentPlayer;
        public static int currentPlayer
        {
            get
            {
                return _currentPlayer;
            }
            set
            {
                _currentPlayer = value;
            }
        }

        public int[,] getBoard()
        {
            int[,] board = new int[8, 8];
            return board;
        }

        public static implicit operator GameState(NavigationEventArgs v)
        {
            throw new NotImplementedException();
        }
    }
}
