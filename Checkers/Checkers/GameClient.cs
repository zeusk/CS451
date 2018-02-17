using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    class GameClient
    {
        private Boolean inGame = false;
        private Boolean isConnected = false;
        private GameObj remoteGame = null;

        public int connect(String netAddress)
        {
            isConnected = true;
            return 0;
        }

        public int disconnect()
        {
            isConnected = false;
            return 0;
        }

        public List<String> listPlayers()
        {
            List<String> ret = new List<String>();

            if (isConnected)
            {
                
            }

            return ret;
        }

        public List<GameObj> listGames()
        {
            List<GameObj> ret = new List<GameObj>();

            if (isConnected)
            {

            }

            return ret;
        }

        public int joinGame(GameObj remote)
        {
            inGame = true;
            remoteGame = remote;
            return 0;
        }

        public int quitGame()
        {
            inGame = false;
            remoteGame = null;
            return 0;
        }

        public int sendMove(GameState state)
        {
            if (!isConnected || !inGame)
                return -1;

            return this.sendState(this.remoteGame, state);
        }

        private int sendState(GameObj remoteGame, GameState state)
        {
            return 0;
        }

        // Note: This call may block, call in async state or in a worker thread
        public GameState receiveMove()
        {
            if (!isConnected || !inGame)
                return null;

            return this.receiveState(this.remoteGame);
        }

        private GameState receiveState(GameObj game)
        {
            return new GameState();
        }
    }
}
