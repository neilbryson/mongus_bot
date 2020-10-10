using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using mongus_bot.Services;

namespace mongus_bot.Modules
{
    public class VoiceModule : ModuleBase<SocketCommandContext>
    {
        public VoiceService VoiceService { get; set; }

        [Command("muteall")]
        [Alias("mute")]
        public async Task MuteAsync()
        {
            var res = await MuteFactoryAsync(true);
            if (res.Item1)
            {
                var embed = BuildEmbed(res.Item2, true);
                await ReplyAsync(embed: embed);
            }
        }

        [Command("unmuteall")]
        [Alias("unmute")]
        public async Task UnmuteAsync()
        {
            var res = await MuteFactoryAsync(false);
            if (res.Item1)
            {
                var embed = BuildEmbed(res.Item2, false);
                await ReplyAsync(embed: embed);
            }
        }

        private Embed BuildEmbed(List<string> users, bool isMuted)
        {
            string title = isMuted ? "Shhhhhhhhh" : "Find the impostor!";
            string footer = isMuted ? "You are now muted." : "You are now unmuted.";
            Color color = isMuted ? Color.DarkRed : Color.Teal;
            var userList = UserList(users);
            return new EmbedBuilder().WithTitle(title)
                .WithDescription(userList)
                .WithColor(color)
                .WithFooter(f => f.Text = footer)
                .WithCurrentTimestamp()
                .Build();
        }

        private string UserList(List<string> users)
        {
            var sb = new StringBuilder();
            users.ForEach(u =>
            {
                sb.Append($"- {u}\n");
            });
            return sb.ToString();
        }

        private async Task<Tuple<bool, List<string>>> MuteFactoryAsync(bool shouldMute)
        {
            var res = await VoiceService.MuteAllAsync(shouldMute);
            if (!res.Item1)
            {
                await ReplyAsync("Whaaaat? There are no users in the voice channel!");
                return res;
            }
            return res;
        }
    }
}
