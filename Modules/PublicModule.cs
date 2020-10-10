using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace mongus_bot.Modules
{
    public class PublicModule : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        [Alias("pong", "hello")]
        public Task PingAsync()
            => ReplyAsync("pong!");

        [Command("f")]
        [Alias("F")]
        public async Task RespectAsync()
        {
            var message = await ReplyAsync("F");
            await message.AddReactionAsync(new Emoji("\uD83C\uDDEB"));
        }
    }
}
