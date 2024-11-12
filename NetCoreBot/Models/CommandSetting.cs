using System;
using System.Text.RegularExpressions;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace NetCoreBot.Models
{
    public class CommandSetting
    {
        public string RegularExpression { get; set; }
        public int Timeout { get; set; }
        public Func<ChatMessage, string, GroupCollection, bool> Command { get; set; }

        // User Priviledged levels
        public bool IsGlobalCommand { get; set; }
        public bool Mod { get; set; }
        public bool Vip { get; set; }
        public bool Subscriber { get; set; }
        public bool Follower { get; set; }
        public bool Viewer { get; set; }
    }
}
