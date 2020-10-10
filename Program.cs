using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using mongus_bot.Services;

namespace mongus_bot
{
    class Program
    {
        private IConfiguration _config;
        private DiscordSocketClient _client;

        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            _config = BuildConfig();
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info
            });
            var services = ConfigureServices();
            _client.Log += LogAsync;
            services.GetRequiredService<CommandService>().Log += LogAsync;
            await _client.LoginAsync(TokenType.Bot, _config["token"]);
            await _client.StartAsync();
            await services.GetRequiredService<CommandHandlingService>().InitialiseAsync();
            await Task.Delay(-1);
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton<CommandService>()
                .AddSingleton<VoiceService>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton(_config)
                .BuildServiceProvider();
        }

        private IConfiguration BuildConfig()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("config.json")
                .Build();
        }
    }
}
