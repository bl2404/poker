using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webpoker.Enums;

namespace webpoker.GameModels
{
    public class CardSuit
    {
        private List<Card> _suit;
        public CardSuit()
        {
            _suit = new List<Card>();
            foreach(Figures figure in Enum.GetValues(typeof(Figures)))
            {
                foreach(Numbers number in Enum.GetValues(typeof(Numbers)))
                {
                    _suit.Add(new Card(figure, number));
                }
            }
        }

        public Card TakeCard()
        {
            var index = new Random().Next(0, _suit.Count - 1);
            Card card= _suit[index];
            _suit.Remove(card);
            return card;
        }
    }
}
