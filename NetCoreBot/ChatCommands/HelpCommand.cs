using Microsoft.Extensions.Logging;
using NetCoreBot.ChatCommands;
using System;
using System.Text.RegularExpressions;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;

namespace twitchbot_sharp.ChatCommands
{
    public class HelpCommand : IChatCommand
    {
        private readonly ILogger<HelpCommand> logger;
        private readonly ITwitchClient twitchClient;

        private readonly string[] responses = new string[]
        {
            "I am stuck in this corner and unable to assist you at this time?",
            "I could really use some help right now. Do you know where I can find a better knife?",
            "HELP!",
            "I am afraid I can't do that",
            "Are you... my friend?",
        };

        public HelpCommand(ILogger<HelpCommand> logger, ITwitchClient twitchClient)
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
            twitchClient.SendMessage(chatMessage.Channel, responses[new Random().Next(0, responses.Length)]);

            logger.LogInformation($"* Executed `Help` in ${chatMessage.Channel} || {chatMessage.DisplayName}");

            return true;
        }
    }
}
