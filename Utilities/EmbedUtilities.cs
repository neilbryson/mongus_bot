using System.Collections.Generic;
using System.Text;
using Discord;

namespace mongus_bot.Utilities
{
    class EmbedUtilities
    {
        public static Embed BuildEmbed(string title, string description, string footer, Color color)
            => new EmbedBuilder().WithTitle(title)
                .WithDescription(description)
                .WithFooter(f => f.Text = footer)
                .WithColor(color)
                .WithCurrentTimestamp()
                .Build();

        public static string CreateList(List<string> list)
        {
            var sb = new StringBuilder();
            list.ForEach(l =>
            {
                sb.Append($"- {l}\n");
            });
            return sb.ToString();
        }
    }
}
