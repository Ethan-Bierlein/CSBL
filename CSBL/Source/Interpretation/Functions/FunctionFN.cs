using System;
using CSBL.Reporting;

namespace CSBL.Interpretation.Functions
{
    /// <summary>
    /// This class is a subclass of the FunctionBase class and represents
    /// the [fn] function.
    /// </summary>
    public class FunctionFN : FunctionBase
    {
        /// <summary>
        /// Constructor for the FunctionFN class.
        /// </summary>
        public FunctionFN()
            : base("fn")
        { }

        /// <summary>
        /// Execute the function.
        /// </summary>
        /// <param name="interpreter">A reference to the current interpreter.</param>
        /// <param name="intepreterEnvironment">A reference to the current interpreter environment.</param>
        public override bool Execute(Interpreter interpreter, InterpreterEnvironment intepreterEnvironment)
        {
            return false;
        }
    }
}
