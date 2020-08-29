using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webpoker.Enums;
using webpoker.GameModels;

namespace webpoker.Models
{
    public class User
    {
        public string Name { get; set; }
        public int Wallet { get; set; }
        public string Action { get; private set; }

        public bool Active { get; private set; } = true;

        public Card FirstCard { get; private set; }

        public Card SecondCard { get; private set; }

        public HandChecker Result { get; set; }

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
            if (Action ==string.Empty)
            {
                Action = action.ToString();
            }
            else
            {
                Action += (Convert.ToInt32(Action)+action).ToString();
            }
        }

        public void ResetAction()
        {
            Action = null;
            Active = true;
            Result = null;
        }

        public void SetAction(string action)
        {
            Action = action;
        }

        public void Pass()
        {
            Action = "pass";
            Active = false;
        }

        public void GiveUserCards(Card card1, Card card2)
        {
            FirstCard = card1;
            SecondCard = card2;
        }

        public void ResetUserCards()
        {
            FirstCard = null;
            SecondCard = null;
        }

        public string GetUserInfo()
        {
            return string.Format("{0}^{1}^{2}^{3}^{4}^{5}", Name, Wallet, Action, Active,
                FirstCard?.GetCardDescription() ?? "",
                SecondCard?.GetCardDescription() ?? "");
        }
    }
}
