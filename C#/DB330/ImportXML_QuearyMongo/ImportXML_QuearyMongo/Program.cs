using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using System.Xml;
using System.Xml.Linq;
using System.IO;

using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;

namespace ImportXML_QuearyMongo
{
    class Program
    {
        private static string xmlImportPath = "E:\\Neumont\\C#\\DB330\\ImportXML_QuearyMongo\\gigantic.xml";//!!!Change values here to point to your gigantic.xml
        private static string mongoConnectionPath = "mongodb://localhost";//!!!Ignore this
        static void Main(string[] args)
        {
            //!!!Ignore all these startup parameters. Unused and untested
            bool p = args.Contains("-p");
            if (p || args.Contains("--path"))
            {
                int index = Array.IndexOf(args, p ? "-p" : "--path");
                xmlImportPath = args[index + 1];
            }

            bool c = args.Contains("-c");
            if (c || args.Contains("--connect"))
            {
                int index = Array.IndexOf(args, c ? "-c" : "--connect");
                mongoConnectionPath = args[index + 1];
            }

            StartAsync().GetAwaiter().GetResult();

            Console.ReadLine();
        }

        static async Task StartAsync()
        {
            MongoClientSettings settings = new MongoClientSettings();
            settings.Server = new MongoServerAddress("127.0.0.1", 27017); //!!!Change values here to connect to your MongoDB
            MongoClient client = new MongoClient(settings);

            IMongoDatabase db = client.GetDatabase("Customers");
            IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("customers");

            Console.WriteLine("Wiping DB");
            await collection.DeleteManyAsync(Builders<BsonDocument>.Filter.Where(x => true));

            Console.Write("Importing in 5...");
            Thread.Sleep(5000);
            Console.WriteLine(" GO!");

            using (FileStream fStream = new FileStream(xmlImportPath, FileMode.Open))
            {
                using (XmlReader xReader = XmlReader.Create(fStream))
                {
                    xReader.MoveToContent();
                    while (xReader.Read())
                    {
                        Console.Write("|");
                        if (xReader.Name == "customer")
                        {
                            string xml = xReader.ReadOuterXml();
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(xml);

                            string json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
                            BsonDocument convertedValue = BsonDocument.Parse(json);
                            await collection.InsertOneAsync(convertedValue);
                        }
                    }
                }
            }

            Console.WriteLine("Done!");
        }
    }
}
