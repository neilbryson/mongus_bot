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
            var res = await MuteFactoryAsync(true);
            if (res) await ReplyAsync("Shhhhhhhh");
        }

        [Command("unmuteall")]
        [Alias("unmute")]
        public async Task UnmuteAsync()
        {
            var res = await MuteFactoryAsync(false);
            if (res) await ReplyAsync("Find the impostor!");
        }

        private async Task<bool> MuteFactoryAsync(bool shouldMute)
        {
            var res = await VoiceService.MuteAllAsync(shouldMute);
            if (!res)
            {
                await ReplyAsync("Whaaaat? There are no users in the voice channel!");
                return false;
            }
            return true;
        }
    }
}
