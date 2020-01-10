using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace GameServer
{
    public class NetworkServer
    {
        private TcpListener serverSocket;

        public static int maxClients = 50;
        public List<Client> Clients = new List<Client>();

        public void InitNetwork()
        {
            var port = 5555;

            serverSocket = new TcpListener(IPAddress.Any, port);
            serverSocket.Start();
            serverSocket.BeginAcceptTcpClient(ClientConnectCallback, null);

            Console.WriteLine("Listening on port " + port);
        }

        private void ClientConnectCallback(IAsyncResult result)
        {
            TcpClient tempClient = serverSocket.EndAcceptTcpClient(result);
            serverSocket.BeginAcceptTcpClient(ClientConnectCallback, null);

            var count = 0;
            foreach (Client c in Clients)
            {

                if (c.socket == null)
                {
                    c.socket = tempClient;
                    c.connectionId = Clients.IndexOf(c);
                }

                count++;
            }
        }
    }
}
