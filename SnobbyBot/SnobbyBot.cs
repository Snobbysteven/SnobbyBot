using System;
using TwitchLib.Api;
using TwitchLib.Api.Helix.Models.Users;
using TwitchLib.Api.V5.Models.Subscriptions;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace SnobbyBot
{
    class SnobbyBot
    {

        ConnectionCredentials credentials = new ConnectionCredentials(TwitchInfo.BotUsername, TwitchInfo.BotToken);
        TwitchClient client;
        TwitchAPI api;
        //ITwitchClient client1;



        public void Connect()
        {
            Console.WriteLine("Connecting");
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            WebSocketClient customClient = new WebSocketClient(clientOptions);
            client = new TwitchClient(customClient);
            client.Initialize(credentials, TwitchInfo.ChannelName);

            //client.ChatThrottler = new TwitchLib.Client.Services.MessageThrottler(client1, 20 / 2, TimeSpan.FromSeconds(30));
            //client.WhisperThrottler = new TwitchLib.Client.Services.MessageThrottler(client1, 20 / 2, TimeSpan.FromSeconds(30));

            client.OnLog += Client_OnLog;
            client.OnConnectionError += client_OnConnectionError;
            client.OnMessageReceived += client_OnMessageReceived;
            client.OnWhisperReceived += client_OnWhisperReceived;
            client.OnUserTimedout += client_OnUserTimedout;
            client.OnUserJoined += client_OnUserJoined;
            client.OnUserLeft += client_OnUserLeft;

            client.Connect();

            Console.WriteLine("Connected!");

            api = new TwitchAPI();
            //api.Settings.ClientId = TwitchInfo.ClientId;
        }

        //User leaves Chatroom
        private void client_OnUserLeft(object sender, OnUserLeftArgs e)
        {
            client.SendWhisper(TwitchInfo.ChannelName, $"{e.Username} has left  :(");
        }

        //User Joins Chatroom
        private void client_OnUserJoined(object sender, OnUserJoinedArgs e)
        {
            if (e.Username != TwitchInfo.ChannelName && e.Username != TwitchInfo.BotUsername) {
                client.SendWhisper(TwitchInfo.ChannelName, $"{e.Username} has joined!");
            }
        }

        //User Timedout in Chatroom
        private void client_OnUserTimedout(object sender, OnUserTimedoutArgs e)
        {
            client.SendMessage(TwitchInfo.ChannelName, $"Get rekt bro");
        }

        //User Whispers SnobbyBot
        private void client_OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            client.SendWhisper(e.WhisperMessage.Username, $"You said: {e.WhisperMessage.Message}");
        }

        //User Messages Snobbybot
        private void client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            //Hi response message
            if (e.ChatMessage.Message.StartsWith("hi", StringComparison.InvariantCultureIgnoreCase))
            {
                client.SendMessage(TwitchInfo.ChannelName, $"Hey there {e.ChatMessage.DisplayName}");
            }
            //!schedule
            if (e.ChatMessage.Message.Equals("!schedule", StringComparison.InvariantCultureIgnoreCase))
            {
                client.SendMessage(TwitchInfo.ChannelName, $"@{e.ChatMessage.DisplayName} Steven has no schedule at all sadly, he streams mostly in the evenings and on weekends");
            }
            //!uptime
            if (e.ChatMessage.Message.Equals("!uptime", StringComparison.InvariantCultureIgnoreCase))
            {
                //TODO: Add uptime guts
                //client.SendMessage(TwitchInfo.ChannelName, { e.ChatMessage.})
            }
            //!SnobbyBot
            if (e.ChatMessage.Message.Equals("!snobbybot", StringComparison.InvariantCultureIgnoreCase))
            {
                client.SendMessage(TwitchInfo.ChannelName, $"Hello, I am SnobbyBot, I am Snobbysteven's custom made bot, I am pretty dumb at the moment :)");
            }
        }

        //Error Connecting SnobbyBot
        private void client_OnConnectionError(object sender, OnConnectionErrorArgs e)
        {
            Console.WriteLine($"Error!! {e.Error}");
        }

        //Logger
        private void Client_OnLog(object sender, OnLogArgs e)
        {
            //TODO: Setup Logger System
            //Console.WriteLine(e.Data);
        }

        //Disconnects SnobbyBot
        public void Disconnect()
        {
            Console.WriteLine("Disconnecting");
        }


    }
}
