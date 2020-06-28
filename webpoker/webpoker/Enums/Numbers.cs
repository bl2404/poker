using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace webpoker.Enums
{
    public enum Numbers
    {
        [Display(Name = "2")]
        Two,
        [Display(Name = "3")]
        Three,
        [Display(Name = "4")]
        Four,
        [Display(Name = "5")]
        Five,
        [Display(Name = "6")]
        Six,
        [Display(Name = "7")]
        Seven,
        [Display(Name = "8")]
        Eight,
        [Display(Name = "9")]
        Nine,
        [Display(Name = "10")]
        Ten,
        J,
        Q,
        K,
        A
    }
}
