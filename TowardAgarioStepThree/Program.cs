using AgarioModels;
using Communications;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace TowardAgarioStepThree 
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger<Program> logger = new(new LoggerFactory());
            Networking client = new Networking(logger, OnConnect, OnDisconnect, OnMessage, '\n');
            client.Connect("localhost", 11000);
            client.AwaitMessagesAsync();
            Console.ReadLine();
        }

        public static void OnConnect(Networking channel) { }
        public static void OnDisconnect(Networking channel) { }
        public static void OnMessage(Networking channel, string message) 
        {
            if (message.StartsWith("{Command Food}"))
            {
                Food[] foods = JsonSerializer.Deserialize<Food[]>(message.Replace("{Command Food}", ""))
            ?? throw new Exception("bad json"); ;

                for (int i = 0; i < 10; i++) 
                {
                    Food f = foods[i];
                    Console.WriteLine($"X: {f.X}, Y: {f.Y}, Color: {f.ARGBColor}, Mass: {f.Mass}");
                }
            }
        }
    }
}