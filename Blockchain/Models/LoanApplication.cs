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
        [Display(Name = "Loan Amount")]
        public decimal LoanAmount { get; set; }
        [Display(Name = "Action")]
        public bool? Decision { get; set; }
        [Display(Name = "Permit Status")]
        public bool? PermitStatus { get; set; }

    }
    
}
