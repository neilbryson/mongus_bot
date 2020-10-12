using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace mongus_bot.Modules
{
    public class PublicModule : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        [Alias("pong", "hello")]
        [Summary("You know it.")]
        public Task PingAsync()
            => ReplyAsync("pong!");

        [Command("f")]
        [Summary("Pay your respects.")]
        public async Task RespectAsync()
        {
            var message = await ReplyAsync("F");
            await message.AddReactionAsync(new Emoji("\uD83C\uDDEB"));
        }
    }
}
