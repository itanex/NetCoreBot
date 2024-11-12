using Microsoft.Extensions.Logging;
using NetCoreBot.ChatCommands;
using System.Text.RegularExpressions;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;

namespace twitchbot_sharp.ChatCommands
{
    public class HostCommand : IChatCommand
    {
        private readonly ILogger<HostCommand> logger;
        private readonly ITwitchClient twitchClient;

        public HostCommand(ILogger<HostCommand> logger, ITwitchClient twitchClient)
        {
            this.logger = logger;
            this.twitchClient = twitchClient;

            if (!twitchClient.IsConnected)
            {
                twitchClient.Connect();
            }
        }

        public bool Execute(ChatMessage chatMessage, string inputCommand, GroupCollection args)
        {
            twitchClient.SendMessage(chatMessage.Channel, "Today, I am currently running as a DotNetCore Console Application. I am designed to replace the old Split Personality Bot that previously helped out Timy");

            logger.LogInformation($"* Executed `Host` in ${chatMessage.Channel} || {chatMessage.DisplayName}");

            return true;
        }
    }
}
