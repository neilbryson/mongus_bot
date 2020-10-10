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
            await ReplyAsync("Shhhhhh");
        }

        [Command("unmuteall")]
        [Alias("unmute")]
        public async Task UnmuteAsync()
        {
            await ReplyAsync("Find the impostor!");
        }
    }
}
