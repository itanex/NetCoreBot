using Microsoft.Extensions.Logging;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Interfaces;

namespace NetCoreBot.ChannelEvents
{
    public class RaidEvents
    {
        private readonly ILogger<RaidEvents> logger;
        private readonly ITwitchClient twitchClient;

        public RaidEvents(ILogger<RaidEvents> logger, ITwitchClient twitchClient)
        {
            this.logger = logger;
            this.twitchClient = twitchClient;
        }

        public void RaidNotification(object sender, OnRaidNotificationArgs e)
        {
            twitchClient.Announce(e.Channel, $"RAID: Thank you, {e.RaidNotification.DisplayName}, for bringing the {e.RaidNotification.MsgParamViewerCount} viewer(s) with you!");

            // TODO: This should wait 3 seconds before sending the following message
            var link = $"https://twitch.tv/${e.RaidNotification.DisplayName}";

            twitchClient.SendMessage(e.Channel, $"Check out @{e.RaidNotification.DisplayName} at ${link}");
        }
    }
}
