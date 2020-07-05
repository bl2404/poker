using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace webpoker.Models
{
    public class Game
    {
        public User CurrentUser { get; private set; }
        public int MinBid { get; private set; } = 0;
        public int MaxBid { get; private set; } = 0;
        public int Pool { get; private set; } = 0;

        public string MessageToSend { get; private set; }

        private List<User> _activeUsers;
        private int _actionmax = 0;

        public Game()
        {
            _activeUsers = Application.Instance.Tables[0].Users;
            StartGame();
            MaxBid = _activeUsers.Min(x => x.Wallet);
        }

        public void NextStep(string message)
        {
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

            foreach(var user in Application.Instance.Tables[0].Users)
            {
                Debug.WriteLine("user: " + user.Name + " " + user.Wallet);
            }
            Debug.WriteLine("poola: " + Pool);
        }



    }
}
