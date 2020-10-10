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

        public Task<Tuple<bool, List<string>>> MuteAllAsync(bool shouldMute)
        {
            var taskCompletion = new TaskCompletionSource<Tuple<bool, List<string>>>();
            var taskList = new List<Task>();
            var users = GetConnectedUsers();
            var mutedUsers = new List<string>();
            if (users.Count == 0)
            {
                taskCompletion.SetResult(Tuple.Create(false, mutedUsers));
                return taskCompletion.Task;
            }

            foreach (var user in users)
            {
                var isMuted = user.IsMuted || user.IsSelfMuted;
                if ((shouldMute && isMuted) || (!shouldMute && !isMuted)) continue;
                taskList.Add(MuteUserAsync(user, shouldMute));
                mutedUsers.Add($"{user.Username}#{user.Discriminator}");
            }

            taskCompletion.SetResult(Tuple.Create(true, mutedUsers));
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
