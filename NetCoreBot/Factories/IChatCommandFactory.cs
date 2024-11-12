using NetCoreBot.ChatCommands;
using twitchbot_sharp.ChatCommands;

namespace NetCoreBot.Factories
{
    public interface IChatCommandFactory
    {
        AboutCommand GetAboutCommand();
        DeathCommand GetDeathCommand();
        DiceCommand GetDiceCommand();
        HelpCommand GetHelpCommand();
        HostCommand GetHostCommand();
        SlopezCommand GetSlopezCommand();
    }
}