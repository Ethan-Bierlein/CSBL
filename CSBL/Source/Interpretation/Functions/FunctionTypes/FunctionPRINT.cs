using System;
using CSBL.Reporting;

namespace CSBL.Interpretation.Functions.FunctionTypes
{
    /// <summary>
    /// This class is a subclass of the FunctionBase class and represents
    /// the [print] function.
    /// </summary>
    public class FunctionPRINT : FunctionBase
    {
        /// <summary>
        /// Constructor for the FunctionPRINT class.
        /// </summary>
        public FunctionPRINT()
            : base("print")
        { }

        /// <summary>
        /// Execute the function.
        /// </summary>
        /// <param name="interpreter">A reference to the current interpreter.</param>
        /// <param name="intepreterEnvironment">A reference to the current interpreter environment.</param>
        public override bool Execute(Interpreter interpreter, InterpreterEnvironment interpreterEnvironment)
        {
            Console.WriteLine(string.Join(" ", interpreterEnvironment.ValueStack.Pop().Data));
            interpreterEnvironment.CurrentTokenIndex++;
            return true;
        }
    }
}
