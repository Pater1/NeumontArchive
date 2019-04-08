using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using Newtonsoft.Json;
using System.Threading;

namespace QuearyMongo
{
    class Program
    {
        private static IMongoDatabase db;
        static void Main(string[] args)
        {
            MongoClientSettings settings = new MongoClientSettings();
            settings.Server = new MongoServerAddress("127.0.0.1", 27017); //!!!Change values here to connect to your MongoDB
            MongoClient client = new MongoClient(settings);

            db = client.GetDatabase("Customers");
            IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("customers");

            Task<string> t100 = TopCustomersAlphabeticallyByLastName(collection, 100, false);
            t100.Wait();
            Console.WriteLine(t100.Result);
            Console.WriteLine("");

            Task<string> getCust = GetCustByID(collection, "1010008");
            getCust.Wait();
            Console.WriteLine(getCust.Result);
            Console.WriteLine("");

            Task<string> t25Prod = TopPopularProductsByQuantity(collection, 25, false);
            t25Prod.Wait();
            Console.WriteLine(t25Prod.Result);
            Console.WriteLine("");

            Task<string> t25Spend = TopBiggestSpenders(collection, 25, false);
            t25Spend.Wait();
            Console.WriteLine(t25Spend.Result);
        }
        
        public static async Task<string> TopBiggestSpenders(IMongoCollection<BsonDocument> collection, int retCount = 25, bool forceRecalc = false)
        {
            bool needRecalc = true;
            List<string> l2 = null;
            IMongoCollection<BsonDocument> cache = db.GetCollection<BsonDocument>("cache");

            var cha = cache.Find(Builders<BsonDocument>.Filter.Empty).ToEnumerable();
            foreach (BsonDocument bs in cha)
            {
                if (bs["metadata"].AsBsonDocument["tag"].AsString == "TopBiggestSpenders")
                {
                    if (!forceRecalc)
                    {
                        l2 = bs["values"].AsBsonArray.Select(p => p.AsString).ToList();
                        needRecalc = false;
                    }
                    else
                    {
                        cache.DeleteOne(bs);
                    }
                    break;
                }
            }

            if (needRecalc)
            {
                Dictionary<string, decimal> idToOrderCount = new Dictionary<string, decimal>();
                var f = collection.Find(Builders<BsonDocument>.Filter.Empty);
                var e = f.ToEnumerable();
                foreach (BsonDocument bs in e)
                {
                    var jsonWriterSettings = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict }; // key part
                    string json = "{customer:" + bs["customer"].ToJson(jsonWriterSettings) + "}";
                    //Console.WriteLine(json);

                    XmlDocument doc = (XmlDocument)Newtonsoft.Json.JsonConvert.DeserializeXmlNode(json);
                    XmlNodeList nl = doc.GetElementsByTagName("order");

                    foreach (XmlNode n in nl)
                    {
                        string key = bs["customer"].AsBsonDocument["name"].ToString();
                        bool hasKey = idToOrderCount.ContainsKey(key);
                        decimal value = hasKey ? idToOrderCount[key] : 0;
                        value += decimal.Parse(n["orderTotal"].InnerText);

                        if (hasKey)
                        {
                            idToOrderCount[key] = value;
                        }
                        else
                        {
                            idToOrderCount.Add(key, value);
                        }
                    }
                }

                List<KeyValuePair<string, decimal>> l1 = idToOrderCount.ToList();
                l1.Sort((x, y) => y.Value.CompareTo(x.Value));
                l2 = l1.ConvertAll(x => x.Key + ": $" + x.Value);

                string cacheImport = "{metadata: {tag: \"TopBiggestSpenders\"}, values: " + l2.ToJson() + "}";
                cache.InsertOne(BsonDocument.Parse(cacheImport));
            }
            if (l2.Count > retCount) l2.RemoveRange(retCount, l2.Count - retCount);

            string ret = "<Top25BiggestSpenders>";
            foreach (string s in l2)
            {
                ret += "\n    " + s;
            }
            ret += "\n</Top25BiggestSpenders>";

            Thread.Sleep(10); //make sure the queary doesn;t finish before the callback is setup
            return ret;
        }

