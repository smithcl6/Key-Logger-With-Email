using System;
using System.Net.Sockets;

namespace Networking {
class Client {
    public void clientSocket(string buffer) {
        string serverName = "127.0.0.1";
        int serverPort = 12000;

        // create a socket object
        Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        client.Connect(serverName, serverPort);

        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(buffer);
        client.Send(inputBytes);  // Send input string

        client.Shutdown(SocketShutdown.Both);
        client.Close();  // Close connection with server
    }
}
}