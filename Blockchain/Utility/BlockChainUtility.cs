using Blockchain.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Blockchain
{
    public static class BlockChainUtility
    {
        public static BlockChain _chain = new BlockChain(2);
        private const string FileName = @"../SavedChain.json";

        public static IEnumerable<T> Clone<T>(this IEnumerable<T> target) where T : ICloneable
        {
            if(target == null)
                return null;

            List<T> retVal = new List<T>();

            foreach (T currentItem in target)
                retVal.Add((T)(currentItem.Clone()));

            return retVal.AsEnumerable();
        }


        public static BlockChain TempChain()
        {
            BlockChain temp = new BlockChain();
            if (System.IO.File.Exists(FileName))
            {
                using (StreamReader file = File.OpenText(FileName))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.TypeNameHandling= TypeNameHandling.Auto;
                    temp = (BlockChain)serializer.Deserialize(file, typeof(BlockChain));
                }
            }
            return temp;
        }
        public static void Use(BCApplication app)
        {
            Load();
            _chain.CreateTransaction(app);
            Save();
        }

        public static async void VerifyIntegrity()
        {
            if (!_chain.IsValidChain())
            {
                Console.Error.WriteLine("Blockchain integrity is invalid!");
            } else
            {
                Console.WriteLine("Blockchain integrity is OK");
            }
        }

        public static void Mine()
        {
            _chain.MineBlock();
            Save();
        }
        public static void Save()
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            using (StreamWriter sw = new StreamWriter(FileName))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.TypeNameHandling = TypeNameHandling.Auto;
                serializer.Serialize(writer, _chain);
            }
        }

        public static void Load()
        {
            if (System.IO.File.Exists(FileName))
            {
                using (StreamReader file = File.OpenText(FileName))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.TypeNameHandling= TypeNameHandling.Auto;
                    _chain = (BlockChain)serializer.Deserialize(file, typeof(BlockChain));
                }
            } // if no file exists, initialize and create
            else
            {

                _chain.InitializeChain();
                Save();
            }
        }

    }
}
