using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using mongus_bot.Services;
using mongus_bot.Utilities;

namespace mongus_bot.Modules
{
    public class GameModule : ModuleBase<SocketCommandContext>
    {
        public VoiceService VoiceService { get; set; }
        public GameService GameService { get; set; }

        [Command("start")]
        [Alias("g", "game")]
        public async Task StartGameAsync()
        {
            var players = VoiceService.GetConnectedUsers();
            if (players.Count == 0)
            {
                await ReplyAsync("I see no one.");
                return;
            }

            try
            {
                var playerList = DiscordUserUtilities.ToList(players);
                GameService.StartGame(playerList);
                await VoiceService.MuteUsersAsync(playerList, true);
                var desc = EmbedUtilities.CreateList(DiscordUserUtilities.GetUsernames(players));
                var embed = EmbedUtilities.BuildEmbed(
                    "Game started",
                    desc,
                    "You are now muted.",
                    Color.Teal
                );
                await ReplyAsync(embed: embed);
            } catch (InvalidOperationException e)
            {
                await ReplyAsync(e.Message);
            }
        }

        [Command("restart")]
        [Alias("g+", "game+")]
        public async Task RestartGameAsync()
        {
            try
            {
                var players = GameService.RestartGame();
                var playerList = DiscordUserUtilities.ToList(players);
                await VoiceService.MuteUsersAsync(playerList, true);
                var embed = EmbedUtilities.BuildEmbed(
                    "Game restarted",
                    EmbedUtilities.CreateList(DiscordUserUtilities.GetUsernames(players)),
                    "You are now muted",
                    Color.Teal
                );
                await ReplyAsync(embed: embed);
            } catch (InvalidOperationException e)
            {
                await ReplyAsync(e.Message);
            }
        }

        [Command("stop")]
        [Alias("end", "gg")]
        public async Task StopGameAsync()
        {
            try
            {
                var players = GameService.StopGame();
                var playerList = DiscordUserUtilities.ToList(players);
                await VoiceService.MuteUsersAsync(playerList, false);
                var embed = EmbedUtilities.BuildEmbed(
                    "Game ended",
                    EmbedUtilities.CreateList(DiscordUserUtilities.GetUsernames(players)),
                    "gg",
                    Color.Gold
                );
                await ReplyAsync(embed: embed);
            } catch (InvalidOperationException e)
            {
                await ReplyAsync(e.Message);
            }
        }

        [Command("dead")]
        [Alias("ded", "rip", "setdead")]
        public async Task SetAsDeadAsync(SocketGuildUser user)
        {
            try
            {
                GameService.SetAsDead(user);
                var embed = EmbedUtilities.BuildEmbed(
                    "Press F to pay respects",
                    $"RIP {user.Username}#{user.Discriminator}",
                    "You are now muted.",
                    Color.DarkGrey
                );
                var message = await ReplyAsync(embed: embed);
                await message.AddReactionAsync(new Emoji("\uD83C\uDDEB"));
            } catch (ArgumentException e)
            {
                await ReplyAsync(e.Message);
            } catch (InvalidOperationException e)
            {
                await ReplyAsync(e.Message);
            }
        }
    }
}
