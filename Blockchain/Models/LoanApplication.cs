using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blockchain.Models
{
    public class LoanApplication : BCApplication
    {
        public Buyer Buyer { get; set; } 
        public string Address { get; set; }
        [Required, DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal LoanAmount { get; set; }
        public bool? Decision { get; set; }

    }
    
}
