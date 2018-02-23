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

        public static implicit operator GameState(NavigationEventArgs v)
        {
            throw new NotImplementedException();
        }
    }
}
