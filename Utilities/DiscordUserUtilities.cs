using System.Collections.Generic;
using Discord.WebSocket;

namespace mongus_bot.Utilities
{
    class DiscordUserUtilities
    {
        public static List<string> GetUsernames(IReadOnlyCollection<SocketGuildUser> users)
        {
            var list = new List<string>();
            foreach (var user in users) list.Add($"{user.Username}#{user.Discriminator}");
            return list;
        }

        public static List<ulong> GetIds(IReadOnlyCollection<SocketGuildUser> users)
        {
            var list = new List<ulong>();
            foreach (var user in users) list.Add(user.Id);
            return list;
        }

        public static List<SocketGuildUser> ToList(IReadOnlyCollection<SocketGuildUser> users)
        {
            var list = new List<SocketGuildUser>();
            foreach (var user in users) list.Add(user);
            return list;
        }
    }
}
