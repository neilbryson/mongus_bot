using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Discord.WebSocket;

namespace mongus_bot.Services
{
    public class VoiceService
    {
        private readonly DiscordSocketClient _discord;
        private readonly IConfiguration _config;

        public VoiceService(IServiceProvider services)
        {
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _config = services.GetRequiredService<IConfiguration>();
        }

        public Task MuteUsersAsync(List<SocketGuildUser> users, bool shouldMute)
        {
            var mutedUsers = new List<SocketGuildUser>();
            var tasks = new List<Task>();
            if (users.Count == 0) return Task.FromResult<List<SocketGuildUser>>(mutedUsers);

            foreach (var user in users)
            {
                var isMuted = user.IsMuted || user.IsSelfMuted;
                if ((shouldMute && isMuted) || (!shouldMute && !isMuted)) continue;
                tasks.Add(MuteUserAsync(user, shouldMute));
                mutedUsers.Add(user);
            }

            return Task.FromResult<List<SocketGuildUser>>(mutedUsers);
        }

        private async Task MuteUserAsync(SocketGuildUser user, bool shouldMute)
        {
            Console.WriteLine($"Modifying user {user.Id}, shouldMute : {shouldMute}");
            await user.ModifyAsync(u =>
            {
                u.Mute = shouldMute;
            });
        }

        public IReadOnlyCollection<SocketGuildUser> GetConnectedUsers()
        {
            var channel = GetVoiceChannel();
            return channel.Users;
        }

        private SocketVoiceChannel GetVoiceChannel()
        {
            var channelId = ulong.Parse(_config["voice_channel_id"]);
            var channel = _discord.GetChannel(channelId) as SocketVoiceChannel;
            return channel;
        }
    }
}
