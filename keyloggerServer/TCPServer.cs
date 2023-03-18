using System;
using System.Net;
using System.Net.Sockets;

namespace Networking
{
    class TCPServer
    {
        // Counts number of vowels in a given string
        public string VowelCounter(string sentence)
        {
            int counter = 0;
            foreach (char character in sentence)
            {
                switch (character)
                {
                    case 'a':
                    case 'e':
                    case 'i':
                    case 'o':
                    case 'u':
                        counter++;
                        break;
                }
            }
            return counter.ToString();
        }

        // Counts number of words in a given string
        public string WordCounter(string sentence)
        {
            string[] substrings = sentence.Split(' ');
            int counter = 0;
            foreach (string index in substrings)
            {
                // Do not count if not a word
                if (index == "" || int.TryParse(index, out int _))
                {
                    continue;
                }
                counter++;
            }
            return counter.ToString();
        }

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
                string sentence = System.Text.Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Message Received: " + sentence);
                string instructions = @"
                Enter comma-separated numbers for which choices to return.
                5 Functionalities:
                ------------------
                1) Conversion to upper-case characters
                2) Conversion to lower-case characters
                3) Length of the string
                4) Count vowels
                5) Count words
                ";
                buffer = System.Text.Encoding.ASCII.GetBytes(instructions);
                stream.Write(buffer, 0, buffer.Length);

                buffer = new byte[2048];
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                string choices = System.Text.Encoding.ASCII.GetString(buffer, 0, bytesRead);
                choices = choices.Replace(" ", "");
                Console.WriteLine(choices);
                string[] chosenOptions = choices.Split(',');

                string returnString = "\n";
                foreach (string option in chosenOptions)
                {
                    switch (option)
                    {
                        case "1":
                            returnString += "1) " + sentence.ToUpper() + "\n";
                            break;
                        case "2":
                            returnString += "2) " + sentence.ToLower() + "\n";
                            break;
                        case "3":
                            returnString += "3) " + sentence.Length + "\n";
                            break;
                        case "4":
                            returnString += "4) " + VowelCounter(sentence) + "\n";
                            break;
                        case "5":
                            returnString += "5) " + WordCounter(sentence) + "\n";
                            break;
                        default:
                            returnString += "Error: only input 1 through 5, comma separated\n";
                            break;
                    }
                }
                buffer = System.Text.Encoding.ASCII.GetBytes(returnString);
                stream.Write(buffer, 0, buffer.Length);

                connectionSocket.Close();  // Close connection with client
                // Environment.Exit(0);
            }
        }
    }
}
