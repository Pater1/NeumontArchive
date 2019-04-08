using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections;

namespace MongoDB_ExampleApp
{
    class Program
    {

        //NOTE! Limit(10) applied to all "return all" queuries for sake of readability in console. They have been tested, and work without the limiting.
        static void Main(string[] args)
        {
            MongoClientSettings settings = new MongoClientSettings();
            settings.Server = new MongoServerAddress("192.168.238.131", 27017);
            MongoClient client = new MongoClient(settings);
            IMongoCollection<BsonDocument> collection = client.GetDatabase("test").GetCollection<BsonDocument>("restaurants");

            //a. Return a single document by _id. (In SQL: …Where PrimaryKey = x)
            Console.WriteLine(collection.Find(Builders<BsonDocument>.Filter.Where(a => a["_id"] == ObjectId.Parse("596690382a7400741c3d7c22"))).ToList()[0]);
            Console.WriteLine("");

            //b. Return all the documents with the same value for a field. (Where "borough" = "Brooklyn")
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("borough", "Brooklyn");
            foreach (BsonDocument d in collection.Find(filter).Limit(10).ToList())
                Console.WriteLine("{0}", d["name"]);
            Console.WriteLine("");

            //c. Return all the documents who’s field value contains a string (similar to like ‘%test%’ in SQL)
            FilterDefinition<BsonDocument> filter2 = Builders<BsonDocument>.Filter.Where(a => ((string)a["name"]).Contains("bur"));
            foreach (BsonDocument d in collection.Find(filter2).Limit(10).ToList())
                Console.WriteLine("{0}", d["name"]);
            Console.WriteLine("");

            //d. Return all the documents _having_ a particular field.  (Is there a SQL equivalent for this?)
            FilterDefinition<BsonDocument> filter3 = Builders<BsonDocument>.Filter.Exists("cuisine");
            foreach (BsonDocument d in collection.Find(filter3).Limit(10).ToList())
                Console.WriteLine("{0}", d["name"]);
            Console.WriteLine("");

            //e. List the documents missing a particular field.
            FilterDefinition<BsonDocument> filter4 = Builders<BsonDocument>.Filter.Not(filter3);
            foreach (BsonDocument d in collection.Find(filter4).Limit(10).ToList())
                Console.WriteLine("{0}", d["name"]);
            Console.WriteLine("");

            //f. Return the number of documents that have a particular value for a field.  (Count(*) …. Where Field = ‘test’)
            FilterDefinition<BsonDocument> filter5 = Builders<BsonDocument>.Filter.Eq("borough", "Brooklyn");
            Console.WriteLine(collection.Find(filter5).ToList().Count());
            Console.WriteLine("");

            //g. Write an aggregate (sum, average, count etc.) grouped by a particular field. (Select state, sum(population) … Group by state)
            Console.WriteLine(collection.CountByField("borough")["Brooklyn"]); //see extention methods in class Extentions below for implementation.

            //h. Be prepared to explain the MongoDB equivalent of 
            //      Select* From Customers c Join Orders r On c.CustId = r.CustId Join OrderDetails d on r.OrderId = d.OrderId

        }
    }

    public static class Extentions
    {
        //Returns Dictionary<field value, count with that value>
        public static Dictionary<string, ulong> CountByField(this IMongoCollection<BsonDocument> collection, string field)
        {
            Dictionary<string, ulong> ret = new Dictionary<string, ulong>();
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Empty;
            IEnumerable<BsonDocument> enumer = collection.Find(filter).ToList();

            foreach(BsonDocument e in enumer)
            {
                string val = (string)e[field];
                if (ret.ContainsKey(val))
                {
                    ret[val]++;
                }
                else
                {
                    ret.Add(val, 0);
                }
            }

            return ret;
        }
    }
}
