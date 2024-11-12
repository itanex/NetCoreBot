using TwitchLib.Client.Models;

namespace NetCoreBot.Models
{
    record TwitchCredentials(string Username, string AccessToken)
    {
        public ConnectionCredentials Credentials { get; } = new ConnectionCredentials(Username, AccessToken);
    }
}
