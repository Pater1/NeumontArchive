using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Mathegates
{
    class ParamMath
    {
        public struct Operation
        {
            public Operation(string nm, MathOne op)
            {
                name = nm;
                operation = op;
            }
            public string name { get; private set; }
            public MathOne operation { get; private set; }
        }
        public static int[] PromptUserForVals()
        {
            Console.Write("Input N comma-seperated integer values (example: 1,2,34,0,-6):\n\t");
            string input = Console.ReadLine();
            Console.WriteLine();
            return TryParseAll(input);
        }
        public static int[] TryParseAll(string csv)
        {
            string[] csvs = Regex.Replace(csv, @"\s+", "").Split(',');
            Converter<string, int?> conversion = str => { int j = 0; return int.TryParse(str, out j) ? (int?)j : null; };
            return Array.ConvertAll(csvs, conversion).Where(x => x != null).Cast<int>().ToArray();
        }

        public delegate void MathAll(params int[] i);
        #region basic methods
        public static void AddAll(params int[] i)
        {
            Console.WriteLine($"Result, Addition Result: {Operate((w, p) => w + p, null, i)}");
        }
        public static void SubtractAll(params int[] i)
        {
            Console.WriteLine($"Result, Subtraction Result: {Operate((w, p) => w - p, null, i)}");
        }
        public static void MultiplyAll(params int[] i)
        {
            Console.WriteLine($"Result, Multiplication Result: {Operate((w, p) => w * p, null, i)}");
        }
        public static void DivideAll(params int[] i)
        {
            Console.WriteLine($"Result, Division Result: {Operate((w, p) => w / p, null, i)}");
        }
        public static void ModAll(params int[] i)
        {
            Console.WriteLine($"Result, Modular Reduction Result: {Operate((w, p) => w & p, null, i)}");
        }
        #endregion
        public static MathAll Create(DefaultOperation mask, params dynamic[] ops)
        {
            return Create(mask) + Create(ops);
        }
        public enum DefaultOperation : int
        {
            Add = 1,
            Subtract = 2,
            Multiply = 4,
            Divide = 8,
            Mod = 16,
            All = -1
        }
        public static MathAll Create(DefaultOperation mask)
        {
            List<dynamic> dyn = new List<dynamic>();

            if ((mask & DefaultOperation.Add) != 0)
            {
                dyn.Add(new Operation("Addition", (i, j) => i + j));
            }
            if ((mask & DefaultOperation.Subtract) != 0)
            {
                dyn.Add(new Operation("Subtraction", (i, j) => i - j));
            }
            if ((mask & DefaultOperation.Multiply) != 0)
            {
                dyn.Add(new Operation("Multiplication", (i, j) => i * j));
            }
            if ((mask & DefaultOperation.Divide) != 0)
            {
                dyn.Add(new Operation("Division", (i, j) => i / j));
            }
            if ((mask & DefaultOperation.Mod) != 0)
            {
                dyn.Add(new Operation("Modular Reduction", (i, j) => i % j));
            }

            return Create(dyn.ToArray());
        }
        public static MathAll Create(params dynamic[] ops)
        {
            MathAll maths = Delegate.CreateDelegate(typeof(MathAll), new object(), "PurposefulMiss", true, false) as MathAll;
            foreach (var op in ops)
            {
                try
                {
                    maths += (i => Console.WriteLine($"Result, {op.name}: {Operate(op.operation, null, i)}"));
                }
                catch { }
            }
            return maths;
        }

        public delegate int MathOne(int i, int j);
        private static int Operate(MathOne operation, int? a, params int[] i)
        {
            return (a == null ? (Func<int>)(() => {
                return i.Length == 0 ? 0 : i.Length == 1 ? i[0] : i.Length == 2 ? operation(i[0], i[1]) : Operate(operation, operation(i[0], i[1]), i.Skip(2).ToArray());
            }) : (Func<int>)(() => {
                return i.Length == 1 ? operation((int)a, i[0]) : Operate(operation, operation((int)a, i[0]), i.Skip(1).ToArray());
            }))();
        }
    }
}
