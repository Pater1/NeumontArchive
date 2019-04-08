using System;
using RiakClient;

namespace RAIK
{
    class Program
    {
        static void Main(string[] args){
            const string contributors = "contributors";
            IRiakEndPoint cluster = RiakCluster.FromConfig("riakConfig");
            IRiakClient client = cluster.CreateClient();

            var pingResult = client.Ping();
            if (pingResult.IsSuccess)
            {
                Console.WriteLine("pong");
            }
            else
            {
                Console.WriteLine("Are you sure Riak is running?");
                Console.WriteLine("{0}: {1}", pingResult.ResultCode, pingResult.ErrorMessage);
            }
        }
    }
}
