using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreBot.Services
{
    public class ConsoleService : IHostedService
    {
        private readonly IHostApplicationLifetime applicationLifetime;
        private readonly IBot bot;

        public ConsoleService(IHostApplicationLifetime applicationLifetime, IBot bot)
        {
            this.applicationLifetime = applicationLifetime;
            this.bot = bot;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            applicationLifetime.ApplicationStarted
                .Register(() =>
                {
                    Task.Run(() =>
                    {
                        bot.ConnectAndRun();

                        Console.ReadKey();

                        applicationLifetime.StopApplication();

                        return Task.CompletedTask;
                    });
                });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"STOPINGAync: Stopping application");
            return Task.CompletedTask;
        }
    }
}