using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace Checkers
{
    public class GameClient
    {
        private readonly Boolean testLocal = true;
        private Boolean inGame = false;
        private Boolean isConnected = false;


        private static GameClient gcInstance = null;
        public static GameClient GetInstance()
        {
            if (gcInstance == null)
                gcInstance = new GameClient();
            return gcInstance;
        }


        private GameState game = null;
        public GameState GetGameState()
        {
            return game;
        }


        private String userId = null;
        private Socket client = null;
        private IPEndPoint remote = null;

        private void Init(String netAddress, String userName)
        {
            userId = userName;
            remote = new IPEndPoint(IPAddress.Parse(netAddress), 9001);
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        private void DisInit()
        {
            client = null;
            remote = null;
            userId = null;
        }


        private string SendRecv(string s)
        {
            String r = "";
            int    r_sz;
            byte[] r_buff = new byte[8192];
            byte[] s_buff = Encoding.ASCII.GetBytes(userId + ": " + s + "<EOT>");

            Debug.WriteLine($"GameClient::sendRecv()+ {s}");
            try {
                client.Connect(remote);
                client.Send(s_buff);

                while (true) {
                    r_sz = client.Receive(r_buff);
                    r += Encoding.ASCII.GetString(r_buff, 0, r_sz);
                    if (r.IndexOf("<EOT>") > -1)
                        break;
                }

                r = r.Substring(0, r.IndexOf("<EOT>"));
            } catch (Exception e) {
                Debug.WriteLine($"GameClient::sendRecv() EXCEPTION\n{e}");
            } finally {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
            Debug.WriteLine($"GameClient::sendRecv()- {r}");

            return r;
        }


        public int Connect(String netAddress, String userName)
        {
            if (testLocal)
                return 0;
            if (!isConnected)
            {
                Init(netAddress, userName);

                String r = SendRecv("HELLO");
                if (r.StartsWith("OKAY", StringComparison.OrdinalIgnoreCase))
                {
                    isConnected = true;
                    return 0;
                }
                if (r.StartsWith("USER EXISTS", StringComparison.OrdinalIgnoreCase))
                    return -2;
            }

            return -1;
        }

        public int Disconnect()
        {
            if (testLocal)
                return 0;
            if (!isConnected)
                return -1;

            DisInit();

            return 0;
        }


        public List<String> ListPlayers()
        {
            if (testLocal)
                return new List<String>{ "Shantanu", "Vivian", "Guruansh" };

            if (isConnected)
            {
                String r = SendRecv("LIST PLAYERS");
                if (r.StartsWith("OKAY", StringComparison.OrdinalIgnoreCase))
                {
                    r = r.Substring(5);
                    return r.Split('~').ToList();
                }
            }

            return null;
        }

        public List<GameState> ListGames()
        {
            if (testLocal)
                return new List<GameState>();
            if (isConnected)
            {
                String r = SendRecv("LIST GAMES");
                if (r.StartsWith("OKAY", StringComparison.OrdinalIgnoreCase))
                {
                    r = r.Substring(5);
                    return r.Split('~').Select(gString => GameState.fromString(gString)).ToList();
                }
            }

            return null;
        }


        public int JoinGame()
        {
            if (testLocal)
            {
                game = new GameState(userId);
                return 0;
            }
            if (isConnected && !inGame)
            {
                game = new GameState(userId);
                String r = SendRecv("NEW " + game.ToString());
                if (r.StartsWith("OKAY", StringComparison.OrdinalIgnoreCase))
                {
                    inGame = true;
                    return 0;
                }
            }

            return -1;
        }

        public int JoinGame(GameState remote)
        {
            if (testLocal)
            {
                game = remote;
                return 0;
            }
            if (isConnected && !inGame)
            {
                String r = SendRecv("JOIN " + remote.player1Name);
                if (r.StartsWith("OKAY", StringComparison.OrdinalIgnoreCase))
                {
                    r = r.Substring(5);
                    game = GameState.fromString(r);
                    inGame = true;
                    return 0;
                }
            }

            return -1;
        }

        public int QuitGame()
        {
            if (testLocal)
            {
                game = null;
                inGame = false;
                return 0;
            }
            if (isConnected && inGame)
            {
                String r = SendRecv("QUIT");
                if (r.StartsWith("OKAY", StringComparison.OrdinalIgnoreCase))
                {
                    game = null;
                    inGame = false;
                    return 0;
                }
            }

            return -1;
        }


        public int SendState(GameState game)
        {
            if (testLocal)
            {
                this.game = game;
                return 0;
            }
            if (isConnected && inGame)
            {
                String r = SendRecv("SEND " + GameState.toString(game));
                if (r.StartsWith("OKAY", StringComparison.OrdinalIgnoreCase))
                {
                    this.game = game;
                    return 0;
                }
            }

            return -1;
        }

        public int ReceiveState(GameState game)
        {
            if (testLocal)
            {
                this.game = game;
                return 0;
            }
            if (isConnected && inGame)
            {
                String r = SendRecv("RECV");
                if (r.StartsWith("OKAY", StringComparison.OrdinalIgnoreCase))
                {
                    r = r.Substring(5);
                    this.game = GameState.fromString(r);
                    return 0;
                }
            }

            return -1;
        }
    }
}
