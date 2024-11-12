using Microsoft.Extensions.Logging;
using NetCoreBot.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;

namespace NetCoreBot.ChatCommands
{
    public class DiceCommand : IChatCommand
    {
        private readonly ILogger<DiceCommand> logger;
        private readonly ITwitchClient client;

        public DiceCommand(ILogger<DiceCommand> logger, ITwitchClient twitchClient)
        {
            this.logger = logger;
            this.client = twitchClient;

            if(!twitchClient.IsConnected)
            {
                twitchClient.Connect();
            }
        }

        public bool Execute(ChatMessage chatMessage, string inputCommand, GroupCollection args)
        {
            if (!int.TryParse(args[3].Value, out int amount))
            {
                amount = 1;
            }

            if (!int.TryParse(args[4].Value, out int sides))
            {
                return false;
            }

            var results = RollDice(amount, sides);

            client.SendMessage(chatMessage.Channel, $"You rolled a {amount}d{sides} that resulted in [ {String.Join(", ", results.Rolls)} ] in total {results.total}");

            logger.LogInformation($"* Executed `Dice Command` command :: in {chatMessage.Channel} || {chatMessage.DisplayName} > {chatMessage.Message}", results);

            return true;
        }

        public static RollResult RollDice(int numberOfDice, int sides)
        {
            var rolls = new List<int>(numberOfDice);
            var total = 0;
            var amount = numberOfDice;

            do
            {
                var rnd = new Random();

                int score = rnd.Next(1, sides);
                total += score;
                rolls.Add(score);
                amount--;
            } while (amount > 0);

            return new RollResult(rolls.ToArray(), total);
        }
    }
}
