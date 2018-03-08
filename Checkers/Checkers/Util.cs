using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    class Util
    {
        private static string userName = null;

        public static void SetMyName(string uName)
        {
            userName = uName; // TODO: Save to disk
        }

        public static string GetMyName()
        {
            if (userName == null)
                userName = "Mike"; // TODO: Read from disk

            return userName; 
        }

        public static string GetOpponentName()
        {
            return GetOpponentName(GetGameState());
        }

        public static string GetOpponentName(GameState gs)
        {
            return amPlayer1(gs) ? gs.player2Name : gs.player1Name;
        }

        public static bool isMyTurn()
        {
            return isMyTurn(GetGameState());
        }

        public static bool isMyTurn(GameState gs)
        {
            return gs.playerTurn.Equals(GetMyName());
        }

        public static string GetPlayerTurnName()
        {
            return GetPlayerTurnName(GetGameState());
        }

        public static string GetPlayerTurnName(GameState gs)
        {
            return gs.playerTurn;
        }

        public static string GetPlayer1Name()
        {
            return GetPlayer1Name(GetGameState());
        }

        public static string GetPlayer1Name(GameState gs)
        {
            return gs.player1Name;
        }

        public static string GetPlayer2Name()
        {
            return GetPlayer2Name(GetGameState());
        }

        public static string GetPlayer2Name(GameState gs)
        {
            return gs.player2Name;
        }

        public static int myPlayerNum()
        {
            return myPlayerNum(GetGameState());
        }

        public static int myPlayerNum(GameState gs)
        {
            return amPlayer1(gs) ? 1 : 2;
        }

        public static bool amPlayer1()
        {
            return amPlayer1(GetGameState());
        }

        public static bool amPlayer1(GameState gs)
        {
            return gs.player1Name.Equals(GetMyName());
        }

        public static GameState GetGameState()
        {
            return GameClient.GetInstance().GetGameState();
        }

        public static bool ValidateIPv4(string ipString)
        {
            if (String.IsNullOrWhiteSpace(ipString))
                return false;

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
                return false;

            return splitValues.All(r => byte.TryParse(r, out byte tempForParsing));
        }
    }
}
