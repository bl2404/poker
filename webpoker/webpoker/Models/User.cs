using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webpoker.Models
{
    public class User
    {
        public string Name { get; set; }
        public int Wallet { get; set; }
        public int? Action { get; private set; }

        public void RemoveFromWallet(int value)
        {
            Wallet = Wallet - value;
        }

        public void AddToWallet(int value)
        {
            Wallet = Wallet + value;
        }

        public void CalculateTotalAction(int action)
        {
            if (Action ==null)
            {
                Action = action;
            }
            else
            {
                Action += action;
            }
        }

        public void ResetAction()
        {
            Action = null;
        }
    }
}
