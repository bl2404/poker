using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webpoker.Enums;

namespace webpoker.Models
{
    public class Card
    {
        public Card(Figures figure, Numbers number)
        {
            Figure = figure;
            Number = number;
        }

        public Figures Figure { get; private set; }
        public Numbers Number { get; private set; }

    }
}
