using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using webpoker.Enums;

namespace webpoker.Models
{
    public class HandChecker
    {
        private Card[] cards;
        private PairsCounter[] groupedInPairs;
        private ColorCounter[] groupedInColors;

        public Card HighCard { get; private set; }
        public Hand Hand { get; private set; }

        public HandChecker(Card flop1, Card flop2, Card flop3, Card turn, Card river, Card hand1, Card hand2)
        {
            cards = new Card[] { flop1, flop2, flop3, turn, river, hand1, hand2 };
            GroupInPairs();
            GroupInColors();
            CheckForHighCard();
            CheckForHandConfiguration();
        }

        private void CheckForHandConfiguration()
        {
            Hand = Hand.HighCard;
            if (CheckForOnePair())
                Hand = Hand.OnePair;
            if (CheckForTwoPairs())
                Hand = Hand.TwoPair;
            if (CheckForThree())
                Hand = Hand.Three;
            if (CheckForStraight())
                Hand = Hand.Straight;
            if (CheckForColor())
                Hand = Hand.Flush;
            if (CheckForFull())
                Hand = Hand.Full;
            if (CheckForFour())
                Hand = Hand.Four;
            if (CheckForPoker())
                Hand = Hand.Poker;
        }

        private void CheckForHighCard()
        {
            HighCard = cards.OrderByDescending(x => x.Number).First();
        }

        private void GroupInPairs()
        {
            groupedInPairs = cards.GroupBy(x => x.Number).Select(y => new PairsCounter
            {
                Number = y.Key,
                Count = y.Count(),
            }).OrderByDescending(z => z.Count).ToArray();
        }

        private void GroupInColors()
        {
            groupedInColors = cards.GroupBy(x => x.Figure).Select(y => new ColorCounter
            {
                Figure = y.Key,
                Count = y.Count(),
            }).OrderByDescending(z => z.Count).ToArray();
        }

        private bool CheckForStraight()
        {
            Card[] orderedCards = cards.OrderByDescending(x => x.Number).ToArray();
            Card highCard = null;
            int sum = 0;


            for (int i = 1; i < orderedCards.Count(); i++)
            {
                if ((int)orderedCards[i].Number - (int)orderedCards[i - 1].Number == -1)
                {
                    if(sum==0)
                        highCard=orderedCards[i - 1];
                    sum++;
                    if (sum == 4)
                    {
                        HighCard = highCard;
                        return true;
                    }
                }
                else
                    sum = 0;
            }
            return false;
        }

        public bool CheckForOnePair()
        {
            if (groupedInPairs.First().Count == 2)
            {
                HighCard = cards.First(x => x.Number == groupedInPairs.First().Number);
                return true;
            }
            else
                return false;
        }

        public bool CheckForTwoPairs()
        {
            if (groupedInPairs[0].Count == 2 && groupedInPairs[1].Count == 2)
            {
                HighCard = cards.First(x => x.Number == groupedInPairs.Where(z => z.Count == 2).OrderByDescending(y => y.Number).First().Number);
                return true;
            }
            else
                return false;
        }

        public bool CheckForThree()
        {
            if (groupedInPairs.First().Count == 3)
            {
                HighCard = cards.First(x => x.Number == groupedInPairs.First().Number);
                return true;
            }
            else
                return false;
        }

        public bool CheckForFull()
        {
            if (groupedInPairs[0].Count == 3 && groupedInPairs[1].Count == 2)
            {
                HighCard = cards.First(x => x.Number == groupedInPairs.First().Number);
                return true;
            }
            else
                return false; 
        }

        public bool CheckForFour()
        {
            if (groupedInPairs.First().Count == 4)
            {
                HighCard = cards.First(x => x.Number == groupedInPairs.First().Number);
                return true;
            }
            else
                return false;
        }

        public bool CheckForColor()
        {
            if (groupedInColors.First().Count == 5)
            {
                HighCard = cards.Where(x => x.Figure == groupedInColors.First().Figure).OrderByDescending(y=>y.Number).First();
                return true;
            }
            else
                return false;
        }

        public bool CheckForPoker()
        {
            if (CheckForStraight() == true && CheckForColor() == true)
            {
                CheckForStraight();
                return true;
            }
            else
                return false;
        }

        private class PairsCounter
        {
            public Numbers Number { get; set; }

            public int Count { get; set; }
        }

        private class ColorCounter
        {
            public Figures Figure { get; set; }

            public int Count { get; set; }
        }

    }
}
