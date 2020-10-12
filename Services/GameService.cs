using System;
using System.Collections.Generic;
using Discord.WebSocket;

namespace mongus_bot.Services
{
    public class GameService
    {
        private bool _isGameInProgress = false;
        private bool _isVoteInProgress = false;
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
            _isVoteInProgress = false;
            _players = players;
        }

        public List<SocketGuildUser> RestartGame()
        {
            CheckGameProgress();
            _deadPlayers = new List<SocketGuildUser>();
            _isVoteInProgress = false;
            return _players;
        }

        public List<SocketGuildUser> StopGame()
        {
            CheckGameProgress();
            var tmp = _players;
            _isGameInProgress = false;
            _isVoteInProgress = false;
            _players = new List<SocketGuildUser>();
            _deadPlayers = new List<SocketGuildUser>();
            return tmp;
        }

        public void SetAsDead(SocketGuildUser user)
        {
            CheckGameProgress();
            if (_deadPlayers.Contains(user))
                throw new ArgumentException($"{user.Username}#{user.Discriminator} is already dead! No more!!!");

            _deadPlayers.Add(user);
        }

        public void SetAsLiving(SocketGuildUser user)
        {
            CheckGameProgress();
            if (!_deadPlayers.Contains(user))
                throw new ArgumentException("Resurrecting the living? wat");

            _deadPlayers.Remove(user);
        }

        public void SetVoteStart()
        {
            CheckGameProgress();
            if (_isVoteInProgress) throw new InvalidOperationException("Voting already in progress");
            _isVoteInProgress = true;
        }

        public void SetVoteEnd()
        {
            CheckGameProgress();
            if (!_isVoteInProgress) throw new InvalidOperationException("Voting already stopped");
            _isVoteInProgress = false;
        }

        public List<SocketGuildUser> GetPlayers() => _players;

        public List<SocketGuildUser> GetDeadPlayers() => _deadPlayers;

        public List<SocketGuildUser> GetLivingPlayers() => _players.FindAll(p => !_deadPlayers.Contains(p));
    
        private void CheckGameProgress()
        {
            if (!_isGameInProgress) throw new InvalidOperationException("What the hell? There's no game!");
        }
    }
}
