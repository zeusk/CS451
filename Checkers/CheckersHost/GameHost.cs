using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace CheckersHost
{
    class GameHost
    {
        static void Main(string[] args)
        {
            int p = 9001;
            if (args.Length > 0)
                p = Int32.Parse(args[0]);

            try { new GameHost(p).Run(); }
            catch (Exception e) { Debug.WriteLine(e.ToString()); Console.WriteLine(e.ToString()); }
        }

        private readonly int port;
        public GameHost(int p)
        {
            port = p;
        }

        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public void Run()
        {
            byte[] bytes = new Byte[8192];
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(127);

                for (; allDone.Reset(); allDone.WaitOne())
                {
                    Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                }
            }
            catch (Exception e)
            { Console.WriteLine(e.ToString()); }

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

                if (content.IndexOf("<EOT>") > -1)
                {
                    Console.WriteLine($"RECV {content.Length} bytes from socket\nData: {content}");
                    //Send(handler, content);

                    String[] frag = content.Substring(0, content.IndexOf("<EOT>")).Split(": ");

                    HandleCmd(handler, frag[0], frag[1]);
                }
                else // Not all data received. Get more.
                {
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
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
            try
            {
                Socket handler = (Socket)ar.AsyncState;
                int bytesSent = handler.EndSend(ar);

                Console.WriteLine($"SENT {bytesSent} bytes to client");

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception e)
            { Console.WriteLine(e.ToString()); }
        }

        private static void HandleCmd(Socket handler, String userId, String userCd)
        {
            Console.WriteLine($"Serving {userId} for {userCd}");
            Send(handler, userId);
        }
    }
}
