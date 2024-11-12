using Microsoft.Extensions.Logging;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Interfaces;

namespace NetCoreBot.EventCommands
{
    public class SubscriptionEvents
    {
        private readonly ILogger<SubscriptionEvents> logger;
        private readonly ITwitchClient twitchClient;

        public SubscriptionEvents(ILogger<SubscriptionEvents> logger, ITwitchClient twitchClient)
        {
            this.logger = logger;
            this.twitchClient = twitchClient;
        }

        public void NewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            var username = e.Subscriber.DisplayName;

            switch (e.Subscriber.SubscriptionPlan)
            {
                case SubscriptionPlan.Prime:
                    twitchClient.SendMessage(e.Channel, $"Thank you, @{username}, for using your Prime sub to join the Colony!");
                    break;
                case SubscriptionPlan.Tier1:
                    twitchClient.SendMessage(e.Channel, $"Thank you, @{username}, for subscribing to the colony at Tier 1!");
                    break;
                case SubscriptionPlan.Tier2:
                    twitchClient.SendMessage(e.Channel, $"Thank you, @{username}, for subscribing to the colony at Tier 2!");
                    break;
                case SubscriptionPlan.Tier3:
                    twitchClient.SendMessage(e.Channel, $"Thank you, @{username}, for subscribing to the colony at Tier 3!");
                    break;
                default:
                    twitchClient.SendMessage(e.Channel, $"Thank you, @{username}, for subscribing to the colony!!!");
                    break;
            }
        }

        public void ReSubscriber(object sender, OnReSubscriberArgs e)
        {
            var username = e.ReSubscriber.DisplayName;

            switch (e.ReSubscriber.SubscriptionPlan)
            {
                case SubscriptionPlan.Prime:
                    twitchClient.SendMessage(e.Channel, $"Thank you, @{username}, for using your Prime sub to join the Colony!");
                    break;
                case SubscriptionPlan.Tier1:
                    twitchClient.SendMessage(e.Channel, $"Thank you, @{username}, for continuing your subscription with the colony at Tier 1!");
                    break;
                case SubscriptionPlan.Tier2:
                    twitchClient.SendMessage(e.Channel, $"Thank you, @{username}, for continuing your subscription with the colony at Tier 2!");
                    break;
                case SubscriptionPlan.Tier3:
                    twitchClient.SendMessage(e.Channel, $"Thank you, @{username}, for continuing your subscription with the colony at Tier 3!");
                    break;
                default:
                    twitchClient.SendMessage(e.Channel, $"Thank you, @{username}, for continuing your subscription with the colony!!!");
                    break;
            }
        }

        public void GiftedSubscription(object sender, OnGiftedSubscriptionArgs e)
        {
            var username = e.GiftedSubscription.MsgParamRecipientDisplayName;
            var gifter = e.GiftedSubscription.DisplayName;

            switch (e.GiftedSubscription.MsgParamSubPlan)
            {
                case SubscriptionPlan.Tier1:
                    twitchClient.SendMessage(e.Channel, $"Thank you, @{gifter}, for recruiting ${username} into the colony at Tier 1!");
                    break;
                case SubscriptionPlan.Tier2:
                    twitchClient.SendMessage(e.Channel, $"Thank you, @{gifter}, for recruiting ${username} into the colony at Tier 2!");
                    break;
                case SubscriptionPlan.Tier3:
                    twitchClient.SendMessage(e.Channel, $"Thank you, @{gifter}, for recruiting ${username} into the colony at Tier 3!");
                    break;
                default:
                    twitchClient.SendMessage(e.Channel, $"Thank you, @{gifter}, for recruiting ${username} into the colony!!!");
                    break;
            }
        }

        public void CommunitySubscription(object sender, OnCommunitySubscriptionArgs e)
        {
            var giftCount = e.GiftedSubscription.MsgParamMassGiftCount;
            var gifter = e.GiftedSubscription.DisplayName;

            switch (e.GiftedSubscription.MsgParamSubPlan)
            {
                case SubscriptionPlan.Tier1:
                    twitchClient.SendMessage(e.Channel, $"Thank you, @{gifter}, for gifting ${giftCount} Tier 1 subs to the community!");
                    break;
                case SubscriptionPlan.Tier2:
                    twitchClient.SendMessage(e.Channel, $"Thank you, @{gifter}, for gifting ${giftCount} Tier 2 subs to the community!");
                    break;
                case SubscriptionPlan.Tier3:
                    twitchClient.SendMessage(e.Channel, $"Thank you, @{gifter}, for gifting ${giftCount} Tier 3 subs to the community!");
                    break;
                default:
                    twitchClient.SendMessage(e.Channel, $"Thank you, @{gifter}, for gifting ${giftCount} subs to the community!!!");
                    break;
            }
        }
    }
}
