using System.Text.RegularExpressions;
using TwitchLib.Client.Models;

namespace NetCoreBot.ChatCommands
{
    public interface IChatCommand
    {
        bool Execute(ChatMessage chatMessage, string inputCommand, GroupCollection args);
    }
}