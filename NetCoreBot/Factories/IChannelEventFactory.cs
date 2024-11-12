using NetCoreBot.ChannelEvents;
using NetCoreBot.EventCommands;

namespace NetCoreBot.Factories
{
    public interface IChannelEventFactory
    {
        RaidEvents GetRaidEvents();
        SubscriptionEvents GetSubscriptionEvents();
    }
}