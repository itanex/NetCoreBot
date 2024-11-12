using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NetCoreBot.Factories;
using NetCoreBot.Models;
using TwitchLib.Client.Events;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;

namespace NetCoreBot
{

    public class Bot : IBot
    {
        private readonly string appName = "NetCoreBot";
        private readonly string defaultChannel = "timythetermite";

        private readonly List<CommandSetting> commands;

        // Dependency Injections
        private readonly ILogger<Bot> log;
        private readonly IChatCommandFactory chatCommandFactory;
        private readonly IChannelEventFactory channelEvents;
        private readonly ITwitchClient client;

        public Bot(
            ILogger<Bot> log,
            IChatCommandFactory chatCommands,
            IChannelEventFactory channelEvents,
            ITwitchClient client)
        {
            this.log = log;
            this.chatCommandFactory = chatCommands;
            this.channelEvents = channelEvents;
            this.client = client;

            if (!client.IsConnected)
            {
                client.Connect();
            }

            commands = new List<CommandSetting>()
            {
                new CommandSetting()
                {
                    RegularExpression = "#(slopez)",
                    IsGlobalCommand = true,
                    Timeout = 20,
                    Mod = true,
                    Vip = true,
                    Subscriber = true,
                    Follower = true,
                    Viewer = true,
                    Command = chatCommands.GetSlopezCommand().Execute
                },
                new CommandSetting()
                {
                    RegularExpression= @"#(dice) ((\d?)d(\d{1,3}))",
                    IsGlobalCommand = true,
                    Timeout = 2,
                    Mod = true,
                    Vip = true,
                    Subscriber = true,
                    Follower = true,
                    Viewer = true,
                    Command = chatCommands.GetDiceCommand().Execute
                },
                new CommandSetting()
                {
                    RegularExpression= @"#(death|died)",
                    IsGlobalCommand = true,
                    Timeout = 5,
                    Mod = true,
                    Vip = true,
                    Subscriber = true,
                    Follower = false,
                    Viewer = false,
                    Command = chatCommands.GetDeathCommand().Execute
                },
                new CommandSetting()
                {
                    RegularExpression= @"#(help)",
                    IsGlobalCommand = true,
                    Timeout = 15,
                    Mod = true,
                    Vip = true,
                    Subscriber = true,
                    Follower = true,
                    Viewer = true,
                    Command = chatCommands.GetHelpCommand().Execute
                },
                new CommandSetting()
                {
                    RegularExpression= @"#(host)",
                    IsGlobalCommand = true,
                    Timeout = 5,
                    Mod = true,
                    Vip = true,
                    Subscriber = true,
                    Follower = true,
                    Viewer = true,
                    Command = chatCommands.GetHostCommand().Execute
                },
                new CommandSetting()
                {
                    RegularExpression= @"#(about)",
                    IsGlobalCommand = true,
                    Timeout = 5,
                    Mod = true,
                    Vip = true,
                    Subscriber = true,
                    Follower = true,
                    Viewer = true,
                    Command = chatCommands.GetAboutCommand().Execute
                },
            };
        }

        public void ConnectAndRun()
        {
            client.OnLog += Client_OnLog;

            client.OnConnected += Client_OnConnected;
            client.OnIncorrectLogin += Client_OnIncorrectLogin;

            client.OnMessageReceived += Client_OnMessageReceived;

            client.OnNewSubscriber += channelEvents.GetSubscriptionEvents().NewSubscriber;
            client.OnReSubscriber += channelEvents.GetSubscriptionEvents().ReSubscriber;
            client.OnGiftedSubscription += channelEvents.GetSubscriptionEvents().GiftedSubscription;
            client.OnCommunitySubscription += channelEvents.GetSubscriptionEvents().CommunitySubscription;

            client.OnRaidNotification += channelEvents.GetRaidEvents().RaidNotification;
        }

        private void Client_OnIncorrectLogin(object sender, OnIncorrectLoginArgs e)
        {
            log.LogError("Unable to log in using current credentials");
        }

        private void Client_OnLog(object sender, OnLogArgs e)
        {
            log.LogInformation($"{e.DateTime.ToUniversalTime()}: {e.BotUsername} - {e.Data}");
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            log.LogInformation($"Connected to {e.AutoJoinChannel}");
        }

        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            var message = $"Hey guys! I am {appName}, connected via TwitchLib!";

            log.LogInformation(message);

            client.SendMessage(e.Channel, message);
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (!e.ChatMessage.Channel.Equals(defaultChannel, StringComparison.InvariantCultureIgnoreCase))
            {
                log.LogWarning("Message recieved for incorrect channel", e.ChatMessage.Channel);

                return;
            }

            var inputCommand = e.ChatMessage.Message.Trim();

            foreach (var command in commands)
            {
                if (!IsAuthorized(command, e.ChatMessage))
                {
                    continue;
                }

                // Parse command
                var regEx = new Regex(command.RegularExpression, RegexOptions.IgnoreCase);
                var commandFrags = regEx.Match(inputCommand);

                if (!commandFrags.Success)
                {
                    continue;
                }

                // TODO: need command time out handled

                command.Command(e.ChatMessage, inputCommand, commandFrags.Groups);
            }
        }

        private static bool IsAuthorized(CommandSetting command, ChatMessage chat)
        {
            if (command.Viewer && chat.UserType == TwitchLib.Client.Enums.UserType.Viewer)
            {
                return true;
            }

            if (chat.UserType == TwitchLib.Client.Enums.UserType.Broadcaster)
            {
                return true;
            }

            if (chat.UserType == TwitchLib.Client.Enums.UserType.Moderator && command.Mod)
            {
                return true;
            }

            if (chat.IsSubscriber && command.Subscriber)
            {
                return true;
            }

            if (chat.IsVip && command.Vip)
            {
                return true;
            }

            // ChatUser does not have a follower property
            // if (user.follower && command.follower) {
            //     return true;
            // }

            return false;
        }

        private void Client_OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            client.SendMessage(e.WhisperMessage.Username, $"Roger, Roger!");
            //if (e.WhisperMessage.Username == "my_friend")
            //    client.SendWhisper(e.WhisperMessage.Username, "Hey! Whispers are so cool!!");
        }
    }
}
