using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using webpoker.Enums;

namespace webpoker.Models
{
    public class Game
    {
        public User CurrentUser { get; private set; }
        public int MinBid { get; private set; } = 0;
        public int MaxBid { get; private set; } = 0;
        public int Pool { get; private set; } = 0;
        public string MessageToSend { get; private set; }


        private Bets? _bet;
        private CardSuit _cardSuid;
        private bool _newBet = true;
        private List<User> _activeUsers;
        private int _actionmax = 0;
        private Card _flop1;
        private Card _flop2;
        private Card _flop3;
        private Card _turn;
        private Card _river;

        public Game()
        {
            _activeUsers = Application.Instance.Tables[0].Users;
            _bet = Bets.Preflop;
            StartGame();
            MaxBid = _activeUsers.Min(x => x.Wallet);
            _cardSuid = new CardSuit();
        }

        public void NextStep(string message)
        {
            if (_newBet == true)
            {
                ShowCard();
            }
            _activeUsers = Application.Instance.Tables[0].Users.Where(x=>x.Active).ToList();
            MaxBid = _activeUsers.Min(x => x.Wallet);
            if (message == "pass")
            {
                Pass();
            }
            else
            {
                var value = Convert.ToInt32(message);
                CalculateGameParameters(value);
            }
            FindNextUser();
            FindNextBet();
            GetGameInfo();
        }

        public string GetGameInfo()
        {
            string usersInfo= Application.Instance.Tables[0].Users[0].GetUserInfo();
            string gameinfo = string.Format("{0}^{1}^{2}^{3}^{4}^{5}^{6}^{7}^{8}^{9}", CurrentUser.Name, MinBid, MaxBid,
                Pool, MessageToSend,
                _flop1?.GetCardDescription() ?? "",
                _flop2?.GetCardDescription() ?? "",
                _flop3?.GetCardDescription() ?? "",
                _turn?.GetCardDescription() ?? "",
                _river?.GetCardDescription() ?? "");
            return string.Format("{0}:{1}", usersInfo, gameinfo);
        }

        private void StartGame()
        {
            //CurrentUser = RandomUser();
            CurrentUser = _activeUsers.First();
        }

        private void Pass()
        {
            CurrentUser.Pass();
            _activeUsers = Application.Instance.Tables[0].Users.Where(x => x.Active).ToList();
            MessageToSend = "Pass";
        }

        private void CalculateGameParameters(int value)
        {
            if (value > MinBid)
            {
                _actionmax = value;
                MinBid = value;
            }
            CurrentUser.RemoveFromWallet(value);
            CurrentUser.CalculateTotalAction(value);
            Pool += value;
            MessageToSend = CurrentUser.Action.ToString();
        }

        private void FindNextUser()
        {
            CurrentUser = _activeUsers.FirstOrDefault(x => x.Action == null);
            if (CurrentUser == null)
            {
                CurrentUser = _activeUsers.FirstOrDefault(x => x.Action < _actionmax);
                if (CurrentUser == null)
                {
                    FinishAuction();
                }
                else
                {
                    MinBid = _actionmax - (int)CurrentUser.Action;
                    MaxBid = MinBid;
                }
            }
        }

        private User RandomUser()
        {
            var random = new Random();
            int index = random.Next(0, _activeUsers.Count - 1);
            return _activeUsers[index];
        }

        private void FinishAuction()
        {
            foreach (var user in _activeUsers)
            {
                user.ResetAction();
            }
            MinBid = 0;
            _actionmax = 0;
            MaxBid = _activeUsers.Min(x => x.Wallet);
            CurrentUser = _activeUsers.First();
            _newBet = true;

            foreach(var user in Application.Instance.Tables[0].Users)
            {
                Debug.WriteLine("user: " + user.Name + " " + user.Wallet);
            }
            Debug.WriteLine("poola: " + Pool);
        }

        private void FindNextBet()
        {
            var maxval=(int)Enum.GetValues(typeof(Bets)).Cast<Bets>().Last();
            int index = (int)_bet+1;
            if (index <= maxval)
                _bet = (Bets)index;
            else
                _bet = null;
        }

        private void ShowCard()
        {
            switch (_bet)
            {
                case Bets.Preflop:
                    foreach (var user in Application.Instance.Tables[0].Users.Where(x => x.Active))
                        user.GiveUserCards(_cardSuid.TakeCard(), _cardSuid.TakeCard());
                    break;
                case Bets.Flop:
                    _flop1 = _cardSuid.TakeCard();
                    _flop2 = _cardSuid.TakeCard();
                    _flop3 = _cardSuid.TakeCard();
                    break;
                case Bets.Turn:
                    _turn = _cardSuid.TakeCard();
                    break;
                case Bets.River:
                    _river = _cardSuid.TakeCard();
                    break;
            }
            _newBet = false;
        }
    }
}
