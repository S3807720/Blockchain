using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blockchain.Models
{
    public class PermitDecision : BCApplication
    {
        public int PermitID { get; set; }
        [Required]
        public string Address { get; set; }
        public bool? Approved { get; set; }

    }
}
