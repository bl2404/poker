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
        Two=2,
        [Display(Name = "3")]
        Three=3,
        [Display(Name = "4")]
        Four=4,
        [Display(Name = "5")]
        Five=5,
        [Display(Name = "6")]
        Six=6,
        [Display(Name = "7")]
        Seven=7,
        [Display(Name = "8")]
        Eight=8,
        [Display(Name = "9")]
        Nine=9,
        [Display(Name = "10")]
        Ten=10,
        [Display(Name = "J")]
        J=11,
        [Display(Name = "Q")]
        Q=12,
        [Display(Name = "K")]
        K=13,
        [Display(Name = "A")]
        A=14
    }
}
