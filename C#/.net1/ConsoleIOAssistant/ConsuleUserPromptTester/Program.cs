using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSC160_ConsoleMenu;

namespace ConsuleUserPromptTester
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> testList = new List <string> { "A", "B", "C", "D" };
            int num = CIO.PromptForMenuSelection(testList, false);
            if(num >= 0)
                Console.WriteLine(testList[num]);
            else
                Console.WriteLine("Quit");
        }
    }
}
