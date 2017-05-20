using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChilindoBankLtd.Data.Entities
{
    public class BankAccount
    {
        public int Id { get; set; }
        [Required]
        public int AccountNumber { get; set; }
        [Required]
        public decimal Balance { get; set; }
        [Required]
        [StringLength(2, MinimumLength = 2)]
        public string Currency { get; set; }
        [Required]
        public bool IsLocked { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
