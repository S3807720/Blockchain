namespace Blockchain.Models
{
    public class PendingTransactions
    {
        public int PendingTransactionsID { get; set; }
        public List<BCApplication> BCApplications { get; set; }

        public PendingTransactions()
        {
            BCApplications = new List<BCApplication>();
        }
        public void Add(BCApplication ap)
        {
            BCApplications.Add(ap);
        }
    }
}
