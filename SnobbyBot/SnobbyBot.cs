using System;
using TwitchLib.Api;
using TwitchLib.Api.Models.Helix.Users.GetUsersFollows;
using TwitchLib.Api.Models.v5.Subscriptions;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;

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
            client = new TwitchClient();
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
            client.OnJoinedChannel += client_OnJoinedChannel;
            client.OnLeftChannel += client_OnLeftChannel;
            client.OnBeingHosted += client_OnBeingHosted;


            client.Connect();

            Console.WriteLine("Connected!");
            Console.WriteLine("SnobbyBot has joined " + TwitchInfo.ChannelName);


            api = new TwitchAPI();
            //api.Settings.ClientId = TwitchInfo.ClientId;
        }

        //User leaves Chatroom
        private void client_OnUserLeft(object sender, OnUserLeftArgs e)
        {
            client.SendWhisper(TwitchInfo.ChannelName, $"{e.Username} has left :(");
        }

        //User Joins Chatroom
        private void client_OnUserJoined(object sender, OnUserJoinedArgs e)
        {
            if (e.Username != TwitchInfo.ChannelName && e.Username != TwitchInfo.BotUsername)
            {
                client.SendWhisper(TwitchInfo.ChannelName, $"{e.Username} has joined!");
            }
        }

        //User Timedout in Chatroom
        private void client_OnUserTimedout(object sender, OnUserTimedoutArgs e)
        {
            client.SendMessage(TwitchInfo.ChannelName, $"Get rekt bro");
        }

        //User Banned in Chatroom
        

        //SnobbyBot Message when joining channel
        private void client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            client.SendMessage(e.Channel, "I have arrived bitches!");
        }

        //SnobbyBot Message when leaving channel
        private void client_OnLeftChannel(Object sender, OnLeftChannelArgs e)
        {
            Console.WriteLine("SnobbyBot has left " + TwitchInfo.ChannelName);
            client.SendMessage(e.Channel, "Peace out!");
        }

        //User Whispers SnobbyBot
        private void client_OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            client.SendWhisper(e.WhisperMessage.Username, $"You said: {e.WhisperMessage.Message}");
        }

        private void client_SayMessage(String messageText)
        {
            client.SendMessage(TwitchInfo.ChannelName, messageText);
        }

        //User Messages Snobbybot
        private void client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            switch (e.ChatMessage.Message)
            {
                case "hi":
                    client.SendMessage(TwitchInfo.ChannelName, $"Hey there {e.ChatMessage.DisplayName}");
                    break;

                case "!schedule":
                    client.SendMessage(TwitchInfo.ChannelName, $"@{e.ChatMessage.DisplayName} Steven has no schedule at all sadly, he streams mostly in the evenings and on weekends");
                    break;

                case "!uptime":
                    //TODO: Add uptime guts
                    break;

                case "!snobbybot":
                    client.SendMessage(TwitchInfo.ChannelName, $"Hello, I am SnobbyBot, I am Snobbysteven's custom made bot, I am pretty dumb at the moment :)");
                    break;

                case "!res":
                    client.SendMessage(TwitchInfo.ChannelName, $"1024x768 Stretched");
                    break;

                //Allow Twitch streamer to tell the bot to leave
                case "!leave":
                    if (e.ChatMessage.IsBroadcaster)
                    {
                        client.SendMessage(TwitchInfo.ChannelName, $"I'm out of here! Peace!");
                        Disconnect();
                    }
                    break;

                case "!twitter":
                    client.SendMessage(TwitchInfo.ChannelName, $"You can follow snobbysteven on Twitter at {TwitchInfo.TwitterUrl}");
                    break;

            }
        }


        //Channel Mod Settings and controls
        //TODO: Add Mod settings
        //private void client_TimeOutUser(object sender, )


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

        //Hosted by another streamer
        private void client_OnBeingHosted(object sender, OnBeingHostedArgs e)
        {
            client.SendMessage(TwitchInfo.ChannelName, $"Hey {e.BeingHostedNotification.HostedByChannel} thanks for the host!");
        }



        //Disconnects SnobbyBot
        public void Disconnect()
        {
            Console.WriteLine("Disconnecting");
            client.Disconnect();
        }


    }
}