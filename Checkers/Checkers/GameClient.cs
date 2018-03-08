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
        public readonly Boolean testLocal = true;
        private Boolean _inGame = false;
        private Boolean _isConnected = false;

        public Boolean inGame { get { return _inGame; } }
        public Boolean isConnected { get { return _isConnected; } }

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


        private IPEndPoint remote = null;

        private int Init(String netAddress)
        {
            int port = 9001;

            try
            {
                if (netAddress.IndexOf(":") > -1)
                {
                    String[] frag = netAddress.Split(':');

                    netAddress = frag[0];
                    port = Int32.Parse(frag[1]);
                }

                if (!Util.ValidateIPv4(netAddress))
                    return -2;

                remote = new IPEndPoint(IPAddress.Parse(netAddress), port);
            } catch (Exception e) { Debug.WriteLine(e.ToString()); return -1; }

            return 0;
        }

        private void DisInit()
        {
            remote = null;
        }


        private string SendRecv(string s)
        {
            String r = "";
            int    r_sz;
            byte[] r_buff = new byte[8192];
            byte[] s_buff = Encoding.ASCII.GetBytes(Util.GetMyName() + ": " + s + "<EOT>");
            Socket client = null;

            Debug.WriteLine($"GameClient::sendRecv()+ {s}");
            try {
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                client.Connect(remote);
                client.Send(s_buff);

                do {
                    r_sz = client.Receive(r_buff);
                    r += Encoding.ASCII.GetString(r_buff, 0, r_sz);
                } while (r.IndexOf("<EOT>") < 0);

                r = r.Substring(0, r.IndexOf("<EOT>"));
            } catch (Exception e) {
                Debug.WriteLine($"GameClient::sendRecv() EXCEPTION\n{e}");
            } finally {
                try {
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                } catch (Exception e) { Debug.WriteLine(e.ToString()); }
            }
            Debug.WriteLine($"GameClient::sendRecv()- {r}");

            return r;
        }


        public int Connect(String netAddress)
        {
            if (testLocal)
                return 0;
            if (!isConnected)
            {
                switch (Init(netAddress))
                {
                    case 0: break;
                    case -2: return -3; // invalid IP
                    default: return -1; // unknown error
                }

                String r = SendRecv("UREG"); // UserRegister
                if (r.StartsWith("OKAY", StringComparison.OrdinalIgnoreCase))
                {
                    _isConnected = true;
                    return 0;
                }
                if (r.StartsWith("USER EXISTS", StringComparison.OrdinalIgnoreCase))
                    return -2; // User exists
            }

            return -1; // unknown error
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
                String r = SendRecv("LSPL"); // ListPlayers
                if (r.StartsWith("OKAY", StringComparison.OrdinalIgnoreCase))
                    return r
                        .Substring(5)
                        .Split('~')
                        .Where(p => !string.IsNullOrEmpty(p))
                        .ToList();
            }

            return null;
        }

        public List<GameState> ListGames()
        {
            if (testLocal)
                return new List<GameState>();
            if (isConnected)
            {
                String r = SendRecv("LSGS"); // ListGames
                if (r.StartsWith("OKAY", StringComparison.OrdinalIgnoreCase))
                    return r
                        .Substring(5)
                        .Split('~')
                        .Where(g => !string.IsNullOrEmpty(g))
                        .Select(g => GameState.fromString(g))
                        .ToList();
            }

            return null;
        }


        public int JoinGame()
        {
            game = new GameState(Util.GetMyName());

            // Use this format to load a gameState:
            // game = GameState.fromString("Mike|Mike|testLocalPlayer|0|1|0|0|0|1|0|1|1|0|1|0|1|0|1|0|0|1|0|1|0|0|0|0|0|0|0|0|0|0|1|0|0|2|0|2|0|0|0|0|2|0|0|0|0|0|2|0|0|0|0|2|0|2|0|2|2|0|3|0|0|0|2|0");

            if (testLocal)
                return 0;

            if (isConnected && !inGame)
            {
                String r = SendRecv("NEWG " + game.toString());
                if (r.StartsWith("OKAY", StringComparison.OrdinalIgnoreCase))
                {
                    _inGame = true;
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
                    game = GameState.fromString(r.Substring(5));
                    _inGame = true;
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
                _inGame = false;
                return 0;
            }
            if (isConnected && inGame)
            {
                String r = SendRecv("QUIT");
                if (r.StartsWith("OKAY", StringComparison.OrdinalIgnoreCase))
                {
                    game = null;
                    _inGame = false;
                    return 0;
                }
            }

            return -1;
        }

        public int SendState()
        {
            return ReceiveState(GetGameState());
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

        public int ReceiveState()
        {
            return SendState(GetGameState());
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
                    this.game = GameState.fromString(r.Substring(5));
                    return 0;
                }
            }

            return -1;
        }
    }
}
