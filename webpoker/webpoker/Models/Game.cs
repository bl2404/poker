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

        public int MinBid { get; private set; }
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
            MinBid = 0;
            MaxBid = _activeUsers.Min(x => x.Wallet);
            if (CurrentUser == null)
            {
                StartGame();
            }
            else
            {
                var value = Convert.ToInt32(message);
                if (value > MinBid)
                {
                    _actionmax = value;
                    MinBid = value;
                }
                CurrentUser.CalculateTotalAction(value);
                CurrentUser = _activeUsers.FirstOrDefault(x => x.Action == null);
                if (CurrentUser == null)
                {
                    CurrentUser = _activeUsers.FirstOrDefault(x => x.Action <_actionmax);
                    if (CurrentUser == null)
                    {
                        FinishAuction();
                        Debug.WriteLine("");
                    }
                    else
                    {
                        MinBid = _actionmax - (int)CurrentUser.Action;
                        MaxBid = MinBid;
                    }
                }
            }

            //else
            //{
            //    int value = Convert.ToInt32(message);
            //    CurrentUser.CalculateTotalAction(value);
            //    CurrentUser.RemoveFromWallet(value);
            //    _pool += value;

            //    if (!_askPreviousUser)
            //    {
            //        PrimaryLoop(value);
            //    }
            //    else
            //    {
            //        SecondaryLoop();
            //    }
            //}
        }

        private void PrimaryLoop(int value)
        {
            if (value > MinBid)
            {
                MinBid = value;
                _actionmax = value;
            }
            if (CurrentUser == _activeUsers.Last())
            {
                if (_activeUsers.Any(x => x.Action < _actionmax))
                {
                    //SecondaryLoop();
                }
                else
                {
                    FinishAuction();
                }
            }
            else
            {
                CurrentUser = _activeUsers[_activeUsers.IndexOf(CurrentUser) + 1];
            }
        }

        //private void SecondaryLoop()
        //{
        //    CurrentUser = _activeUsers.FirstOrDefault(x => x.Action < _actionmax);
        //    if (CurrentUser == null)
        //    {
        //        FinishAuction();
        //    }
        //    else
        //    {
        //        MinBid = _actionmax - CurrentUser.Action;
        //        MaxBid = MinBid;
        //        _askPreviousUser = true;
        //    }
        //}

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
        }



    }
}
