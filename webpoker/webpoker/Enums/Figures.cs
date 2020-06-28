using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace webpoker.Enums
{
    public enum Figures
    {
        [Display(Name = "♦")]
        Diamonds,
        [Display(Name = "♥")]
        Heart,
        [Display(Name = "♣")]
        Clubs,
        [Display(Name = "♠")]
        Spades
    }
}
