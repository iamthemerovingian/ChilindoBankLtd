using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChilindoBankLtd.Models
{
    public class WidthrawModel
    {
        [Required]
        public int AccountNumber { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(2, MinimumLength = 2)]
        public string Currency { get; set; }
    }
}