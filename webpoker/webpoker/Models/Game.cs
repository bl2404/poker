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

        public Card _flop1 { get; private set; }
        public Card _flop2 { get; private set; }
        public Card _flop3 { get; private set; }
        public Card _turn { get; private set; }
        public Card _river { get; private set; }

        private int _enterFee = 1;

        public Game()
        {
            CurrentUser = GetActiveUsers().First();
            MinBid = _enterFee;
            MaxBid = MinBid;
            _cardSuit = new CardSuit();
            _bet = Bets.Entrance;
            ResetUserActions(GetAllUsers());
            foreach (var usr in Application.Instance.Tables[0].Users)
                usr.ResetUserCards();
        }

        public void NextStep(string message)
        {
            if (newBet)
            {
                ResetUserActions(GetActiveUsers());
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

        private User[] GetAllUsers()
        {
            return Application.Instance.Tables[0].Users.ToArray();
        }

        private void Pass()
        {
            CurrentUser.Pass();
        }

        private void GameOver()
        {
            finish = true;
            bool finishErlierByPass = false;
            if (_flop1 == null || _flop2 == null || _flop3 == null || _turn == null || _river == null)
            {
                finishErlierByPass = true;
            }
            foreach (var user in GetActiveUsers())
            {
                if(finishErlierByPass==false)
                {
                    user.Result = new HandChecker(_flop1, _flop2, _flop3, _turn, _river, user.FirstCard, user.SecondCard);
                }
                string card1 = user.FirstCard?.GetCardDescription() ?? "";
                string card2 = user.SecondCard?.GetCardDescription() ?? "";
                user.SetAction(string.Format("{0} {1} - {2}", card1, card2, user.Result?.Hand.ToString() ?? ""));
            }

            User[] winners;
            if (finishErlierByPass)
                winners = new User[] { GetActiveUsers().First() };
            else
                winners = FindWinners();

            Math.DivRem(Pool, winners.Count(), out int rest);
            foreach (var user in winners)
            {
                user.Wallet += (Pool - rest) / winners.Count();
            }
            Pool = rest;

            //Application.Instance.Tables[0].Game = null;
            //MessageToSend = "";
        }

        private User[] FindWinners()
        {
            var winCombination = GetActiveUsers().OrderByDescending(x => x.Result.Hand).First().Result.Hand;
            var highCard = GetActiveUsers().Where(x => x.Result.Hand == winCombination).
                OrderByDescending(y => y.Result.HighCard).First().Result.HighCard;
            return GetActiveUsers().Where(x => x.Result.Hand == winCombination).
                Where(y => y.Result.HighCard == highCard).ToArray();
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

        private void ResetUserActions(User[] users)
        {
            foreach (var user in users)
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
            if (_bet == Bets.River || GetActiveUsers().Count()==1)
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
                    foreach (var user in GetActiveUsers())
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
