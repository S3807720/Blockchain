using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blockchain.Models
{
    public class BCApplication
    {
        [Display(Name = "Application ID")]
        public int BCApplicationID { get; set; }

    }
}
