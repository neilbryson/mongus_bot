using System;
using System.Collections.Generic;
using Discord.WebSocket;

namespace mongus_bot.Services
{
    public class GameService
    {
        private bool _isGameInProgress = false;
        private List<SocketGuildUser> _players;
        private List<SocketGuildUser> _deadPlayers;

        public GameService()
        {
            _players = new List<SocketGuildUser>();
            _deadPlayers = new List<SocketGuildUser>();
        }

        public void StartGame(List<SocketGuildUser> players)
        {
            if (_isGameInProgress) throw new InvalidOperationException("Game already in progress");
            _isGameInProgress = true;
            _players = players;
        }

        public List<SocketGuildUser> RestartGame()
        {
            if (!_isGameInProgress) throw new InvalidOperationException("Nothing to restart");
            _deadPlayers = new List<SocketGuildUser>();
            return _players;
        }

        public List<SocketGuildUser> StopGame()
        {
            if (!_isGameInProgress) throw new InvalidOperationException("???");
            var tmp = _players;
            _isGameInProgress = false;
            _players = new List<SocketGuildUser>();
            _deadPlayers = new List<SocketGuildUser>();
            return tmp;
        }

        public void SetAsDead(SocketGuildUser user)
        {
            if (!_isGameInProgress) throw new InvalidOperationException("What the hell? There's no game!");
            if (_deadPlayers.Contains(user))
                throw new ArgumentException($"{user.Username}#{user.Discriminator} is already dead! No more!!!");

            _deadPlayers.Add(user);
        }

        public List<SocketGuildUser> GetPlayers() => _players;
        public List<SocketGuildUser> GetDeadPlayers() => _deadPlayers;
    }
}
