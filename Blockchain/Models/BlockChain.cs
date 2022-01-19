
using System.ComponentModel.DataAnnotations;

namespace Blockchain.Models
{
    [Serializable()]
    public class BlockChain
    {
        [Required]
        private readonly int proofOfWorkDifficulty;
        public List<BCApplication> pendingTransactions { get; set; }
        [Required]
        public List<Block> Chain { get; set; }
        public BlockChain(int difficulty)
        {
            proofOfWorkDifficulty = difficulty;
            pendingTransactions = new List<BCApplication>();
            //Chain = new List<Block> { CreateGenesisBlock() };
        }
        public BlockChain()
        {
            proofOfWorkDifficulty = 2;
            pendingTransactions = new List<BCApplication>();
           // Chain = new List<Block> { CreateGenesisBlock() };
        }

        public void InitializeChain()
        {
            Chain = new List<Block> { CreateGenesisBlock() };
        }
        public void CreateTransaction(BCApplication transaction)
        {
            pendingTransactions.Add(transaction);
           // MineBlock(transaction);
        }

        public void MineBlock(BCApplication trans)
        {
            Block block = new(trans);
            block.MineBlock(proofOfWorkDifficulty);
            block.PreviousHash = Chain.Last().Hash;
            Chain.Add(block);
            pendingTransactions = new List<BCApplication>();
        }

        public void MineBlock()
        {
            Block block = new Block(pendingTransactions);
            block.MineBlock(proofOfWorkDifficulty);
            block.PreviousHash = Chain.Last().Hash;
            Chain.Add(block);
            pendingTransactions = new List<BCApplication>();
        }

        public bool IsValidChain()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                Block previousBlock = Chain[i - 1];
                Block currentBlock = Chain[i];
                if (currentBlock.Hash != currentBlock.CreateHash())
                    return false;
                if (currentBlock.PreviousHash != previousBlock.Hash)
                    return false;
            }
            return true;
        }

        private Block CreateGenesisBlock()
        {
            return new Block(new BCApplication { BCApplicationID = 0});
        }

        [field: NonSerialized()]
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

    }
}
