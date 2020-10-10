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

        public Task<bool> MuteAllAsync(bool shouldMute)
        {
            var taskCompletion = new TaskCompletionSource<bool>();
            var taskList = new List<Task>();
            var users = GetConnectedUsers();
            if (users.Count == 0)
            {
                taskCompletion.SetResult(false);
                return taskCompletion.Task;
            }

            foreach (var user in users)
            {
                var isMuted = user.IsMuted || user.IsSelfMuted;
                if ((shouldMute && isMuted) || (!shouldMute && !isMuted)) continue;
                taskList.Add(MuteUserAsync(user, shouldMute));
            }

            taskCompletion.SetResult(true);
            return taskCompletion.Task;
        }

        private async Task MuteUserAsync(SocketGuildUser user, bool shouldMute)
        {
            Console.WriteLine($"Modifying user {user.Id}, shouldMute : {shouldMute}");
            await user.ModifyAsync(u =>
            {
                u.Mute = shouldMute;
                u.Deaf = shouldMute;
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
