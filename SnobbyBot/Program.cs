using System;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;

namespace SnobbyBot
{
    class Program
    {
        static void Main(String[] args)
        {
            SnobbyBot snobbyBot = new SnobbyBot();
            snobbyBot.Connect();

            Console.ReadLine();

            snobbyBot.Disconnect();


        }

    }
}
