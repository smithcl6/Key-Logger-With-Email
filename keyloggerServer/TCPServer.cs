using System;
using System.Net;
using System.Net.Sockets;

namespace Networking
{
    class TCPServer
    {
        public void socketServer()
        {
            int serverPort = 12000;
            TcpListener server = new TcpListener(IPAddress.Any, serverPort);
            server.Start();
            Console.WriteLine("The server is ready to receive");
            while (true)
            {
                Console.WriteLine("Waiting ...");
                TcpClient connectionSocket = server.AcceptTcpClient();
                Console.WriteLine("accept");
                NetworkStream stream = connectionSocket.GetStream();
                byte[] buffer = new byte[2048];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string log = System.Text.Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Message Received:" + log);

                connectionSocket.Close();  // Close connection with client
                // Environment.Exit(0);
            }
        }
    }
}
