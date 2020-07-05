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
        public bool Active { get; private set; }

        private List<User> _activeUsers;

        private int _pool=0;
        private bool _askPreviousUser = false;
        private int _actionmax = 0;


        public Game()
        {
            _activeUsers = Application.Instance.Tables[0].Users;
        }

        public void NextStep(string message)
        {
            _activeUsers = Application.Instance.Tables[0].Users.Where(x=>x.Active).ToList();
            MaxBid = _activeUsers.Min(x => x.Wallet);
            if (CurrentUser == null)
            {
                StartGame();
            }
            else
            {
                if (message == "pass")
                {
                    CurrentUser.Pass();
                    _activeUsers = Application.Instance.Tables[0].Users.Where(x => x.Active).ToList();
                }
                else
                {
                    var value = Convert.ToInt32(message);
                    if (value > MinBid)
                    {
                        _actionmax = value;
                        MinBid = value;
                    }
                    CurrentUser.RemoveFromWallet(value);
                    CurrentUser.CalculateTotalAction(value);
                    _pool += value;
                }
                CurrentUser = _activeUsers.FirstOrDefault(x => x.Action == null);
                if (CurrentUser == null)
                {
                    CurrentUser = _activeUsers.FirstOrDefault(x => x.Action <_actionmax);
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
        }

        private void StartGame()
        {
            //CurrentUser = RandomUser();
            CurrentUser = _activeUsers.First();
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
            _askPreviousUser = false;
            CurrentUser = _activeUsers.First();

            foreach(var user in Application.Instance.Tables[0].Users)
            {
                Debug.WriteLine("user: " + user.Name + " " + user.Wallet);
            }
            Debug.WriteLine("poola: " + _pool);
        }



    }
}
