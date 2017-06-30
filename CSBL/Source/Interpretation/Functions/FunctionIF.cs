using System;
using CSBL.Reporting;

namespace CSBL.Interpretation.Functions
{
    /// <summary>
    /// This class is a subclass of the FunctionBase class and represents
    /// the [if] function.
    /// </summary>
    public class FunctionIF : FunctionBase
    {
        /// <summary>
        /// Constructor for the FunctionIF class.
        /// </summary>
        public FunctionIF()
            : base("if")
        { }

        /// <summary>
        /// Execute the function.
        /// </summary>
        /// <param name="interpreter">A reference to the current interpreter.</param>
        /// <param name="intepreterEnvironment">A reference to the current interpreter environment.</param>
        public override void Execute(Interpreter interpreter, InterpreterEnvironment intepreterEnvironment)
        {

        }
    }
}
