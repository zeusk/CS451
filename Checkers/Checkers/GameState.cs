using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public class GameState
    {
        private static string player1Name = "";
        private static string player2Name = "";

        public static string getNamePlayer1()
        {
            return player1Name;
        }

        public static string getNamePlayer2()
        {
            return player2Name;
        }
    }
}
