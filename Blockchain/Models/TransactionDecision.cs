using System.ComponentModel.DataAnnotations;

namespace Blockchain.Models
{
    public class TransactionDecision : BCApplication
    {
        [Required]
        public int PendingTransactionsID { get; set; }
        public bool? Accepted { get; set; }
    }
}
