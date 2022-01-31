namespace Blockchain.Models
{
    public class OfferViewModel
    {
        public String Buyers { get; set; }
        public LoanDecision Loan { get; set; }
        public String Address { get; set; }
        public int PendingTransactionID { get; set; }
        public string Status { get; set; } = "Unconfirmed";

    }
}
