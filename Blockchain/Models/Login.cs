using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blockchain.Models
{
    public class Login
    {
        [Column(TypeName = "nchar")]
        [StringLength(8)]
        public int LoginID { get; set; }
        public int UserID { get; set; }
        public virtual BCUser User { get; set; }

        [Column(TypeName = "nchar")]
        [Required, StringLength(64)]
        public string PasswordHash { get; set; }

    }
}
