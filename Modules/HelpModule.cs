using System;
using System.Threading.Tasks;
using System.Text;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace mongus_bot.Modules
{
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _commands;

        public HelpModule(IServiceProvider services)
        {
            _commands = services.GetRequiredService<CommandService>();
        }

        [Command("help")]
        [Alias("h")]
        [Summary("Lists all the available commands")]
        public async Task HelpAsync()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("A simple Discord bot for Among Us players!\n");
            foreach (var c in _commands.Commands)
            {
                sb.Append($"**{c.Name}** {GetCommandAliases(c)}\n{c.Summary}\n");
            }
            var embed = new EmbedBuilder()
                .WithAuthor(Context.Client.CurrentUser)
                .WithDescription(sb.ToString())
                .Build();
            await ReplyAsync(embed: embed);
        }

        private string GetCommandAliases(CommandInfo command)
        {
            if (command.Aliases.Count <= 1) return "";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < command.Aliases.Count; i++)
            {
                if (i == 0) sb.Append("(");
                string alias = command.Aliases[i];
                if (alias == command.Name) continue;
                sb.Append($"**{command.Aliases[i]}**");
                if (i != command.Aliases.Count - 1) sb.Append(", ");
                else sb.Append(")");
            }
            return sb.ToString();
        }
    }
}
