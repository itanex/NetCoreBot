using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCoreBot.ChatCommands;
using TwitchLib.Client.Interfaces;
using twitchbot_sharp.ChatCommands;
using Microsoft.Extensions.Configuration;

namespace NetCoreBot.Factories
{
    public class ChatCommandFactory : IChatCommandFactory
    {
        private readonly ITwitchClient twitchClient;
        private readonly ILoggerFactory loggerFactory;
        private readonly IConfiguration configuration;

        public ChatCommandFactory(ITwitchClient twitchClient, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            this.twitchClient = twitchClient;
            this.loggerFactory = loggerFactory;
            this.configuration = configuration;
        }

        public DiceCommand GetDiceCommand()
        {
            return new DiceCommand(loggerFactory.CreateLogger<DiceCommand>(), twitchClient);
        }

        public SlopezCommand GetSlopezCommand()
        {
            return new SlopezCommand(loggerFactory.CreateLogger<SlopezCommand>(), twitchClient);
        }

        public DeathCommand GetDeathCommand()
        {
            return new DeathCommand(loggerFactory.CreateLogger<DeathCommand>(), twitchClient);
        }

        public AboutCommand GetAboutCommand()
        {
            return new AboutCommand(loggerFactory.CreateLogger<AboutCommand>(), twitchClient, configuration);
        }

        public HostCommand GetHostCommand()
        {
            return new HostCommand(loggerFactory.CreateLogger<HostCommand>(), twitchClient);
        }

        public HelpCommand GetHelpCommand()
        {
            return new HelpCommand(loggerFactory.CreateLogger<HelpCommand>(), twitchClient);
        }
    }
}
