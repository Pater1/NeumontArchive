using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BinarySerializationTutorial
{
    class Program
    {
        static void Main(string[] args)
        {
            DemoData demo = new DemoData() { Demo = 1L, Data = "This is custom data" };

            BinaryFormatter biFor = new BinaryFormatter();
            using (SaveFileDialog save = new SaveFileDialog())
            {
                save.DefaultExt = ".bi";
                save.ShowDialog();
                using (FileStream fs = new FileStream(save.FileName, FileMode.Create, FileAccess.Write))
                {
                    biFor.Serialize(fs, demo);
                }
            }

            DemoData d = null;
            using (OpenFileDialog open = new OpenFileDialog())
            {
                open.DefaultExt = ".bi";
                open.ShowDialog();
                using (FileStream it = new FileStream(open.FileName, FileMode.Open, FileAccess.Read))
                {
                    d = (DemoData)biFor.Deserialize(it);
                }
            }

            Console.WriteLine($"Demo {d.Demo}: {d.Data}");
        }
    }
}
