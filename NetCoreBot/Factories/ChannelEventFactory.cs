using Microsoft.Extensions.Logging;
using NetCoreBot.ChannelEvents;
using NetCoreBot.EventCommands;
using TwitchLib.Client.Interfaces;

namespace NetCoreBot.Factories
{
    public class ChannelEventFactory : IChannelEventFactory
    {
        private readonly ITwitchClient twitchClient;
        private readonly ILoggerFactory loggerFactory;

        public ChannelEventFactory(ITwitchClient twitchClient, ILoggerFactory loggerFactory)
        {
            this.twitchClient = twitchClient;
            this.loggerFactory = loggerFactory;
        }

        public RaidEvents GetRaidEvents()
        {
            return new RaidEvents(loggerFactory.CreateLogger<RaidEvents>(), twitchClient);
        }

        public SubscriptionEvents GetSubscriptionEvents()
        {
            return new SubscriptionEvents(loggerFactory.CreateLogger<SubscriptionEvents>(), twitchClient);
        }
    }
}
