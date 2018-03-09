using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using NUnit.Framework;

namespace CheckersUnitTests
{
    [TestFixture]
    class GameClientTests
    {
        private readonly string ipAddress = "127.0.0.1";
        private readonly int port = 9002;

        private IPEndPoint localEndPoint;
        private Socket host;
        private Socket handle;

        [SetUp]
        private void setupSockets()
        {
            localEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            host = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            host.Bind(localEndPoint);
            host.Listen(127);
        }

        [TearDown]
        private void TearupSockets()
        {
            localEndPoint = null;
            host = null;
        }

        [Test]
        private void testConnect()
        {
            Checkers.GameClient gc = Checkers.GameClient.GetInstance();
        }
    }
}
