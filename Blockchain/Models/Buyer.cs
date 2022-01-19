
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blockchain.Models
{
    public class Buyer : BCUser
    {
        public string DOB { get; set; }
        [Display(Name = "Home")]
        public string Address { get; set; }
        [Display(Name ="Phone")]
        public string ContactNumber { get; set; }
        public string Employer { get; set; }
        [Required, DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        [Display(Name = "Income")]
        public decimal AnnualIncome { get; set; }


    }
}
