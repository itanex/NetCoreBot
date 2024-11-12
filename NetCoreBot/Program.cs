using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Threading.Tasks;
using NetCoreBot.Factories;
using NetCoreBot.Models;
using NetCoreBot.Services;
using TwitchLib.Client;
using TwitchLib.Client.Interfaces;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Enums;
using TwitchLib.Communication.Interfaces;
using TwitchLib.Communication.Models;

namespace NetCoreBot
{
    class Program
    {
        public static IConfiguration Configuration { get; private set; }
        private static string AccessToken
        {
            get
            {
                return Configuration
                    .GetSection("twitch")
                    .GetValue<string>("accessToken");
            }
        }
        private static string TwitchUsername
        {
            get
            {
                return Configuration
                    .GetSection("twitch")
                    .GetValue<string>("twitchUsername");
            }
        }
        private static string DefaultChannel
        {
            get
            {
                return Configuration
                    .GetSection("twitch")
                    .GetValue<string>("defaultChannel");
            }
        }

        public static async Task Main(string[] args)
        {
            var application = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(ConfigureApplication)
                .ConfigureServices(ConfigureServices)
                .UseSerilog()
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Log.Logger.Information("Application starting");

            await application.RunAsync();
        }

        public static void ConfigureApplication(HostBuilderContext hostBuilder, IConfigurationBuilder configuration)
        {
            configuration.Sources.Clear();
            configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddUserSecrets<Program>(false, true);

            Configuration = configuration.Build();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<ConsoleService>();
            services.AddSingleton<IBot, Bot>();
            services.AddSingleton<IChatCommandFactory, ChatCommandFactory>();
            services.AddSingleton<IChannelEventFactory, ChannelEventFactory>();

            services.AddSingleton<ITwitchClient, TwitchClient>(x =>
            {
                var credentials = new TwitchCredentials(TwitchUsername, AccessToken);

                var clientOptions = new ClientOptions()
                {
                    ClientType = ClientType.Chat,
                    UseSsl = true,
                    MessagesAllowedInPeriod = 750,
                    ThrottlingPeriod = TimeSpan.FromSeconds(30),
                };

                var customClient = new WebSocketClient(clientOptions);
                var client = new TwitchClient(customClient);
                client.Initialize(credentials.Credentials, DefaultChannel);

                return client;
            });
        }
    }
}
