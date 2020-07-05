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

        public bool Active { get; private set; } = true;

        public Card FirstCard { get; private set; }

        public Card SecondCard { get; private set; }

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
            Active = true;
        }

        public void Pass()
        {
            Action = -1;
            Active = false;
        }

        public void GiveUserCards(Card card1, Card card2)
        {
            FirstCard = card1;
            SecondCard = card2;
        }

        public string GetUserInfo()
        {
            return string.Format("{0}^{1}^{2}^{3}^{4}^{5}", Name, Wallet, Action, Active,
                FirstCard?.GetCardDescription() ?? "",
                SecondCard?.GetCardDescription() ?? "");
        }
    }
}
