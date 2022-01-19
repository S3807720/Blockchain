using Blockchain.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace Blockchain
{
    public static class BlockChainUtility
    {
        public static BlockChain _chain = new BlockChain(2);
        private const string FileName = @"../SavedChain.json";

        public static void Use(BCApplication app)
        {
            Load();
            _chain.CreateTransaction(app);
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
            }
            else
            {

                _chain.InitializeChain();
                Save();
            }
        }

    }
}
