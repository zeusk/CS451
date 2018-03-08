using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class StateObject
{
    public Socket workSocket = null;
    
    public const int BufferSize = 8192;
    public byte[] buffer = new byte[BufferSize];

    public StringBuilder sb = new StringBuilder();
}

public class gameObject
{
    public UInt64 gameId;
    public string gameState;

    public gameObject(UInt64 id, string state)
    {
        gameId = id;
        gameState = state;
    }
}

namespace CheckersHost
{
    public static class GameHost
    {
        public static void Main(string[] args)
        {
            int p = 9001;
            if (args.Length > 0)
                p = Int32.Parse(args[0]);

            try {
                GameHost.Run(p);
            } catch (Exception e) { Debug.WriteLine(e.ToString()); }
        }

        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public static void Run(int p)
        {
            byte[] bytes = new Byte[8192];
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, p);
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(127);

                for (; allDone.Reset(); allDone.WaitOne())
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
            } catch (Exception e) { Console.WriteLine(e.ToString()); }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            allDone.Set();

            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);
            StateObject state = new StateObject{ workSocket = handler };

            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty; 
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                content = state.sb.ToString();

                if (content.IndexOf("<EOT>") < 0)
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                else {
                    String[] frag = content.Substring(0, content.IndexOf("<EOT>")).Split(": ");
                    String cmd = frag[1].Substring(0, 4);
                    String arg;

                    try {
                        arg = frag[1].Substring(5);
                    } catch (ArgumentOutOfRangeException) { arg = ""; }

                    try {
                        lock (lockObj)
                            HandleCmd(handler, frag[0], cmd, arg);
                    } catch (Exception e) { Send(handler, $"E: {e.ToString()}"); }
                }
            }
        }

        private static void Send(Socket handler, String data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data + "<EOT>");

            handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try {
                Socket handler = (Socket) ar.AsyncState;
                int bytesSent = handler.EndSend(ar);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            } catch (Exception e) { Console.WriteLine(e.ToString()); }
        }

        private static UInt64 gIndex = 0;
        private static List<String> players = new List<String>();
        private static List<gameObject> games = new List<gameObject>();
        private static Dictionary<String, UInt64> inGame = new Dictionary<String, UInt64>();
        private static object lockObj = new object();
        // TODO
        // * When two players fill game, remove it from visible list
        // * When one player quits, end game || add it back to visible list
        // * when both players quit, end game
        private static void HandleCmd(Socket handler, String userId, String userCmd, String userArg)
        {
            String resp = "UNKN";

            switch (userCmd)
            {
                case "UREG":
                    {
                        if (players.Any(u => u.Equals(userId, StringComparison.OrdinalIgnoreCase)))
                            resp = "USER EXISTS";
                        else
                        {
                            players.Add(userId);
                            resp = "OKAY";
                        }
                    } break;
                case "EXIT":
                    {
                        gameObject jGame;

                        try {
                            jGame = games.Single(g => g.gameState.IndexOf(userArg) >= 0);

                            string[] frags = jGame.gameState.Split("|");

                            if (frags[2].Equals(userId))
                                frags[2] = "";
                            if (frags[1].Equals(userId))
                                frags[1] = ""; // TODO: Destroy game?
                            if (frags[0].Equals(userId))
                                frags[0] = frags[1].Equals(userId) ? frags[2] : frags[1]; // End move

                            inGame.Remove(userId);
                            jGame.gameState = string.Join("|", frags);
                        } catch (Exception) { }

                        players.Remove(userId);
                    } break;
                case "LSPL":
                    {
                        resp = "OKAY " + string.Join('~', players);
                    } break;
                case "LSGS":
                    {
                        resp = "OKAY " + string.Join('~', games.Select(g => g.gameState));
                    } break;
                case "NEWG":
                    {
                        UInt64 gId = gIndex++;
                        gameObject nGame = new gameObject(gId, userArg);

                        games.Add(nGame);
                        inGame.Add(userId, gId);

                        resp = "OKAY";
                    } break;
                case "JOIN":
                    {
                        gameObject jGame;

                        try {
                            jGame = games.Single(g => g.gameState.IndexOf(userArg) >= 0);
                        } catch (Exception e) { resp = "E: Failed to find game " + e.ToString(); break; }

                        string[] frags = jGame.gameState.Split("|");

                        if (string.IsNullOrEmpty(frags[2]))
                        {
                            frags[2] = userId;
                            inGame.Add(userId, jGame.gameId);
                            jGame.gameState = string.Join("|", frags);
                            resp = "OKAY " + jGame.gameState;
                        } else resp = "WARN Game is full";
                    } break;
                case "QUIT":
                    {
                        gameObject jGame;

                        try {
                            jGame = games.Single(g => g.gameState.IndexOf(userArg) >= 0);
                        } catch (Exception e) { resp = "E: Failed to find game " + e.ToString(); break; }

                        string[] frags = jGame.gameState.Split("|");

                        if (frags[2].Equals(userId))
                            frags[2] = "";
                        if (frags[1].Equals(userId))
                            frags[1] = ""; // TODO: Destroy game?
                        if (frags[0].Equals(userId))
                            frags[0] = frags[1].Equals(userId) ? frags[2] : frags[1]; // End move

                        inGame.Remove(userId);
                        jGame.gameState = string.Join("|", frags);

                        resp = "OKAY";
                    } break;
                case "SEND":
                    {
                        UInt64 iGameIdx = inGame[userId];
                        gameObject iGame;

                        try {
                            iGame = games.Single(g => g.gameId == iGameIdx);
                        } catch (Exception e) { resp = "E: Failed to find game " + e.ToString(); break; }

                        iGame.gameState = userArg;

                        resp = "OKAY";
                    } break;
                case "RECV":
                    {
                        UInt64 iGameIdx = inGame[userId];
                        gameObject iGame;

                        try {
                            iGame = games.Single(g => g.gameId == iGameIdx);
                        } catch (Exception e) { resp = "E: Failed to find game " + e.ToString(); break; }

                        resp = "OKAY " + iGame.gameState;
                    } break;
            }

            Console.WriteLine($"{userId}: {userCmd} {userArg}");
            Console.WriteLine($"SERV: {userCmd} for {userId}: {resp}");

            Send(handler, resp);
        }
    }
}
