using System.Threading.Tasks;
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
            var res = await VoiceService.MuteAllAsync(true);
            if (!res)
            {
                await ReplyAsync("Whaaaat? There are no users in the voice channel!");
                return;
            }
            await ReplyAsync("Shhhhhhhh");
        }

        [Command("unmuteall")]
        [Alias("unmute")]
        public async Task UnmuteAsync()
        {
            var res = await VoiceService.MuteAllAsync(false);
            if (!res)
            {
                await ReplyAsync("Whaaaat? There are no users in the voice channel!");
                return;
            }
            await ReplyAsync("Find the impostor!");
        }
    }
}
