using System;

namespace Mathegates
{
    class Program
    {
        public static ParamMath.MathAll MathOpAgregateDelegate { get; set; }
        public static void Run()
        {
            MathOpAgregateDelegate = ParamMath.Create();

            MathOpAgregateDelegate += ParamMath.AddAll;
            MathOpAgregateDelegate += ParamMath.SubtractAll;
            MathOpAgregateDelegate += ParamMath.MultiplyAll;
            MathOpAgregateDelegate += ParamMath.DivideAll;
            MathOpAgregateDelegate += ParamMath.ModAll;

            int[] userVals = ParamMath.PromptUserForVals();
            MathOpAgregateDelegate(userVals);


            //ParamMath.Create(ParamMath.DefaultOperation.All,
            //    new { name = "Pow", operation = (ParamMath.MathOne)((i, j) => (int)Math.Pow(i, j)) },
            //    new ParamMath.Operation("Root", (i, j) => (int)Math.Pow(i, 1.0 / j)))(ParamMath.PromptUserForVals());
        }
        static void Main(string[] args)
        {
            Run();
        }
    }
}