using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webpoker.Enums;

namespace webpoker.Models
{
    public class CardSuit
    {
        public CardSuit()
        {
            Suit = new List<Card>();
            foreach(Figures figure in Enum.GetValues(typeof(Figures)))
            {
                foreach(Numbers number in Enum.GetValues(typeof(Numbers)))
                {
                    Suit.Add(new Card(figure, number));
                }
            }
        }

        public List<Card> Suit { get; private set; }
    }
}
