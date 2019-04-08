using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializationTutorial
{
    [System.Serializable]
    public class DemoData
    {
        public long ID { get; set; } = 1298765432L;
        public string Data { get; set; } = "This is test data!";
    }
}
