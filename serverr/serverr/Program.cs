using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        int port = 8888;
        int requestCount = 0; // Счетчик запросов

        TcpListener listener = new TcpListener(ipAddress, port);
        listener.Start();
        Console.WriteLine("Server started!");

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            HandleClientRequest(client, ref requestCount);
        }
    }

    static void HandleClientRequest(TcpClient client, ref int requestCount)
    {
        NetworkStream stream = client.GetStream();
        StreamReader reader = new StreamReader(stream);
        StreamWriter writer = new StreamWriter(stream);

        string znach = reader.ReadLine();
        Console.WriteLine("Received request: " + znach);

        string[] znachh = znach.Split(' ');
        string action = znachh[0];
        string fileName = znachh[1];

        string otvet = "";

        if (action == "PUT")
        {
            string fileContent = znachh[2];
            otvet = CreateFile(fileName, fileContent);
        }
        else if (action == "GET")
        {
            otvet = GetFileContent(fileName);
        }
        else if (action == "DELETE")
        {
            otvet = DeleteFile(fileName);
        }
        else
        {
            otvet = "Invalid action";
        }

        Console.WriteLine("Sending response: " + otvet);

        writer.WriteLine(otvet);
        writer.Flush();

        client.Close();

        requestCount++; // Увеличиваем счетчик запросов
        Console.WriteLine($"Total requests processed: {requestCount}");
    }

    static string CreateFile(string fileName, string fileContent)
    {
        string directoryPath = @"C:\Users\79969\Desktop\artem";
        string filePath = Path.Combine(directoryPath, fileName);

        if (File.Exists(filePath))
        {
            return "403";
        }

        File.WriteAllText(filePath, fileContent);
        return "200";
    }

    static string GetFileContent(string fileName)
    {
        string directoryPath = @"C:\Users\79969\Desktop\artem";
        string filePath = Path.Combine(directoryPath, fileName);

        if (!File.Exists(filePath))
        {
            return "404";
        }

        string fileContent = File.ReadAllText(filePath);
        return "200 " + "\n" + fileContent;
    }

    static string DeleteFile(string fileName)
    {
        string directoryPath = @"C:\Users\79969\Desktop\artem";
        string filePath = Path.Combine(directoryPath, fileName);

        if (!File.Exists(filePath))
        {
            return "404";
        }

        File.Delete(filePath);
        return "200";
    }
}