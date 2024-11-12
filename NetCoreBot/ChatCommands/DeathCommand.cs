using Microsoft.Extensions.Logging;
using System;
using System.Text.RegularExpressions;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;

namespace NetCoreBot.ChatCommands
{
    public class DeathCommand : IChatCommand
    {
        private readonly ILogger<DeathCommand> logger;
        private readonly ITwitchClient twitchClient;

        // Time for command to not announce
        private readonly TimeSpan duration = TimeSpan.FromMinutes(5);

        private readonly string[] responses = new string[]
        {
            "We're gonna need another Timy!",
            "Timy is finding the quickest way to spawn new Timys",
            "Timy tried taking on gravity and lost",
            "Gonna need an abacus for this many deaths",
            "Timy needs to stop dying"
        };

        private DateTime lastExecuted = DateTime.Now;
        private DateTime lastAnnouncement = DateTime.Now;
        private bool firstRun = true;
        private int deathCount = 0;

        public DeathCommand(ILogger<DeathCommand> logger, ITwitchClient twitchClient)
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
            // Update DeathCount
            deathCount++;

            // Record last time command executed
            lastExecuted = DateTime.Now;

            // if lastAnnouncement is > 5 minutes since lastExecuted or we have a factor of 10 deaths
            if (firstRun || lastAnnouncement - lastExecuted > duration || deathCount % 10 == 0)
            {
                firstRun = false;
                // Record last announcement
                lastAnnouncement = DateTime.Now;

                twitchClient.SendMessage(chatMessage.Channel, responses[new Random().Next(0, responses.Length)]);
            }

            logger.LogInformation($"* Executed `Death` in ${chatMessage.Channel} :: {deathCount} || {chatMessage.DisplayName}");

            return true;
        }
    }
}
