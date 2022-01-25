using System.ComponentModel.DataAnnotations;

namespace Blockchain.Models
{
    public class PendingTransaction : BCApplication
    {
        [Required]
        public int PropertyID { get; set; }
        [Required]
        public int BuyerID { get; set; }
        public bool? Accepted { get; set; }
    }
}
