using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NetCoreBot.ChatCommands;
using System.Text.RegularExpressions;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;

namespace twitchbot_sharp.ChatCommands
{
    public class AboutCommand : IChatCommand
    {
        private readonly ILogger<AboutCommand> logger;
        private readonly ITwitchClient twitchClient;

        public string BotName { get; init; }

        public AboutCommand(ILogger<AboutCommand> logger, ITwitchClient twitchClient, IConfiguration configuration)
        {
            this.logger = logger;
            this.twitchClient = twitchClient;

            BotName = configuration
                .GetSection("twitch")
                .GetValue<string>("botName");

            if (!twitchClient.IsConnected)
            {
                twitchClient.Connect();
            }
        }

        public bool Execute(ChatMessage chatMessage, string inputCommand, GroupCollection args)
        {
            twitchClient.SendMessage(chatMessage.Channel, $"Hi! I am {BotName}. I am written as a DotNetCore console application using the TwitchLib library. https://twitchlib.github.io/");

            logger.LogInformation($"* Executed `About` in ${chatMessage.Channel} || {chatMessage.DisplayName}");

            return true;
        }
    }
}
