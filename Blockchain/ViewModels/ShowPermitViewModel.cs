using Blockchain.Models;

namespace Blockchain.ViewModels
{
    public class ShowPermitViewModel
    {
        public List<PermitApplication> Permits { get; set; }

        public int PermitID { get; set; }

        public int PropertyID { get; set; } 

        public bool Approve { get; set; }
    }
}