        public static async Task<string> TopPopularProductsByQuantity(IMongoCollection<BsonDocument> collection, int retCount = 25, bool forceRecalc = false)
        {
            bool needRecalc = true;
            List<string> l2 = null;
            IMongoCollection<BsonDocument> cache = db.GetCollection<BsonDocument>("cache");

            var cha = cache.Find(Builders<BsonDocument>.Filter.Empty).ToEnumerable();
            foreach (BsonDocument bs in cha)
            {
                if (bs["metadata"].AsBsonDocument["tag"].AsString == "TopPopularProductsByQuantity")
                {
                    if (!forceRecalc)
                    {
                        l2 = bs["values"].AsBsonArray.Select(p => p.AsString).ToList();
                        needRecalc = false;
                    }
                    else
                    {
                        cache.DeleteOne(bs);
                    }
                    break;
                }
            }

            if (needRecalc)
            {
                Dictionary<string, long> idToOrderCount = new Dictionary<string, long>();

                var f = collection.Find(Builders<BsonDocument>.Filter.Empty);
                var e = f.ToEnumerable();
                foreach (BsonDocument bs in e)
                {
                    var jsonWriterSettings = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict }; // key part
                    string json = "{customer:" + bs["customer"].ToJson(jsonWriterSettings) + "}";
                    //Console.WriteLine(json);

                    XmlDocument doc = (XmlDocument)Newtonsoft.Json.JsonConvert.DeserializeXmlNode(json);
                    XmlNodeList nl = doc.GetElementsByTagName("line");

                    foreach (XmlNode n in nl)
                    {
                        string key = n["productId"].InnerText;
                        bool hasKey = idToOrderCount.ContainsKey(key);
                        long value = hasKey ? idToOrderCount[key] : 0;
                        value += long.Parse(n["qty"].InnerText);

                        if (hasKey)
                        {
                            idToOrderCount[key] = value;
                        }
                        else
                        {
                            idToOrderCount.Add(key, value);
                        }
                    }
                }
                
                List<KeyValuePair<string, long>> l1 = idToOrderCount.ToList();
                l1.Sort((x, y) => y.Value.CompareTo(x.Value));
                l2 = l1.ConvertAll(x => x.Key + ": " + x.Value);
                
                string cacheImport = "{metadata: {tag: \"TopPopularProductsByQuantity\"}, values: " + l2.ToJson() + "}";
                cache.InsertOne(BsonDocument.Parse(cacheImport));
            }

            if (l2.Count > retCount) l2.RemoveRange(retCount, l2.Count - retCount);

            string ret = "<Top25PopProductsByQuantity>";
            foreach (string s in l2)
            {
                ret += "\n    " + s;
            }
            ret += "\n</Top25PopProductsByQuantity>";

            Thread.Sleep(10); //make sure the queary doesn;t finish before the callback is setup
            return ret;
        }

        public static async Task<string> GetCustByID(IMongoCollection<BsonDocument> collection, string id)
        {
            BsonDocument result = null;

            var f = collection.Find(Builders<BsonDocument>.Filter.Empty);
            var e = f.ToEnumerable();
            foreach (BsonDocument bs in e)
            {
                if((bs["customer"].AsBsonDocument["customerId"].AsString as string) == id)
                {
                    result = bs;
                    break;
                }
            }

            string ret =    "<GetCustByID_Result>";
            ret +=          "\n    "+result.ToString();
            ret +=          "\n</GetCustByID_Result>";

            Thread.Sleep(10); //make sure the queary doesn't finish before the callback is setup
            return ret;
        }

        public static async Task<string> TopCustomersAlphabeticallyByLastName(IMongoCollection<BsonDocument> collection, int retCount = 100, bool forceRecalc = false)
        {
            bool needRecalc = true;
            List<string> strs = null;
            IMongoCollection<BsonDocument> cache = db.GetCollection<BsonDocument>("cache");
            
            var cha = cache.Find(Builders<BsonDocument>.Filter.Empty).ToEnumerable();
            foreach (BsonDocument bs in cha)
            {
                if (bs["metadata"].AsBsonDocument["tag"].AsString == "TopCustomersAlphabeticallyByLastName")
                {
                    if (!forceRecalc)
                    {
                        strs = bs["values"].AsBsonArray.Select(p => p.AsString).ToList();
                        needRecalc = false;
                    }
                    else
                    {
                        cache.DeleteOne(bs);
                    }
                    break;
                }
            }

            if (needRecalc) {
                var f = collection.Find(Builders<BsonDocument>.Filter.Empty);
                var e = f.ToEnumerable();
                strs = new List<string>();
                foreach (BsonDocument bs in e)
                {
                    string s = bs["customer"].AsBsonDocument["name"].ToString();
                    if (!strs.Contains(s)) strs.Add(s);
                }
                strs.Sort((y, x) => {
                    string[] yss = y.Split(' ');
                    string[] xss = x.Split(' ');

                    int l = yss[1].CompareTo(xss[1]);

                    return l != 0 ? l : yss[0].CompareTo(xss[0]);
                });
                string cacheImport = "{metadata: {tag: \"TopCustomersAlphabeticallyByLastName\"}, values: " + strs.ToJson() + "}";
                cache.InsertOne(BsonDocument.Parse(cacheImport));
            }


            if (strs.Count > retCount) strs.RemoveRange(retCount, strs.Count - retCount);

            string ret = "<TopCustomersAlphabeticallyByLastName>";
            foreach (string s in strs)
            {
                ret += "\n    " + s;
            }

            ret += "\n</TopCustomersAlphabeticallyByLastName>";

            Thread.Sleep(10); //make sure the queary doesn;t finish before the callback is setup
            return ret;
        }
    }
}
