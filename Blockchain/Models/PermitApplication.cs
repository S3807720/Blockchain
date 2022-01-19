using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blockchain.Models
{
    public class PermitApplication : BCApplication
    {
        [Required]
        public Property Property { get; set; }
        public bool? Decision { get; set; }
    }
}
