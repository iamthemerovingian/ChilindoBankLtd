using System.ComponentModel.DataAnnotations;


namespace ChilindoBankLtd.Models
{
    public class BankAccountModel
    {
        [Required]
        public int AccountNumber { get; set; }
        [Required]
        public decimal Balance { get; set; }
        [Required]
        [StringLength(2, MinimumLength = 2)]
        public string Currency { get; set; }
        [Required]
        public bool IsLocked { get; set; }
    }
}