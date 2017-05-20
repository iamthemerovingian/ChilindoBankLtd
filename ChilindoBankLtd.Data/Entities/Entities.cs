using System.ComponentModel.DataAnnotations;

namespace ChilindoBankLtd.Data.Entities
{
    public class BankAccount
    {
        public int Id { get; set; }
        [Required]
        public int AccountNumber { get; set; }
        [Required]
        [Range(0, double.PositiveInfinity)]
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
