
namespace ChilindoBankLtd.Models
{
    public class RequestResponse
    {
        public int AccountNumber { get; set; }
        public bool Successful { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }
        public string Message { get; set; }
    }
}