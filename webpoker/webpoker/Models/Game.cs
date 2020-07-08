using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
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

        private Bets _bet;
        private CardSuit _cardSuit;

        private int _actionmax = 0;
        private bool newBet;
        private bool finish = false;

        private Card _flop1;
        private Card _flop2;
        private Card _flop3;
        private Card _turn;
        private Card _river;

        private int _enterFee = 1;

        public Game()
        {
            CurrentUser=GetActiveUsers().First();
            MinBid = _enterFee;
            MaxBid = MinBid;
            _cardSuit = new CardSuit();
            _bet = Bets.Entrance;
            ResetUserActions();
            foreach (var usr in Application.Instance.Tables[0].Users)
                usr.ResetUserCards();
        }

        public void NextStep(string message)
        {
            if (newBet)
            {
                ResetUserActions();
            }
            newBet = false;
            MaxBid = GetActiveUsers().Min(x => x.Wallet);


            CalculateGameParameters(message);
            FindNextUser();
        }

        public string GetGameInfo()
        {
            var users = Application.Instance.AllUsers.Select(x => x.GetUserInfo()).ToArray();
            string usersInfo = string.Join(";", users);
            string gameinfo = string.Format("{0}^{1}^{2}^{3}^{4}^{5}^{6}^{7}^{8}^{9}", CurrentUser.Name, MinBid, MaxBid,
                Pool,
                _flop1?.GetCardDescription() ?? "",
                _flop2?.GetCardDescription() ?? "",
                _flop3?.GetCardDescription() ?? "",
                _turn?.GetCardDescription() ?? "",
                _river?.GetCardDescription() ?? "",
                finish);
            return string.Format("{0}:{1}", usersInfo, gameinfo);
        }

        private User[] GetActiveUsers()
        {
            return Application.Instance.Tables[0].Users.Where(x => x.Active).ToArray();
        }

        private void Pass()
        {
            CurrentUser.Pass();
        }

        private void GameOver()
        {
            finish = true;
            foreach (var user in GetActiveUsers())
            {
                user.SetAction(user.FirstCard.GetCardDescription() + " " + user.SecondCard.GetCardDescription());
            }
                
            //Application.Instance.Tables[0].Game = null;
            //MessageToSend = "";
        }

        private void CalculateGameParameters(string message)
        {
            if(Int32.TryParse(message,out int value))
            {
                if (value >= MinBid)
                {
                    _actionmax = value;
                    MinBid = value;
                }
                CurrentUser.RemoveFromWallet(value);
                CurrentUser.CalculateTotalAction(value);
                Pool += value;
            }
            else
            {
                Pass();
            }
        }

        private void FindNextUser() //zwracac usera
        {
            CurrentUser = GetActiveUsers().FirstOrDefault(x => x.Action == null);
            if (CurrentUser == null)
            {
                CurrentUser = GetActiveUsers().FirstOrDefault(x => Convert.ToInt32(x.Action) < _actionmax);
                if (CurrentUser == null)
                {
                    StartNewAuction();
                }
                else
                {
                    MinBid = _actionmax - Convert.ToInt32(CurrentUser.Action);
                    MaxBid = MinBid;
                }
            }
        }

        private void ResetUserActions()
        {
            foreach (var user in GetActiveUsers())
            {
                user.ResetAction();
            }
        }

        private void StartNewAuction()
        {
            newBet = true;
            CurrentUser = GetActiveUsers().First();
            MinBid = 0;
            _actionmax = 0;
            MaxBid = GetActiveUsers().Min(x => x.Wallet);
            if (_bet == Bets.River)
                GameOver();
            else
            {
                FindNextBet();
                ShowCard();
            }
        }

        private void FindNextBet()
        {
            var maxval=(int)Enum.GetValues(typeof(Bets)).Cast<Bets>().Last();
            int index = (int)_bet+1;
            if (index <= maxval)
                _bet = (Bets)index;
        }

        private void ShowCard()
        {
            switch (_bet)
            {
                case Bets.Preflop:
                    foreach (var user in Application.Instance.Tables[0].Users.Where(x => x.Active))
                        user.GiveUserCards(_cardSuit.TakeCard(), _cardSuit.TakeCard());
                    break;
                case Bets.Flop:
                    _flop1 = _cardSuit.TakeCard();
                    _flop2 = _cardSuit.TakeCard();
                    _flop3 = _cardSuit.TakeCard();
                    break;
                case Bets.Turn:
                    _turn = _cardSuit.TakeCard();
                    break;
                case Bets.River:
                    _river = _cardSuit.TakeCard();
                    break;
            }
        }
    }
}
