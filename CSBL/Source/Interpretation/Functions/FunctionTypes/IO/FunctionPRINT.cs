using System;
using CSBL.Reporting;
using CSBL.Transformation;

namespace CSBL.Interpretation.Functions.FunctionTypes.IO
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
        /// <param name="interpreterEnvironment">A reference to the current interpreter environment.</param>
        public override bool Execute(Interpreter interpreter, InterpreterEnvironment interpreterEnvironment)
        {
            TransformedToken tokenToWrite = interpreterEnvironment.ValueStack.Pop();
            Console.WriteLine(string.Join("", tokenToWrite.Data));
            return true;
        }
    }
}
