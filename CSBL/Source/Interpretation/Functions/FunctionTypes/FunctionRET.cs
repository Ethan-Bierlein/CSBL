using System;
using CSBL.Reporting;

namespace CSBL.Interpretation.Functions.FunctionTypes
{
    /// <summary>
    /// This class is a subclass of the FunctionBase class and represents
    /// the [ret] function.
    /// </summary>
    public class FunctionRET : FunctionBase
    {
        /// <summary>
        /// Constructor for the FunctionRET class.
        /// </summary>
        public FunctionRET()
            : base("ret")
        { }

        /// <summary>
        /// Execute the function.
        /// </summary>
        /// <param name="interpreter">A reference to the current interpreter.</param>
        /// <param name="intepreterEnvironment">A reference to the current interpreter environment.</param>
        public override bool Execute(Interpreter interpreter, InterpreterEnvironment interpreterEnvironment)
        {
            int returnTo = interpreterEnvironment.CallStack.Pop();
            interpreterEnvironment.CurrentTokenIndex = returnTo++;
            return true;
        }
    }
}
