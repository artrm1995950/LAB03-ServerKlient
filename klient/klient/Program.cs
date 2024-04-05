using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

class Client
{
    static void Main(string[] args)
    {
        try
        {
            TcpClient client = new TcpClient("127.0.0.1", 8888);
            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream);

            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("Enter action (1 - get a file, 2 - create a file, 3 - delete a file, exit - to quit):");
                string znach = Console.ReadLine();

                string request = "";

                if (znach == "exit")
                {
                    exit = true;
                    break;
                }

                if (znach == "1")
                {
                    Console.WriteLine("Enter filename:");
                    string getFileName = Console.ReadLine();
                    request = "GET " + getFileName;
                }
                else if (znach == "2")
                {
                    Console.WriteLine("Enter filename:");
                    string putFileName = Console.ReadLine();
                    Console.WriteLine("Enter file content:");
                    string fileContent = Console.ReadLine();
                    request = "PUT " + putFileName + " " + fileContent;
                }
                else if (znach == "3")
                {
                    Console.WriteLine("Enter filename:");
                    string deleteFileName = Console.ReadLine();
                    request = "DELETE " + deleteFileName;
                }
                else
                {
                    Console.WriteLine("Invalid action");
                    continue;
                }

                writer.WriteLine(request);
                writer.Flush();

                string response = reader.ReadToEnd();
                Console.WriteLine("The request was sent.");

                Console.WriteLine("Server response: " + response);
                Console.WriteLine();

                string[] responseParts = response.Split(' ');
                string statusCode = responseParts[0];

                if (statusCode == "200")
                {
                    if (znach == "2")
                    {
                        Console.WriteLine("The response says that the file was created!");
                    }
                    else if (znach == "3")
                    {
                        Console.WriteLine("The response says that the file was successfully deleted!");
                    }
                    else if (znach == "1")
                    {
                        string fileContent = responseParts[1];
                        Console.WriteLine("The content of the file is: " + fileContent);
                    }
                }
            }

            client.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}