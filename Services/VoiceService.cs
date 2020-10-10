using System;
using Microsoft.Extensions.DependencyInjection;
using Discord.WebSocket;

namespace mongus_bot.Services
{
    public class VoiceService
    {
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;

        public VoiceService(IServiceProvider services)
        {
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _services = services;
        }
    }
}
