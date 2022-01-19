using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blockchain.Models
{
    public class LoanDecision : BCApplication
    {
        public int LoanID { get; set; }
        public Buyer Buyer { get; set; }    
        public bool? Approved { get; set; }

    }
}
