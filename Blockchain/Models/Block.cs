using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace Blockchain.Models
{
    [Serializable()]
    public class Block
    {
        private DateTime _timeStamp;
        private long _nonce;
        public string PreviousHash { get; set; }
        public List<BCApplication> Transactions { get; set; }

        public string Hash { get; private set; }
        public Block(List<BCApplication> transactions, string previousHash = "")
        {
            _timeStamp = DateTime.UtcNow;
            _nonce = 0;
            Transactions = transactions;
            PreviousHash = previousHash;
            Hash = CreateHash();
        }
        public Block(BCApplication transaction, string previousHash = "")
        {
            _timeStamp = DateTime.UtcNow;
            _nonce = 0;
            Transactions = new List<BCApplication> { transaction };
            PreviousHash = previousHash;
            Hash = CreateHash();
        }
        public Block() { }
        public void MineBlock(int difficulty)
        {
            string hashValidationTemplate = new String('0', difficulty);
            //increase difficulty of processing based on set value
            //will brute force until the hash starts with X amount of 0s
            //before confirming the block
            while (Hash.Substring(0, difficulty) != hashValidationTemplate)
            {
                _nonce++;
                Hash = CreateHash();
            }
        }
        public string CreateHash()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                string rawData = PreviousHash + _timeStamp + Transactions + _nonce;
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                return Encoding.Default.GetString(bytes);
            }
        }

        [field: NonSerialized()]
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

    }
}
