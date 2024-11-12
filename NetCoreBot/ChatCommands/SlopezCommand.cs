using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;

namespace NetCoreBot.ChatCommands
{
    public class SlopezCommand : IChatCommand
    {
        private readonly string[] responses = new string[]
        {
            "GET IN YOUR CORNER!!!",
            "I hear there is a box somewhere that really needs to be cleaned",
            "What did you do now?",
            "Really! Slopez, What were you thinking. Oh yeah, that function has been broken for a while hasn't it.",
            "There must be something less troublesome you could be doing.",
            "Don't make me promote your further!",
        };

        private readonly ILogger<SlopezCommand> logger;
        private readonly ITwitchClient twitchClient;

        public SlopezCommand(ILogger<SlopezCommand> logger, ITwitchClient twitchClient)
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
            if (responses.Any())
            {
                var message = responses[new Random().Next(0, responses.Length)];

                twitchClient.SendMessage(chatMessage.Channel, message);

                logger.LogInformation($"* Executed `Slopez` command :: in {chatMessage.Channel} || {chatMessage.DisplayName} > {message}");
            }

            return true;
        }
    }
}
