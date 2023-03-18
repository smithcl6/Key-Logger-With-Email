using System;
using System.Net.Sockets;

namespace Networking {
class Client {
    public void clientSocket() {
        Console.WriteLine("client was made!");
        string serverName = "127.0.0.1";
        int serverPort = 12000;

        // create a socket object
        Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        client.Connect(serverName, serverPort);

        Console.Write("Input: ");  // Input string
        string inputString = Console.ReadLine();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(inputString);
        client.Send(inputBytes);  // Send input string

        byte[] responseBytes = new byte[2048];
        int responseLength = client.Receive(responseBytes);  // Response including instructions
        string response = System.Text.Encoding.ASCII.GetString(responseBytes, 0, responseLength);
        Console.WriteLine(response);

        Console.Write("Choices: ");
        string choices = Console.ReadLine();
        byte[] choicesBytes = System.Text.Encoding.ASCII.GetBytes(choices);
        client.Send(choicesBytes);  // Send choices as string

        responseLength = client.Receive(responseBytes);
        response = System.Text.Encoding.ASCII.GetString(responseBytes, 0, responseLength);
        Console.WriteLine(response);

        client.Shutdown(SocketShutdown.Both);
        client.Close();  // Close connection with server
    }
}
}