
namespace Blockchain.Models
{
    public class Seller : BCUser
    {
       public virtual List<Property> Properties { get; set; }
    }
}
